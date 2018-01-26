using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever.Riff;
using NAudio.Midi;

namespace BFForever.MIDI
{
    public class MIDIImport : ChartImport
    {
        private MidiFile _midiFile;
        private List<TempoIndex> _tempoIdx;
        
        public MIDIImport(string input)
        {
            _midiFile = new MidiFile(input, false);
            CalculateTempo();
        }

        private void CalculateTempo()
        {
            _tempoIdx = new List<TempoIndex>();
            double realTime = 0.0;

            List<TempoEvent> events = _midiFile.Events[0].Where(x => x is TempoEvent).Select(x => x as TempoEvent).ToList<TempoEvent>();

            if (events.Count <= 0)
            {
                // Default to 120 bpm
                _tempoIdx.Add(new TempoIndex()
                {
                    AbsoluteTime = 0,
                    RealTime = 0,
                    BPM = 120
                });
                return;
            }
            else if (events[0].AbsoluteTime != 0)
            {
                // Default to 120 bpm
                _tempoIdx.Add(new TempoIndex()
                {
                    AbsoluteTime = 0,
                    RealTime = 0,
                    BPM = 120
                });

                // Delta time from previous event
                realTime += ((double)events.First().AbsoluteTime / _midiFile.DeltaTicksPerQuarterNote) * (60000.0 / _tempoIdx.Last().BPM);
            }

            // Adds first entry
            _tempoIdx.Add(new TempoIndex()
            {
                AbsoluteTime = events.First().AbsoluteTime,
                RealTime = realTime,
                BPM = events.First().Tempo
            });
            
            foreach (TempoEvent ev in events.Skip(1))
            {
                TempoIndex previous = _tempoIdx.Last();
                double relativeDelta = ev.AbsoluteTime - previous.AbsoluteTime;
                
                double relativeTime = ((double)relativeDelta / _midiFile.DeltaTicksPerQuarterNote) * (60000.0 / previous.BPM);
                realTime += relativeTime;

                _tempoIdx.Add(new TempoIndex()
                {
                    AbsoluteTime = ev.AbsoluteTime,
                    RealTime = realTime,
                    BPM = ev.Tempo
                });
            }
        }

        private double GetRealTime(long absoluteTime)
        {
            TempoIndex previous = _tempoIdx.Last(x => x.AbsoluteTime <= absoluteTime);
            if (previous.AbsoluteTime == absoluteTime) return previous.RealTime;

            double relativeDelta = absoluteTime - previous.AbsoluteTime;
            return previous.RealTime + ((double)relativeDelta / _midiFile.DeltaTicksPerQuarterNote) * (60000.0 / previous.BPM);
        }
        
        private List<MeasureEntry> ExportMeasureEntries()
        {
            const int BEAT_DOWN = 12;
            const int BEAT_UP = 13;
            
            List<MeasureEntry> measures = new List<MeasureEntry>();
            var beatTrack = _midiFile.Events.FirstOrDefault(x => x[0].ToString().Contains("BEAT"));

            foreach (NoteOnEvent note in beatTrack.Where(x => x is NoteOnEvent).Select(x => x as NoteOnEvent))
            {
                if (note.NoteNumber != BEAT_UP && note.NoteNumber != BEAT_DOWN) continue;

                MeasureEntry entry = new MeasureEntry()
                {
                    Start = (float)GetRealTime(note.AbsoluteTime),
                    End = (float)GetRealTime(note.AbsoluteTime + note.NoteLength),
                    Beat = note.NoteNumber == BEAT_DOWN ? 1.0f : 2.0f
                };

                measures.Add(entry);
            }

            return measures;
        }

        private List<TempoEntry> ExportTempoEntries()
        {
            return _tempoIdx.Select(x => new TempoEntry()
            {
                Start = (float)x.RealTime,
                End = (float)x.RealTime,
                BPM = (float)x.BPM
            }).ToList();
        }

        private List<TimeSignatureEntry> ExportTimeSignatureEntries()
        {
            List<TimeSignatureEntry> tsEntries = new List<TimeSignatureEntry>();

            foreach (TimeSignatureEvent ev in _midiFile.Events[0].Where(x => x is TimeSignatureEvent))
            {
                string[] number = ev.TimeSignature.Split('/'); // "4/4"

                TimeSignatureEntry entry = new TimeSignatureEntry()
                {
                    Start = (float)GetRealTime(ev.AbsoluteTime),
                    End = (float)GetRealTime(ev.AbsoluteTime),
                    Beat = int.Parse(number[0]),
                    Measure = int.Parse(number[1])
                };

                tsEntries.Add(entry);
            }

            return tsEntries;
        }

