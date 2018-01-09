using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Measure ZObject
 * =============
 * INT32 - Constant (4)
 * INT32 - Event Size (12)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff2
{
    public class Measure : ZObject
    {
        public Measure(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<MeasureEntry>();
        }
        
        protected override int CalculateSize()
        {
            throw new NotImplementedException();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            Events.Clear();
            ar.BaseStream.Position += 8; // Skips constants

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4;

            for (int i = 0; i < count; i++)
            {
                MeasureEntry ev = new MeasureEntry();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();
                ev.Beat = ar.ReadSingle();
                
                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)4);
            aw.Write((int)12);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (MeasureEntry ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);
                aw.Write((float)ev.Beat);
            }
        }

        protected override HKey Type => Global.ZOBJ_Measure;

        public List<MeasureEntry> Events { get; set; }
    }

    public class MeasureEntry : TimeEvent
    {
        public float Beat { get; set; } = 1.0f; // 1.0f (Up) or 2.0f (Down)
    }
}
