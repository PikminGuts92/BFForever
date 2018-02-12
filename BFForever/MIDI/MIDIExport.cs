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
                var track = instruments.First(x => x.InstrumentType == type && x.Difficulty == difficulty);
                return track.TrackPaths.Select(x => _objects.First(y => y.FilePath == x)).ToList();
            }

            var master = GetInstrumentTracks("master", "");
            var tempo = master.First(x => x is Tempo) as Tempo;
            var ts = master.First(x => x is TimeSignature) as TimeSignature;

            // Build tempo
            mid.AddTrack(CreateTempoTrack(tempo, ts));

            foreach(var track in master)
            {
                switch(track)
                {
                    case Measure m:
                        mid.AddTrack(CreateBeatTrack(m));
                        break;
                }
            }

            var vox = GetInstrumentTracks("vocals", "");
            mid.AddTrack(CreateVoxTrack(vox));

            // Guitar tracks

            // Bass tracks

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
    }
}
