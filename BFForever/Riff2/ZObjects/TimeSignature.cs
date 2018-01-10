using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * TimeSignature ZObject
 * =====================
 * INT32 - Constant (0)
 * INT32 - Event Size (16)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff2
{
    public class TimeSignature : ZObject
    {
        public TimeSignature(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<TimeSignatureEntry>();
        }

        protected override void AddMemberStrings(List<FString> strings) { }

        internal override void ReadData(AwesomeReader ar)
        {
            Events.Clear();
            ar.BaseStream.Position += 8; // Skips constants

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4;

            for (int i = 0; i < count; i++)
            {
                TimeSignatureEntry ev = new TimeSignatureEntry();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();
                ev.Beat = ar.ReadInt32();
                ev.Measure = ar.ReadInt32();

                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)0);
            aw.Write((int)16);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (TimeSignatureEntry ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);
                aw.Write((int)ev.Beat);
                aw.Write((int)ev.Measure);
            }
        }

        public override HKey Type => Global.ZOBJ_TimeSignature;

        public List<TimeSignatureEntry> Events { get; set; }
    }

    public class TimeSignatureEntry : TimeEvent
    {
        public int Beat { get; set; }
        public int Measure { get; set; }
    }
}
