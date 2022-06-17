using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever.MIDI;
using BFForever.Riff;

namespace Rif2Mid
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: fused.rif song.mid");
                return;
            }

            string outPath = (args.Length > 1)
                ? args[1]
                : Path.Combine(Path.GetDirectoryName(args[0]), $"{Path.GetFileNameWithoutExtension(args[0])}.mid");

            try
            {
                // Open rif file
                var rifFile = RiffFile.FromFile(args[0]);

                // Convert chart and save
                var midExporter = new MIDIExport(rifFile.Objects);
                midExporter.Export(outPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return;
            }

            Console.WriteLine("Successfully wrote output mid file to {0}", outPath);
        }
    }
}
