using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * VoxPushPhrase ZObject
 * =====================
 * INT32 - Constant (10)
 * INT32 - Event Size (8)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff
{
    public class VoxPushPhrase : ZObject
    {
        public VoxPushPhrase(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<TimeEvent>();
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
                // Reads time entry (8 bytes)
                TimeEvent ev = new TimeEvent();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();

                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)10);
            aw.Write((int)8);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (TimeEvent ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);
            }
        }

        public override HKey Type => Global.ZOBJ_VoxPushPhrase;

        public List<TimeEvent> Events { get; set; }
    }
}
