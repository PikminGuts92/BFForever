using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever.Riff;
using NAudio.Midi;

namespace BFForever.MIDI
{
    public class MIDIExport
    {
        private const int DELTA_TICKS_PER_QUARTER = 480;
        private readonly List<ZObject> _objects;
        private readonly List<TempoIndex> _tempoIdx = new List<TempoIndex>();
        
        public MIDIExport(List<ZObject> objects)
        {
            _objects = objects;
        }

        public void Export(string path)
        {
            MidiEventCollection mid = new MidiEventCollection(1, DELTA_TICKS_PER_QUARTER);

            Song song = _objects.First(x => x is Song) as Song;
            var instruments = song.InstrumentPaths.Select(x => _objects.First(y => y.FilePath == x) as Instrument).ToList();
            
            List<ZObject> GetInstrumentTracks(string type, string difficulty)
            {
                var insTracks = instruments.FirstOrDefault(x => x.InstrumentType == type && x.Difficulty == difficulty);
                if (insTracks == null) return new List<ZObject>();

                return insTracks.TrackPaths.Select(x => _objects.First(y => y.FilePath == x)).ToList();
            }

            var master = GetInstrumentTracks("master", "");
            var tempo = master.First(x => x is Tempo) as Tempo;
            var ts = master.First(x => x is TimeSignature) as TimeSignature;

            // Tempo track
            mid.AddTrack(CreateTempoTrack(tempo, ts));
            
            // Bass tracks
            var track = CreateTabTrack(GetInstrumentTracks("bass", "jam"), "PART BASS_E");
            if (track.Count > 1) mid.AddTrack(track);
            track = CreateTabTrack(GetInstrumentTracks("bass", "nov"), "PART BASS_M");
            if (track.Count > 1) mid.AddTrack(track);
            track = CreateTabTrack(GetInstrumentTracks("bass", "beg"), "PART BASS_H");
            if (track.Count > 1) mid.AddTrack(track);
            track = CreateTabTrack(GetInstrumentTracks("bass", "int"), "PART BASS_X");
            if (track.Count > 1) mid.AddTrack(track);
            track = CreateTabTrack(GetInstrumentTracks("bass", "adv"), "PART BASS_R");
            if (track.Count > 1) mid.AddTrack(track);

            // Guitar tracks
            track = CreateTabTrack(GetInstrumentTracks("guitar", "jam"), "PART GUITAR_E");
            if (track.Count > 1) mid.AddTrack(track);
            track = CreateTabTrack(GetInstrumentTracks("guitar", "nov"), "PART GUITAR_M");
            if (track.Count > 1) mid.AddTrack(track);
            track = CreateTabTrack(GetInstrumentTracks("guitar", "beg"), "PART GUITAR_H");
            if (track.Count > 1) mid.AddTrack(track);
            track = CreateTabTrack(GetInstrumentTracks("guitar", "int"), "PART GUITAR_X");
            if (track.Count > 1) mid.AddTrack(track);
            track = CreateTabTrack(GetInstrumentTracks("guitar", "adv"), "PART GUITAR_R");
            if (track.Count > 1) mid.AddTrack(track);
            track = CreateTabTrack(GetInstrumentTracks("guitar", "rhy"), "PART GUITAR_R_RHYTHM");
            if (track.Count > 1) mid.AddTrack(track);

            // Vox track
            track = CreateVoxTrack(GetInstrumentTracks("vocals", ""));
            if (track.Count > 1) mid.AddTrack(track);

            // TODO: Add section/event track

            // Beat track
            var measure = master.First(x => x is Measure) as Measure;
            track = CreateBeatTrack(measure);
            mid.AddTrack(track);

            MidiFile.Export(path, mid);
        }

        private long GetAbsoluteTime(double startTime)
        {
            TempoIndex currentTempo = _tempoIdx.First();

            // Finds last tempo change before event
            foreach(TempoIndex idx in _tempoIdx.Skip(1))
            {
                if (idx.RealTime <= startTime) currentTempo = idx;
                else break;
            }

            double difference = startTime - currentTempo.RealTime;
            long absoluteTicks = currentTempo.AbsoluteTime + (1000L * (long)difference * DELTA_TICKS_PER_QUARTER) / currentTempo.MicroPerQuarter;

            // Applies quantization and snaps to grid
            int q = DELTA_TICKS_PER_QUARTER / 32; // 1/128th quantization
            if (absoluteTicks % q != 0)
            {
                long before = absoluteTicks % q;
                long after = q - before;

                if (before < after)
                    absoluteTicks -= before;
                else
                    absoluteTicks += after;
            }

            return absoluteTicks;
        }

