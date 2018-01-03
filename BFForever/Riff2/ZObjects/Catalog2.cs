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
 * INT32 - Always 0
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
 * INT32 - Always 0
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
 * INT32 - Flags?
 *   [0] - Sometimes 1
 *   [1] - Sometimes 1
 *   [2] - Always 0
 *   [3] - Always 0
 * INT32 - Unknown
 * INT32 - Unknown
 * INT32 - Unknown
 * INT64 - Always 0
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
 * INT32 - String 3  |
 * INT32 - String 4  |
 * INT32 - String 5  | Same as first string data
 * INT32 - String 6  |
 * INT32 - String 7  |
 * INT32 - String 8 /
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
                ar.BaseStream.Position += 4; // Should be 0

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
                ar.BaseStream.Position += 4; // Should be 0

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

                int unknown = ar.ReadInt32();
                entry.Unknown1 = (byte)((unknown & 0xFF000000) >> 24);
                entry.Unknown2 = (byte)((unknown & 0x00FF0000) >> 16);
                entry.Unknown3 = ar.ReadInt32();
                entry.Unknown4 = ar.ReadInt32();
                entry.Unknown5 = ar.ReadInt32();
                ar.BaseStream.Position += 8; // Should be 0

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

            tuning.String7 = ar.ReadInt24() & 0xFF;
            tuning.String7Alt = ar.ReadByte();

            tuning.String8 = ar.ReadInt24() & 0xFF;
            tuning.String8Alt = ar.ReadByte();
            
            return tuning;
        }

        private void WriteTuning(AwesomeWriter aw, Tuning tuning)
        {
            // 40 bytes
            aw.Write((long)tuning.Name);
            aw.Write((int)(tuning.String1 << 8 | tuning.String1Alt));
            aw.Write((int)(tuning.String2 << 8 | tuning.String2Alt));
            aw.Write((int)(tuning.String3 << 8 | tuning.String3Alt));
            aw.Write((int)(tuning.String4 << 8 | tuning.String4Alt));
            aw.Write((int)(tuning.String5 << 8 | tuning.String5Alt));
            aw.Write((int)(tuning.String6 << 8 | tuning.String6Alt));
            aw.Write((int)(tuning.String7 << 8 | tuning.String7Alt));
            aw.Write((int)(tuning.String8 << 8 | tuning.String8Alt));
        }

        protected override void WriteObjectData(AwesomeWriter aw)
        {
            // Combines all the tags together
            List<FString> tags = new List<FString>();
            
            aw.Write((int)Entries.Count);
            aw.Write((int)4);

            long tagOffset = aw.BaseStream.Position + (Entries.Count * 280);

            // Writes entries
            foreach (Catalog2Entry entry in Entries)
            {
                aw.Write((long)entry.Identifier);
                aw.Write((int)entry.SongType);
                aw.Write((int)0);

                aw.Write((long)entry.Title);
                aw.Write((long)entry.Artist);
                aw.Write((long)entry.Album);
                aw.Write((long)entry.Description);
                aw.Write((long)entry.LegendTag);

                aw.Write((float)entry.SongLength);
                aw.Write((float)entry.GuitarIntensity);
                aw.Write((float)entry.BassIntensity);
                aw.Write((float)entry.VoxIntensity);

                aw.Write((long)entry.EraTag);
                aw.Write((int)entry.Year);
                aw.Write((int)0);

                // Tunings
                WriteTuning(aw, entry.LeadGuitarTuning);
                WriteTuning(aw, entry.RhythmGuitarTuning);
                WriteTuning(aw, entry.BassTuning);

                // Label tags
                aw.Write((int)entry.LabelTags.Count);
                aw.Write((int)(tagOffset - aw.BaseStream.Position));
                tagOffset += entry.LabelTags.Count * 8;
                tags.AddRange(entry.LabelTags);

                aw.Write((long)entry.SongPath);
                aw.Write((long)entry.TexturePath);
                aw.Write((long)entry.PreviewPath);

                // Metadata tags
                aw.Write((int)entry.MetadataTags.Count);
                aw.Write((int)(tagOffset - aw.BaseStream.Position));
                tagOffset += entry.MetadataTags.Count * 8;
                tags.AddRange(entry.MetadataTags);

                // Genre tags
                aw.Write((int)entry.GenreTags.Count);
                aw.Write((int)(tagOffset - aw.BaseStream.Position));
                tagOffset += entry.GenreTags.Count * 8;
                tags.AddRange(entry.GenreTags);
                
                aw.Write((int)(entry.Unknown1 << 24 | entry.Unknown2 << 16));
                aw.Write((int)entry.Unknown3);
                aw.Write((int)entry.Unknown4);
                aw.Write((int)entry.Unknown5);
                aw.BaseStream.Position += 8;
            }

            // Writes tags
            foreach (FString tag in tags)
                aw.Write((long)tag);
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

        public Tuning LeadGuitarTuning { get; set; }
        public Tuning RhythmGuitarTuning { get; set; }
        public Tuning BassTuning { get; set; }

        public List<HKey> LabelTags { get; set; }
        public HKey SongPath { get; set; }
        public HKey TexturePath { get; set; }
        public HKey PreviewPath { get; set; }

        public List<HKey> MetadataTags { get; set; }
        public List<HKey> GenreTags { get; set; }

        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public int Unknown4 { get; set; }
        public int Unknown5 { get; set; }

        public override string ToString() => Identifier ?? base.ToString();
    }
}
