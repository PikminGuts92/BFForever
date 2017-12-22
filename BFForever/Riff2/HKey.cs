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
        public HKey(long key, FEnvironment env) : base(key, env)
        {

        }
    }
}
