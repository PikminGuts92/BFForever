using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever.Riff;

namespace BFForever.MIDI
{
    public abstract class ChartImport
    {
        protected static readonly FString EventAudioStart = StringKey.UpdateValue(0xd80b9a12e613a2dc, "AudioStart");
        protected static readonly FString EventAudioEnd = StringKey.UpdateValue(0x87c0360d9d948e2b, "AudioEnd");
        protected static readonly FString EventSongEnd = StringKey.UpdateValue(0x00d80ba698c7cf65, "SongEnd");

        // Purple, yellow, blue, orange, green, red
        protected readonly TabFinger[] RB3Colors = { TabFinger.Open, TabFinger.Three, TabFinger.Four, TabFinger.Five, TabFinger.One, TabFinger.Two };

        // Purple, green, orange, blue, yellow, red
        protected readonly TabFinger[] RSColors = { TabFinger.Open, TabFinger.One, TabFinger.Five, TabFinger.Four, TabFinger.Three, TabFinger.Two };

        protected abstract List<ZObject> GetMasterObjects(HKey directoryPath);
        protected abstract List<ZObject> GetVoxObjects(HKey directoryPath);
        protected abstract List<ZObject> GetGuitarObjects(HKey directoryPath);

        private List<ZObject> CreateInstrument(HKey directoryPath, string instrumentType, string difficulty, InstrumentTuning tuning)
        {
            // Sets directory name
            if (instrumentType == "guitar" || instrumentType == "bass")
                directoryPath += (instrumentType == "guitar" ? ".gtr_" : ".bss_") + difficulty;
            else
                directoryPath += "." + instrumentType;

            // Creates instrument
            Instrument instrument = new Instrument(directoryPath + ".instrument", directoryPath);
            instrument.InstrumentType = instrumentType == "vox" ? "vocals" : instrumentType;

            // Sets difficulty + tuning
            if (instrumentType == "vox" || instrumentType == "master")
            {
                instrument.Difficulty = "";
                instrument.Tuning = InstrumentTuning.Guitar_EStandard;
            }
            else
            {
                instrument.Difficulty = difficulty;
                instrument.Tuning = tuning;
            }

            // Creates tracks for instrument
            List<ZObject> objects;
            switch (instrumentType.ToLower())
            {
                case "bass":
                case "guitar":
                    objects = GetGuitarObjects(directoryPath);
                    break;
                case "master":
                    // event, measure, section, tempo, timesignature
                    objects = GetMasterObjects(directoryPath);
                    break;
                case "vox":
                    // vox, voxpushphrase, voxspread
                    objects = GetVoxObjects(directoryPath);
                    break;
                default:
                    // Shouldn't really return anything
                    return new List<ZObject>();
            }

            objects.ForEach(x => instrument.TrackPaths.Add(x.FilePath));
            objects.Add(instrument);
            return objects;
        }

        public List<ZObject> ExportZObjects(HKey directory, InstrumentTuning leadGtr, InstrumentTuning rhythmGtr, InstrumentTuning bass)
        {
            List<ZObject> objects = new List<ZObject>();

            objects.AddRange(CreateInstrument(directory, "master", "", InstrumentTuning.Guitar_EStandard));
            objects.AddRange(CreateInstrument(directory, "vox", "", InstrumentTuning.Guitar_EStandard));
            
            // Guitar tracks
            objects.AddRange(CreateInstrument(directory, "guitar", "jam", rhythmGtr));
            objects.AddRange(CreateInstrument(directory, "guitar", "nov", rhythmGtr));
            objects.AddRange(CreateInstrument(directory, "guitar", "beg", rhythmGtr));
            objects.AddRange(CreateInstrument(directory, "guitar", "int", rhythmGtr));
            objects.AddRange(CreateInstrument(directory, "guitar", "rhy", rhythmGtr));
            objects.AddRange(CreateInstrument(directory, "guitar", "adv", leadGtr));

            // Bass tracks
            objects.AddRange(CreateInstrument(directory, "bass", "jam", bass));
            objects.AddRange(CreateInstrument(directory, "bass", "nov", bass));
            objects.AddRange(CreateInstrument(directory, "bass", "beg", bass));
            objects.AddRange(CreateInstrument(directory, "bass", "int", bass));
            objects.AddRange(CreateInstrument(directory, "bass", "adv", bass));

            return objects;
        }
    }
}
