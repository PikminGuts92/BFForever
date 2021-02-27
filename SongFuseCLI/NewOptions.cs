using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SongFuseCLI
{
    [Verb("new", HelpText = "Create new song directory")]
    internal class NewOptions
    {
        [Value(0, Required = true, MetaName = "Project Path", MetaValue = "STRING", HelpText = "Directory path to create project")]
        public string ProjectPath { get; set; }
    }
}
