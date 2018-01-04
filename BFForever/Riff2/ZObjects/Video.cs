using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Video ZObject
 * =============
 * INT64 - Always 0
 *  HKEY - Path
 */

namespace BFForever.Riff2
{
    public class Video : ZObject
    {
        public Video(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
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
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.BaseStream.Position += 8;
            aw.Write((long)Path);
        }


        protected override HKey Type => Hashes.ZOBJ_Video;

        public HKey Path { get; set; }
    }
}