        private TabFinger GetStringColor(int idx)
        {
            if (idx < 1 || idx > 6)
                return TabFinger.Tap; // Black

            return RB3Colors[--idx];
        }

        private IList<MidiEvent> GetGuitarTrack(bool guitar)
        {
            string trackNameBegin = "PART REAL_" + (guitar ? "GUITAR" : "BASS");
            var tracks = _midiFile.Events.Where(x => x[0].ToString().Contains(trackNameBegin)).ToList();

            switch(tracks.Count)
            {
                case 0:
                    return new List<MidiEvent>();
                case 1:
                    return tracks[0];
                default:
                    // Looks for 22-fret version, returns first if not found
                    var track22 = tracks.FirstOrDefault((x => x[0].ToString().Contains(trackNameBegin + "_22")));
                    return (track22 != null) ? track22 : tracks.First();
            }
        }

        private List<EventEntry> ExportMasterEventEntries()
        {
            List<EventEntry> entries = new List<EventEntry>();
            var voxTrack = _midiFile.Events.FirstOrDefault(x => x[0].ToString().Contains("EVENTS"));
            if (voxTrack == null) return entries;

            foreach (var ev in  voxTrack.Where(x => x is MetaEvent && ((MetaEvent)x).MetaEventType == MetaEventType.TextEvent).Select(x => x as NAudio.Midi.TextEvent))
            {
                FString text;

                switch(ev.Text)
                {
                    case "[music_start]":
                        text = EventAudioStart;
                        break;
                    case "[music_end]":
                        text = EventAudioEnd;
                        break;
                    case "[end]":
                        text = EventSongEnd;
                        break;
                    default:
                        continue;
                }

                EventEntry entry = new EventEntry()
                {
                    Start = (float)GetRealTime(ev.AbsoluteTime),
                    End = (float)GetRealTime(ev.AbsoluteTime) + 150.0f, // TODO: Figure out exact length
                    EventName = text
                };

                entries.Add(entry);
            }

            return entries;
        }
        
        protected override List<ZObject> GetMasterObjects(HKey directoryPath)
        {
            List<ZObject> objects = new List<ZObject>();
            HKey directory = directoryPath + ".master";

            // Creates event track
            Event ev = new Event(directory + ".event", directory);
            ev.Events = ExportMasterEventEntries();
            objects.Add(ev);

            // Creates measure track (BEAT)
            Measure measure = new Measure(directory + ".measure", directory);
            measure.Events = ExportMeasureEntries();
            objects.Add(measure);

            // Creates section track
            // TODO: Implement importing of sections
            /*
            Section section = new Section(directory + ".section", directory);
            section.Events = ExportSectionEntries();
            objects.Add(section);
            */

            // Creates tempo track
            Tempo tempo = new Tempo(directory + ".tempo", directory);
            tempo.Events = ExportTempoEntries();
            objects.Add(tempo);

            // Creates time signature track
            TimeSignature ts = new TimeSignature(directory + ".timesignature", directory);
            ts.Events = ExportTimeSignatureEntries();
            objects.Add(ts);

            return objects;
        }

