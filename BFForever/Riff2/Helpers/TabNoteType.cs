using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    // Note: Bends and 'Harmonic' are deprecated it seems
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
