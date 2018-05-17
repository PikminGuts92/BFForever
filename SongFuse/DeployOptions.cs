using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SongFuse
{
    [Verb("deploy", HelpText = "Deploys song package to remote RIFF archive")]
    internal class DeployOptions
    {
        [Option(Required = false, MetaValue = "STRING", HelpText = "Path of remote RIFF archive")]
        public string RemotePath { get; set; }
    }
}
