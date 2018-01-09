using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Audio ZObject
 * =============
 * INT64 - Always 0
 *  HKEY - Audio Path
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
            AudioPath = ar.ReadUInt64();
            ar.BaseStream.Position += 8;
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.BaseStream.Position += 8;
            aw.Write((ulong)AudioPath);
            aw.Write((ulong)0);
        }

        protected override HKey Type => Global.ZOBJ_Audio;

        public HKey AudioPath { get; set; }
    }
}
