using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever;
using BFForever.Riff;
using BFForever.Texture;

namespace SongFuse
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 2) return;
            
            FEnvironment env = new FEnvironment();
            env.LoadPackage(args[0]);

            // TODO: Implement cool interface
        }
    }
}
