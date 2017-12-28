using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff2
{
    public class FEnvironment
    {
        private static Dictionary<long, StringKey> _strings = StringsInit();

        public FEnvironment()
        {

        }

        private static Dictionary<long, StringKey> StringsInit()
        {
            Dictionary<long, StringKey> strings = new Dictionary<long, StringKey>()
            {
                // String tables types
                { HKey.GetHash("stringTable@enUS"), StringKey.FromHKey("stringTable@enUS") },
                { HKey.GetHash("stringTable@jaJP"), StringKey.FromHKey("stringTable@jaJP") },
                { HKey.GetHash("stringTable@deDE"), StringKey.FromHKey("stringTable@deDE") },
                { HKey.GetHash("stringTable@itIT"), StringKey.FromHKey("stringTable@itIT") },
                { HKey.GetHash("stringTable@esES"), StringKey.FromHKey("stringTable@esES") },
                { HKey.GetHash("stringTable@frFR"), StringKey.FromHKey("stringTable@frFR") },
                // ZObject types
                { HKey.GetHash("PackageDef"), StringKey.FromHKey("PackageDef") },
                { HKey.GetHash("Index2"), StringKey.FromHKey("Index2") },
                { HKey.GetHash("Catalog2"), StringKey.FromHKey("Catalog2") }
            };

            return strings;
        }

        // String management
        internal static string GetStringValue(long key) => _strings[key][Localization];

        public static Localization Localization { get; set; } = Localization.English;
    }
}
