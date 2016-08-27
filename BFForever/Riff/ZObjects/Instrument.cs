using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Instrument : ZObject
    {
        public Instrument(FString idx) : base(idx)
        {
            RealTuning1 = 0;
            RealTuning2 = 0;
            RealTuning3 = 0;
            RealTuning4 = 0;
            RealTuning5 = 0;
            RealTuning6 = 0;

            OffsetTuning1 = 0;
            OffsetTuning2 = 0;
            OffsetTuning3 = 0;
            OffsetTuning4 = 0;
            OffsetTuning5 = 0;
            OffsetTuning6 = 0;

            EntryPaths = new List<FString>();
        }

        public override void ImportData(AwesomeReader ar)
        {
            InstrumentType = ar.ReadInt64();
            Difficulty = ar.ReadInt64();
            ar.ReadInt64(); // Should be zero'd
            TuningName = ar.ReadInt64();

            // Reads 1st string info.
            ar.ReadInt16();
            RealTuning1 = ar.ReadByte();
            OffsetTuning1 = ar.ReadByte();

            // Reads 2nd string info.
            ar.ReadInt16();
            RealTuning2 = ar.ReadByte();
            OffsetTuning2 = ar.ReadByte();

            // Reads 3rd string info.
            ar.ReadInt16();
            RealTuning3 = ar.ReadByte();
            OffsetTuning3 = ar.ReadByte();

            // Reads 4th string info.
            ar.ReadInt16();
            RealTuning4 = ar.ReadByte();
            OffsetTuning4 = ar.ReadByte();

            // Reads 5th string info.
            ar.ReadInt16();
            RealTuning5 = ar.ReadByte();
            OffsetTuning5 = ar.ReadByte();

            // Reads 6th string info.
            ar.ReadInt16();
            RealTuning6 = ar.ReadByte();
            OffsetTuning6 = ar.ReadByte();

            ar.ReadInt64(); // Should be zero'd
            int count = ar.ReadInt32();
            int offset = ar.ReadInt32();
            long previousPosition = ar.BaseStream.Position;

            // Reads entry paths.
            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                EntryPaths.Add(ar.ReadInt64());
            }
        }

        public FString InstrumentType { get; set; }
        public FString Difficulty { get; set; }
        public FString TuningName { get; set; }

        public byte RealTuning1 { get; set; }
        public byte RealTuning2 { get; set; }
        public byte RealTuning3 { get; set; }
        public byte RealTuning4 { get; set; }
        public byte RealTuning5 { get; set; }
        public byte RealTuning6 { get; set; }

        public byte OffsetTuning1 { get; set; }
        public byte OffsetTuning2 { get; set; }
        public byte OffsetTuning3 { get; set; }
        public byte OffsetTuning4 { get; set; }
        public byte OffsetTuning5 { get; set; }
        public byte OffsetTuning6 { get; set; }

        public List<FString> EntryPaths { get; set; }
    }
}
