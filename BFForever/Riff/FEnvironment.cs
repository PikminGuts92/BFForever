using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BFForever.Riff
{
    // Package Manager?
    public class FEnvironment
    {
        private List<PackageDef> _packageDefinitions;
        private Dictionary<HKey, string> _packagePaths;
        private List<ZObject> _tempObjects;
        private Index2PackageEntry _tempObjectsPackageEntry;
        private readonly List<ZObject> _pendingChanges;

        public FEnvironment()
        {
            _packageDefinitions = new List<PackageDef>();
            _packagePaths = new Dictionary<HKey, string>();
            _tempObjects = new List<ZObject>();
            _pendingChanges = new List<ZObject>();
        }

        public static FEnvironment New(string directory, string packageName, int version)
        {
            FEnvironment env = new FEnvironment();

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (string.IsNullOrEmpty(packageName))
                packageName = "bfforever";

            Index2 index = new Index2($"packages.{packageName}.index2", $"packages.{packageName}")
            {
                Version = version
            };

            PackageDef def = new PackageDef($"PackageDefs.{packageName}.PackageDef", $"PackageDefs.{packageName}")
            {
                PackageName = packageName,
                Version = version
            };

            Catalog2 cat = new Catalog2("catalog2", "catalog2");
            
            env.Index = index;
            env._packageDefinitions.Add(def);
            env._packagePaths.Add(def.FilePath, Path.GetFullPath(directory));

            env.AddZObjectAsPending(cat);
            env.UpdateIndexEntryAsPending(cat.FilePath, cat.Type, "catalog2.rif", env.Definition.FilePath);

            return env;
        }

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
                
                CreateHKeys(newPackage.PackageName);
                newPackage.Entries.ForEach(x => CreateHKeys(x));
                
                // Adds package definition to collection
                _packageDefinitions.RemoveAll(x => x.Version == newPackage.Version);
                _packageDefinitions.Add(newPackage);
            }

            ReloadIndex(fullRootPath);
        }

        private static void CreateHKeys(string name)
        {
            // Creates paths for packagedef + index2
            HKey packageFilePath = new HKey("PackageDefs." + name + ".PackageDef");
            HKey packageDirectory = packageFilePath.GetParentDirectory();
            CreateStringTablePaths(packageDirectory);

            HKey indexFilePath = new HKey("packages." + name + ".index2");
            HKey indexDirectory = indexFilePath.GetParentDirectory();
            CreateStringTablePaths(indexDirectory);
        }

        // Used mostly for debugging
        private static List<HKey> CreateStringTablePaths(HKey directory)=> Global.StringTableLocalizations.Select(x => (HKey)(directory.Value + "." + x.Value)).ToList();

        private ZObject GetZObject(Index2Entry entry)
        {
            if (entry == null || !entry.IsZObject()) return null;

            foreach (Index2PackageEntry pkEntry in entry.PackageEntries)
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
                CreateStringTablePaths(obj.DirectoryPath); // Creates string table path
                _tempObjects.Add(obj);
            }

            return true;
        }

        private void ReloadIndex(string rootPath)
        {
            string indexPath = Path.Combine(rootPath, "index2.rif");
            if (!File.Exists(indexPath)) return;

            RiffFile rif = RiffFile.FromFile(indexPath);
            Index2 newIndex = rif.Objects.FirstOrDefault(x => x is Index2) as Index2;
            if (newIndex == null) return;

            // Updates index2 object
            if (Index == null || newIndex.Version >= Index.Version)
            {
                Index = newIndex;

                // Creates directory paths for entries
                foreach (Index2Entry entry in Index.Entries)
                    entry.FilePath.GetParentDirectory();
            }
        }

        public void AddZObjectAsPending(ZObject obj) => _pendingChanges.Add(obj);

        public void AddZObjectsAsPending(List<ZObject> objects) => _pendingChanges.AddRange(objects);

        public void SavePendingChanges()
        {
            if (!PendingChanges) return;

            // Groups objects by package
            var packages = _pendingChanges.Select(obj => new
            {
                ZObject = obj,
                PackageEntry = Index.Entries.First(idx => idx.FilePath == obj.FilePath).PackageEntries.First()
            }).OrderBy(x => x.PackageEntry.ExternalFilePath).GroupBy(x => x.PackageEntry.Package);

            foreach (var package in packages)
            {
                HKey packagePath = package.Key;
                string physicalPackagePath = _packagePaths[packagePath];

                var fusedFiles = package.GroupBy(x => x.PackageEntry.ExternalFilePath);

                foreach (var fusedFile in fusedFiles)
                {
                    string fusedFilePath = Path.Combine(physicalPackagePath, fusedFile.Key);
                    RiffFile rif = null;

                    // TODO: Find a better solution (Less ugly)
                    if (File.Exists(fusedFilePath))
                    {
                        try
                        {
                            // Opens existing file to preserve zobjects
                            rif = RiffFile.FromFile(fusedFilePath);
                        }
                        catch
                        {

                        }
                        finally
                        {
                            if (rif == null) rif = new RiffFile();
                        }
                    }
                    else
                        rif = new RiffFile();

                    foreach (var zobject in fusedFile.Select(x => x.ZObject))
                    {
                        // Removes existing zobject if present
                        rif.Objects.RemoveAll(x => x.FilePath == zobject.FilePath);

                        // Adds new zobject
                        rif.Objects.Add(zobject);
                    }

                    // Saves new rif file
                    rif.WriteToFile(fusedFilePath);
                }
            }

            // Sorts index entries by file paths
            Index.Entries.Sort((x, y) => string.Compare(x.FilePath, y.FilePath, true));
            
            // Saves index for current package only
            RiffFile indexRif = new RiffFile();
            indexRif.Objects.Add(Index);
            indexRif.WriteToFile(Path.Combine(CurrentPackageDirectory, "index2.rif"));

            _pendingChanges.Clear();
        }

        public void UpdateIndexEntryAsPending(HKey filePath, HKey type, string physicalPath, HKey packageFilePath)
        {
            Index2Entry indexEntry = Index.Entries.FirstOrDefault(x => x.FilePath == filePath);

            if (indexEntry == null)
            {
                // Creates new entry
                indexEntry = new Index2Entry()
                {
                    FilePath = filePath,
                    Type = type
                };

                Index.Entries.Add(indexEntry);
            }

            Index2PackageEntry packageEntry = indexEntry.PackageEntries.FirstOrDefault(x => x.Package == packageFilePath);

            if (packageEntry == null)
            {
                // Creates new entry
                packageEntry = new Index2PackageEntry()
                {
                    Package = packageFilePath,
                    ExternalFilePath = physicalPath
                };

                // Inserts at the front
                indexEntry.PackageEntries.Insert(0, packageEntry);
            }
            else
            {
                // Updates external file path
                packageEntry.ExternalFilePath = physicalPath;
            }
        }

        public void ClearCache()
        {
            _tempObjects.Clear();
            _tempObjectsPackageEntry = null;
        }

        public PackageDef Definition => Index == null ? null : _packageDefinitions.FirstOrDefault(x => x.PackageName == Index.DirectoryPath.GetLastText());
        public Index2 Index { get; set; }

        public bool PendingChanges => _pendingChanges.Count > 0;

        public string CurrentPackageDirectory
        {
            get
            {
                PackageDef def = Definition;
                if (def == null || !_packagePaths.ContainsKey(def.FilePath)) return null;

                return _packagePaths[def.FilePath];
            }
        }
        
        public static Localization Localization { get; set; } = Localization.English;
    }
}
