using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    internal static class Hashes
    {
        // String tables types
        public static readonly long STbl_English = HKey.GetHash("stringTable@enUS");
        public static readonly long STbl_Japanese = HKey.GetHash("stringTable@jaJP");
        public static readonly long STbl_German = HKey.GetHash("stringTable@deDE");
        public static readonly long STbl_Italian = HKey.GetHash("stringTable@itIT");
        public static readonly long STbl_Spanish = HKey.GetHash("stringTable@esES");
        public static readonly long STbl_French = HKey.GetHash("stringTable@frFR");
        // ZObject types
        public static readonly long ZOBJ_PackageDef = HKey.GetHash("PackageDef");
        public static readonly long ZOBJ_Index2 = HKey.GetHash("Index2");
        public static readonly long ZOBJ_Catalog2 = HKey.GetHash("Catalog2");
        // Instrument related
        public static readonly long ZOBJ_Audio = HKey.GetHash("Audio");
        public static readonly long ZOBJ_AudioEffect = HKey.GetHash("AudioEffect");
        public static readonly long ZOBJ_Chord = HKey.GetHash("Chord");
        public static readonly long ZOBJ_Concert = HKey.GetHash("Concert");
        public static readonly long ZOBJ_Event = HKey.GetHash("Event");
        public static readonly long ZOBJ_Instrument = HKey.GetHash("Instrument");
        public static readonly long ZOBJ_Measure = HKey.GetHash("Measure");
        public static readonly long ZOBJ_Section = HKey.GetHash("Section");
        public static readonly long ZOBJ_Song = HKey.GetHash("Song");
        public static readonly long ZOBJ_Spread = HKey.GetHash("Spread");
        public static readonly long ZOBJ_Tab = HKey.GetHash("Tab");
        public static readonly long ZOBJ_Tempo = HKey.GetHash("Tempo");
        public static readonly long ZOBJ_Texture = HKey.GetHash("Texture");
        public static readonly long ZOBJ_TimeSignature = HKey.GetHash("TimeSignature");
        public static readonly long ZOBJ_Tone2 = HKey.GetHash("Tone2");
        public static readonly long ZOBJ_Video = HKey.GetHash("Video");
        public static readonly long ZOBJ_Vox = HKey.GetHash("Vox");
        public static readonly long ZOBJ_VoxPushPhrase = HKey.GetHash("VoxPushPhrase");
        public static readonly long ZOBJ_VoxSpread = HKey.GetHash("VoxSpread");
        public static readonly long ZOBJ_Whammy = HKey.GetHash("Whammy");
    }
}
