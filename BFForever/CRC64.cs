using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// CRC64 code was taken from here: https://gist.github.com/rickbutton/ef7b8b48888837c283d6
// Credit goes to 'rickbutton'

namespace BFForever
{
    public class CRC64
    {
        private ulong[] _table;

        /// <summary>
        /// CRC64 Constructor - ECMA polynomial
        /// </summary>
        public CRC64() : this(0x42F0E1EBA9EA3693) // ECMA Standard
        {

        }

        /// <summary>
        /// CRC64 Constructor
        /// </summary>
        /// <param name="poly">User-defined polynomial</param>
        public CRC64(ulong poly)
        {
            _table = GenStdCrcTable(poly);
        }

        private ulong CmTab(int index, ulong poly)
        {
            ulong retval = (ulong)index;
            ulong topbit = (ulong)1L << (64 - 1);
            ulong mask = 0xffffffffffffffffUL;

            retval <<= (64 - 8);
            for (int i = 0; i < 8; i++)
            {
                if ((retval & topbit) != 0)
                    retval = (retval << 1) ^ poly;
                else
                    retval <<= 1;
            }
            return retval & mask;
        }

        /// <summary>
        /// Generates table
        /// </summary>
        /// <param name="poly"></param>
        /// <returns></returns>
        private ulong[] GenStdCrcTable(ulong poly)
        {
            ulong[] table = new ulong[256];
            for (var i = 0; i < 256; i++)
                table[i] = CmTab(i, poly);
            return table;
        }

        private ulong TableValue(ulong[] table, byte b, ulong crc)
        {
            return table[((crc >> 56) ^ b) & 0xffUL] ^ (crc << 8);
        }
        
        /// <summary>
        /// Computes 64-bit CRC hash from bytes
        /// </summary>
        /// <param name="bytes">Input bytes</param>
        /// <returns>Hash</returns>
        public ulong Compute(byte[] bytes)
        {
            ulong current = Initial;
            
            for (var i = 0; i < bytes.Length; i++)
            {
                current = TableValue(_table, bytes[i], current);
            }

            return current ^ Final;
        }

        /// <summary>
        /// Computes 64-bit CRC hash from string
        /// </summary>
        /// <param name="text">Input string</param>
        /// <returns>Hash</returns>
        public ulong Compute(string text, bool ignoreCase = false)
        {
            if (ignoreCase)
                // Ex: songs.Halestorm.LoveBites -> songs.halestorm.lovebites
                text = text.ToLowerInvariant().Replace("\\", ".").Replace("/", ".");

            return Compute(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// Initial XOR'd value
        /// </summary>
        public ulong Initial { get; set; } = 0xFFFFFFFFFFFFFFFF;

        /// <summary>
        /// Final XOR'd value
        /// </summary>
        public ulong Final { get; set; } = 0xFFFFFFFFFFFFFFFF;
    }
}
