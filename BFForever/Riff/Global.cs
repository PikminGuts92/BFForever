using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public static class Global
    {
        private static CRC64 crc64 = new CRC64();

        public static long ComputeHash(string str)
        {
            if (str == null || str == "") return 0;

            str = str.ToLower().Replace('\\', '.').Replace('/', '.');

            return (long)crc64.Compute(str);
        }

    }
}
