﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFForever.Audio;
using BFForever.MIDI;
using BFForever.Riff;
using Newtonsoft.Json;

namespace BFForever
{
    public class SongManager
    {
        private string _jsonDirectory;
        private FEnvironment _packageManager;

        public SongManager(string packageRoot)
        {
            _packageManager = new FEnvironment();
            _packageManager.LoadPackage(packageRoot);
        }

        public SongManager(FEnvironment env)
        {
            _packageManager = env;
        }

        private string GetFilePath(string path) => (Path.IsPathRooted(path)) ? path : Path.Combine(_jsonDirectory + path);

        private InstrumentTuning UpdateTuning(InstrumentTuning tuning, bool guitar)
        {
            if (string.IsNullOrEmpty(tuning.Name))
            {
                // Return default guitar/bass tuning
                return guitar ? InstrumentTuning.Guitar_EStandard : InstrumentTuning.Bass_EStandard;
            }

            if (guitar)
            {
                switch (tuning.Name.Value.ToLower())
                {
                    default:
                    case "e standard":
                        return InstrumentTuning.Guitar_EStandard;
                    case "drop d":
                        return InstrumentTuning.Guitar_DropD;
                    case "e♭ standard":
                    case "eb standard":
                        return InstrumentTuning.Guitar_EbStandard;
                }
            }
            else // Bass
            {
                switch (tuning.Name.Value.ToLower())
                {
                    default:
                    case "e standard":
                        return InstrumentTuning.Bass_EStandard;
                }
            }
        }

