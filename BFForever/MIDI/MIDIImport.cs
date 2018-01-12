using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever.Riff2;
using NAudio.Midi;

namespace BFForever.MIDI
{
    public class MIDIImport
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

        public List<VoxEntry> ExportVoxEntries()
        {
            const int VOX_PHRASE = 105;
            const int VOX_MAX_PITCH = 84;
            const int VOX_MIN_PITCH = 36;

            List<VoxEntry> voxNotes = new List<VoxEntry>();
            var voxTrack = _midiFile.Events.FirstOrDefault(x => x[0].ToString().Contains("PART VOCALS"));
            if (voxTrack == null) return voxNotes;
            
            var phrases = voxTrack.Where(x => x is NoteOnEvent && ((NoteOnEvent)x).NoteNumber == VOX_PHRASE && ((NoteOnEvent)x).Velocity > 0).Select(x => x as NoteOnEvent).ToList();
            var lyrics = voxTrack.Where(x => x is MetaEvent && ((MetaEvent)x).MetaEventType == MetaEventType.Lyric).Select(x => x as NAudio.Midi.TextEvent).ToDictionary(key => key.AbsoluteTime, value => value.Text);

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

                if (entry.Lyric.Value.Contains('+') && voxNotes.Count > 0)
                    voxNotes.Last().NoteType = VoxNote.Extended;

                voxNotes.Add(entry);
            }
            
            return voxNotes;
        }

        public List<MeasureEntry> ExportMeasureEntries()
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

        public List<TempoEntry> ExportTempoEntries()
        {
            return _tempoIdx.Select(x => new TempoEntry()
            {
                Start = (float)x.RealTime,
                End = (float)x.RealTime,
                BPM = (float)x.BPM
            }).ToList();
        }
    }
}
