using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Audio ZObject
 * =============
 * INT64 - Always 0
 *  HKEY - Path
 * INT64 - Always 0
 */

namespace BFForever.Riff2
{
    public class Audio : ZObject
    {
        public Audio(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {

        }
        
        protected override int CalculateSize()
        {
            throw new NotImplementedException();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            ar.BaseStream.Position += 8;
            Path = ar.ReadInt64();
            ar.BaseStream.Position += 8;
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.BaseStream.Position += 8;
            aw.Write((long)Path);
            aw.Write((long)0);
        }

        protected override HKey Type => Hashes.ZOBJ_Audio;

        public HKey Path { get; set; }
    }
}
