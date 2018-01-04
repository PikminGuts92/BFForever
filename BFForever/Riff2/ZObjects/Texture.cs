using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Texture ZObject
 * ===============
 * INT64 - Always 0
 *  HKEY - Texture Path
 */

namespace BFForever.Riff2
{
    public class Texture : ZObject
    {
        public Texture(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {

        }
        
        protected override int CalculateSize()
        {
            throw new NotImplementedException();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            ar.BaseStream.Position += 8;
            TexturePath = ar.ReadInt64();
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.BaseStream.Position += 8;
            aw.Write((long)TexturePath);
        }

        protected override HKey Type => Hashes.ZOBJ_Texture;

        public HKey TexturePath { get; set; }
    }
}
