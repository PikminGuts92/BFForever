using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Whammy ZObject
 * ==============
 * INT32 - Constant (12)
 * INT32 - Event Size (16)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff
{
    public class Whammy : ZObject
    {
        public Whammy(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<WhammyEntry>();
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
                WhammyEntry ev = new WhammyEntry();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();
                ar.BaseStream.Position += 8; // Unknown

                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)12);
            aw.Write((int)16);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (WhammyEntry ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);
                aw.Write((long)0);
            }
        }
        
        public override HKey Type => Global.ZOBJ_Whammy;

        public List<WhammyEntry> Events { get; set; }
    }

    public class WhammyEntry : TimeEvent
    {

    }
}
