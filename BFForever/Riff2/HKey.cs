using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    // Hierarchy Key - Globally unique
    public class HKey : FString
    {
        private static readonly CRC64 _global = new CRC64();

        public HKey(long key) : base(key)
        {

        }
        
        internal static long GetHash(string value) => (long)_global.Compute(value, true);
    }
}
