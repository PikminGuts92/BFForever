﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    // Constant hierarchy keys
    internal static class Global
    {
        public static readonly HKey NULL_STRING = new HKey(0);
        // String tables types (Only english/japanese on-disc)
        public static readonly HKey STbl_English       = new HKey("stringTable@enUS");
        public static readonly HKey STbl_Japanese      = new HKey("stringTable@jaJP");
        public static readonly HKey STbl_German        = new HKey("stringTable@deDE");
        public static readonly HKey STbl_Italian       = new HKey("stringTable@itIT");
        public static readonly HKey STbl_Spanish       = new HKey("stringTable@esES");
        public static readonly HKey STbl_French        = new HKey("stringTable@frFR");
        // ZObject types
        public static readonly HKey ZOBJ_PackageDef    = new HKey("PackageDef");
        public static readonly HKey ZOBJ_Index2        = new HKey("Index2");
        public static readonly HKey ZOBJ_Catalog2      = new HKey("Catalog2");
        // Instrument related
        public static readonly HKey ZOBJ_Audio         = new HKey("Audio");
        public static readonly HKey ZOBJ_AudioEffect   = new HKey("AudioEffect");
        public static readonly HKey ZOBJ_Chord         = new HKey("Chord");
        public static readonly HKey ZOBJ_Concert       = new HKey("Concert");
        public static readonly HKey ZOBJ_Event         = new HKey("Event");
        public static readonly HKey ZOBJ_Instrument    = new HKey("Instrument");
        public static readonly HKey ZOBJ_Measure       = new HKey("Measure");
        public static readonly HKey ZOBJ_Section       = new HKey("Section");
        public static readonly HKey ZOBJ_Song          = new HKey("Song");
        public static readonly HKey ZOBJ_Spread        = new HKey("Spread");
        public static readonly HKey ZOBJ_Tab           = new HKey("Tab");
        public static readonly HKey ZOBJ_Tempo         = new HKey("Tempo");
        public static readonly HKey ZOBJ_Texture       = new HKey("Texture");
        public static readonly HKey ZOBJ_TimeSignature = new HKey("TimeSignature");
        public static readonly HKey ZOBJ_Tone2         = new HKey("Tone2");
        public static readonly HKey ZOBJ_Video         = new HKey("Video");
        public static readonly HKey ZOBJ_Vox           = new HKey("Vox");
        public static readonly HKey ZOBJ_VoxPushPhrase = new HKey("VoxPushPhrase");
        public static readonly HKey ZOBJ_VoxSpread     = new HKey("VoxSpread");
        public static readonly HKey ZOBJ_Whammy        = new HKey("Whammy");
        // Misc
        public static readonly HKey ZOBJ_HKeyList      = new HKey("HKeyList");
        public static readonly HKey ZOBJ_Tuning        = new HKey("Tuning");
        // Guitar tuning names
        public static readonly FString Tuning_Guitar_EStandard  = StringKey.UpdateValue(0x63d3970adb6267c7, "E Standard");
        public static readonly FString Tuning_Guitar_DropD      = StringKey.UpdateValue(0xa1510f525450426e, "Drop D");
        public static readonly FString Tuning_Guitar_EbStandard = StringKey.UpdateValue(0xebe8757f7bdc5717, "E♭ Standard");
        public static readonly FString Tuning_Guitar_DropDb     = StringKey.UpdateValue(0xc12903888f3f1a04, "Drop D♭");
        public static readonly FString Tuning_Guitar_DStandard  = StringKey.UpdateValue(0x3e286f538f531e3d, "D Standard");
        public static readonly FString Tuning_Guitar_DropC      = StringKey.UpdateValue(0x0846a162b1ffa955, "Drop C");
        public static readonly FString Tuning_Guitar_CStandard  = StringKey.UpdateValue(0x99adfc8a8b6b2157, "C Standard");
        public static readonly FString Tuning_Guitar_FStandard  = StringKey.UpdateValue(0x61e38b468a8e2f39, "F Standard");
        public static readonly FString Tuning_Guitar_OpenG      = StringKey.UpdateValue(0x8a45d9eea049e7e2, "Open G");
        public static readonly FString Tuning_Guitar_OpenA      = StringKey.UpdateValue(0xd75a3046483b64ad, "Open A");
        public static readonly FString Tuning_Guitar_OpenB      = StringKey.UpdateValue(0xc6b882240289a6fd, "Open B");
        public static readonly FString Tuning_Guitar_OpenC      = StringKey.UpdateValue(0xe3af68de10151a68, "Open C");
        public static readonly FString Tuning_Guitar_OpenD      = StringKey.UpdateValue(0xe57de6e097ec225d, "Open D");
        public static readonly FString Tuning_Guitar_OpenE      = StringKey.UpdateValue(0xbeb08176f8679927, "Open E");
        public static readonly FString Tuning_Guitar_OpenF      = StringKey.UpdateValue(0xaf523314b2d55b77, "Open F");
        // Bass tuning names
        public static readonly FString Tuning_Bass_EStandard    = StringKey.UpdateValue(0x29294c17e9109170, "E Standard");
        public static readonly FString Tuning_Bass_EbStandard   = StringKey.UpdateValue(0x6262ca106ee4d479, "E♭ Standard");
        public static readonly FString Tuning_Bass_DropD        = StringKey.UpdateValue(0xfa4c0ec1cbac6203, "Drop D");
        public static readonly FString Tuning_Bass_DropDb       = StringKey.UpdateValue(0x18ec2e5bf56882c6, "Drop D♭");
        public static readonly FString Tuning_Bass_DStandard    = StringKey.UpdateValue(0xb441576fce43e299, "D Standard");
        public static readonly FString Tuning_Bass_DropC        = StringKey.UpdateValue(0x45631bc8f9fbe958, "Drop C");
        public static readonly FString Tuning_Bass_FStandard    = StringKey.UpdateValue(0x67396d519a525ba1, "F Standard");

        public static readonly IReadOnlyDictionary<HKey, Type> ZObjectTypes = new Dictionary<HKey, Type>()
        {
            { ZOBJ_PackageDef, typeof(PackageDef) },
            { ZOBJ_Index2, typeof(Index2) },
            { ZOBJ_Catalog2, typeof(Catalog2) },
            { ZOBJ_Audio, typeof(Audio) },
            { ZOBJ_AudioEffect, typeof(AudioEffect) },
            { ZOBJ_Chord, typeof(Chord) },
            //{ ZOBJ_Concert, typeof(Concert) },
            { ZOBJ_Event, typeof(Event) },
            { ZOBJ_Instrument, typeof(Instrument) },
            { ZOBJ_Measure, typeof(Measure) },
            { ZOBJ_Section, typeof(Section) },
            { ZOBJ_Song, typeof(Song) },
            { ZOBJ_Spread, typeof(Spread) },
            { ZOBJ_Tab, typeof(Tab) },
            { ZOBJ_Tempo, typeof(Tempo) },
            { ZOBJ_Texture, typeof(Texture) },
            { ZOBJ_TimeSignature, typeof(TimeSignature) },
            { ZOBJ_Tone2, typeof(Tone2) },
            { ZOBJ_Video, typeof(Video) },
            { ZOBJ_Vox, typeof(Vox) },
            { ZOBJ_VoxPushPhrase, typeof(VoxPushPhrase) },
            { ZOBJ_VoxSpread, typeof(VoxSpread) },
            { ZOBJ_Whammy, typeof(Whammy) },
            { ZOBJ_HKeyList, typeof(HKeyList) },
            { ZOBJ_Tuning, typeof(Tuning) },
        };

        public static readonly HKey[] StringTableLocalizations = new HKey[]
        {
            STbl_English,
            STbl_Japanese,
            STbl_German,
            STbl_Italian,
            STbl_Spanish,
            STbl_French
        };

        public static readonly HKey[] StringTableLocalizationsOnDisc = new HKey[]
        {
            STbl_English,
            STbl_Japanese
        };

        public static readonly HKey[] StringTableLocalizationsDLC = new HKey[]
        {
            STbl_German,
            STbl_Italian,
            STbl_Spanish,
            STbl_French
        };
    }
}
