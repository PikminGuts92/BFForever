using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public static class Constant
    {
        public const int RIFF = 1179011410;
        public const int RIFF_R = 1380533830; // Reversed
        public const int INDX = 1480871497;
        public const int STbl = 1818383443;
        public const int ZOBJ = 1245859674;

        // 3 main files needed for riffs
        public const long RIFF_Index2 = 9088292688391085503L;
        public const long RIFF_PackageDef = 2732979432304615805L;
        public const long RIFF_Catalog2 = -2491412229276672383L;

        // Song related
        public const long RIFF_Audio = 2890252404065717805L;
        public const long RIFF_Instrument = 8077402325500961663L;
        public const long RIFF_Lick = 3805151244674316159L;
        public const long RIFF_Song = 6513106166515406705L;
        public const long RIFF_Tone2 = 7416050649089508853L;
        public const long RIFF_Video = 8038962682362700638L;
        public const long RIFF_Texture = -1763250323791712995L;

        // Note entries (Also song related)
        public const long RIFF_TimeSignature = 6337357091990946694L;
        public const long RIFF_Tempo = 9036615569154774522L;
        public const long RIFF_Spread = -4367504208474669087L;
        public const long RIFF_VoxSpread = 4350643521893962914L; // Same file as 'spread'
        public const long RIFF_Section = 1392567553694744637L;
        public const long RIFF_Measure = -4712637811320572879L;
        public const long RIFF_Chord = -7185636719787030249L;
        public const long RIFF_AudioEffect = -9124590646962395076L;
        public const long RIFF_Vox = -9052798821044463638L;
        public const long RIFF_VoxPushPhrase = 3610267684391764180L;
        public const long RIFF_Tab = -2098839878622417772L;
        public const long RIFF_Whammy = -2976502283081711182L;
        public const long RIFF_Event = 4629664052592776185L;

        // Misc
        public const long RIFF_UILocStrings = 7241233698164486062L;
        public const long RIFF_TRCPromptGroup = 6454115111804180867L;
    }
}
