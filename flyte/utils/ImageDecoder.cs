/*
    © 2019 - shibboleet
    flyte is free software: you can redistribute it and/or modify it under
    the terms of the GNU General Public License as published blockY the Free
    Software Foundation, either version 3 of the License, or (at your option)
    any later version.
    flyte is distributed in the hope that it will be useful, but WITHOUT ANY 
    WARRANTY; See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along 
    with flyte. If not, see http://www.gnu.org/licenses/.
*/

using flyte.io;

namespace flyte.utils
{
    class ImageDecoder
    {
        #region Wii formats
        public enum ImageFormat_Wii : byte
        {
            I4 = 0x0,
            I8 = 0x1,
            IA4 = 0x2,
            IA8 = 0x3,
            RGB565 = 0x4,
            RGB5A3 = 0x5,
            RGBA32 = 0x6,
            C4 = 0x8,
            C8 = 0x9,
            C14X2 = 0xA,
            CMPR = 0xE
        }

        public static byte[] DecodeI4(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] image = new byte[width * height * 4];

            for (int blockY = 0; blockY < height; blockY += 8)
            {
                for (int blockX = 0; blockX < width; blockX += 8)
                {
                    for (int y = blockY; y < blockY + 8; y++)
                    {
                        for (int x = blockX; x < (blockX + 8); x += 2)
                        {
                            byte val = reader.ReadByte();
                            int index = ((y * width) + x) * 4;

                            if (x < width && y < height)
                            {
                                byte component = (byte)(((val >> 4) & 0xF) * 0x11);

                                image[index] = component;
                                image[index + 1] = component;
                                image[index + 2] = component;
                                image[index + 3] = 0xFF;
                            }
                        }
                    }
                }
            }

            return image;
        }

        public static byte[] DecodeI8(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] image = new byte[width * height * 4];

            for (int blockY = 0; blockY < height; blockY += 4)
            {
                for (int blockX = 0; blockX < width; blockX += 8)
                {
                    for (int y = blockY; y < blockY + 4; y++)
                    {
                        for (int x = blockX; x < (blockX + 8); x++)
                        {
                            byte component = reader.ReadByte();

                            int index = ((y * width) + x) * 4;

                            if (x < width && y < height)
                            {
                                image[index] = component;
                                image[index + 1] = component;
                                image[index + 2] = component;
                                image[index + 3] = 0xFF;
                            }
                        }
                    }
                }
            }

