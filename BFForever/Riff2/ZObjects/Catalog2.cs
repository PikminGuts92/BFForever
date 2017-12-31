using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * Catalog2 ZObject
 * ================
 * INT32 - Count of Entries
 * INT32 - Offset
 * Catalog2Entry[] - Catalog2 Entries
 * HKEY[] - Tags
 * 
 * Catalog2Entry (280 bytes)
 * =========================
 *  HKEY - Identifier
 * INT32 - Song Type? (1-5)
 * INT32 - Unknown (Always 0?)
 *  SKEY - Title
 *  SKEY - Artist
 *  SKEY - Album
 *  SKEY - Description
 *  HKEY - Legend Tag
 * FLOAT - Song Length
 * FLOAT - Guitar Intensity
 * FLOAT - Bass Intensity
 * FLOAT - Vocals Intensity
 *  HKEY - Era Tag
 * INT32 - Year
 * INT32 - Unknown (Always 0?)
 * Tuning - Lead Guitar  \
 * Tuning - Rhythm Guitar | Each are 40 bytes (120 bytes total)
 * Tuning - Bass         /
 * INT32 - Count of Label Tags
 * INT32 - Label Tags Offset
 *  HKEY - Song Path
 *  HKEY - Texture Path
 *  HKEY - Audio Preview Path
 * INT32 - Count of Metadata Tags
 * INT32 - Metadata Tags Offset
 * INT32 - Count of Genre Tags
 * INT32 - Genre Tags Offset
 * BYTE[24] - Zero'd Data
 * 
 * Tuning (40 bytes)
 * =================
 *  SKEY - Tuning Name
 * INT32 - String 1
 *   [0] - Always 0
 *   [1] - Always 0
 *   [2] - Pitch
 *   [3] - Alternate Pitch?
 * INT32 - String 2 \
 * INT32 - String 3  | Same as first string data
 * INT32 - String 4  |
 * INT32 - String 5  |
 * INT32 - String 6 /
 * BYTE[8] - Zero'd Data
 */

namespace BFForever.Riff2
{
    public class Catalog2 : ZObject
    {
        public Catalog2(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Entries = new List<Catalog2Entry>();
        }

        protected override int CalculateSize()
        {
            throw new NotImplementedException();
        }

        internal override void ReadData(AwesomeReader ar)
        {
            Entries.Clear();

            int entryCount = ar.ReadInt32();
            ar.BaseStream.Position += 4; // Should be 4

            while (entryCount > 0)
            {
                Catalog2Entry entry = new Catalog2Entry();

                // 280 bytes
                entry.Identifier = ar.ReadInt64();
                entry.SongType = ar.ReadInt32();
                entry.Unknown1 = ar.ReadInt32(); // Should be 0

                entry.Title = ar.ReadInt64();
                entry.Artist = ar.ReadInt64();
                entry.Album = ar.ReadInt64();
                entry.Description = ar.ReadInt64();
                entry.LegendTag = ar.ReadInt64();

                entry.SongLength = ar.ReadSingle();
                entry.GuitarIntensity = ar.ReadSingle();
                entry.BassIntensity = ar.ReadSingle();
                entry.VoxIntensity = ar.ReadSingle();

                entry.EraTag = ar.ReadInt64();
                entry.Year = ar.ReadInt32();
                entry.Unknown2 = ar.ReadInt32(); // Should be 0

                entry.LeadGuitarTuning = ReadTuning(ar);
                entry.RhythmGuitarTuning = ReadTuning(ar);
                entry.BassTuning = ReadTuning(ar);

                // Reads label tags
                int count = ar.ReadInt32();
                int offset = ar.ReadInt32();
                long previousPosition = ar.BaseStream.Position;

                ar.BaseStream.Position += offset - 4;
                for (int i = 0; i < count; i++)
                {
                    entry.LabelTags.Add(ar.ReadInt64());
                }
                ar.BaseStream.Position = previousPosition;

                entry.SongPath = ar.ReadInt64();
                entry.TexturePath = ar.ReadInt64();
                entry.PreviewPath = ar.ReadInt64();

                // Reads metadata tags
                count = ar.ReadInt32();
                offset = ar.ReadInt32();
                previousPosition = ar.BaseStream.Position;

                ar.BaseStream.Position += offset - 4;
                for (int i = 0; i < count; i++)
                {
                    entry.MetadataTags.Add(ar.ReadInt64());
                }
                ar.BaseStream.Position = previousPosition;

                // Reads genre tags
                count = ar.ReadInt32();
                offset = ar.ReadInt32();
                previousPosition = ar.BaseStream.Position;

                ar.BaseStream.Position += offset - 4;
                for (int i = 0; i < count; i++)
                {
                    entry.GenreTags.Add(ar.ReadInt64());
                }

                ar.BaseStream.Position = previousPosition;
                ar.BaseStream.Position += 24;

                Entries.Add(entry);
                entryCount--;
            }
        }

        private Tuning ReadTuning(AwesomeReader ar)
        {
            Tuning tuning = new Tuning();

            // 40 bytes
            tuning.Name = ar.ReadInt64();

            tuning.String1 = ar.ReadInt24() & 0xFF;
            tuning.String1Alt = ar.ReadByte();

            tuning.String2 = ar.ReadInt24() & 0xFF;
            tuning.String2Alt = ar.ReadByte();

            tuning.String3 = ar.ReadInt24() & 0xFF;
            tuning.String3Alt = ar.ReadByte();

            tuning.String4 = ar.ReadInt24() & 0xFF;
            tuning.String4Alt = ar.ReadByte();

            tuning.String5 = ar.ReadInt24() & 0xFF;
            tuning.String5Alt = ar.ReadByte();

            tuning.String6 = ar.ReadInt24() & 0xFF;
            tuning.String6Alt = ar.ReadByte();

            ar.BaseStream.Position += 8;
            return tuning;
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            throw new NotImplementedException();
        }

        protected override HKey Type => Hashes.ZOBJ_Catalog2;

        public List<Catalog2Entry> Entries { get; set; }
    }

    public class Catalog2Entry
    {
        public Catalog2Entry()
        {
            LabelTags = new List<HKey>();
            MetadataTags = new List<HKey>();
            GenreTags = new List<HKey>();
        }

        public HKey Identifier { get; set; }
        public int SongType { get; set; }
        public int Unknown1 { get; set; }

        public FString Title { get; set; }
        public FString Artist { get; set; }
        public FString Album { get; set; }
        public FString Description { get; set; }
        public HKey LegendTag { get; set; }

        public float SongLength { get; set; }
        public float GuitarIntensity { get; set; }
        public float BassIntensity { get; set; }
        public float VoxIntensity { get; set; }

        public HKey EraTag { get; set; }
        public int Year { get; set; }
        public int Unknown2 { get; set; }

        public Tuning LeadGuitarTuning { get; set; }
        public Tuning RhythmGuitarTuning { get; set; }
        public Tuning BassTuning { get; set; }

        public List<HKey> LabelTags { get; set; }
        public HKey SongPath { get; set; }
        public HKey TexturePath { get; set; }
        public HKey PreviewPath { get; set; }

        public List<HKey> MetadataTags { get; set; }
        public List<HKey> GenreTags { get; set; }
    }
}
