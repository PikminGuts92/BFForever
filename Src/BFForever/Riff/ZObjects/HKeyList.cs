using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * HKeyList
 * ========
 *  INT32 - Count of Entries
 *  INT32 - Entries Offset
 * HKEY[] - HKeys
 */

namespace BFForever.Riff
{
    public class HKeyList : ZObject
    {
        public HKeyList(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Entries = new List<HKey>();
        }
        
        protected override void AddMemberStrings(List<FString> strings)
        {
            strings.AddRange(Entries);
        }

        internal override void ReadData(AwesomeReader ar)
        {
            Entries.Clear();

            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4; // Offset is always 4

            // Reads entries
            for (int i = 0; i < count; i++)
                Entries.Add(ar.ReadUInt64());
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)Entries.Count);
            aw.Write((int)4);

            Entries.ForEach(x => aw.Write((ulong)x));
        }

        public List<HKey> Entries { get; set; }
        
        public override HKey Type => Global.ZOBJ_HKeyList;
    }
}
