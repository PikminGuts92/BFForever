using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Pedal (24 bytes)
 * ================
 * HKEY - Pedal Path
 * BOOL - \ 
 * BOOL -  | Possible flags?
 * BOOL -  |
 * BOOL - /
 * FLOAT - Knob 1
 * FLOAT - Knob 2
 * FLOAT - Knob 3
 */

namespace BFForever.Riff
{
    public struct Pedal
    {
        public HKey ModelPath { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }
        public bool Flag3 { get; set; }
        public bool Flag4 { get; set; }
        public float Knob1 { get; set; }
        public float Knob2 { get; set; }
        public float Knob3 { get; set; }
    }
}
