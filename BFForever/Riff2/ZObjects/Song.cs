using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Song ZObject
 * ============
 *  SKEY - Title
 *  SKEY - Artist
 *  SKEY - Description
 *  SKEY - Album
 *  HKEY - Texture Path
 *  HKEY - Legend Tag
 *  HKEY - Era Tag
 * INT32 - Year
 * FLOAT - Guitar Intensity
 * FLOAT - Bass Indensity
 * FLOAT - Vocals Intensity
 * INT32 - Count of Metadata Tags
 * INT32 - Metadata Tags Offset
 * INT32 - Count of Genre Tags
 * INT32 - Genre Tags Offset
 * INT32 - Count of Label Tags
 * INT32 - Label Tags Offset
 * FLOAT - Song Length
 * INT32 - Always 0?
 *  HKEY - Preview Path
 *  HKEY - Video Path
 * INT64 - Always 0?
 * INT32 - Count of Instrument Paths
 * INT32 - Instrument Paths Offset
 *  HKEY - Backing Audio Path
 *  HKEY - Bass Audio Path
 *  HKEY - Drums Audio Path
 *  HKEY - Lead Guitar Audio Path
 *  HKEY - Rhythm Guitar Audio Path
 *  HKEY - Vocals Audio Path
 * HKEY[] - Paths + Tags
 */

namespace BFForever.Riff2
{
    public class Song : ZObject
    {
        public Song(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            MetadataTags = new List<HKey>();
            GenreTags = new List<HKey>();
            LabelTags = new List<HKey>();
            InstrumentPaths = new List<HKey>();
        }

        protected override int CalculateSize()
        {
            throw new NotImplementedException();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            // Clears tag/path lists
            MetadataTags.Clear();
            GenreTags.Clear();
            LabelTags.Clear();
            InstrumentPaths.Clear();

            // 184 bytes
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

            // Reads metadata tags
            int count = ar.ReadInt32();
            int offset = ar.ReadInt32();
            long previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                MetadataTags.Add(ar.ReadInt64());
            }
            ar.BaseStream.Position = previousPosition;

            // Reads genre tags
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                GenreTags.Add(ar.ReadInt64());
            }
            ar.BaseStream.Position = previousPosition;

            // Reads label tags
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                LabelTags.Add(ar.ReadInt64());
            }
            ar.BaseStream.Position = previousPosition;

            SongLength = ar.ReadSingle();
            ar.BaseStream.Position += 4; // Should be zero

            PreviewPath = ar.ReadInt64();
            VideoPath = ar.ReadInt64();
            ar.BaseStream.Position += 8; // Should be zero

            // Reads instrument paths.
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                InstrumentPaths.Add(ar.ReadInt64());
            }
            ar.BaseStream.Position = previousPosition;

            // Reads audio paths
            BackingAudioPath = ar.ReadInt64();
            BassAudioPath = ar.ReadInt64();
            DrumsAudioPath = ar.ReadInt64();
            LeadGuitarAudioPath = ar.ReadInt64();
            RhythmGuitarAudioPath = ar.ReadInt64();
            VoxAudioPath = ar.ReadInt64();
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            // Combines all the tags together
            List<FString> tags = new List<FString>();
            long tagOffset = aw.BaseStream.Position + 184;

            aw.Write((long)Title);
            aw.Write((long)Artist);
            aw.Write((long)Description);
            aw.Write((long)Album);
            aw.Write((long)TexturePath);
            aw.Write((long)LegendTag);
            aw.Write((long)EraTag);

            aw.Write((int)Year);
            aw.Write((float)GuitarIntensity);
            aw.Write((float)BassIntensity);
            aw.Write((float)VoxIntensity);

            // Metadata tags
            aw.Write((int)MetadataTags.Count);
            aw.Write((int)(tagOffset - aw.BaseStream.Position));
            tagOffset += MetadataTags.Count * 8;
            tags.AddRange(MetadataTags);

            // Genre tags
            aw.Write((int)GenreTags.Count);
            aw.Write((int)(tagOffset - aw.BaseStream.Position));
            tagOffset += GenreTags.Count * 8;
            tags.AddRange(GenreTags);

            // Label tags
            aw.Write((int)LabelTags.Count);
            aw.Write((int)(tagOffset - aw.BaseStream.Position));
            tagOffset += LabelTags.Count * 8;
            tags.AddRange(LabelTags);

            aw.Write((float)SongLength);
            aw.BaseStream.Position += 4; // Should be zero

            aw.Write((long)PreviewPath);
            aw.Write((long)VideoPath);
            aw.BaseStream.Position += 8; // Should be zero

            // Instrument paths
            aw.Write((int)InstrumentPaths.Count);
            aw.Write((int)(tagOffset - aw.BaseStream.Position));
            tagOffset += InstrumentPaths.Count * 8;
            tags.AddRange(InstrumentPaths);

            // Audio paths
            aw.Write((long)BackingAudioPath);
            aw.Write((long)BassAudioPath);
            aw.Write((long)DrumsAudioPath);
            aw.Write((long)LeadGuitarAudioPath);
            aw.Write((long)RhythmGuitarAudioPath);
            aw.Write((long)VoxAudioPath);

            // Writes tags
            foreach (FString tag in tags)
                aw.Write((long)tag);
        }

        protected override HKey Type => Hashes.ZOBJ_Song;

        public FString Title { get; set; }
        public FString Artist { get; set; }
        public FString Description { get; set; }
        public FString Album { get; set; }
        public HKey TexturePath { get; set; }
        public HKey LegendTag { get; set; }
        public HKey EraTag { get; set; }
        public int Year { get; set; }
        public float GuitarIntensity { get; set; }
        public float BassIntensity { get; set; }
        public float VoxIntensity { get; set; }
        public List<HKey> MetadataTags { get; set; }
        public List<HKey> GenreTags { get; set; }
        public List<HKey> LabelTags { get; set; }
        public float SongLength { get; set; }
        public HKey PreviewPath { get; set; }
        public HKey VideoPath { get; set; }
        public List<HKey> InstrumentPaths { get; set; }
        public HKey BackingAudioPath { get; set; }
        public HKey BassAudioPath { get; set; }
        public HKey DrumsAudioPath { get; set; }
        public HKey LeadGuitarAudioPath { get; set; }
        public HKey RhythmGuitarAudioPath { get; set; }
        public HKey VoxAudioPath { get; set; }
    }
}
