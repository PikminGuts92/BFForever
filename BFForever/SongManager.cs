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

        public SongManager()
        {

        }

        private string GetFilePath(string path) => (Path.IsPathRooted(path)) ? path : Path.Combine(_jsonDirectory + path);

        public void ImportSong(string jsonPath)
        {
            _jsonDirectory = Path.GetDirectoryName(jsonPath) + "\\";
            FusedSong fusedSong = FusedSong.Import(jsonPath);

            // Create song objects
            List<ZObject> songObjects = CreateSongObjects(fusedSong);
            Song song = songObjects.First(x => x is Song) as Song;
            //string realPath = song.DirectoryPath.Value.Replace(".", "\\");
            //AddObjectsToIndex(songObjects, realPath);

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
            songObjects.Add(texture); // Don't write to index

            AddFileToIndex(texture.FilePath, "texture", texture.DirectoryPath.Value.Replace(".", "/") + "/album.xpr");

            RiffFile fusedRif = new RiffFile();
            fusedRif.Objects.AddRange(songObjects);
        }

        private List<ZObject> CreateSongObjects(FusedSong input)
        {
            List<ZObject> objects = new List<ZObject>();
            HKey songDirectory = "songs." + input.Identifier;

            // Create song object
            Song song = new Song(songDirectory + ".song", songDirectory);
            song.Title = input.Title;
            song.Artist = input.Artist;
            song.Description = input.Description;
            song.Album = input.Description;
            song.Year = input.Year;

            song.GuitarIntensity = input.GuitarIntensity;
            song.BassIntensity = input.BassIntensity;
            song.VoxIntensity = input.BassIntensity;

            song.SongLength = input.SongLength;
            
            // Imports note tracks
            // TODO: Check if RB import vs custom spec
            MIDIImport mid = new MIDIImport(GetFilePath(input.TabPath));
            mid.SongDirectory = songDirectory;
            
            // Local function for creating/adding instrument zobject
            Instrument CreateInstrument(string trackType, string difficulty = "")
            {
                // Creates master instrument
                Instrument instrument = new Instrument(songDirectory + "." + trackType + ".instrument", songDirectory + "." + trackType);
                instrument.InstrumentType = trackType == "vox" ? "vocals" : trackType;
                instrument.Difficulty = difficulty;
                instrument.Tuning = new Tuning(); // TODO: Set to E Standard

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

            objects.Add(song);
            return objects;
        }

        private void AddFileToIndex(string filePath, string fileType, string physicalPath)
        {

        }

        private void AddObjectToIndex(ZObject obj, string physicalPath)
        {
            if (obj is Riff.Texture) return; // Not written for some reason (at least for album art)

        }

        private void AddObjectsToIndex(List<ZObject> objects, string physicalPath)
        {
            // Add zobjects to index2 entries
        }

        private void AddToCatalog(Song song)
        {
            // Add song to catalog2 entries
        }
    }
}