        public void ImportSong(string jsonPath)
        {
            _jsonDirectory = Path.GetDirectoryName(jsonPath) + "\\";
            FusedSong fusedSong = FusedSong.Import(jsonPath);

            // Updates tuning info
            fusedSong.LeadGuitarTuning = UpdateTuning(fusedSong.LeadGuitarTuning, true);
            fusedSong.RhythmGuitarTuning = UpdateTuning(fusedSong.RhythmGuitarTuning, true);
            fusedSong.BassTuning = UpdateTuning(fusedSong.BassTuning, false);

            // Create song objects
            List<ZObject> songObjects = CreateSongObjects(fusedSong);
            Song song = songObjects.First(x => x is Song) as Song;
            
            // Create audio stem paths
            void CreateAudioPath(HKey path)
            {
                // Creates audio object
                Riff.Audio audio = new Riff.Audio(path, path.GetParentDirectory());
                audio.AudioPath = audio.FilePath + ".clt";

                // Adds audio object song objects
                songObjects.Add(audio);

                // Adds celt to index
                AddFileToIndex(audio.AudioPath, "clt", audio.FilePath.Value.Replace(".", "/") + ".clt");
            }

            CreateAudioPath(song.PreviewPath);
            CreateAudioPath(song.BackingAudioPath);
            CreateAudioPath(song.BassAudioPath);
            CreateAudioPath(song.DrumsAudioPath);
            CreateAudioPath(song.LeadGuitarAudioPath);
            CreateAudioPath(song.RhythmGuitarAudioPath);
            CreateAudioPath(song.VoxAudioPath);

            // Create video path
            Video video = new Video(song.VideoPath, song.VideoPath.GetParentDirectory());
            video.VideoPath = video.FilePath + ".bik";
            songObjects.Add(video);

            AddFileToIndex(video.VideoPath, "bik", video.FilePath.Value.Replace(".", "/") + ".bik");

            // Create texture path
            // TODO: Add texture conversion
            /*
            Riff.Texture texture = new Riff.Texture(song.TexturePath, song.TexturePath.GetParentDirectory());
            texture.TexturePath = texture.DirectoryPath + ".album.xpr";
            //songObjects.Add(texture); // Don't write to index

            AddFileToIndex(texture.FilePath, "texture", texture.DirectoryPath.Value.Replace(".", "/") + "/album.xpr");
            */

            // Adds song to catalog
            AddToCatalog(song, fusedSong.LeadGuitarTuning, fusedSong.RhythmGuitarTuning, fusedSong.BassTuning);

            string realPath = song.DirectoryPath.Value.Replace(".", "/");
            AddObjectsToIndex(songObjects, realPath + "/fused.rif");

            _packageManager.AddZObjectsAsPending(songObjects);
            _packageManager.SavePendingChanges();

            // Copies files over to song directory
            void CopyFile(string input, string output)
            {
                if (input == null || !File.Exists(input)) return;

                string outPath = Path.Combine(_packageManager.CurrentPackageDirectory + "\\songs\\", fusedSong.Identifier.Replace(".", "\\") + "\\" + output);

                // Creates directory
                if (!Directory.Exists(Path.GetDirectoryName(outPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(outPath));

                File.Copy(input, outPath, true);
            }

            //CopyFile(GetFilePath(fusedSong.TexturePath), "album.xpr");
            CopyFile(GetFilePath(fusedSong.VideoPath), "musicvideo\\video.bik");
            
            if (fusedSong.AudioPaths == null) return;

            EncodeAudio(fusedSong);
        }

        private void EncodeAudio(FusedSong song)
        {
            string mapPath = GetFilePath("hashes.json");

            // Imports existing mappings if found
            AudioHashMappings audioMap = File.Exists(mapPath)
                ? AudioHashMappings.Import(mapPath)
                : new AudioHashMappings();
            
            string GetPackageFilePath(string relativePath) =>
                Path.Combine(_packageManager.CurrentPackageDirectory + "\\songs\\", song.Identifier.Replace(".", "\\") + "\\" + relativePath);

            bool IsCeltFile(string path)
            {
                string ext = Path.GetExtension(path);
                return ext.Equals(".clt", StringComparison.CurrentCultureIgnoreCase);
            }

            HashMapping EncodeAudio(string input, string output, HashMapping oldMap)
            {
                if (!File.Exists(input))
                {
                    // Delete if no input
                    if (File.Exists(output)) File.Delete(output);

                    return oldMap;
                }
                else if (!File.Exists(output))
                {
                    // Encodes audio (Import from celt supported)
                    Celt celt = IsCeltFile(input) ? Celt.FromFile(input) : Celt.FromAudio(input);
                    celt.Export(output);

                    // Updates hashes
                    return HashMapping.CreateMapping(input, output);
                }

                HashMapping newMap = HashMapping.CreateMapping(input, output);

                if (newMap.Input != oldMap.Input || newMap.Output != oldMap.Output)
                {
                    // Encodes audio
                    Celt celt = IsCeltFile(input) ? Celt.FromFile(input) : Celt.FromAudio(input);
                    celt.Export(output);

                    // Updates output hash
                    newMap.Output = HashMapping.ComputeHashFromFile(output);
                }
                // Else means they're equal, no need to re-encode

                // Returns new hashes
                return newMap;
            }

            audioMap.Preview = EncodeAudio(GetFilePath(song.AudioPaths.Preview), GetPackageFilePath("preview\\audio.clt"), audioMap.Preview);
            audioMap.Backing = EncodeAudio(GetFilePath(song.AudioPaths.Backing), GetPackageFilePath("gamestems\\back\\audio.clt"), audioMap.Backing);
            audioMap.Bass = EncodeAudio(GetFilePath(song.AudioPaths.Bass), GetPackageFilePath("gamestems\\bass\\audio.clt"), audioMap.Bass);
            audioMap.Drums = EncodeAudio(GetFilePath(song.AudioPaths.Drums), GetPackageFilePath("gamestems\\drums\\audio.clt"), audioMap.Drums);
            audioMap.LeadGuitar = EncodeAudio(GetFilePath(song.AudioPaths.LeadGuitar), GetPackageFilePath("gamestems\\gtr1\\audio.clt"), audioMap.LeadGuitar);
            audioMap.RhythmGuitar = EncodeAudio(GetFilePath(song.AudioPaths.RhythmGuitar), GetPackageFilePath("gamestems\\gtr2\\audio.clt"), audioMap.RhythmGuitar);
            audioMap.Vox = EncodeAudio(GetFilePath(song.AudioPaths.Vox), GetPackageFilePath("gamestems\\vox\\audio.clt"), audioMap.Vox);

            audioMap.Export(mapPath);
        }

        private List<ZObject> CreateSongObjects(FusedSong input)
        {
            List<ZObject> objects = new List<ZObject>();
            HKey songDirectory = "songs." + input.Identifier;

            // Create song object
            Song song = new Song(songDirectory + ".song", songDirectory)
            {
                Title = input.Title,
                Artist = input.Artist,
                Description = input.Description,
                Album = input.Album,

                LegendTag = "Tags.Legends.NoLegend.Tag",
                EraTag = "Tags.Eras.Timeless.Tag",
                Year = input.Year,

                GuitarIntensity = input.GuitarIntensity,
                BassIntensity = input.BassIntensity,
                VoxIntensity = input.BassIntensity,

                SongLength = input.SongLength
            };
            
            // Imports note tracks
            // TODO: Check if RB import vs custom spec
            MIDIImport mid = new MIDIImport(GetFilePath(input.TabPath));
            var tracks = mid.ExportZObjects(songDirectory, input.LeadGuitarTuning, input.RhythmGuitarTuning, input.BassTuning);

            // Adds instrument file paths
            foreach (ZObject instrument in tracks.Where(x => x is Instrument))
                song.InstrumentPaths.Add(instrument.FilePath);

            objects.AddRange(tracks);
            
            // Adds audio paths
            song.PreviewPath = songDirectory + ".preview.audio";
            song.BackingAudioPath = songDirectory + ".gamestems.back.audio";
            song.BassAudioPath = songDirectory + ".gamestems.bass.audio";
            song.DrumsAudioPath = songDirectory + ".gamestems.drums.audio";
            song.LeadGuitarAudioPath = songDirectory + ".gamestems.gtr1.audio";
            song.RhythmGuitarAudioPath = songDirectory + ".gamestems.gtr2.audio";
            song.VoxAudioPath = songDirectory + ".gamestems.vox.audio";

            // Adds video path
            song.VideoPath = songDirectory + ".musicvideo.video";

            // Create texture path
            // TODO: Add texture conversion
            //song.TexturePath = songDirectory + ".texture";
            song.TexturePath = "textures.albumart.bandfuse.originalcontent.texture";

            // Adds tags
            song.Labels.Add("BFForever");
            song.GenreTags.Add("Tags.Genres.rock.Tag"); // TODO: Read from json input
            song.MetadataTags = CreateMetadataTags(song);

            objects.Add(song);
            return objects;
        }

        private List<HKey> CreateMetadataTags(Song song)
        {
            List<HKey> tags = new List<HKey>()
            {
                "Tags.Sources.BFRLDisc.Tag",
                "Tags.mediaType.song.Tag",
                "Tags.Modes.NoCountIn.Tag" // TODO: Make this optional?
            };

            // Adds genre/legend tags
            tags.AddRange(song.GenreTags);
            tags.Add(song.LegendTag);

            // Adds era tag
            if (song.Year < 1950 || song.Year >= 2020)
            {
                tags.Add("Tags.Eras.Timeless.Tag");
            }
            else
            {
                if (song.Year >= 2010) tags.Add("Tags.Eras.2010s.Tag");
                else if (song.Year >= 2000) tags.Add("Tags.Eras.2000s.Tag");
                else if (song.Year >= 1990) tags.Add("Tags.Eras.1990s.Tag");
                else if (song.Year >= 1980) tags.Add("Tags.Eras.1980s.Tag");
                else if (song.Year >= 1970) tags.Add("Tags.Eras.1970s.Tag");
                else if (song.Year >= 1960) tags.Add("Tags.Eras.1960s.Tag");
                else tags.Add("Tags.Eras.1950s.Tag");
            }

            // Adds instrument tags
            // TODO: Set dynamically based on tracks available in tabs
            tags.Add("Tags.instruments.guitar.Tag");
            tags.Add("Tags.instruments.bass.Tag");
            tags.Add("Tags.instruments.vocals.Tag");
            
            tags.Add("Tags.instruments.guitar.jam.Tag");
            tags.Add("Tags.instruments.guitar.novice.Tag");
            tags.Add("Tags.instruments.guitar.beginner.Tag");
            tags.Add("Tags.instruments.guitar.intermediate.Tag");
            tags.Add("Tags.instruments.guitar.advanced.Tag");
            tags.Add("Tags.instruments.guitar.rhythm.Tag");
            
            tags.Add("Tags.instruments.bass.jam.Tag");
            tags.Add("Tags.instruments.bass.novice.Tag");
            tags.Add("Tags.instruments.bass.beginner.Tag");
            tags.Add("Tags.instruments.bass.intermediate.Tag");
            tags.Add("Tags.instruments.bass.advanced.Tag");

            // Sets technique tags
            // TODO: Set dynamically
            tags.Add("Tags.Instruments.Guitar.Techniques.None.Tag");
            tags.Add("Tags.Instruments.Bass.Techniques.None.Tag");

            // Sets tuning tags
            // TODO: Again, set this dynamically
            tags.Add("Tags.Instruments.Guitar.Tunings.EStandard.Tag");
            tags.Add("Tags.Instruments.Bass.Tunings.EStandard.Tag");

            // Sets challenge levels
            void AddChallengeLevelTag(string insName, int intensity)
            {
                if (intensity < 0 || intensity > 5) return;
                tags.Add($"Tags.instruments.{insName}.ChallengeLevels.Level{intensity}.Tag");
            }
            
            AddChallengeLevelTag("guitar", (int)song.GuitarIntensity);
            AddChallengeLevelTag("bass", (int)song.BassIntensity);
            AddChallengeLevelTag("vocals", (int)song.VoxIntensity);

            return tags;
        }

        private void AddFileToIndex(string filePath, string fileType, string physicalPath)
        {
            _packageManager.UpdateIndexEntryAsPending(filePath, fileType, physicalPath, _packageManager.Definition.FilePath);
        }

        private void AddObjectToIndex(ZObject obj, string physicalPath)
        {
            if (obj is Riff.Texture) return; // Not written for some reason (at least for album art)

            AddFileToIndex(obj.FilePath, obj.Type, physicalPath);
        }

        private void AddObjectsToIndex(List<ZObject> objects, string physicalPath)
        {
            // Add zobjects to index2 entries
            objects.ForEach(x => AddObjectToIndex(x, physicalPath));
        }

        private void AddToCatalog(Song song, InstrumentTuning leadGtr, InstrumentTuning rhythmGtr, InstrumentTuning bass)
        {
            // Add song to catalog2 entries
            Catalog2 catalog = _packageManager["catalog2"] as Catalog2;

            Catalog2Entry entry = new Catalog2Entry()
            {
                Identifier = song.DirectoryPath + ".MediaEntry2",
                SongType = 1,

                Title = song.Title,
                Artist = song.Artist,
                Album = song.Album,
                Description = song.Description,
                LegendTag = song.LegendTag,

                SongLength = song.SongLength,
                GuitarIntensity = song.GuitarIntensity,
                BassIntensity = song.BassIntensity,
                VoxIntensity = song.VoxIntensity,

                EraTag = song.EraTag,
                Year = song.Year,
                
                LeadGuitarTuning = leadGtr,
                RhythmGuitarTuning = rhythmGtr,
                BassTuning = bass,

                Labels = song.Labels,
                SongPath = song.FilePath,
                TexturePath = song.TexturePath,
                PreviewPath = song.PreviewPath,

                MetadataTags = song.MetadataTags,
                GenreTags = song.GenreTags
            };
            
            // Removes previous entry and adds to pending change
            catalog.Entries.RemoveAll(x => x.Identifier == entry.Identifier);
            catalog.Entries.Add(entry);

            // Sorts alphabetically and ensures "ShredUs" entries are first
            catalog.Entries.Sort((x, y) =>
            {
                string a = x.Identifier, b = y.Identifier;
                if (a.StartsWith("ShredUs", StringComparison.CurrentCultureIgnoreCase) && b.StartsWith("ShredUs", StringComparison.CurrentCultureIgnoreCase))
                    return string.Compare(a, b);
                else if (a.StartsWith("ShredUs", StringComparison.CurrentCultureIgnoreCase))
                    return -1;
                else if (b.StartsWith("ShredUs", StringComparison.CurrentCultureIgnoreCase))
                    return 1;
                else
                    return string.Compare(a, b);
            });

            _packageManager.AddZObjectAsPending(catalog);
        }
    }
}
