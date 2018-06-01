using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SongFuse
{
    [Verb("texture", HelpText = "Encode/decode texture files")]
    internal class TextureOptions
    {
        [Value(0, Required = true, MetaName = "Input Path", MetaValue = "STRING", HelpText = "Path to input texture file")]
        public string InputPath { get; set; }

        [Value(1, Required = true, MetaName = "Output Path", MetaValue = "STRING", HelpText = "Path to output texture file")]
        public string OutputPath { get; set; }
    }
}
