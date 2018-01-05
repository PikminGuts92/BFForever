using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public enum TabNoteType : int
    {
        Note,
        Chukka, // Palm mute
        Slide,
        SlideUpFromNowhere,
        SlideDownToNowhere,
        Bend,
        BendRelease,
        Prebend,
        Holdbend,
        Trill,
        Harmonic,
        PinchHarmonic,
        TappedHarmonic,
        Ghost
    }
}