        private List<MidiEvent> CreateTempoTrack(Tempo tempo, TimeSignature ts)
        {
            List<MidiEvent> track = new List<MidiEvent>();
            _tempoIdx.Clear();
            track.Add(new NAudio.Midi.TextEvent("bfTempo", MetaEventType.SequenceTrackName, 0));

            if (tempo.Events.Count <= 0 || tempo.Events[0].Start > 0.0f)
            {
                var idxEntry = new TempoIndex(0, 0, 120);
                //track.Add(new NAudio.Midi.TempoEvent(idxEntry.MicroPerQuarter, idxEntry.AbsoluteTime));
                _tempoIdx.Add(idxEntry);
            }

            long GetAbsoluteTime(double startTime, TempoIndex currentTempo)
            {
                double difference = startTime - currentTempo.RealTime;
                long absoluteTicks = currentTempo.AbsoluteTime + (1000L * (long)difference * DELTA_TICKS_PER_QUARTER) / currentTempo.MicroPerQuarter;
                
                //int q = DELTA_TICKS_PER_QUARTER / 32; // 1/128th quantization
                //if (absoluteTicks % q != 0) absoluteTicks += q - (absoluteTicks % q);

                // Applies quantization and snaps to grid
                int q = DELTA_TICKS_PER_QUARTER / 32; // 1/128th quantization
                if (absoluteTicks % q != 0)
                {
                    long before = absoluteTicks % q;
                    long after = q - before;

                    if (before < after)
                        absoluteTicks -= before;
                    else
                        absoluteTicks += after;
                }

                return absoluteTicks;
            }

            // Adds tempo changes
            if (tempo.Events.Count > 0)
            {
                var firstTempo = tempo.Events.First();
                var idxEntry = new TempoIndex()
                {
                    AbsoluteTime = _tempoIdx.Count > 0 ? GetAbsoluteTime(firstTempo.Start, _tempoIdx.Last()) : 0,
                    RealTime = firstTempo.Start,
                    BPM = firstTempo.BPM
                };
                
                track.Add(new NAudio.Midi.TempoEvent(idxEntry.MicroPerQuarter, idxEntry.AbsoluteTime));
                _tempoIdx.Add(idxEntry);

                foreach (var tempoEntry in tempo.Events.Skip(1))
                {
                    idxEntry = new TempoIndex()
                    {
                        AbsoluteTime = GetAbsoluteTime(tempoEntry.Start, _tempoIdx.Last()),
                        RealTime = tempoEntry.Start,
                        BPM = tempoEntry.BPM
                    };

                    track.Add(new NAudio.Midi.TempoEvent(idxEntry.MicroPerQuarter, idxEntry.AbsoluteTime));
                    _tempoIdx.Add(idxEntry);
                }
            }

            // TODO: Add time signature changes

            // Sort by absolute time (And ensure track name is first event)
            track.Sort((x, y) => (int)(x is NAudio.Midi.TextEvent ? int.MinValue : x.AbsoluteTime - y.AbsoluteTime));

            // Adds end track
            track.Add(new MetaEvent(MetaEventType.EndTrack, 0, track.Last().AbsoluteTime));
            return track;
        }

        private List<MidiEvent> CreateBeatTrack(Measure measure)
        {
            List<MidiEvent> track = new List<MidiEvent>();
            track.Add(new NAudio.Midi.TextEvent("BEAT", MetaEventType.SequenceTrackName, 0));
            
            foreach(MeasureEntry meEntry in measure.Events)
            {
                long start = GetAbsoluteTime(meEntry.Start);
                int length = DELTA_TICKS_PER_QUARTER / 4; // 1/16th note
                int pitch = (meEntry.Beat == 1.0f) ? 13 : 12; // 1.0 = Up (Default), 2.0 = Down

                track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, pitch, 100));
                track.Add(new NoteEvent(start + length, 1, MidiCommandCode.NoteOff, pitch, 100));
            }

