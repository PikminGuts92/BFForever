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
        private Dictionary<HKey, string> _packagePaths;
        private List<ZObject> _tempObjects;
        private Index2PackageEntry _tempObjectsPackageEntry;

        public FEnvironment()
        {
            _packagePaths = new Dictionary<HKey, string>();
            _tempObjects = new List<ZObject>();
        }

        public ZObject this[string indexPath] => GetZObject(Index?.Entries.SingleOrDefault(x => x.FilePath.Value == indexPath));
        public ZObject this[long indexKey] => GetZObject(Index?.Entries.SingleOrDefault(x => x.FilePath.Key == indexKey));
        public ZObject this[HKey index] => GetZObject(Index?.Entries.SingleOrDefault(x => x.FilePath == index));

        public void LoadPackage(string rootPath)
        {
            string fullRootPath = Path.GetFullPath(rootPath);
            string defDirectory = Path.Combine(rootPath, "packagedefs");

            // Loads PackageDef
            if (!Directory.Exists(fullRootPath) || !Directory.Exists(defDirectory)) return;
            
            string[] packageRifs = Directory.GetFiles(defDirectory, "*.rif", SearchOption.AllDirectories);
            if (packageRifs.Length <= 0) return;

            foreach (string packageRif in packageRifs)
            {
                RiffFile rif = RiffFile.FromFile(packageRif);
                PackageDef newPackage = rif.Objects.FirstOrDefault(x => x is PackageDef) as PackageDef;
                if (newPackage == null) continue;

                // Updates package path
                if (_packagePaths.ContainsKey(newPackage.FilePath))
                    _packagePaths.Remove(newPackage.FilePath);
                _packagePaths.Add(newPackage.FilePath, fullRootPath);

                // Updates packagedef object
                PackageDef oldPackage = Definition;
                if (oldPackage == null || newPackage.Version >= oldPackage.Version)
                    Definition = newPackage;
            }

            ReloadIndex(fullRootPath);
        }

        private ZObject GetZObject(Index2Entry entry)
        {
            if (entry == null || !entry.IsZObject()) return null;
            
            foreach(Index2PackageEntry pkEntry in entry.PackageEntries)
            {
                if (!_packagePaths.ContainsKey(pkEntry.Package)) continue;
                string filePath = Path.Combine(_packagePaths[pkEntry.Package], pkEntry.ExternalFilePath);

                // Checks if cached
                if (_tempObjectsPackageEntry != null
                    && _tempObjectsPackageEntry.ExternalFilePath == pkEntry.ExternalFilePath
                    && _tempObjectsPackageEntry.Package == pkEntry.Package)
                {
                    return _tempObjects.SingleOrDefault(x => x.FilePath == entry.FilePath);
                }

                // Checks if file exists
                if (!File.Exists(filePath)) continue;
                if (LoadRiffFile(filePath, pkEntry))
                    return _tempObjects.SingleOrDefault(x => x.FilePath == entry.FilePath);
            }
            
            return null;
        }

        private bool LoadRiffFile(string path, Index2PackageEntry packageEntry)
        {
            RiffFile rif = RiffFile.FromFile(path);
            if (rif == null) return false;

            _tempObjects.Clear();
            _tempObjectsPackageEntry = packageEntry;

            // Loads all zobjects from riff file
            foreach (ZObject obj in rif.Objects.Where(x => !(x is StringTable)))
            {
                _tempObjects.Add(obj);
            }

            return true;
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
            if (oldIndex == null || newIndex.Version >= oldIndex.Version)
                Index = newIndex;
        }

        public void ClearCache()
        {
            _tempObjects.Clear();
            _tempObjectsPackageEntry = null;
        }

        public PackageDef Definition { get; set; }
        public Index2 Index { get; set; }

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
                { HKey.GetHash("Catalog2"), StringKey.FromHKey("Catalog2") },
                // Instrument related
                { HKey.GetHash("Audio"), StringKey.FromHKey("Audio") },
                { HKey.GetHash("AudioEffect"), StringKey.FromHKey("AudioEffect") },
                { HKey.GetHash("Chord"), StringKey.FromHKey("Chord") },
                { HKey.GetHash("Concert"), StringKey.FromHKey("Concert") },
                { HKey.GetHash("Event"), StringKey.FromHKey("Event") },
                { HKey.GetHash("Instrument"), StringKey.FromHKey("Instrument") },
                { HKey.GetHash("Measure"), StringKey.FromHKey("Measure") },
                { HKey.GetHash("Section"), StringKey.FromHKey("Section") },
                { HKey.GetHash("Song"), StringKey.FromHKey("Song") },
                { HKey.GetHash("Spread"), StringKey.FromHKey("Spread") },
                { HKey.GetHash("Tab"), StringKey.FromHKey("Tab") },
                { HKey.GetHash("Tempo"), StringKey.FromHKey("Tempo") },
                { HKey.GetHash("Texture"), StringKey.FromHKey("Texture") },
                { HKey.GetHash("TimeSignature"), StringKey.FromHKey("TimeSignature") },
                { HKey.GetHash("Tone2"), StringKey.FromHKey("Tone2") },
                { HKey.GetHash("Video"), StringKey.FromHKey("Video") },
                { HKey.GetHash("Vox"), StringKey.FromHKey("Vox") },
                { HKey.GetHash("VoxPushPhrase"), StringKey.FromHKey("VoxPushPhrase") },
                { HKey.GetHash("VoxSpread"), StringKey.FromHKey("VoxSpread") },
                { HKey.GetHash("Whammy"), StringKey.FromHKey("Whammy") }
            };

            return strings;
        }

        // String management
        internal static string GetStringValue(long key) => _globalStrings.ContainsKey(key) ?  _globalStrings[key][Localization] : null;

        internal static StringKey FindCreate(long key)
        {
            if (_globalStrings.ContainsKey(key)) return _globalStrings[key];

            StringKey sk = new StringKey(key);
            _globalStrings.Add(key, sk);
            return sk;
        }

        internal static void AddStringKey(StringKey sk) => _globalStrings.Add(sk.Key, sk);
        internal static bool ContainsStringKey(long key) => _globalStrings.ContainsKey(key);
        
        public static Localization Localization { get; set; } = Localization.English;
    }
}
