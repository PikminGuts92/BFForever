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
 * INT32 - Count of Labels
 * INT32 - Labels Offset
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
 * SKEY[]/HKEY[] - Labels + Paths + Tags
 */

namespace BFForever.Riff2
{
    public class Song : ZObject
    {
        public Song(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            MetadataTags = new List<HKey>();
            GenreTags = new List<HKey>();
            Labels = new List<FString>();
            InstrumentPaths = new List<HKey>();
        }

        protected override void AddMemberStrings(List<FString> strings)
        {
            strings.Add(Title);
            strings.Add(Artist);
            strings.Add(Description);
            strings.Add(Album);
            strings.Add(TexturePath);
            strings.Add(LegendTag);
            strings.Add(EraTag);
            
            strings.AddRange(MetadataTags);
            strings.AddRange(GenreTags);
            strings.AddRange(Labels);

            strings.Add(PreviewPath);
            strings.Add(VideoPath);

            strings.AddRange(InstrumentPaths);

            strings.Add(BackingAudioPath);
            strings.Add(BassAudioPath);
            strings.Add(DrumsAudioPath);
            strings.Add(LeadGuitarAudioPath);
            strings.Add(RhythmGuitarAudioPath);
            strings.Add(VoxAudioPath);
        }

        internal override void ReadData(AwesomeReader ar)
        {
            // Clears tag/path lists
            MetadataTags.Clear();
            GenreTags.Clear();
            Labels.Clear();
            InstrumentPaths.Clear();

            // 184 bytes
            Title = ar.ReadUInt64();
            Artist = ar.ReadUInt64();
            Description = ar.ReadUInt64();
            Album = ar.ReadUInt64();
            TexturePath = ar.ReadUInt64();
            LegendTag = ar.ReadUInt64();
            EraTag = ar.ReadUInt64();

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
                MetadataTags.Add(ar.ReadUInt64());
            }
            ar.BaseStream.Position = previousPosition;

            // Reads genre tags
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                GenreTags.Add(ar.ReadUInt64());
            }
            ar.BaseStream.Position = previousPosition;

            // Reads labels
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                Labels.Add(ar.ReadUInt64());
            }
            ar.BaseStream.Position = previousPosition;

            SongLength = ar.ReadSingle();
            ar.BaseStream.Position += 4; // Should be zero

            PreviewPath = ar.ReadUInt64();
            VideoPath = ar.ReadUInt64();
            ar.BaseStream.Position += 8; // Should be zero

            // Reads instrument paths.
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                InstrumentPaths.Add(ar.ReadUInt64());
            }
            ar.BaseStream.Position = previousPosition;

            // Reads audio paths
            BackingAudioPath = ar.ReadUInt64();
            BassAudioPath = ar.ReadUInt64();
            DrumsAudioPath = ar.ReadUInt64();
            LeadGuitarAudioPath = ar.ReadUInt64();
            RhythmGuitarAudioPath = ar.ReadUInt64();
            VoxAudioPath = ar.ReadUInt64();
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            // Combines all the tags together
            List<FString> tags = new List<FString>();
            long tagOffset = aw.BaseStream.Position + 184;

            aw.Write((ulong)Title);
            aw.Write((ulong)Artist);
            aw.Write((ulong)Description);
            aw.Write((ulong)Album);
            aw.Write((ulong)TexturePath);
            aw.Write((ulong)LegendTag);
            aw.Write((ulong)EraTag);

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

            // Labels
            aw.Write((int)Labels.Count);
            aw.Write((int)(tagOffset - aw.BaseStream.Position));
            tagOffset += Labels.Count * 8;
            tags.AddRange(Labels);

            aw.Write((float)SongLength);
            aw.BaseStream.Position += 4; // Should be zero

            aw.Write((ulong)PreviewPath);
            aw.Write((ulong)VideoPath);
            aw.BaseStream.Position += 8; // Should be zero

            // Instrument paths
            aw.Write((int)InstrumentPaths.Count);
            aw.Write((int)(tagOffset - aw.BaseStream.Position));
            tagOffset += InstrumentPaths.Count * 8;
            tags.AddRange(InstrumentPaths);

            // Audio paths
            aw.Write((ulong)BackingAudioPath);
            aw.Write((ulong)BassAudioPath);
            aw.Write((ulong)DrumsAudioPath);
            aw.Write((ulong)LeadGuitarAudioPath);
            aw.Write((ulong)RhythmGuitarAudioPath);
            aw.Write((ulong)VoxAudioPath);

            // Writes tags
            foreach (FString tag in tags)
                aw.Write((ulong)tag);
        }

        public override HKey Type => Global.ZOBJ_Song;

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
        public List<FString> Labels { get; set; }
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
