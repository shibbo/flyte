using flyte.io;

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
            byte[] image = new byte[width * height];

            // block width and block height are both 8
            for (int blockY = 0; blockY < height; blockY += 8)
            {
                for (int blockX = 0; blockX < width; blockX += 8)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        // 4 bits per pixel
                        for (int x = 0; x < 8; x += 2)
                        {
                            byte val = reader.ReadByte();

                            if (blockX + x >= width || blockY + y >= height)
                                continue;

                            int output = (((blockY + y) * width) + (blockX + x));
                            // 4 bits greyscale, 4 bits alpha
                            image[output++] = (byte)((val & 0xF0) | (val >> 4));
                            image[output] = (byte)((val << 4) | (val & 0xF));
                        }
                    }
                }
            }

            return image;
        }

        public static byte[] DecodeI8(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] image = new byte[width * height];

            for (int blockY = 0; blockY < height; blockY += 4)
            {
                for (int blockX = 0; blockX < width; blockX += 8)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            byte val = reader.ReadByte();

                            if (blockX + x >= width || blockY + y >= height)
                                continue;

                            int output = (((blockY + y) * width) + (blockX + x));
                            image[output] = val;
                        }
                    }
                }
            }

            return image;
        }

        public static byte[] DecodeIA4(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] image = new byte[width * height * 2];

            for (int blockY = 0; blockY < height; blockY += 4)
            {
                for (int blockX = 0; blockX < width; blockX += 8)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            byte val = reader.ReadByte();

                            if (blockX + x >= width || blockY + y >= height)
                                continue;

                            int output = (((blockY + y) * width) + (blockX + x)) * 2;
                            image[output++] = (byte)((val << 4) | (val & 0xF));
                            image[output] = (byte)((val & 0xF0) | (val >> 4));
                        }
                    }
                }
            }

            return image;
        }

        public static byte[] DecodeIA8(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] image = new byte[width * height * 2];

            for (int blockY = 0; blockY < height; blockY += 4)
            {
                for (int blockX = 0; blockX < width; blockX += 4)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            byte alpha = reader.ReadByte();
                            byte val = reader.ReadByte();

                            if (blockX + x >= width || blockY + y >= height)
                                continue;

                            int output = (((blockY + y) * width) + (blockX + x)) * 2;
                            image[output++] = val;
                            image[output] = alpha;
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
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            ushort val = reader.ReadUInt16();

                            if (blockX + x >= width || blockY + y >= height)
                                continue;

                            // now we figure out our position
                            int output = (((blockY + y) * width) + (blockX + x)) * 4;

                            // 5 bits R, 6 bits G, 5 bits B, alpha is always 0xFF
                            image[output++] = (byte)(((val & 0x001F) << 3) | ((val & 0x001F) >> 2));
                            image[output++] = (byte)(((val & 0x07E0) >> 3) | ((val & 0x07E0) >> 8));
                            image[output++] = (byte)(((val & 0xF800) >> 8) | ((val & 0xF800) >> 13));
                            image[output] = 0xFF;
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
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            byte r, g, b, a;

                            ushort val = reader.ReadUInt16();

                            if (blockX + x >= width || blockY + y >= height)
                                continue;

                            int output = (((blockY + y) * width) + (blockX + x)) * 4;

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

                            image[output++] = b;
                            image[output++] = g;
                            image[output++] = r;
                            image[output] = a;
                        }
                    }
                }
            }

            return image;
        }
    }
}
