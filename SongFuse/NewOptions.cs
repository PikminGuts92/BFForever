using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SongFuse
{
    [Verb("new", HelpText = "Create new song directory")]
    public class NewOptions
    {
        [Value(0, Required = true, HelpText = "Directory path to create project")]
        public string ProjectPath { get; set; }
    }
}
