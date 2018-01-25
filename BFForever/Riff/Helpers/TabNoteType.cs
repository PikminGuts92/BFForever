using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    // Note: Bends are deprecated
    public enum TabNoteType : int
    {
        Note,
        Chukka, // Left-hand mute
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
