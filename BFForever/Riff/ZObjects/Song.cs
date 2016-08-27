using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Song : ZObject
    {
        public Song(FString idx) : base(idx)
        {
            TechniqueTags = new List<FString>();
            GenreTags = new List<FString>();
            Labels = new List<FString>();
            InstrumentPaths = new List<FString>();
        }

        public override void ImportData(AwesomeReader ar)
        {
            Title = ar.ReadInt64();
            Artist = ar.ReadInt64();
            Description = ar.ReadInt64();
            Album = ar.ReadInt64();
            TexturePath = ar.ReadInt64();
            LegendTag = ar.ReadInt64();
            EraTag = ar.ReadInt64();

            Year = ar.ReadInt32();
            GuitarIntensity = ar.ReadSingle();
            BassIntensity = ar.ReadSingle();
            VoxIntensity = ar.ReadSingle();

            // Reads technique tags.
            int count = ar.ReadInt32();
            int offset = ar.ReadInt32();
            long previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                TechniqueTags.Add(ar.ReadInt64());
            }
            ar.BaseStream.Position = previousPosition;

            // Reads genre tags.
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                GenreTags.Add(ar.ReadInt64());
            }
            ar.BaseStream.Position = previousPosition;

            // Reads labels.
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                Labels.Add(ar.ReadInt64());
            }
            ar.BaseStream.Position = previousPosition;

            SongLength = ar.ReadSingle();
            ar.ReadInt32(); // Should be zero

            PreviewPath = ar.ReadInt64();
            VideoPath = ar.ReadInt64();
            ar.ReadInt64(); // Should be zero

            // Reads instrument paths.
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                InstrumentPaths.Add(ar.ReadInt64());
            }

            // Reads audio paths
            ar.BaseStream.Position = previousPosition;

            BackingAudioPath = ar.ReadInt64();
            BassAudioPath = ar.ReadInt64();
            DrumsAudioPath = ar.ReadInt64();
            Guitar1AudioPath = ar.ReadInt64();
            Guitar2AudioPath = ar.ReadInt64();
            VoxAudioPath = ar.ReadInt64();
        }

        public FString Title { get; set; }
        public FString Artist { get; set; }
        public FString Description { get; set; }
        public FString Album { get; set; }
        public FString TexturePath { get; set; }
        public FString LegendTag { get; set; }
        public FString EraTag { get; set; }

        public int Year { get; set; }
        public float GuitarIntensity { get; set; }
        public float BassIntensity { get; set; }
        public float VoxIntensity { get; set; }

        public List<FString> TechniqueTags { get; set; }
        public List<FString> GenreTags { get; set; }
        public List<FString> Labels { get; set; }
        public List<FString> InstrumentPaths { get; set; }

        public float SongLength { get; set; }
        public FString PreviewPath { get; set; }
        public FString VideoPath { get; set; }

        public FString BackingAudioPath { get; set; }
        public FString BassAudioPath { get; set; }
        public FString DrumsAudioPath { get; set; }
        public FString Guitar1AudioPath { get; set; }
        public FString Guitar2AudioPath { get; set; }
        public FString VoxAudioPath { get; set; }
    }
}
