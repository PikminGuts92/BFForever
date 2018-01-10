using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Event ZObject
 * =============
 * INT32 - Constant (13)
 * INT32 - Event Size (24)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff2
{
    public class Event : ZObject
    {
        public Event(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<EventEntry>();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            Events.Clear();
            ar.BaseStream.Position += 8; // Skips constants

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4;

            for (int i = 0; i < count; i++)
            {
                EventEntry ev = new EventEntry();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();
                ev.EventName = ar.ReadUInt64();

                ar.BaseStream.Position += 4; // Always 0
                ev.Index = ar.ReadInt32();

                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)13);
            aw.Write((int)24);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (EventEntry ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);
                aw.Write((ulong)ev.EventName);
                aw.Write((int)0);
                aw.Write((int)ev.Index);
            }
        }

        protected override HKey Type => Global.ZOBJ_Event;

        public List<EventEntry> Events { get; set; }
    }

    public class EventEntry : TextEvent
    {
        public int Index { get; set; }
    }
}
