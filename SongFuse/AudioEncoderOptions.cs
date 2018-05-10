using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;


namespace SongFuse
{
    [Verb("audio", HelpText = "Encode/decode celt audio files")]
    internal class AudioEncoderOptions
    {
        [Value(0, Required = true, MetaName = "Input Path", MetaValue = "STRING", HelpText = "Path to input audio file")]
        public string InputPath { get; set; }

        [Value(1, Required = true, MetaName = "Output Path", MetaValue = "STRING", HelpText = "Path to output audio file")]
        public string OutputPath { get; set; }
    }
}
