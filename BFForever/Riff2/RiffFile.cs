using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BFForever.Riff2
{
    public class RiffFile
    {
        private const int MAGIC = 0x46464952; // "RIFF"
        private const int MAGIC_R = 0x52494646;

        public RiffFile()
        {
            BigEndian = true;
            Objects = new List<ZObject>();
        }

        public static RiffFile FromFile(string input)
        {
            if (!File.Exists(input)) return null; // Returns if file doesn't exist

            using (FileStream fs = File.OpenRead(input))
            {
                return FromStream(fs);
            }
        }

        private static RiffFile FromStream(Stream stream)
        {
            RiffFile riff = new RiffFile();
            AwesomeReader ar = new AwesomeReader(stream);

            // Checks for "RIFF" magic.
            switch (ar.ReadInt32())
            {
                case MAGIC:
                    ar.BigEndian = false;
                    break;
                case MAGIC_R:
                    ar.BigEndian = true;
                    break;
                default:
                    throw new Exception("Invalid magic. Expected \"RIFF\"");
            }

            riff.BigEndian = ar.BigEndian; // Sets endianess

            string chunkType; uint size;
            GetChunkInfo(ar, out chunkType, out size);

            if (chunkType != "INDX")
                throw new Exception("First chunk was not an Index!");

            Index index = new Index(ar);

            foreach(IndexEntry entry in index.Entries)
            {
                ar.BaseStream.Position = entry.Offset; // Jumps to offset
                GetChunkInfo(ar, out chunkType, out size);

                if (chunkType != "STbl" && chunkType != "ZOBJ") continue;
                
                // Reads header info
                HKey filePath = new HKey(ar.ReadInt64());
                HKey directoryPath = new HKey(ar.ReadInt64());
                HKey type = new HKey(ar.ReadInt64());
                ar.BaseStream.Position += 8;

                ZObject obj;

                if (chunkType == "STbl")
                {
                    Localization loc;

                    // Gets localization
                    switch(type.Value.ToLower())
                    {
                        case "stringtable@enus":
                            loc = Localization.English;
                            break;
                        case "stringtable@jajp":
                            loc = Localization.Japanese;
                            break;
                        case "stringtable@dede":
                            loc = Localization.German;
                            break;
                        case "stringtable@itit":
                            loc = Localization.Italian;
                            break;
                        case "stringtable@eses":
                            loc = Localization.Spanish;
                            break;
                        case "stringtable@frfr":
                            loc = Localization.French;
                            break;
                        default:
                            continue;
                    }

                    // Loads string table
                    obj = new StringTable(filePath, directoryPath, loc);
                    obj.ReadData(ar);
                }
                else if (chunkType == "ZOBJ")
                {
                    // Loads ZObject
                    switch(type.Value.ToLower())
                    {
                        case "packagedef":
                            obj = new PackageDef(filePath, directoryPath);
                            break;
                        case "index2":
                            obj = new Index2(filePath, directoryPath);
                            break;
                        case "catalog2":
                            obj = new Catalog2(filePath, directoryPath);
                            break;
                        default:
                            continue;
                    }

                    // Loads zobject
                    obj.ReadData(ar);
                }
            }

            return riff;
        }

        private static void GetChunkInfo(AwesomeReader ar, out string type, out uint size)
        {
            byte[] data = ar.ReadBytes(4);
            if (ar.BigEndian) Array.Reverse(data);

            type = Encoding.UTF8.GetString(data);
            size = ar.ReadUInt32();
        }

        public bool BigEndian { get; set; }
        public List<ZObject> Objects { get; set; }
    }
}
