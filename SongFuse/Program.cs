using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever;
//using BFForever.Riff;
using BFForever.Riff2;
using BFForever.Texture;

namespace SongFuse
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 2) return;

            //RiffFile rif = RiffFile.FromFile(args[0]);
            FEnvironment env = new FEnvironment();
            env.LoadPackage(args[0]);

            // Loads texture
            //XPR2 xp = XPR2.FromFile(args[0]);
            //return;
            /*
            // Loads song resources
            SongManager sm = new SongManager(args[0]);

            // Loads single rif file
            RiffFile rif = sm.LoadRiffFile(args[1]);

            if (args.Length < 3) return;

            // Exports strings to file
            StringKey.ExportToFile(args[2]);*/
        }
    }
}
