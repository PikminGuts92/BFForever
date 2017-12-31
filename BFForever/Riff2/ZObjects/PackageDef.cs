using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * PackageDef ZObject
 * ==================
 * INT32 - Version
 * STRNG - Package Name (256 bytes)
 * INT32 - Count of Entries
 * INT32 - Offset
 * STRNG[] - Previous Package Names (256 bytes)
 */

namespace BFForever.Riff2
{
    public class PackageDef : ZObject
    {
        public PackageDef(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Entries = new List<string>();
        }

        protected override int CalculateSize()
        {
            throw new NotImplementedException();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            Entries.Clear();

            Version = ar.ReadInt32();

            long nextString = ar.BaseStream.Position + 256;
            PackageName = ar.ReadNullString();

            ar.BaseStream.Position = nextString;
            int count = ar.ReadInt32(); // # of strings

            // Offset - Always 4
            ar.BaseStream.Position += 4;
            nextString = ar.BaseStream.Position;
            
            for (int i = 0; i < count; i++)
            {
                ar.BaseStream.Position = nextString;

                // Reads string
                Entries.Add(ar.ReadNullString());
                nextString += 256;
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)Version);
            aw.WriteNullString(PackageName, 256);
            aw.Write((int)Entries.Count);
            aw.Write((int)4);
            
            foreach (string entry in Entries)
                aw.WriteNullString(entry, 256);
        }

        protected override HKey Type => Hashes.ZOBJ_PackageDef;
        
        public int Version { get; set; }
        public string PackageName { get; set; }
        public List<string> Entries { get; set; }
    }
}
