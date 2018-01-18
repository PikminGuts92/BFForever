using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Processor (16 bytes)
 * ====================
 *  HKEY - Model Path
 * FLOAT - Knob 1
 * FLOAT - Knob 2
 */

namespace BFForever.Riff
{
    public struct AudioProcessor
    {
        public HKey ModelPath { get; set; }
        public float Knob1 { get; set; }
        public float Knob2 { get; set; }
    }
}
