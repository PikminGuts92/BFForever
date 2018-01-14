using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BFForever.Riff;
using BFForever.MIDI;
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

        private string GetFilePath(string path) => (Path.IsPathRooted(path)) ? path : Path.Combine(_jsonDirectory + path);

        public void ImportSong(string jsonPath)
        {
            _jsonDirectory = Path.GetDirectoryName(jsonPath) + "\\";
            FusedSong fusedSong = FusedSong.Import(jsonPath);

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
            Riff.Texture texture = new Riff.Texture(song.TexturePath, song.TexturePath.GetParentDirectory());
            texture.TexturePath = texture.DirectoryPath + ".album.xpr";
            //songObjects.Add(texture); // Don't write to index

            AddFileToIndex(texture.FilePath, "texture", texture.DirectoryPath.Value.Replace(".", "/") + "/album.xpr");

            // Adds song to catalog
            AddToCatalog(song);

            string realPath = song.DirectoryPath.Value.Replace(".", "/");
            AddObjectsToIndex(songObjects, realPath + "/fused.rif");

            _packageManager.AddZObjectsAsPending(songObjects);
            _packageManager.SavePendingChanges();
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
                Album = input.Description,

                LegendTag = "Tags.Legends.NoLogend.Tag",
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
            mid.SongDirectory = songDirectory;
            
            // Local function for creating/adding instrument zobject
            Instrument CreateInstrument(string trackType, string difficulty = "")
            {
                string insDirectory = trackType;
                if (trackType == "guitar" || trackType == "bass")
                    insDirectory = (trackType == "guitar" ? "gtr_" : "bss_") + difficulty;

                // Creates master instrument
                Instrument instrument = new Instrument(songDirectory + "." + insDirectory + ".instrument", songDirectory + "." + trackType);
                instrument.InstrumentType = trackType == "vox" ? "vocals" : trackType;
                instrument.Difficulty = difficulty;
                instrument.Tuning = new Tuning(); // TODO: Set to E Standard
                instrument.Tuning.Name = $"Test Song ({trackType})";

                // Adds instrument tracks
                List<ZObject> instrumentTracks = mid.ExportInstrumentTracks(trackType, difficulty);
                instrument.TrackPaths = instrumentTracks.Select(x => x.FilePath).ToList();

                // Adds instrument to song
                song.InstrumentPaths.Add(instrument.FilePath);

                // Adds objects to collection
                objects.AddRange(instrumentTracks);
                objects.Add(instrument);

                return instrument;
            }

            CreateInstrument("master");
            CreateInstrument("vox");

            // Guitar tracks
            CreateInstrument("guitar", "jam");
            CreateInstrument("guitar", "nov");
            CreateInstrument("guitar", "beg");
            CreateInstrument("guitar", "int");
            CreateInstrument("guitar", "rhy");
            CreateInstrument("guitar", "adv");

            // Bass tracks
            CreateInstrument("bass", "jam");
            CreateInstrument("bass", "nov");
            CreateInstrument("bass", "beg");
            CreateInstrument("bass", "int");
            CreateInstrument("bass", "adv");

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
            song.TexturePath = songDirectory + ".texture";

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

            // TODO: Set technique tags

            // Sets tuning tags
            // TODO: Again, set this dynamically
            tags.Add("Tags.Instruments.Guitar.Tunings.EStandard.Tag");
            tags.Add("Tags.Instruments.Bass.Tunings.EStandard.Tag");

            // Sets challenge levels
            string ChallengeLevelTag(string insName, int intensity) =>
                (intensity < 0 || intensity > 5) ? "" : $"Tags.instruments.{insName}.ChallengeLevels.Level{intensity}.Tag";
            
            tags.Add(ChallengeLevelTag("guitar", (int)song.GuitarIntensity));
            tags.Add(ChallengeLevelTag("bass", (int)song.BassIntensity));
            tags.Add(ChallengeLevelTag("vocals", (int)song.VoxIntensity));

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

        private void AddToCatalog(Song song)
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

                // TODO: Change to tuning from json input
                LeadGuitarTuning = new Tuning() { Name = "Test Catalog (Lead)" },
                RhythmGuitarTuning = new Tuning() { Name = "Test Catalog (Rhythm)" },
                BassTuning = new Tuning() { Name = "Test Catalog (Bass)" },

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
