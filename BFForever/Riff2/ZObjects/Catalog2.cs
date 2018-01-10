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
 * SKEY[]/HKEY[] - Labels + Tags
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
 * INT32 - Count of Labels
 * INT32 - Labels Offset
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
 */

namespace BFForever.Riff2
{
    public class Catalog2 : ZObject
    {
        public Catalog2(HKey filePath, HKey directoryPath) : base(filePath, directoryPath)
        {
            Entries = new List<Catalog2Entry>();
        }

        protected override void AddMemberStrings(List<FString> strings)
        {
            foreach(Catalog2Entry entry in Entries)
            {
                strings.Add(entry.Identifier);

                strings.Add(entry.Title);
                strings.Add(entry.Artist);
                strings.Add(entry.Album);
                strings.Add(entry.Description);
                strings.Add(entry.LegendTag);
                
                strings.Add(entry.EraTag);
                strings.Add(entry.LeadGuitarTuning.Name);
                strings.Add(entry.RhythmGuitarTuning.Name);
                strings.Add(entry.BassTuning.Name);

                strings.AddRange(entry.Labels);
                strings.Add(entry.SongPath);
                strings.Add(entry.TexturePath);
                strings.Add(entry.PreviewPath);

                strings.AddRange(entry.MetadataTags);
                strings.AddRange(entry.GenreTags);
            }
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
                entry.Identifier = ar.ReadUInt64();
                entry.SongType = ar.ReadInt32();
                ar.BaseStream.Position += 4; // Should be 0

                entry.Title = ar.ReadUInt64();
                entry.Artist = ar.ReadUInt64();
                entry.Album = ar.ReadUInt64();
                entry.Description = ar.ReadUInt64();
                entry.LegendTag = ar.ReadUInt64();

                entry.SongLength = ar.ReadSingle();
                entry.GuitarIntensity = ar.ReadSingle();
                entry.BassIntensity = ar.ReadSingle();
                entry.VoxIntensity = ar.ReadSingle();

                entry.EraTag = ar.ReadUInt64();
                entry.Year = ar.ReadInt32();
                ar.BaseStream.Position += 4; // Should be 0

                entry.LeadGuitarTuning = Tuning.ReadData(ar);
                entry.RhythmGuitarTuning = Tuning.ReadData(ar);
                entry.BassTuning = Tuning.ReadData(ar);

                // Reads labels
                int count = ar.ReadInt32();
                int offset = ar.ReadInt32();
                long previousPosition = ar.BaseStream.Position;

                ar.BaseStream.Position += offset - 4;
                for (int i = 0; i < count; i++)
                {
                    entry.Labels.Add(ar.ReadUInt64());
                }
                ar.BaseStream.Position = previousPosition;

                entry.SongPath = ar.ReadUInt64();
                entry.TexturePath = ar.ReadUInt64();
                entry.PreviewPath = ar.ReadUInt64();

                // Reads metadata tags
                count = ar.ReadInt32();
                offset = ar.ReadInt32();
                previousPosition = ar.BaseStream.Position;

                ar.BaseStream.Position += offset - 4;
                for (int i = 0; i < count; i++)
                {
                    entry.MetadataTags.Add(ar.ReadUInt64());
                }
                ar.BaseStream.Position = previousPosition;

                // Reads genre tags
                count = ar.ReadInt32();
                offset = ar.ReadInt32();
                previousPosition = ar.BaseStream.Position;

                ar.BaseStream.Position += offset - 4;
                for (int i = 0; i < count; i++)
                {
                    entry.GenreTags.Add(ar.ReadUInt64());
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
                aw.Write((ulong)entry.Identifier);
                aw.Write((int)entry.SongType);
                aw.Write((int)0);

                aw.Write((ulong)entry.Title);
                aw.Write((ulong)entry.Artist);
                aw.Write((ulong)entry.Album);
                aw.Write((ulong)entry.Description);
                aw.Write((ulong)entry.LegendTag);

                aw.Write((float)entry.SongLength);
                aw.Write((float)entry.GuitarIntensity);
                aw.Write((float)entry.BassIntensity);
                aw.Write((float)entry.VoxIntensity);

                aw.Write((ulong)entry.EraTag);
                aw.Write((int)entry.Year);
                aw.Write((int)0);

                // Tunings
                Tuning.WriteData(aw, entry.LeadGuitarTuning);
                Tuning.WriteData(aw, entry.RhythmGuitarTuning);
                Tuning.WriteData(aw, entry.BassTuning);

                // Labels
                aw.Write((int)entry.Labels.Count);
                aw.Write((int)(tagOffset - aw.BaseStream.Position));
                tagOffset += entry.Labels.Count * 8;
                tags.AddRange(entry.Labels);

                aw.Write((ulong)entry.SongPath);
                aw.Write((ulong)entry.TexturePath);
                aw.Write((ulong)entry.PreviewPath);

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
                aw.Write((ulong)tag);
        }

        public override HKey Type => Global.ZOBJ_Catalog2;

        public List<Catalog2Entry> Entries { get; set; }
    }

    public class Catalog2Entry
    {
        public Catalog2Entry()
        {
            Labels = new List<FString>();
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

        public List<FString> Labels { get; set; }
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
