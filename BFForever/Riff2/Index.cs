using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * Description:
 *  The index chunk lists internal path names and offsets of zobjects / string tables in a riff file.
 * 
 * INDEX CHUNK
 * ===========
 * INT32 - Count of Entries
 * INT32 - Offset
 * IndexEntry[] - Entries
 * 
 * IndexEntry (16 bytes)
 * =====================
 *  HKEY - File Path
 * INT32 - Offset
 * INT32 - Always 0
 */

namespace BFForever.Riff2
{
    internal class Index
    {
        public Index()
        {
            Entries = new List<IndexEntry>();
        }

        internal Index(AwesomeReader ar) : this()
        {
            Entries = new List<IndexEntry>();
            ReadData(ar);
        }

        internal void ReadData(AwesomeReader ar)
        {
            Entries.Clear();

            uint count = ar.ReadUInt32();
            ar.BaseStream.Position += 4; // Always 4

            for (int i = 0; i < count; i++)
            {
                IndexEntry entry = new IndexEntry();
                entry.FilePath = new HKey(ar.ReadUInt64());
                entry.Offset = ar.ReadUInt32();

                ar.BaseStream.Position += 4; // Always 0?
                Entries.Add(entry);
            }
        }

        internal void WriteData(AwesomeWriter aw)
        {
            aw.Write((int)Entries.Count);
            aw.Write((int)4);
            
            foreach(IndexEntry entry in Entries)
            {
                aw.Write((ulong)entry.FilePath);
                aw.Write((uint)entry.Offset);
                aw.BaseStream.Position += 4;
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
