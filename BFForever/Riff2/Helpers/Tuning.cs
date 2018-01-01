using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class Tuning
    {
        public FString Name { get; set; }
        public Pitch String1 { get; set; } // Highest pitch
        public Pitch String2 { get; set; }
        public Pitch String3 { get; set; }
        public Pitch String4 { get; set; }
        public Pitch String5 { get; set; }
        public Pitch String6 { get; set; }
        public Pitch String7 { get; set; }
        public Pitch String8 { get; set; }

        // I'm not exactly sure what this is but to be safe we'll preserve it
        public Pitch String1Alt { get; set; } // Highest pitch
        public Pitch String2Alt { get; set; }
        public Pitch String3Alt { get; set; }
        public Pitch String4Alt { get; set; }
        public Pitch String5Alt { get; set; }
        public Pitch String6Alt { get; set; }
        public Pitch String7Alt { get; set; }
        public Pitch String8Alt { get; set; }
    }
}
