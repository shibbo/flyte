using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using flyte.utils;

namespace flyte.img._3ds
{
    class BCLIM : ImageContainerBase
    {
        public BCLIM(ref EndianBinaryReader reader)
        {
            reader.SetEndianess(Endian.Endianess.Little);
            
            while(true)
            {
                string a = reader.ReadString(4);

                if (a == "CLIM")
                    break;
            }

            // BOM
            reader.ReadUInt16();
            // total length but we already have that
            reader.ReadUInt32();
            mTileWidth = (byte)(2 << reader.ReadByte());
            mTileHeight = (byte)(2 << reader.ReadByte());
            mLength = reader.ReadUInt32();
            mCount = reader.ReadUInt32();
            
            mImage = new BCLIMImage(ref reader);
        }

        public override ImageBase getImage(int imageIndex)
        {
            return mImage;
        }

        byte mTileWidth;
        byte mTileHeight;
        uint mLength;
        uint mCount;

        BCLIMImage mImage;
    }

    class BCLIMImage : ImageBase
    {
        public BCLIMImage(ref EndianBinaryReader reader)
        {
            // oh noes
            if (reader.ReadString(4) != "imag")
                return;

            reader.ReadUInt32();
            mWidth = reader.ReadUInt16();
            mHeight = reader.ReadUInt16();
            mFormat = (ImageDecoder.ImageFormat_3DS)reader.ReadInt32();
            mDataLength = reader.ReadUInt32();

            reader.Seek(0);

            int[] formatSizes = { 8, 8, 8, 16, 16, 16, 16, 24, 16, 16, 32, 4, 4 };

            ushort dataWidth = mWidth;
            ushort dataHeight = mHeight;

            mData = reader.ReadBytes((int)mDataLength);

            // we need to be careful to not accidentally change the width / height
            if (mFormat == ImageDecoder.ImageFormat_3DS.ETC1 || mFormat == ImageDecoder.ImageFormat_3DS.ETC1A4)
            {
                DecompressETC();
                return;
            }

            // check if width is a power of two
            if ((mWidth & (mWidth - 1)) != 0)
                mWidth = (ushort)(1 << (int)Math.Ceiling(Math.Log(mWidth, 2)));
            // and the height
            if ((mHeight & (mHeight - 1)) != 0)
                mHeight = (ushort)(1 << (int)Math.Ceiling(Math.Log(mHeight, 2)));

            // make sure our sizes are right and adjust them
            if ((dataWidth * dataHeight * formatSizes[(int)mFormat] / 8.0) < mDataLength)
            {
                dataWidth = (ushort)(1 << (ushort)Math.Ceiling(Math.Log(dataWidth, 2)));
                dataHeight = (ushort)(1 << (ushort)Math.Ceiling(Math.Log(dataHeight, 2)));
            }

            int tileHeight = (int)Math.Ceiling(mHeight / 8.0);
            int tileWidth = (int)Math.Ceiling(mWidth / 8.0);

            // * 4 because we're going from 8bit => 32
            byte[] image = new byte[mWidth * mHeight * 4];

            for (int tileY = 0; tileY < tileHeight; tileY++)
            {
                for (int tileX = 0; tileX < tileWidth; tileX++)
                {
                    for (int subTileY = 0; subTileY < 2; subTileY++)
                    {
                        for (int subTileX = 0; subTileX < 2; subTileX++)
                        {
                            for (int pixelGroupY = 0; pixelGroupY < 2; pixelGroupY++)
                            {
                                for (int pixelGroupX = 0; pixelGroupX < 2; pixelGroupX++)
                                {
                                    for (int pixelY = 0; pixelY < 2; pixelY++)
                                    {
                                        for (int pixelX = 0; pixelX < 2; pixelX++)
                                        {
                                            int pixX = (pixelX + (pixelGroupX * 2) + (subTileX * 4) + (tileX * 8));
                                            int pixY = (pixelY + (pixelGroupY * 2) + (subTileY * 4) + (tileY * 8));

                                            if (pixY >= dataHeight || pixY >= mHeight)
                                                continue;

                                            if (pixX >= dataWidth || pixX >= mWidth)
                                                continue;

                                            int pixPos = (pixX + (pixY * mWidth)) * 4;

                                            uint dataX = (uint)(pixelX + (pixelGroupX * 4) + (subTileX * 16) + (tileX * 64));
                                            uint dataY = (uint)((pixelY * 2) + (pixelGroupY * 8) + (subTileY * 32) + (tileY * dataWidth * 8));

                                            uint dataPos = dataX + dataY;

                                            byte[] pixel = GetColorFromData(dataPos);

                                            image[pixPos] = pixel[0];
                                            image[pixPos++] = pixel[1];
                                            image[pixPos++] = pixel[2];
                                            image[pixPos++] = pixel[3];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        byte[] GetColorFromData(uint index)
        {
            // 4 bytes in a 32-bit color
            byte[] outdata = new byte[0x4];

            switch(mFormat)
            {
                case ImageDecoder.ImageFormat_3DS.L4:
                    byte l = mData[index / 2];
                    byte shift = (byte)((index & 0x1) * 0x4);

                    byte r, g, b, a;
                    r = g = b = (byte)(((l >> shift) & 0xF) * 0x11);
                    a = 0xFF;

                    outdata[0] = r;
                    outdata[1] = g;
                    outdata[2] = b;
                    outdata[3] = a;

                    break;
            }

            return outdata;
        }

        void DecompressETC()
        {
            byte[][] Modifiers = new byte[][]
            {
                new byte[] { 2, 8 },
                new byte[] { 5, 17 },
                new byte[] { 9, 29 },
                new byte[] { 13, 42 },
                new byte[] { 18, 60 },
                new byte[] { 24, 80 },
                new byte[] { 33, 106 },
                new byte[] { 47, 183 }
            };

            bool hasAlpha = mFormat == ImageDecoder.ImageFormat_3DS.ETC1A4;
            ushort width = mWidth;
            ushort height = mHeight;

            int blockSize = hasAlpha ? 16 : 8;

            int tileHeight = (int)Math.Ceiling(mHeight / 8.0);
            int tileWidth = (int)Math.Ceiling(mWidth / 8.0);

            tileHeight = (ushort)(1 << (ushort)Math.Ceiling(Math.Log(tileHeight, 2)));
            tileWidth = (ushort)(1 << (ushort)Math.Ceiling(Math.Log(tileWidth, 2)));

            int pos = 0;

            mOutImg = new byte[width * height * 4];

            for (int tileY = 0; tileY < tileHeight; tileY++)
            {
                for (int tileX = 0; tileX < tileWidth; tileX++)
                {
                    for (int blockY = 0; blockY < 2; blockY++)
                    {
                        for (int blockX = 0; blockX < 2; blockX++)
                        {
                            int dataPos = pos;
                            pos += blockSize;

                            byte[] block = getBytesFrom(mData, dataPos, dataPos + blockSize);

                            long alphas = 0xFFFFFFFF;

                            if (hasAlpha)
                                alphas = BitConverter.ToInt64(getBytesFrom(block, 8, 15), 0);

                            long pixels = BitConverter.ToInt64(getBytesFrom(block, 0, 7), 0);

                            bool hasDifferential = ((pixels >> 33) & 0x1) == 1;
                            // 0 == 2x4, 1 == 4x2
                            bool horizontal = ((pixels >> 32) & 0x1) == 1;

                            byte[] table1 = Modifiers[(pixels >> 37) & 0x07];
                            byte[] table2 = Modifiers[(pixels >> 34) & 0x07];

                            byte r, g, b;
                            byte[] color1 = { 0, 0, 0 };
                            byte[] color2 = { 0, 0, 0 };

                            if (hasDifferential)
                            {
                                r = (byte)((pixels >> 59) & 0x1F);
                                g = (byte)((pixels >> 51) & 0x1F);
                                b = (byte)((pixels >> 43) & 0x1F);

                                color1[0] = (byte)((r << 3) | ((r >> 2) & 0x07));
                                color1[1] = (byte)((g << 3) | ((g >> 2) & 0x07));
                                color1[2] = (byte)((b << 3) | ((b >> 2) & 0x07));

                                r += complement((byte)((pixels >> 56) & 0x07), 3);
                                g += complement((byte)((pixels >> 48) & 0x07), 3);
                                b += complement((byte)((pixels >> 40) & 0x07), 3);

                                color2[0] = (byte)((r << 3) | ((r >> 2) & 0x07));
                                color2[1] = (byte)((g << 3) | ((g >> 2) & 0x07));
                                color2[2] = (byte)((b << 3) | ((b >> 2) & 0x07));
                            }
                            else
                            {
                                color1[0] = (byte)(((pixels >> 60) & 0xF) * 0x11);
                                color1[1] = (byte)(((pixels >> 52) & 0xF) * 0x11);
                                color1[2] = (byte)(((pixels >> 44) & 0xF) * 0x11);

                                color2[0] = (byte)(((pixels >> 56) & 0xF) * 0x11);
                                color2[1] = (byte)(((pixels >> 48) & 0xF) * 0x11);
                                color2[2] = (byte)(((pixels >> 40) & 0xF) * 0x11);
                            }

                            int amounts = (int)pixels & 0xFFFF;
                            int signs = (int)((pixels >> 16) & 0xFFFF);

                            for (int pixelY = 0; pixelY < 4; pixelY++)
                            {
                                for (int pixelX = 0; pixelX < 4; pixelX++)
                                {
                                    int x = pixelX + (blockX * 4) + (tileX * 8);
                                    int y = pixelY + (blockY * 4) + (tileY * 8);

                                    if (x >= width)
                                        continue;
                                    if (x >= height)
                                        continue;

                                    int offset = pixelX * 4 + pixelY;

                                    byte[] table = null;
                                    byte[] color = null;

                                    if (horizontal)
                                    {
                                        table = pixelY < 2 ? table1 : table2;
                                        color = pixelY < 2 ? color1 : color2;
                                    }
                                    else
                                    {
                                        table = pixelX < 2 ? table1 : table2;
                                        color = pixelX < 2 ? color1 : color2;
                                    }

                                    int amount = table[(amounts >> offset) & 0x1];
                                    byte sign = (byte)((signs >> offset) & 0x1);

                                    if (sign == 1)
                                        amount *= -1;

                                    byte red = (byte)Math.Max(Math.Min(color[0] + amount, 0xFF), 0);
                                    byte blue = (byte)Math.Max(Math.Min(color[1] + amount, 0xFF), 0);
                                    byte green = (byte)Math.Max(Math.Min(color[2] + amount, 0xFF), 0);
                                    byte alpha = (byte)(((alphas >> (offset * 4)) & 0xF) * 0x11);

                                    int pixelPos = y * width + x;

                                    mOutImg[pixelPos] = red;
                                    mOutImg[pixelPos + 1] = green;
                                    mOutImg[pixelPos + 2] = blue;
                                    mOutImg[pixelPos + 3] = alpha;
                                }
                            }
                        }
                    }
                }
            }
        }

        byte[] getBytesFrom(byte[] data, int start, int end)
        {
            byte[] ret = new byte[(end - start) + 1];

            int curIdx = 0;

            for (int i = start; i < end; i++)
            {
                ret[curIdx] = data[i];
                curIdx++;
            }

            Array.Reverse(ret);
            return ret;
        }

        byte complement(byte input_, byte bits)
        {
            if (input_ >> (bits - 1) == 0)
                return input_;

            return (byte)(input_ - (1 << bits));
        }

        public override Bitmap getImageBitmap()
        {
            if (mOutImg == null)
                return null;

            var outBMP = new Bitmap(mWidth, mHeight);
            var img = outBMP.LockBits(new Rectangle(0, 0, outBMP.Width, outBMP.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(mOutImg, 0, img.Scan0, mOutImg.Length);
            outBMP.UnlockBits(img);

            return outBMP;
        }

        ushort mWidth;
        ushort mHeight;
        ImageDecoder.ImageFormat_3DS mFormat;
        uint mDataLength;

        byte[] mData;

        byte[] mOutImg;

        [DisplayName("Format"), CategoryAttribute("General"), DescriptionAttribute("The image format.")]
        public ImageDecoder.ImageFormat_3DS Format
        {
            get { return mFormat; }
            set { mFormat = value; }
        }

        [DisplayName("Height"), CategoryAttribute("Size"), DescriptionAttribute("The height of the image.")]
        public ushort Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }

        [DisplayName("Width"), CategoryAttribute("Size"), DescriptionAttribute("The width of the image.")]
        public ushort Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }
    }

}