using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    internal class Index
    {
        public Index()
        {
            Entries = new List<IndexEntry>();
        }

        internal void ReadData(AwesomeReader ar, FEnvironment env)
        {
            Entries.Clear();

            uint count = ar.ReadUInt32();
            ar.BaseStream.Position += 4; // Always 4

            for (int i = 0; i < count; i++)
            {
                IndexEntry entry = new IndexEntry();
                entry.FilePath = new HKey(ar.ReadInt64(), env);
                entry.Offset = ar.ReadUInt32();

                ar.BaseStream.Position += 4; // Always 0?
                Entries.Add(entry);
            }
        }

        public List<IndexEntry> Entries { get; set; }
    }

    internal class IndexEntry
    {
        public HKey FilePath { get; set; }
        public uint Offset { get; set; }
    }
}
