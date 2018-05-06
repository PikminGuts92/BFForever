using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
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
            
            SongManager man = new SongManager(args[0]);
            man.ImportSong(args[1]);

            // TODO: Implement cool interface
        }
    }
}
