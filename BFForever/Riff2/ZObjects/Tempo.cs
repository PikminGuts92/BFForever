using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Tempo ZObject
 * =============
 * INT32 - Constant (1)
 * INT32 - Event Size (12)
 * INT32 - Count of Events
 * INT32 - Events Offset
 * Events[]
 */

namespace BFForever.Riff2
{
    public class Tempo : ZObject
    {
        public Tempo(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Events = new List<TempoEntry>();
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
                TempoEntry ev = new TempoEntry();
                ev.Start = ar.ReadSingle();
                ev.End = ar.ReadSingle();
                ev.BPM = ar.ReadSingle();

                Events.Add(ev);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)1);
            aw.Write((int)12);
            aw.Write((int)Events.Count);
            aw.Write((int)4);

            foreach (TempoEntry ev in Events)
            {
                aw.Write((float)ev.Start);
                aw.Write((float)ev.End);
                aw.Write((float)ev.BPM);
            }
        }
        
        protected override HKey Type => Hashes.ZOBJ_Tempo;

        public List<TempoEntry> Events { get; set; }
    }

    public class TempoEntry : TimeEvent
    {
        public float BPM { get; set; }
    }
}
