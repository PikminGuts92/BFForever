using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Index2 : ZObject
    {
        public Index2(FString idx) : base(idx)
        {
            Entries = new List<Index2Entry>();
        }

        public int Version { get; set; }
        public List<Index2Entry> Entries { get; set; }

        protected override void ImportData(AwesomeReader ar)
        {
            Version = ar.ReadInt32();
            int count = ar.ReadInt32();
            ar.ReadInt32(); // Should be 4

            for (int i = 0; i < count; i++)
            {
                Index2Entry entry = new Index2Entry();

                // 24 bytes
                entry.InternalPath = ar.ReadInt64();
                entry.Type = ar.ReadInt64();
                int stringCount = ar.ReadInt32(); // Usually 1 for most entries

                entry.PackagesEntries = new List<Index2PackageEntry>();

                // Jumps to packages/external paths entries
                long stringOffset = (ar.ReadInt32() - 4) + ar.BaseStream.Position;
                long previousPosition = ar.BaseStream.Position;

                for (int ii = 0; ii < stringCount; ii++)
                {
                    // 248 bytes
                    ar.BaseStream.Position = stringOffset;

                    // Reads string + null-terminated string
                    Index2PackageEntry pack = new Index2PackageEntry();
                    pack.Package = ar.ReadInt64();
                    pack.ExternalPath = ar.ReadNullString();
                    entry.PackagesEntries.Add(pack);

                    stringOffset += 248;
                }

                // Returns to next entry
                ar.BaseStream.Position = previousPosition;

                // Adds to entries
                Entries.Add(entry);
            }
        }
    }

    public class Index2Entry
    {
        public FString InternalPath { get; set; }    // songs.Halestorm.LoveBites.bss_adv.tab
        public FString Type { get; set; }            // tab
        public List<Index2PackageEntry> PackagesEntries { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this?.InternalPath, this?.Type);
        }
    }

    public class Index2PackageEntry
    {
        public FString Package { get; set; }     // PackageDefs.core.PackageDef
        public string ExternalPath { get; set; } // songs/halestorm/lovebites/fused.rif (max 240 bytes)

        public override string ToString()
        {
            return string.Format("{0} - {1}", this?.Package?.Value, this?.ExternalPath);
        }
    }
}