            // Adds end track
            track.Add(new MetaEvent(MetaEventType.EndTrack, 0, track.Last().AbsoluteTime));
            return track;
        }

        private List<MidiEvent> CreateVoxTrack(List<ZObject> voxTracks)
        {
            const int VOX_SPREAD = 108;
            const int VOX_PUSH_PHRASE = 107;
            
            List<MidiEvent> track = new List<MidiEvent>();
            track.Add(new NAudio.Midi.TextEvent("PART VOCALS", MetaEventType.SequenceTrackName, 0));

            // Vox
            Vox vox = voxTracks.FirstOrDefault(x => x is Vox) as Vox;
            if (vox != null)
            {
                foreach(VoxEntry entry in vox.Events)
                {
                    long start = GetAbsoluteTime(entry.Start);
                    long end = GetAbsoluteTime(entry.End);

                    track.Add(new NAudio.Midi.TextEvent(entry.Lyric, MetaEventType.Lyric, start));
                    track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, entry.Pitch, 100));
                    track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, entry.Pitch, 100));
                }
            }

            // VoxPushPhrase
            VoxPushPhrase pushPhrase = voxTracks.FirstOrDefault(x => x is VoxPushPhrase) as VoxPushPhrase;
            if (pushPhrase != null)
            {
                foreach (TimeEvent entry in pushPhrase.Events)
                {
                    long start = GetAbsoluteTime(entry.Start);
                    long end = GetAbsoluteTime(entry.End);
                    
                    track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, VOX_PUSH_PHRASE, 100));
                    track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, VOX_PUSH_PHRASE, 100));
                }
            }

            // VoxSpread
            VoxSpread spread = voxTracks.FirstOrDefault(x => x is VoxSpread) as VoxSpread;
            if (spread != null)
            {
                foreach (SpreadEntry entry in spread.Events)
                {
                    long start = GetAbsoluteTime(entry.Start);
                    long end = GetAbsoluteTime(entry.End);

                    // Should be increments of 0.25f
                    int value = (int)((entry.Speed - 1.0f) / 0.25f);
                    
                    track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, VOX_SPREAD, 100 + value));
                    track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, VOX_SPREAD, 100 + value));
                }
            }

            // Sort by absolute time (And ensure track name is first event)
            track.Sort((x, y) => (int)(x is NAudio.Midi.TextEvent
                                       && ((NAudio.Midi.TextEvent)x).MetaEventType == MetaEventType.SequenceTrackName
                                       ? int.MinValue : x.AbsoluteTime - y.AbsoluteTime));

            // Adds end track
            track.Add(new MetaEvent(MetaEventType.EndTrack, 0, track.Last().AbsoluteTime));
            return track;
        }

        private List<MidiEvent> CreateTabTrack(List<ZObject> tabTracks, string trackName)
        {
            const int AUDIO_EFFECT = 127;
            const int CHORD = 126;
            const int EVENT_PHRASE = 125; // Event/phrase
            const int SPREAD = 124;
            const int WHAMMY = 123;

            const int TAB_BASS_START = 79;
            const int TAB_RESERVED_START = 71;
            const int TAB_TREMELO_START = 63;
            const int TAB_PALM_MUTE_START = 55;
            const int TAB_EXTENDED_START = 47;
            const int TAB_VIBRATO_START = 39;
            const int TAB_BEND_STRENGTH_START = 31;
            const int TAB_BEND_START = 23;
            const int TAB_TYPE_START = 15;
            const int TAB_NOTE_START = 7;
            
            List<MidiEvent> track = new List<MidiEvent>();
            track.Add(new NAudio.Midi.TextEvent(trackName, MetaEventType.SequenceTrackName, 0));
            if (tabTracks.Count <= 0) return track;

            Tab tab = tabTracks.FirstOrDefault(x => x is Tab) as Tab;
            if (tab != null)
            {
                foreach (TabEntry entry in tab.Events)
                {
                    long start = GetAbsoluteTime(entry.Start);
                    long end = GetAbsoluteTime(entry.End);
                    int stringNumber = entry.StringNumber - 1;

                    // Playable notes
                    track.Add(new NoteEvent(start, 1 + (int)entry.Finger, MidiCommandCode.NoteOn, TAB_NOTE_START - stringNumber, 100 + entry.FretNumber));
                    track.Add(new NoteEvent(end, 1 + (int)entry.Finger, MidiCommandCode.NoteOff, TAB_NOTE_START - stringNumber, 100 + entry.FretNumber));

                    // Note type (Modifier)
                    if (entry.NoteType > 0)
                    {
                        track.Add(new NoteEvent(start, (int)entry.NoteType, MidiCommandCode.NoteOn, TAB_TYPE_START - stringNumber, 100));
                        track.Add(new NoteEvent(end, (int)entry.NoteType, MidiCommandCode.NoteOff, TAB_TYPE_START - stringNumber, 100));
                    }

                    // Bend type
                    if (entry.BendType > 0)
                    {
                        track.Add(new NoteEvent(start, (int)entry.BendType, MidiCommandCode.NoteOn, TAB_BEND_START - stringNumber, 100));
                        track.Add(new NoteEvent(end, (int)entry.BendType, MidiCommandCode.NoteOff, TAB_BEND_START - stringNumber, 100));
                    }

                    // Bend strength
                    if (entry.BendStrength != 0.0f)
                    {
                        // Should be increments of 0.25f
                        int value = (int)(entry.BendStrength / 0.25f);

                        track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, TAB_BEND_STRENGTH_START - stringNumber, 100 + value));
                        track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, TAB_BEND_STRENGTH_START - stringNumber, 100 + value));
                    }

                    // Vibrato type
                    if (entry.VibratoType > 0)
                    {
                        track.Add(new NoteEvent(start, (int)entry.VibratoType, MidiCommandCode.NoteOn, TAB_VIBRATO_START - stringNumber, 100));
                        track.Add(new NoteEvent(end, (int)entry.VibratoType, MidiCommandCode.NoteOff, TAB_VIBRATO_START - stringNumber, 100));
                    }

                    // Extended note
                    if (entry.ExtendedNote)
                    {
                        track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, TAB_EXTENDED_START - stringNumber, 100));
                        track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, TAB_EXTENDED_START - stringNumber, 100));
                    }

                    // Palm mute
                    if (entry.PalmMute)
                    {
                        track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, TAB_PALM_MUTE_START - stringNumber, 100));
                        track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, TAB_PALM_MUTE_START - stringNumber, 100));
                    }

                    // Extended note
                    if (entry.Tremelo)
                    {
                        track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, TAB_TREMELO_START - stringNumber, 100));
                        track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, TAB_TREMELO_START - stringNumber, 100));
                    }

                    // Bass type
                    if (entry.BassType > 0)
                    {
                        track.Add(new NoteEvent(start, (int)entry.BassType, MidiCommandCode.NoteOn, TAB_BASS_START - stringNumber, 100));
                        track.Add(new NoteEvent(end, (int)entry.BassType, MidiCommandCode.NoteOff, TAB_BASS_START - stringNumber, 100));
                    }
                }
            }
            
            AudioEffect aEffect = tabTracks.FirstOrDefault(x => x is AudioEffect) as AudioEffect;
            if (aEffect != null)
            {
                foreach (AudioEffectEntry entry in aEffect.Events)
                {
                    long start = GetAbsoluteTime(entry.Start);
                    long end = GetAbsoluteTime(entry.End);

                    track.Add(new NAudio.Midi.TextEvent($"a \"{entry.EffectPath}\"", MetaEventType.TextEvent, start));
                    track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, AUDIO_EFFECT, 100));
                    track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, AUDIO_EFFECT, 100));
                }
            }

            Chord chord = tabTracks.FirstOrDefault(x => x is Chord) as Chord;
            if (chord != null)
            {
                foreach (var entry in chord.Events)
                {
                    long start = GetAbsoluteTime(entry.Start);
                    long end = GetAbsoluteTime(entry.End);

                    track.Add(new NAudio.Midi.TextEvent($"c \"{entry.EventName}\"", MetaEventType.TextEvent, start));
                    track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, CHORD, 100));
                    track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, CHORD, 100));
                }
            }

            Event ev = tabTracks.FirstOrDefault(x => x is Event) as Event;
            if (ev != null)
            {
                foreach (var entry in ev.Events)
                {
                    long start = GetAbsoluteTime(entry.Start);
                    long end = GetAbsoluteTime(entry.End);

                    if (entry.EventName.Value != "Phrase")
                        // Should always be a phrase but idk...
                        track.Add(new NAudio.Midi.TextEvent($"e \"{entry.EventName}\"", MetaEventType.TextEvent, start));
                    
                    track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, EVENT_PHRASE, 100));
                    track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, EVENT_PHRASE, 100));
                }
            }
            
            Spread spread = tabTracks.FirstOrDefault(x => x is Spread) as Spread;
            if (spread != null)
            {
                foreach (SpreadEntry entry in spread.Events)
                {
                    long start = GetAbsoluteTime(entry.Start);
                    long end = GetAbsoluteTime(entry.End);

                    // Should be increments of 0.25f
                    int value = (int)((entry.Speed - 1.0f) / 0.25f);

                    track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, SPREAD, 100 + value));
                    track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, SPREAD, 100 + value));
                }
            }

            Whammy whammy = tabTracks.FirstOrDefault(x => x is Whammy) as Whammy;
            if (whammy != null)
            {
                foreach (WhammyEntry entry in whammy.Events)
                {
                    long start = GetAbsoluteTime(entry.Start);
                    long end = GetAbsoluteTime(entry.End);
                    
                    track.Add(new NoteEvent(start, 1, MidiCommandCode.NoteOn, WHAMMY, 100));
                    track.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, WHAMMY, 100));
                }
            }

            // Sort by absolute time (And ensure track name is first event)
            track.Sort((x, y) => (int)(x is NAudio.Midi.TextEvent
                                       && ((NAudio.Midi.TextEvent)x).MetaEventType == MetaEventType.SequenceTrackName
                                       ? int.MinValue : x.AbsoluteTime - y.AbsoluteTime));

            // Adds end track
            track.Add(new MetaEvent(MetaEventType.EndTrack, 0, track.Last().AbsoluteTime));
            return track;
        }
    }
}
