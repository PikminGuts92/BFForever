using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.MIDI
{
    public struct TempoIndex
    {
        public long AbsoluteTime;
        public double RealTime;
        public double BPM;
        public int MicroPerQuarter => (int)(60000000 / BPM);

        public TempoIndex(long absTime, double realTime, double bpm)
        {
            AbsoluteTime = absTime;
            RealTime = realTime;
            BPM = bpm;
        }
    }
}
