using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever.Riff;

namespace SongFuse
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null) return;

            // Opens single rif file
            RiffFile rif = new RiffFile();
            rif.Import(args[0]);

        }
    }
}
