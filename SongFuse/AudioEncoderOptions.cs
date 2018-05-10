using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;


namespace SongFuse
{
    [Verb("audio", HelpText = "Encode/decode celt audio files")]
    public class AudioEncoderOptions
    {
        [Value(0, Required = true,  HelpText = "Path to input audio file")]
        public string InputPath { get; set; }

        [Value(1, Required = true, HelpText = "Path to output audio file")]
        public string OutputPath { get; set; }
    }
}
