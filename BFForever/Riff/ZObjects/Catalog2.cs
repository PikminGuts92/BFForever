using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFForever.Riff
{
    public class Catalog2 : ZObject
    {
        public Catalog2(FString idx) : base(idx)
        {
            Entries = new List<Catalog2Entry>();
        }

        public List<Catalog2Entry> Entries { get; set; }

        protected override void ImportData(AwesomeReader ar)
        {
            int count = ar.ReadInt32();
            ar.ReadInt32(); // Offset to entries (Always 4)

            for (int i = 0; i < count; i++)
            {
                // Reads each entry
                Catalog2Entry entry = new Catalog2Entry();
                entry.ImportData(ar);
                Entries.Add(entry);
            }
        }
    }

    public class Catalog2Entry
    {
        public Catalog2Entry()
        {
            Labels = new List<FString>();
            TechniqueTags = new List<FString>();
            GenreTags = new List<FString>();
        }

        public override string ToString()
        {
            return string.Format("{0}: \"{1}\" by {2}", this.SongType, this?.Title, this?.Artist);
        }

        public void ImportData(AwesomeReader ar)
        {
            // 280 bytes
            Indentifier = ar.ReadInt64();
            SongType = ar.ReadInt32();
            ar.ReadInt32(); // Should be zero.

            Title = ar.ReadInt64();
            Artist = ar.ReadInt64();
            Album = ar.ReadInt64();
            Description = ar.ReadInt64();
            LegendTag = ar.ReadInt64();

            SongLength = ar.ReadSingle();
            GuitarIntensity = ar.ReadSingle();
            BassIntensity = ar.ReadSingle();
            VoxIntensity = ar.ReadSingle();

            EraTag = ar.ReadInt64();
            Year = ar.ReadInt32();
            ar.ReadInt32(); // Should be zero

            #region Vox Tuning
            VoxTuningName = ar.ReadInt64();

            // Reads 1st string info.
            ar.ReadInt16();
            VoxRealTuning1 = ar.ReadByte();
            VoxOffsetTuning1 = ar.ReadByte();

            // Reads 2nd string info.
            ar.ReadInt16();
            VoxRealTuning2 = ar.ReadByte();
            VoxOffsetTuning2 = ar.ReadByte();

            // Reads 3rd string info.
            ar.ReadInt16();
            VoxRealTuning3 = ar.ReadByte();
            VoxOffsetTuning3 = ar.ReadByte();

            // Reads 4th string info.
            ar.ReadInt16();
            VoxRealTuning4 = ar.ReadByte();
            VoxOffsetTuning4 = ar.ReadByte();

            // Reads 5th string info.
            ar.ReadInt16();
            VoxRealTuning5 = ar.ReadByte();
            VoxOffsetTuning5 = ar.ReadByte();

            // Reads 6th string info.
            ar.ReadInt16();
            VoxRealTuning6 = ar.ReadByte();
            VoxOffsetTuning6 = ar.ReadByte();

            ar.ReadInt64(); // Should be zero'd
            #endregion
            #region Guitar Tuning
            GuitarTuningName = ar.ReadInt64();

            // Reads 1st string info.
            ar.ReadInt16();
            GuitarRealTuning1 = ar.ReadByte();
            GuitarOffsetTuning1 = ar.ReadByte();

            // Reads 2nd string info.
            ar.ReadInt16();
            GuitarRealTuning2 = ar.ReadByte();
            GuitarOffsetTuning2 = ar.ReadByte();

            // Reads 3rd string info.
            ar.ReadInt16();
            GuitarRealTuning3 = ar.ReadByte();
            GuitarOffsetTuning3 = ar.ReadByte();

            // Reads 4th string info.
            ar.ReadInt16();
            GuitarRealTuning4 = ar.ReadByte();
            GuitarOffsetTuning4 = ar.ReadByte();

            // Reads 5th string info.
            ar.ReadInt16();
            GuitarRealTuning5 = ar.ReadByte();
            GuitarOffsetTuning5 = ar.ReadByte();

            // Reads 6th string info.
            ar.ReadInt16();
            GuitarRealTuning6 = ar.ReadByte();
            GuitarOffsetTuning6 = ar.ReadByte();

            ar.ReadInt64(); // Should be zero'd
            #endregion
            #region Bass Tuning
            BassTuningName = ar.ReadInt64();

            // Reads 1st string info.
            ar.ReadInt16();
            BassRealTuning1 = ar.ReadByte();
            BassOffsetTuning1 = ar.ReadByte();

            // Reads 2nd string info.
            ar.ReadInt16();
            BassRealTuning2 = ar.ReadByte();
            BassOffsetTuning2 = ar.ReadByte();

            // Reads 3rd string info.
            ar.ReadInt16();
            BassRealTuning3 = ar.ReadByte();
            BassOffsetTuning3 = ar.ReadByte();

            // Reads 4th string info.
            ar.ReadInt16();
            BassRealTuning4 = ar.ReadByte();
            BassOffsetTuning4 = ar.ReadByte();

            // Reads 5th string info.
            ar.ReadInt16();
            BassRealTuning5 = ar.ReadByte();
            BassOffsetTuning5 = ar.ReadByte();

            // Reads 6th string info.
            ar.ReadInt16();
            BassRealTuning6 = ar.ReadByte();
            BassOffsetTuning6 = ar.ReadByte();

            ar.ReadInt64(); // Should be zero'd
            #endregion

            // Reads labels.
            int count = ar.ReadInt32();
            int offset = ar.ReadInt32();
            long previousPosition = ar.BaseStream.Position;

            ar.BaseStream.Position += offset - 4;
            for (int i = 0; i < count; i++)
            {
                Labels.Add(ar.ReadInt64());
            }
            ar.BaseStream.Position = previousPosition;

            LickPath = ar.ReadInt64();
            TexturePath = ar.ReadInt64();
            PreviewPath = ar.ReadInt64();

            // Reads technique tags.
            count = ar.ReadInt32();
            offset = ar.ReadInt32();
            previousPosition = ar.BaseStream.Position;

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

            ar.BaseStream.Position += 24;
        }

        public FString Indentifier { get; set; }
        public int SongType { get; set; }
        public FString Title { get; set; }
        public FString Artist { get; set; }
        public FString Album { get; set; }
        public FString Description { get; set; }
        public FString LegendTag { get; set; }

        public float SongLength { get; set; }
        public float GuitarIntensity { get; set; }
        public float BassIntensity { get; set; }
        public float VoxIntensity { get; set; }

        public FString EraTag { get; set; }
        public int Year { get; set; }

        public FString VoxTuningName { get; set; }

        public byte VoxRealTuning1 { get; set; }
        public byte VoxRealTuning2 { get; set; }
        public byte VoxRealTuning3 { get; set; }
        public byte VoxRealTuning4 { get; set; }
        public byte VoxRealTuning5 { get; set; }
        public byte VoxRealTuning6 { get; set; }

        public byte VoxOffsetTuning1 { get; set; }
        public byte VoxOffsetTuning2 { get; set; }
        public byte VoxOffsetTuning3 { get; set; }
        public byte VoxOffsetTuning4 { get; set; }
        public byte VoxOffsetTuning5 { get; set; }
        public byte VoxOffsetTuning6 { get; set; }

        public FString GuitarTuningName { get; set; }

        public byte GuitarRealTuning1 { get; set; }
        public byte GuitarRealTuning2 { get; set; }
        public byte GuitarRealTuning3 { get; set; }
        public byte GuitarRealTuning4 { get; set; }
        public byte GuitarRealTuning5 { get; set; }
        public byte GuitarRealTuning6 { get; set; }

        public byte GuitarOffsetTuning1 { get; set; }
        public byte GuitarOffsetTuning2 { get; set; }
        public byte GuitarOffsetTuning3 { get; set; }
        public byte GuitarOffsetTuning4 { get; set; }
        public byte GuitarOffsetTuning5 { get; set; }
        public byte GuitarOffsetTuning6 { get; set; }

        public FString BassTuningName { get; set; }

        public byte BassRealTuning1 { get; set; }
        public byte BassRealTuning2 { get; set; }
        public byte BassRealTuning3 { get; set; }
        public byte BassRealTuning4 { get; set; }
        public byte BassRealTuning5 { get; set; }
        public byte BassRealTuning6 { get; set; }

        public byte BassOffsetTuning1 { get; set; }
        public byte BassOffsetTuning2 { get; set; }
        public byte BassOffsetTuning3 { get; set; }
        public byte BassOffsetTuning4 { get; set; }
        public byte BassOffsetTuning5 { get; set; }
        public byte BassOffsetTuning6 { get; set; }

        public List<FString> Labels { get; set; }
        public FString LickPath { get; set; }

        public FString TexturePath { get; set; }
        public FString PreviewPath { get; set; }
        public List<FString> TechniqueTags { get; set; }
        public List<FString> GenreTags { get; set; }
    }
}