            return image;
        }

        public static byte[] DecodeIA4(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] image = new byte[width * height * 4];

            for (int blockY = 0; blockY < height; blockY += 4)
            {
                for (int blockX = 0; blockX < width; blockX += 8)
                {
                    for (int y = blockY; y < blockY + 4; y++)
                    {
                        for (int x = blockX; x < blockX + 8; x++)
                        {
                            byte val = reader.ReadByte();

                            byte component = (byte)((val & 0xF) * 0x11);
                            byte alpha = (byte)(((val >> 4) & 0xF) * 0x11);

                            int index = ((y * width) + x) * 4;

                            if (x < width && y < height)
                            {
                                image[index] = component;
                                image[index + 1] = component;
                                image[index + 2] = component;
                                image[index + 3] = alpha;
                            }
                        }
                    }
                }
            }

            return image;
        }

        public static byte[] DecodeIA8(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] image = new byte[width * height * 4];

            for (int blockY = 0; blockY < height; blockY += 4)
            {
                for (int blockX = 0; blockX < width; blockX += 4)
                {
                    for (int y = blockY; y < blockY + 4; y++)
                    {
                        for (int x = blockX; x < blockX + 4; x++)
                        {
                            ushort val = reader.ReadUInt16();

                            byte alpha = (byte)(val & 0xFF);
                            byte component = (byte)(val >> 8);

                            int index = ((y * width) + x) * 4;

                            if (x < width && y < height)
                            {
                                image[index] = component;
                                image[index + 1] = component;
                                image[index + 2] = component;
                                image[index + 3] = alpha;
                            }
                        }
                    }
                }
            }

            return image;
        }

        public static byte[] DecodeRGB565(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] image = new byte[width * height * 4];

            for (int blockY = 0; blockY < height; blockY += 4)
            {
                for (int blockX = 0; blockX < width; blockX += 4)
                {
                    for (int y = blockY; y < blockY + 4; y++)
                    {
                        for (int x = blockX; x < blockX + 4; x++)
                        {
                            ushort val = reader.ReadUInt16();

                            byte r = (byte)(((val >> 11) & 0x1F) * 0x8);
                            byte g = (byte)(((val >> 5) & 0x3F) * 0x4);
                            byte b = (byte)((val & 0x1F) * 0x8);

                            int index = ((y * width) + x) * 4;

                            if (x < width && y < height)
                            {
                                image[index] = r;
                                image[index + 1] = g;
                                image[index + 2] = b;
                                image[index + 3] = 0xFF;
                            }
                        }
                    }
                }
            }

            return image;
        }

        public static byte[] DecodeRGB5A3(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] image = new byte[width * height * 4];

            for (int blockY = 0; blockY < height; blockY += 4)
            {
                for (int blockX = 0; blockX < width; blockX += 4)
                {
                    for (int y = blockY; y < blockY + 4; y++)
                    {
                        for (int x = blockX; x < blockX + 4; x++)
                        {
                            byte r, g, b, a;

                            ushort val = reader.ReadUInt16();

                            int index = ((y * width) + x) * 4;

                            if (x < width && y < height)
                            {
                                // first we check for alpha
                                bool hasAlpha = ((val >> 15) & 0x1) == 0;

                                if (hasAlpha)
                                {
                                    r = (byte)(((val >> 8) & 0xF) * 0x11);
                                    g = (byte)(((val >> 4) & 0xF) * 0x11);
                                    b = (byte)((val & 0xF) * 0x11);
                                    a = (byte)(((val >> 12) & 0x7) * 0x20);
                                }
                                else
                                {
                                    r = (byte)(((val >> 10) & 0x1F) * 0x8);
                                    g = (byte)(((val >> 5) & 0x1F) * 0x8);
                                    b = (byte)((val & 0x1F) * 0x8);
                                    a = 0xFF;
                                }

                                image[index] = b;
                                image[index + 1] = g;
                                image[index + 2] = r;
                                image[index + 3] = a;
                            }
                        }
                    }
                }
            }

            return image;
        }

        #endregion

        #region BFLIM Formats
        public enum ImageFormat_Common : byte
        {
            L8_UNIFORM = 0,
            A8_UNIFORM = 1,
            L4A_UNIFORM = 2,
            LA8_UNIFORM = 3,
            HILO8 = 4,
            RGB565_UNORM = 5,
            RGBX8_UNORM = 6,
            RGBA5A1_UNORM = 7,
            RGBA4_UNORM = 8,
            RGBA8_UNORM = 9,
            ETC1_UNORM = 0xA,
            ETC1A4_UNORM = 0xB,
            BC1_UNORM = 0xC,
            BC2_UNORM = 0xD,
            BC3_UNORM = 0xE,
            BC4L_UNORM = 0xF,
            BC4A_UNORM = 0x10,
            BC5_UNORM = 0x11,
            L4_UNORM = 0x12,
            A4_UNORM = 0x13,
            RGBA8_SRGB = 0x14,
            BC1_SRGB = 0x15,
            BC2_SRGB = 0x16,
            BC3_SRGB = 0x17,
            RGB10A2_UNORM = 0x18,
            RGB565_INDIR_UNORM = 0x19
        }

        // https://pastebin.com/DCrP1w9x
        public enum TileMode
        {
            GX2_TILE_MODE_DEFAULT = 0x00000000, // driver will choose best mode
            GX2_TILE_MODE_LINEAR_SPECIAL = 0x00000010, // typically not supported by HW
            GX2_TILE_MODE_LINEAR_ALIGNED = 0x00000001, // supported by HW, but not fast
            GX2_TILE_MODE_1D_TILED_THIN1 = 0x00000002,
            GX2_TILE_MODE_1D_TILED_THICK = 0x00000003,
            GX2_TILE_MODE_2D_TILED_THIN1 = 0x00000004, // (a typical default, but not always)
            GX2_TILE_MODE_2D_TILED_THIN2 = 0x00000005,
            GX2_TILE_MODE_2D_TILED_THIN4 = 0x00000006,
            GX2_TILE_MODE_2D_TILED_THICK = 0x00000007,
            GX2_TILE_MODE_2B_TILED_THIN1 = 0x00000008,
            GX2_TILE_MODE_2B_TILED_THIN2 = 0x00000009,
            GX2_TILE_MODE_2B_TILED_THIN4 = 0x0000000a,
            GX2_TILE_MODE_2B_TILED_THICK = 0x0000000b,
            GX2_TILE_MODE_3D_TILED_THIN1 = 0x0000000c,
            GX2_TILE_MODE_3D_TILED_THICK = 0x0000000d,
            GX2_TILE_MODE_3B_TILED_THIN1 = 0x0000000e,
            GX2_TILE_MODE_3B_TILED_THICK = 0x0000000f,
            GX2_TILE_MODE_FIRST = GX2_TILE_MODE_DEFAULT,
            GX2_TILE_MODE_LAST = GX2_TILE_MODE_LINEAR_SPECIAL
        }
        #endregion

        #region BCLIM Formats
        public enum ImageFormat_3DS : uint
        {
            L8 = 0,
            A8 = 1,
            LA4 = 2,
            LA8 = 3,
            HILO8 = 4,
            RGB565 = 5,
            RGBA5551 = 7,
            RGBA4444 = 8,
            RGBA8 = 9,
            ETC1 = 10,
            ETC1A4 = 11,
            L4 = 12,
            A4 = 13
        }
        #endregion
    }
}
