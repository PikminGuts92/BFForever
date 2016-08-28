using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BFForever.Riff
{
    public class RiffFile
    {
        /// <summary>
        /// Constructor for Riff file
        /// </summary>
        public RiffFile()
        {
            BigEndian = true;
            Objects = new List<ZObject>();
            Tables = new List<StringTable>();
        }

        public bool BigEndian { get; set; }
        public List<ZObject> Objects { get; set; }
        public List<StringTable> Tables { get; set; }
        
        /// <summary>
        /// Imports objects from riff file.
        /// </summary>
        /// <param name="input">Riff file path</param>
        public void Import(string input)
        {
            if (!File.Exists(input)) return; // Returns if file doesn't exist

            using (FileStream fs = File.OpenRead(input))
            {
                // Imports objects from file
                ParseRiff(fs);
            }
        }

        /// <summary>
        /// Parse chunk data from riff file
        /// </summary>
        /// <param name="input">Riff stream</param>
        private void ParseRiff(Stream input)
        {
            using (AwesomeReader ar = new AwesomeReader(input))
            {
                // Checks for "RIFF" magic.
                switch (ar.ReadInt32())
                {
                    case 1380533830: // "FFIR"
                        ar.BigEndian = true;
                        break;
                    case Constant.RIFF:
                        // Reader is already little endian by default.
                        break;
                    default:
                        throw new Exception("Invalid magic. Expected \"RIFF\"");
                }

                BigEndian = ar.BigEndian; // Sets endianess

                int size = ar.ReadInt32();
                int id = ar.ReadInt32();
                ar.ReadInt32(); // Reads chunk size (Not needed since we have index chunk)

                // INDX Identifier
                if (id != Constant.INDX)
                    throw new Exception("Could not find index chunk.");

                int[] offset = new int[ar.ReadInt32()];
                ar.ReadInt32(); // Should be 4.

                for (int i = 0; i < offset.Length; i++)
                {
                    ar.ReadInt64(); // Reads index key (Not worth keeping)
                    offset[i] = ar.ReadInt32();
                    ar.ReadInt32(); // Sould be 0.
                }

                foreach (int off in offset)
                {
                    ar.BaseStream.Position = off; // Goes to chunk offset

                    // Reads chunk
                    // - If it was a string table then all values will be added to global strings
                    Chunk chunk = Chunk.FromStream(ar);
                    
                    if (chunk != null && chunk is ZObject)
                        // Adds object to riff collection
                        Objects.Add(chunk as ZObject);
                    else if (chunk != null && chunk is StringTable)
                        // Adds table to riff collection
                        Tables.Add(chunk as StringTable);
                }

            }
        }
    }
}
