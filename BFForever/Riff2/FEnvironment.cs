using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BFForever.Riff2
{
    // Package Manager?
    public class FEnvironment
    {
        private static Dictionary<long, StringKey> _globalStrings = StringsInit();
        private List<ZObject> _objects;
        private Dictionary<string, string> _packagePaths;

        public FEnvironment()
        {
            _objects = new List<ZObject>();
            _packagePaths = new Dictionary<string, string>();
        }

        public ZObject this[string indexPath] => _objects.SingleOrDefault(x => string.Compare(x.FilePath, indexPath, true) == 0);
        public ZObject this[long indexKey] => _objects.SingleOrDefault(x => x.FilePath == indexKey);
        public ZObject this[HKey index] => _objects.SingleOrDefault(x => (FString)x.FilePath == (FString)index); // TODO: Fix this code hack?

        public void LoadPackage(string rootPath)
        {
            // Loads PackageDef
            if (!Directory.Exists(rootPath)) return;
            string fullRootPath = Path.GetFullPath(rootPath);

            string[] packageRifs = Directory.GetFiles(Path.Combine(rootPath, "packagedefs"), "*.rif", SearchOption.AllDirectories);
            if (packageRifs.Length <= 0) return;

            foreach(string packageRif in packageRifs)
            {
                RiffFile rif = RiffFile.FromFile(packageRif);
                PackageDef newPackage = rif.Objects.FirstOrDefault(x => x is PackageDef) as PackageDef;
                if (newPackage == null) continue;
                
                // Updates package path
                if (_packagePaths.ContainsKey(newPackage.PackageName))
                    _packagePaths.Remove(newPackage.PackageName);
                _packagePaths.Add(newPackage.PackageName, fullRootPath);

                // Updates packagedef object
                PackageDef oldPackage = Definition;

                if (oldPackage != null && newPackage.Version >= oldPackage.Version)
                {
                    _objects.Remove(oldPackage);
                    _objects.Add(newPackage);
                }
                else if (oldPackage == null)
                {
                    _objects.Add(newPackage);
                }
            }

            ReloadIndex(fullRootPath);
        }

        private void ReloadIndex(string rootPath)
        {
            string indexPath = Path.Combine(rootPath, "index2.rif");
            if (Definition == null || !File.Exists(indexPath)) return;

            RiffFile rif = RiffFile.FromFile(indexPath);
            Index2 newIndex = rif.Objects.FirstOrDefault(x => x is Index2) as Index2;
            if (newIndex == null) return;

            // Updates index2 object
            Index2 oldIndex = Index;
            if (oldIndex != null && newIndex.Version >= oldIndex.Version)
            {
                _objects.Remove(oldIndex);
                _objects.Add(newIndex);
            }
            else if (oldIndex == null)
            {
                _objects.Add(newIndex);
            }
        }

        public PackageDef Definition => _objects.SingleOrDefault(x => x is PackageDef) as PackageDef;
        public Index2 Index => _objects.SingleOrDefault(x => x is Index2) as Index2;

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
        internal static string GetStringValue(long key) => _globalStrings[key][Localization];

        internal static StringKey FindCreate(long key)
        {
            if (_globalStrings.ContainsKey(key)) return _globalStrings[key];

            StringKey sk = new StringKey(key);
            _globalStrings.Add(key, sk);
            return sk;
        }
        
        public static Localization Localization { get; set; } = Localization.English;
    }
}
