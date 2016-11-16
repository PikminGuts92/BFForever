using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Index : Chunk
    {
        public Index() : base(null)
        {
            Entries = new List<IndexEntry>();
        }

        protected override void ImportData(AwesomeReader ar)
        {
            int count = ar.ReadInt32();
            ar.ReadInt32(); // Always 4

            // Reads all index entries
            for (int i = 0; i < count; i++)
            {
                IndexEntry entry = new IndexEntry();
                entry.IndexKey = new FString(ar.ReadInt64());
                entry.Offset = ar.ReadInt32();

                ar.ReadInt32(); // Always 0?

                Entries.Add(entry);
            }
        }

        public List<IndexEntry> Entries { get; set; }
    }

    public class IndexEntry
    {
        public FString IndexKey { get; set; }
        public int Offset { get; set; }
    }
}
