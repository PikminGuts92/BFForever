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

            return riff;
        }

        public bool BigEndian { get; set; }
        public List<ZObject> Objects { get; set; }
    }
}
