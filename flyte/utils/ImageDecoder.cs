using flyte.io;
using System;

namespace flyte.utils
{
    class ImageDecoder
    {
        public enum ImageFormat : byte
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
    }
}
