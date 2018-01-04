using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Instrument ZObject
 * ==================
 *  HKEY - Instrument Type
 *  HKEY - Difficulty
 * INT64 - Always 0
 * Tuning - (For instrument)
 * INT32 - Count of Paths
 * INT32 - Paths Offset
 * HKEY[] - Track Paths
 */

namespace BFForever.Riff2
{
    public class Instrument : ZObject
    {
        public Instrument(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            TrackPaths = new List<HKey>();
        }
        
        protected override int CalculateSize()
        {
            throw new NotImplementedException();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            TrackPaths.Clear();
            
            InstrumentType = ar.ReadInt64();
            Difficulty = ar.ReadInt64();
            ar.BaseStream.Position += 8; // Should be zero'd

            // Reads tuning info
            Tuning = Tuning.ReadData(ar);

            // Reads track paths
            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4; // Offset - Should be 4

            for (int i = 0; i < count; i++)
            {
                TrackPaths.Add(ar.ReadInt64());
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((long)InstrumentType);
            aw.Write((long)Difficulty);
            aw.BaseStream.Position += 8; // Should be zero'd

            Tuning.WriteData(aw, Tuning);
            aw.Write((int)TrackPaths.Count);
            aw.Write((int)4);

            foreach (HKey path in TrackPaths)
                aw.Write((long)path);
        }

        protected override HKey Type => Hashes.ZOBJ_Instrument;

        public HKey InstrumentType { get; set; }
        public HKey Difficulty { get; set; }
        public Tuning Tuning { get; set; }
        public List<HKey> TrackPaths { get; set; }
    }
}
