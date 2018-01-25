using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Spread ZObject
 * ==============
 * INT32 - Constant (2)
 * INT32 - Event Size (12)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff
{
    public class Spread : ZObject
    {
        public Spread(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<SpreadEntry>();
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
                SpreadEntry ev = new SpreadEntry();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();
                ev.Value = ar.ReadSingle();

                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)2);
            aw.Write((int)12);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (SpreadEntry ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);
                aw.Write((float)ev.Value);
            }
        }
        
        public override HKey Type => Global.ZOBJ_Spread;

        public List<SpreadEntry> Events { get; set; }
    }

    public class SpreadEntry : TimeEvent
    {
        public float Value { get; set; }
    }
}
