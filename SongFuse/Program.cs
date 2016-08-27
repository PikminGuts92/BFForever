using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever;
using BFForever.Riff;

namespace SongFuse
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1) return;

            SongManager sm = new SongManager(args[0]);

        }
    }
}
