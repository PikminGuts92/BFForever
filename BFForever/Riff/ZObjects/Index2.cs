using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * Index2 ZObject
 * ==============
 * INT32 - Version
 * INT32 - Count of Entries
 * INT32 - Offset
 * Index2Entry[] - Index2 Entries
 * Index2PackageEntry[] - Package Entries
 * 
 * Index2Entry (24 bytes)
 * ======================
 *  HKEY - File Path
 *  HKEY - Type
 * INT32 - Count of Package Entries
 * INT32 - Package Entries Offset
 * 
 * Index2PackageEntry (248 bytes)
 * ==============================
 *  HKEY - PackageDef Path
 * STRNG - External File Path
 */

namespace BFForever.Riff
{
    public class Index2 : ZObject
    {
        public Index2(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Entries = new List<Index2Entry>();
        }

        protected override void AddMemberStrings(List<FString> strings)
        {
            foreach(Index2Entry entry in Entries)
            {
                strings.Add(entry.FilePath);
                strings.Add(entry.Type);
                strings.AddRange(entry.PackageEntries.Select(x => x.Package));
            }
        }

        internal override void ReadData(AwesomeReader ar)
        {
            Entries.Clear();

            Version = ar.ReadInt32();
            int count = ar.ReadInt32();
            ar.BaseStream.Position += 4; // Should be 4

            for (int i = 0; i < count; i++)
            {
                Index2Entry entry = new Index2Entry();

                // 24 bytes
                entry.FilePath = ar.ReadUInt64();
                entry.Type = ar.ReadUInt64();
                int stringCount = ar.ReadInt32(); // Usually 1 for most entries

                entry.PackageEntries = new List<Index2PackageEntry>();

                // Jumps to packages/external paths entries
                long stringOffset = (ar.ReadInt32() - 4) + ar.BaseStream.Position;
                long previousPosition = ar.BaseStream.Position;

                for (int ii = 0; ii < stringCount; ii++)
                {
                    // 248 bytes
                    ar.BaseStream.Position = stringOffset;

                    // Reads string + null-terminated string
                    Index2PackageEntry pack = new Index2PackageEntry();
                    pack.Package = ar.ReadUInt64();
                    pack.ExternalFilePath = ar.ReadNullString();
                    entry.PackageEntries.Add(pack);

                    stringOffset += 248;
                }

                // Returns to next entry
                ar.BaseStream.Position = previousPosition;

                // Adds to entries
                Entries.Add(entry);
            }
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            aw.Write((int)Version);
            aw.Write((int)Entries.Count);
            aw.Write((int)4); // Should always be 4

            long nextPackageOffset = aw.BaseStream.Position + (Entries.Count * 24);

            // Writes index2 entries
            foreach(Index2Entry entry in Entries)
            {
                aw.Write((long)entry.FilePath.Key);
                aw.Write((long)entry.Type.Key);
                aw.Write((int)entry.PackageEntries.Count);
                aw.Write((int)(nextPackageOffset - aw.BaseStream.Position));

                nextPackageOffset += entry.PackageEntries.Count * 248;
            }

            // Writes package entries
            foreach(var entry in Entries.SelectMany(x => x.PackageEntries))
            {
                aw.Write((ulong)entry.Package);
                aw.WriteNullString(entry.ExternalFilePath, 240);
            }
        }

        public override HKey Type => Global.ZOBJ_Index2;
        
        public int Version { get; set; }
        public List<Index2Entry> Entries { get; set; }
    }

    public class Index2Entry
    {
        public HKey FilePath { get; set; }
        public HKey Type { get; set; }
        public List<Index2PackageEntry> PackageEntries { get; set; }

        public bool IsZObject() => (PackageEntries == null || PackageEntries.Count <= 0) ? false : PackageEntries.First().ExternalFilePath.EndsWith(".rif");

        public override string ToString() => FilePath ?? base.ToString();
    }

    public class Index2PackageEntry
    {
        public HKey Package { get; set; }
        public string ExternalFilePath { get; set; }
    }
}
