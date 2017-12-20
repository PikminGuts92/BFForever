using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class FEnvironment
    {
        private Dictionary<long, StringKey> _strings;

        public FEnvironment()
        {

        }

        // String management
        internal string GetStringValue(long key) => _strings[key][Localization];

        public Localization Localization { get; set; } = Localization.English;
    }
}
