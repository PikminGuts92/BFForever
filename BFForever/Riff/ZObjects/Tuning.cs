using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Tuning : ZObject
    {
        public Tuning(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {

        }

        internal override void ReadData(AwesomeReader ar)
        {
            ar.BaseStream.Position += 8; // Skips zeros
            InstrumentTuning = InstrumentTuning.ReadData(ar);
        }

        protected override void AddMemberStrings(List<FString> strings)
        {
            strings.Add(InstrumentTuning.Name);
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.BaseStream.Position += 8;
            InstrumentTuning.WriteData(aw, InstrumentTuning);
        }

        public InstrumentTuning InstrumentTuning { get; set; } = InstrumentTuning.Guitar_EStandard;

        public override HKey Type => Global.ZOBJ_Tuning;
    }
}
