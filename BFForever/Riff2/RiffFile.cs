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

        public static RiffFile FromFile(string input, FEnvironment env)
        {
            if (!File.Exists(input)) return null; // Returns if file doesn't exist

            using (FileStream fs = File.OpenRead(input))
            {
                return FromStream(fs, env);
            }
        }

        private static RiffFile FromStream(Stream stream, FEnvironment env)
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

            Index index = new Index(ar, env);

            foreach(IndexEntry entry in index.Entries)
            {
                ar.BaseStream.Position = entry.Offset; // Jumps to offset
                GetChunkInfo(ar, out chunkType, out size);

                if (chunkType != "STbl" && chunkType != "ZOBJ") continue;
                
                // Reads header info
                HKey filePath = new HKey(ar.ReadInt64(), env);
                HKey directoryPath = new HKey(ar.ReadInt64(), env);
                HKey type = new HKey(ar.ReadInt64(), env);
                ar.BaseStream.Position += 8;

                ZObject obj;

                if (chunkType == "Stbl")
                {
                    obj = new StringTable(filePath, directoryPath, type);
                    obj.ReadData(ar, env);
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
