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
        public static readonly long ZOBJ_Song = HKey.GetHash("Song");
    }
}
