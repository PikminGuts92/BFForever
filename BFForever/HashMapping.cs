using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever
{
    public class HashMapping
    {
        private static CRC64 _crc = new CRC64();

        public ulong Input { get; set; }
        public ulong Output { get; set; }

        public static ulong ComputeHashFromFile(string path) =>
            (!File.Exists(path)) ? 0 : _crc.Compute(File.ReadAllBytes(path));

        public static HashMapping CreateMapping(string inputPath, string outputPath)
        {
            HashMapping map = new HashMapping();
            map.Input = ComputeHashFromFile(inputPath);
            map.Output = ComputeHashFromFile(outputPath);
            return map;
        }
    }
}
