using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Tone2 ZObejct
 * =============
 *  HKEY - Amplifier Path
 * FLOAT - Gain Amount   \
 * FLOAT - Bass Amount    | All range between 0-11, you know why
 * FLOAT - Mid Amount     |
 * FLOAT - Treble Amount  |
 * FLOAT - Reverb Amount  |
 * FLOAT - Volume Amount /
 *  HKEY - Reverb Type?
 * INT32 - Count of Pedal Entries
 * INT32 - Pedals Offset
 * INT32 - Count of Audio Processor Entries
 * INT32 - Audio Processors Offset
 * Pedals[]
 * AudioProcessors[]
 */

namespace BFForever.Riff
{
    public class Tone2 : ZObject
    {
        public Tone2(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Pedals = new List<Pedal>();
            Processors = new List<AudioProcessor>();
        }

        protected override void AddMemberStrings(List<FString> strings)
        {
            strings.Add(AmpPath);
            strings.Add(AmpReverb);
            strings.AddRange(Pedals.Select(x => x.ModelPath));
            strings.AddRange(Processors.Select(x => x.ModelPath));
        }

        internal override void ReadData(AwesomeReader ar)
        {
            Pedals.Clear();
            Processors.Clear();

            AmpPath = ar.ReadUInt64();
            GainLevel = ar.ReadSingle();
            BassLevel = ar.ReadSingle();
            MidLevel = ar.ReadSingle();
            TrebleLevel = ar.ReadSingle();
            ReverbLevel = ar.ReadSingle();
            VolumeLevel = ar.ReadSingle();

            AmpReverb = ar.ReadUInt64();

            int pedalCount = ar.ReadInt32();
            ar.BaseStream.Position += 4;

            int processorCount = ar.ReadInt32();
            ar.BaseStream.Position += 4;

            // Reads pedals
            for (int i = 0; i < pedalCount; i++)
            {
                Pedal pedal = new Pedal();
                pedal.ModelPath = ar.ReadUInt64();
                pedal.Flag1 = ar.ReadBoolean();
                pedal.Flag2 = ar.ReadBoolean();
                pedal.Flag3 = ar.ReadBoolean();
                pedal.Flag4 = ar.ReadBoolean();

                pedal.Knob1 = ar.ReadSingle();
                pedal.Knob2 = ar.ReadSingle();
                pedal.Knob3 = ar.ReadSingle();

                Pedals.Add(pedal);
            }

            // Reads audio processors
            for (int i = 0; i < processorCount; i++)
            {
                AudioProcessor processor = new AudioProcessor();
                processor.ModelPath = ar.ReadUInt64();

                processor.Knob1 = ar.ReadSingle();
                processor.Knob2 = ar.ReadSingle();

                Processors.Add(processor);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((ulong)AmpPath);
            aw.Write((float)GainLevel);
            aw.Write((float)BassLevel);
            aw.Write((float)MidLevel);
            aw.Write((float)TrebleLevel);
            aw.Write((float)ReverbLevel);
            aw.Write((float)VolumeLevel);

            aw.Write((ulong)AmpReverb);

            aw.Write((int)Pedals.Count);
            aw.Write((int)12); // Offset is constant
            aw.Write((int)Processors.Count);
            aw.Write((int)(Pedals.Count * 24) + 4);

            foreach(Pedal pedal in Pedals)
            {
                aw.Write((ulong)pedal.ModelPath);
                aw.Write((bool)pedal.Flag1);
                aw.Write((bool)pedal.Flag2);
                aw.Write((bool)pedal.Flag3);
                aw.Write((bool)pedal.Flag4);

                aw.Write((float)pedal.Knob1);
                aw.Write((float)pedal.Knob2);
                aw.Write((float)pedal.Knob3);
            }

            foreach (AudioProcessor processor in Processors)
            {
                aw.Write((ulong)processor.ModelPath);

                aw.Write((float)processor.Knob1);
                aw.Write((float)processor.Knob2);
            }
        }
        
        HKey AmpPath { get; set; }
        public float GainLevel { get; set; }
        public float BassLevel { get; set; }
        public float MidLevel { get; set; }
        public float TrebleLevel { get; set; }
        public float ReverbLevel { get; set; }
        public float VolumeLevel { get; set; }

        public HKey AmpReverb { get; set; }
        public List<Pedal> Pedals { get; set; }
        public List<AudioProcessor> Processors { get; set; }

        public override HKey Type => Global.ZOBJ_Tone2;
    }
}