        protected override List<ZObject> GetVoxObjects(HKey directoryPath)
        {
            const int VOX_PHRASE = 105; // TODO: Read in phrases on 106 as well
            const int VOX_MAX_PITCH = 84;
            const int VOX_MIN_PITCH = 36;

            List<ZObject> objects = new List<ZObject>();
            HKey directory = directoryPath + ".vox";
            
            var voxTrack = _midiFile.Events.FirstOrDefault(x => x[0].ToString().Contains("PART VOCALS"));
            if (voxTrack == null) return objects;

            var phrases = voxTrack.Where(x => x is NoteOnEvent && ((NoteOnEvent)x).NoteNumber == VOX_PHRASE && ((NoteOnEvent)x).Velocity > 0).Select(x => x as NoteOnEvent).ToList();
            var lyrics = voxTrack.Where(x => x is MetaEvent && ((MetaEvent)x).MetaEventType == MetaEventType.Lyric).Select(x => x as NAudio.Midi.TextEvent).ToDictionary(key => key.AbsoluteTime, value => value.Text);

            // Creates vox track
            Vox vox = new Vox(directory + ".vox", directory);
            objects.Add(vox);

            foreach (NoteOnEvent note in voxTrack.Where(x => x is NoteOnEvent).Select(x => x as NoteOnEvent))
            {
                if (note.NoteNumber < VOX_MIN_PITCH || note.NoteNumber > VOX_MAX_PITCH || note.Velocity <= 0) continue;

                VoxEntry entry = new VoxEntry()
                {
                    Start = (float)GetRealTime(note.AbsoluteTime),
                    End = (float)GetRealTime(note.AbsoluteTime + note.NoteLength),
                    Lyric = lyrics.ContainsKey(note.AbsoluteTime) ? lyrics[note.AbsoluteTime] : "",
                    Pitch = note.NoteNumber,
                    PitchAlt = note.NoteNumber // Not sure
                };

                if (entry.Lyric.Value.Contains('#'))
                    entry.NoteType = VoxNote.NonPitched;
                else
                    entry.NoteType = VoxNote.Regular;

                if (entry.Lyric.Value.Contains('+') && vox.Events.Count > 0)
                    vox.Events.Last().NoteType = VoxNote.Extended;

                vox.Events.Add(entry);
            }

            // Creates voxpushphrase track
            if (phrases.Count > 0)
            {
                VoxPushPhrase pushPhrase = new VoxPushPhrase(directory + ".voxpushphrase", directory);
                objects.Add(pushPhrase);

                long firstAbsTime = phrases.First().AbsoluteTime;
                int sixteenth = _midiFile.DeltaTicksPerQuarterNote >> 2; // 1/16 note

                long firstPushTime = firstAbsTime - (_midiFile.DeltaTicksPerQuarterNote << 2);
                if (firstPushTime < 0) firstPushTime = 0;
                
                // Adds push event that starts one measure before first phrase
                pushPhrase.Events.Add(new TimeEvent()
                {
                    Start = (float)GetRealTime(firstPushTime),
                    End = (float)GetRealTime(firstPushTime + sixteenth)
                });

                // Adds push event for start of first phrase
                pushPhrase.Events.Add(new TimeEvent()
                {
                    Start = (float)GetRealTime(firstAbsTime),
                    End = (float)GetRealTime(firstAbsTime + sixteenth)
                });

                // Adds push event for end of each phrase
                pushPhrase.Events.AddRange(phrases.Select(x => new TimeEvent()
                {
                    Start = (float)GetRealTime(x.AbsoluteTime + x.NoteLength),
                    End = (float)GetRealTime(x.AbsoluteTime + x.NoteLength + sixteenth)
                }));
            }

            // TODO: Creates voxspread track

            return objects;
        }

        protected override List<ZObject> GetGuitarObjects(HKey directoryPath)
        {
            string[] current = directoryPath.GetLastText().Value.Split('_'); // instrument_difficulty
            bool guitar = current[0].Equals("gtr", StringComparison.CurrentCultureIgnoreCase);

            List<ZObject> objects = new List<ZObject>();
            List<TabEntry> tabEntries = new List<TabEntry>();
            //List<Riff2.TextEvent> chordEntries = new List<Riff2.TextEvent>();

            int stringStart;
            var trackEvents = GetGuitarTrack(guitar);

            switch (current[1])
            {
                case "jam":
                    // Easy
                    stringStart = 24;
                    break;
                case "nov":
                    // Medium
                    stringStart = 48;
                    break;
                case "beg":
                    // Hard
                    stringStart = 72;
                    break;
                case "int":
                case "rhy":
                case "adv": // Lead
                default:
                    // Expert
                    stringStart = 96;
                    break;
            }

            foreach (NoteOnEvent note in trackEvents.Where(x => x is NoteOnEvent).Select(x => x as NoteOnEvent))
            {
                if (note.NoteNumber < stringStart || note.NoteNumber >= (stringStart + 6) || note.Velocity < 100) continue;
                int stringNumber = 6 - (note.NoteNumber - stringStart); // 1-6

                TabEntry tabEntry = new TabEntry()
                {
                    Start = (float)GetRealTime(note.AbsoluteTime),
                    End = (float)GetRealTime(note.AbsoluteTime + note.NoteLength),
                    FretNumber = note.Velocity - 100,
                    StringNumber = stringNumber,
                    Finger = GetStringColor(stringNumber)
                    // TODO: Implement bends, left hand mutes, etc.
                };

                if (!guitar)
                    tabEntry.StringNumber -= 2; // Hotfix for bass

                if (note.Channel == 4) // Left-hand mute in protar
                    tabEntry.NoteType = TabNoteType.Chukka;

                tabEntries.Add(tabEntry);
            }

            Tab tab = new Tab(directoryPath + ".tab", directoryPath);
            tab.Events = tabEntries;
            objects.Add(tab);

            // TODO: Implement chord events

            return objects;
        }
    }
}
