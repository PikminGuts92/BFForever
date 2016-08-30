using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using S16.Drawing;

// Helpful resource - http://forum.xentax.com/viewtopic.php?p=65649

namespace BFForever.Texture
{
    public class XPR2
    {
        private const int MAGIC_XPR2 = 1481658930;
        private const int MAGIC_TX2D = 1415066180;

        private Bitmap[] images;

        private XPR2()
        {

        }

        public static XPR2 FromFile(string path)
        {
            if (!File.Exists(path)) return null;
            XPR2 xpr = new XPR2();

            // Opens .xpr texture
            using (AwesomeReader ar = new AwesomeReader(File.OpenRead(path), true))
            {
                if (ar.ReadInt32() != MAGIC_XPR2) return null;

                ar.ReadInt32(); // Always 2048?

                int texSize = ar.ReadInt32();
                xpr.images = new Bitmap[ar.ReadInt32()];

                for (int i = 0; i < xpr.images.Length; i++)
                {
                    // TX2D Header
                    if (ar.ReadInt32() != MAGIC_TX2D) continue;

                    int infoOffset = ar.ReadInt32() + 45;
                    ar.ReadInt32(); // Not needed - Always 52?
                    int nameOffset = ar.ReadInt32() + 12; // Texture name offset

                    // Reads texture name
                    ar.BaseStream.Position = nameOffset;
                    string name = ar.ReadNullString(); // "album"

                    // Reads texture info
                    ar.BaseStream.Position = infoOffset;
                    int imageOffset = ar.ReadUInt16();
                    int bytesOffset = (imageOffset << 8) + 0x80C; // Where raw image bytes are
                    byte compressionType = ar.ReadByte(); // 0x52 = DXT1
                    int height = (ar.ReadUInt16() + 1) << 3;
                    int width = (ar.ReadUInt16() + 1) & 0x1FFF;

                    int size = ((texSize >> 8) - imageOffset) << 8;

                    // Reads raw image data
                    ar.BaseStream.Position = bytesOffset;
                    byte[] data = ar.ReadBytes(size);

                    string compression = "";
                    switch(compressionType)
                    {
                        case 0x52: // DXT1
                            compression = "DXT1";
                            break;
                        case 0x53: // DXT3
                        case 0x54: // DXT5
                        case 0x71: // DXT5 - Packed normal map
                        case 0x7C: // DXT1 - PAcked normal map
                        case 0x86: // raw
                            continue;
                    }

                    byte[] dds = new byte[128 + data.Length];
                    SwapBytes(data);

                    using (MemoryStream ms = new MemoryStream(dds))
                    {
                        ms.Write(BuildDDSHeader(compression, width, height, data.Length, 0), 0, 128);
                        ms.Write(data, 0, data.Length);
                        ms.Seek(0, SeekOrigin.Begin);

                        DDSImage image = new DDSImage(ms);
                        xpr.images[i] = image.BitmapImage;
                        //xpr.images[i].Save("converted.png");
                    }
                    //File.WriteAllBytes("test.dds", dds);

                    continue; // Not ready for multiple images yet
                }
            }

            return xpr;
        }

        private static void SwapBytes(byte[] data)
        {
            for (int i = 0; i < data.Length; i += 2)
            {
                // Swaps bytes
                byte b = data[i];
                data[i] = data[i + 1];
                data[i + 1] = b;
            }
        }

        private static byte[] BuildDDSHeader(string format, int width, int height, int size, int mipMaps) // 128 bytes
        {
            byte[] dds = new byte[] //512x512 DXT5  -- 128 Bytes
                {//|-D-----D-----S---------|--Header Size (124)----|-------Flags-----------|-------Height--------|
                    0x44, 0x44, 0x53, 0x20, 0x7C, 0x00, 0x00, 0x00, 0x07, 0x10, 0x0A, 0x00, 0x00, 0x02, 0x00, 0x00,
                 //|--------Width----------|-----Size or Pitch-----|                       |-------Mip Maps------|
                    0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x4E, 0x45, 0x4D, 0x4F, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00,
                    0x04, 0x00, 0x00, 0x00, 0x44, 0x58, 0x54, 0x35, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                };

            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes(height);

            dds[12] = buffer[0];
            dds[13] = buffer[1];
            dds[14] = buffer[2];
            dds[15] = buffer[3];

            buffer = BitConverter.GetBytes(width);

            dds[16] = buffer[0];
            dds[17] = buffer[1];
            dds[18] = buffer[2];
            dds[19] = buffer[3];

            buffer = BitConverter.GetBytes(size);

            dds[20] = buffer[0];
            dds[21] = buffer[1];
            dds[22] = buffer[2];
            dds[23] = buffer[3];

            buffer = BitConverter.GetBytes(mipMaps);

            dds[28] = buffer[0];
            dds[29] = buffer[1];
            dds[30] = buffer[2];
            dds[31] = buffer[3];

            // From what I can tell, Bpp 4 = DXT1 / Bpp 8 = DXT5.
            // 'Normal' textures are ATI2 on X360 and DXT1 on PS3.

            if (format == "DXT1")
            {
                // Sets to DXT1
                dds[87] = 0x31;
            }
            else if (format == "ATI2")
            {
                dds[84] = 0x41;
                dds[85] = 0x54;
                dds[86] = 0x49;
                dds[87] = 0x32;
            }

            return dds;
        }
    }
}
