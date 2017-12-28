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
        public static long STbl_English = HKey.GetHash("stringTable@enUS");
        public static long STbl_Japanese = HKey.GetHash("stringTable@jaJP");
        public static long STbl_German = HKey.GetHash("stringTable@deDE");
        public static long STbl_Italian = HKey.GetHash("stringTable@itIT");
        public static long STbl_Spanish = HKey.GetHash("stringTable@esES");
        public static long STbl_French = HKey.GetHash("stringTable@frFR");
        // ZObject types
        public static long ZOBJ_PackageDef = HKey.GetHash("PackageDef");
        public static long ZOBJ_Index2 = HKey.GetHash("Index2");
        public static long ZOBJ_Catalog2 = HKey.GetHash("Catalog2");
    }
}
