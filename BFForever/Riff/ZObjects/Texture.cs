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

namespace BFForever.Riff
{
    public class Texture : ZObject
    {
        public Texture(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {

        }

        protected override void AddMemberStrings(List<FString> strings) => strings.Add(TexturePath);

        internal override void ReadData(AwesomeReader ar)
        {
            ar.BaseStream.Position += 8;
            TexturePath = ar.ReadUInt64();
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.BaseStream.Position += 8;
            aw.Write((ulong)TexturePath);
        }

        public override HKey Type => Global.ZOBJ_Texture;

        public HKey TexturePath { get; set; }
    }
}
