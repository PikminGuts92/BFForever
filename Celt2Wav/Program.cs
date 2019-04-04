using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever.Audio;

namespace Celt2Wav
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: audio.clt converted_audio.wav");
                return;
            }

            string outPath = (args.Length > 1)
                ? args[1]
                : Path.Combine(Path.GetDirectoryName(args[0]), $"{Path.GetFileNameWithoutExtension(args[0])}.wav");
            
            try
            {
                Celt celt = Celt.FromFile(args[0]);
                celt.WriteToWavFile(outPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return;
            }

            Console.WriteLine("Successfully wrote output wav file to {0}", outPath);
        }
    }
}
