using System;
using System.Collections.Generic;
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
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: audio.clt converted_audio.wav");
                return;
            }

            try
            {
                Celt celt = Celt.FromFile(args[0]);
                celt.WriteToWavFile(args[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return;
            }

            Console.WriteLine("Successfully wrote output wav file to {0}", args[1]);
        }
    }
}
