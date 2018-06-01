using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

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
                        case 0x7C: // DXT1 - Packed normal map
                        case 0x86: // Raw
                            continue;
                    }

                    SwapBytes(data);
                    byte[] outData = UntileCompressedXbox360Texture(data, width, height, width, height, 4, 4, 8);
                    byte[] dds = new byte[128 + outData.Length];
                    
                    // Writes DDS file
                    using (MemoryStream ms = new MemoryStream(dds))
                    {
                        ms.Write(BuildDDSHeader(compression, width, height, width * 4, 0), 0, 128);
                        ms.Write(outData, 0, outData.Length);
                    }
                }
            }

            return xpr;
        }

        public void WriteToDDS(string path)
        {
            // Creates directory if needed
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            
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
                    0x44, 0x44, 0x53, 0x20, 0x7C, 0x00, 0x00, 0x00, 0x07, 0x10, 0x08, 0x00, 0x00, 0x02, 0x00, 0x00,
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
        
        private static uint GetTiledOffset(int x, int y, int width, int logBpb)
        {
            // Width <= 8192 && (x < width)

            int alignedWidth = Align(width, 32);
            // Top bits of coordinates
            int macro = ((x >> 5) + (y >> 5) * (alignedWidth >> 5)) << (logBpb + 7);
            // Lower bits of coordinates (result is 6-bit value)
            int micro = ((x & 7) + ((y & 0xE) << 2)) << logBpb;
            // Mix micro/macro + add few remaining x/y bits
            int offset = macro + ((micro & ~0xF) << 1) + (micro & 0xF) + ((y & 1) << 4);

            // Mix bits again
            return (uint)((((offset & ~0x1FF) << 3) +                  // Upper bits (offset bits [*-9])
                           ((y & 16) << 7) +                           // Next 1 bit
                           ((offset & 0x1C0) << 2) +                   // Next 3 bits (offset bits [8-6])
                           (((((y & 8) >> 2) + (x >> 3)) & 3) << 6) +  // Next 2 bits
                           (offset & 0x3F)                             // Lower 6 bits (offset bits [5-0])
                           ) >> logBpb);
        }

        private static int Align(int ptr, int alignment) => (ptr + alignment - 1) & ~(alignment - 1);

        private static byte[] UntileCompressedXbox360Texture(byte[] src , int tiledWidth, int originalWidth, int tiledHeight, int originalHeight, int blockSizeX, int blockSizeY, int bytesPerBlock)
        {
            // Thanks to UModel: https://github.com/gildor2/UModel/blob/master/Unreal/UnTexture.cpp
            int dstSize = ((tiledHeight * tiledWidth) * ((bytesPerBlock * 8) / (blockSizeX * blockSizeY))) / 8;
            byte[] dst = new byte[dstSize];

            int tiledBlockWidth = tiledWidth / blockSizeX;          // Width of image in blocks
            int originalBlockWidth = originalWidth / blockSizeX;    // Width of image in blocks
            int tiledBlockHeight = tiledHeight / blockSizeY;        // Height of image in blocks
            int originalBlockHeight = originalHeight / blockSizeY;  // Height of image in blocks
            int logBpp = (int)Math.Log(bytesPerBlock, 2);

            int sxOffset = 0;
            if ((tiledBlockWidth >= originalBlockWidth * 2) && (originalWidth == 16))
                sxOffset = originalBlockWidth;

            // Iterate image blocks
            for (int dy = 0; dy < originalBlockHeight; dy++)
            {
                for (int dx = 0; dx < originalBlockWidth; dx++)
                {
                    int swzAddr = (int)GetTiledOffset(dx + sxOffset, dy, tiledBlockWidth, logBpp);  // Do once for whole block
                    int sy = swzAddr / tiledBlockWidth;
                    int sx = swzAddr % tiledBlockWidth;

                    int dstOffset = (dy * originalBlockWidth + dx) * bytesPerBlock;
                    int srcOffset = (sy * tiledBlockWidth + sx) * bytesPerBlock;
                    Array.Copy(src, srcOffset, dst, dstOffset, bytesPerBlock);
                }
            }

            return dst;
        }
    }
}
