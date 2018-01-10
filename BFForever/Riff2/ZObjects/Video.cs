using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Video ZObject
 * =============
 * INT64 - Always 0
 *  HKEY - Video Path
 */

namespace BFForever.Riff2
{
    public class Video : ZObject
    {
        public Video(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {

        }

        protected override void AddMemberStrings(List<FString> strings) => strings.Add(VideoPath);

        internal override void ReadData(AwesomeReader ar)
        {
            ar.BaseStream.Position += 8;
            VideoPath = ar.ReadUInt64();
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.BaseStream.Position += 8;
            aw.Write((ulong)VideoPath);
        }

        public override HKey Type => Global.ZOBJ_Video;

        public HKey VideoPath { get; set; }
    }
}
