using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using BFForever;
using BFForever.Audio;
using BFForever.Riff;
using BFForever.Texture;

namespace SongFuse
{
    class Program
    {
        /* Folder structure
         * ================
         * package/
         * hashes.json
         * settings.json?
         * song.json
         */

        /* Song creation steps
         * ===================
         * 1) New    - Creates project files
         * 2) Build  - Translates chart/audio files to RIFF archive format
         * 3) Deploy - Sends 'package' files to BF archive
         */

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<AudioEncoderOptions, BuildOptions, DeployOptions, NewOptions>(args)
                .WithParsed<AudioEncoderOptions>(ae => AudioEncoder(ae))
                .WithParsed<BuildOptions>(b => BuildSong(b))
                .WithParsed<DeployOptions>(b => DeploySong(b))
                .WithParsed<NewOptions>(n => NewProject(n))
                .WithNotParsed(err => NotParsed(err));
            
            return;
            if (args == null || args.Length < 2) return;
            
            SongManager man = new SongManager(args[0]);
            man.ImportSong(args[1]);

            // TODO: Implement cool interface
        }

        static void AudioEncoder(AudioEncoderOptions options)
        {
            bool IsCeltFile(string path)
            {
                string ext = Path.GetExtension(path);
                return ext.Equals(".clt", StringComparison.CurrentCultureIgnoreCase);
            }

            // Imports audio file
            Celt celt = IsCeltFile(options.InputPath) ? Celt.FromFile(options.InputPath) : Celt.FromAudio(options.InputPath);
            Console.WriteLine($"Opened {options.InputPath}");

            string o = options.OutputPath;

            // Exports audio file
            if (IsCeltFile(o))
            {
                celt.Export(o);
            }
            else
            {
                o = Path.Combine(Path.GetDirectoryName(o), Path.GetFileNameWithoutExtension(o)) + ".wav";

                // Creates directory if it doesn't exist
                if (!Directory.Exists(Path.GetDirectoryName(o)))
                    Directory.CreateDirectory(Path.GetDirectoryName(o));

                celt.WriteToWavFile(o);
            }

            Console.WriteLine($"Saved {o}");
        }

        static void BuildSong(BuildOptions options)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string packagePath = Path.Combine(currentDir, "package");
            string songPath = Path.Combine(currentDir, "song.json");

            if (!File.Exists(songPath))
            {
                Console.WriteLine($"Can't find \"song.json\" file in {currentDir}");
                return;
            }

            if (!Directory.Exists(packagePath))
            {
                // Create package directory
                Directory.CreateDirectory(packagePath);
            }

            var env = FEnvironment.New(packagePath, "bfforever", 2517);
            env.SavePendingChanges();


            SongManager man = new SongManager(env);
            man.ImportSong(songPath);

            //FusedSong song = FusedSong.Import(songPath);

            // TODO: Calculate audio file checksums to determine if re-encoding is needed
        }

        static void DeploySong(DeployOptions options)
        {

        }

        static void NewProject(NewOptions options)
        {
            // Deletes directory and creates a new one
            if (Directory.Exists(options.ProjectPath))
                Directory.Delete(options.ProjectPath, true);

            Directory.CreateDirectory(options.ProjectPath);
            Directory.CreateDirectory(Path.Combine(options.ProjectPath, "package"));

            FusedSong song = new FusedSong();
            song.Export(Path.Combine(options.ProjectPath, "song.json"));

            Console.WriteLine($"Created project in {options.ProjectPath}");
        }

        static void NotParsed(IEnumerable<Error> error)
        {

        }
    }
}
