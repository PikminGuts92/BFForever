using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BFForever.Riff;

namespace BFForever
{
    public class SongManager
    {
        private string _workingDirectory;
        private Index2 _index2;
        private Catalog2 _catalog2;
        private PackageDef _packageDef;

        /// <summary>
        /// Loads and manages Index2, Catalog2, PackageDef objects
        /// </summary>
        /// <param name="workingDirectory"></param>
        public SongManager(string workingDirectory)
        {
            // Returns if directory doesn't exist
            if (!Directory.Exists(workingDirectory)) return;
            _workingDirectory = workingDirectory;

            string[] rifFiles = Directory.GetFiles(_workingDirectory, "*.rif", SearchOption.TopDirectoryOnly);

            // Loads objects from riff files
            foreach (string rifFile in rifFiles)
            {
                RiffFile rif = new RiffFile();
                rif.Import(rifFile);

                foreach (ZObject zobj in rif.Objects)
                {
                    if (zobj is Index2)
                        _index2 = zobj as Index2;
                    else if (zobj is Catalog2)
                        _catalog2 = zobj as Catalog2;
                    else if (zobj is PackageDef)
                        _packageDef = zobj as PackageDef;
                }
            }
        }

        /// <summary>
        /// Loads single .rif file
        /// <para>Absolute Path: c:\bandfuse_dlc\songs\adaytoremember\thedownfallofusall\fused.rif</para>
        /// <para>Relative Path: songs\adaytoremember\thedownfallofusall\fused.rif</para>
        /// </summary>
        /// <param name="rifPath">File path</param>
        /// <param name="relative">Path relative to SongManager directory?</param>
        /// <returns>Riff File</returns>
        public RiffFile LoadRiffFile(string rifPath, bool relative = true)
        {
            // Opens single rif file
            RiffFile rif = new RiffFile();

            if (relative)
                // Combines working directory + input file path
                rif.Import(Path.Combine(_workingDirectory, rifPath));
            else
                rif.Import(rifPath);

            return rif;
        }

        public List<Index2Entry> IndexEntries { get { return _index2.Entries; } }
    }
}
