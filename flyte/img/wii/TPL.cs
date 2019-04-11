/*
    © 2019 - shibboleet
    flyte is free software: you can redistribute it and/or modify it under
    the terms of the GNU General Public License as published by the Free
    Software Foundation, either version 3 of the License, or (at your option)
    any later version.
    flyte is distributed in the hope that it will be useful, but WITHOUT ANY 
    WARRANTY; See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along 
    with flyte. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using flyte.io;
using static flyte.utils.Endian;

namespace flyte.img.wii
{
    class TPL
    {
        public TPL(ref EndianBinaryReader reader)
        {
            throw new NotImplementedException();

            reader.SetEndianess(Endianess.Big);
            mIdentifier = reader.ReadUInt32();
            mNumImages = reader.ReadUInt32();
            mImageTableOffset = reader.ReadUInt32();

            mImageOffsets = new List<ImageOffset>();

            for (int i = 0; i < mNumImages; i++)
            {
                ImageOffset offs = new ImageOffset();
                offs.mImgHeader = reader.ReadUInt32();
                offs.mPaletteHeader = reader.ReadUInt32();

                mImageOffsets.Add(offs);
            }

            mImages = new List<TPLImage>();
            mPalettes = new List<Palette>();

            foreach(ImageOffset offset in mImageOffsets)
            {
                reader.Seek(offset.mImgHeader);
                TPLImage image = new TPLImage(ref reader);
                mImages.Add(image);

                // palettes are optional
                if (offset.mPaletteHeader != 0)
                {
                    reader.Seek(offset.mPaletteHeader);
                    Palette palette = new Palette(ref reader);
                    mPalettes.Add(palette);
                }
            }
        }

        public Bitmap getImage()
        {
            // shrug lol
            return mImages[0].getImage();
        }

        uint mIdentifier;
        uint mNumImages;
        uint mImageTableOffset;

        List<ImageOffset> mImageOffsets;
        List<TPLImage> mImages;
        List<Palette> mPalettes;
    }

    struct ImageOffset
    {
        public uint mImgHeader;
        public uint mPaletteHeader;
    }

    class TPLImage
    {
        int[] TexelWidths = { 8, 8, 8, 4, 4, 4, 4, -1, 8, 8, 4, -1, -1, -1, 8 };
        int[] TexelHeights = { 8, 4, 4, 4, 4, 4, 4, -1, 8, 4, 4, -1, -1, -1, 8 };
        int[] BitsPerPixel = { 4, 8, 8, 16, 16, 16, 32, -1, 8, 16, -1, -1, -1, 4};

        public enum ImageFormat
        {
            I4      = 0x0,
            I8      = 0x1,
            IA4     = 0x2,
            IA8     = 0x3,
            RGB565  = 0x4,
            RGB5A3  = 0x5,
            RGBA32  = 0x6,
            C4      = 0x8,
            C8      = 0x9,
            C14X2   = 0xA,
            CMPR    = 0xE
        }

        public TPLImage(ref EndianBinaryReader reader)
        {
            mHeight = reader.ReadUInt16();
            mWidth = reader.ReadUInt16();
            mFormat = (ImageFormat)reader.ReadUInt32();
            mImageDataAddr = reader.ReadUInt32();
            mWrapS = reader.ReadUInt32();
            mWrapT = reader.ReadUInt32();
            mMinFilter = reader.ReadUInt32();
            mMagFilter = reader.ReadUInt32();
            mLODBias = reader.ReadF32();
            mEdgeLODEnable = reader.ReadByte();
            mMinLOD = reader.ReadByte();
            mMaxLOD = reader.ReadByte();
            mUnpacked = reader.ReadByte();

            int texelHeight = TexelHeights[(int)mFormat];
            int texelWidth = TexelWidths[(int)mFormat];
            int bitsPerPixel = BitsPerPixel[(int)mFormat];

            reader.Seek(mImageDataAddr);

            byte[] mOutImg = null;
            bool unsupported = false;

            switch (mFormat)
            {
                case ImageFormat.I4:
                    mOutImg = ImageDecompressor.DecodeI4(ref reader, mHeight, mWidth);
                    break;
                case ImageFormat.I8:
                    mOutImg = ImageDecompressor.DecodeI8(ref reader, mHeight, mWidth);
                    break;
                case ImageFormat.IA4:
                    mOutImg = ImageDecompressor.DecodeIA4(ref reader, mHeight, mWidth);
                    break;
                case ImageFormat.IA8:
                    mOutImg = ImageDecompressor.DecodeIA8(ref reader, mHeight, mWidth);
                    break;
                default:
                    Console.WriteLine("Format " + mFormat + " not supported...");
                    unsupported = true;
                    break;
            }

            if (unsupported)
                return;
        }

        public Bitmap getImage()
        {
            if (mOutImg == null)
                return null;

            var outBMP = new Bitmap(mHeight, mWidth);
            var img = outBMP.LockBits(new Rectangle(0, 0, outBMP.Width, outBMP.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(mOutImg, 0, img.Scan0, mOutImg.Length);
            outBMP.UnlockBits(img);

            return outBMP;
        }

        ushort mHeight;
        ushort mWidth;
        ImageFormat mFormat;
        uint mImageDataAddr;
        uint mWrapS;
        uint mWrapT;
        uint mMinFilter;
        uint mMagFilter;
        float mLODBias;
        byte mEdgeLODEnable;
        byte mMinLOD;
        byte mMaxLOD;
        byte mUnpacked;

        byte[] mOutImg;
    }

    class Palette
    {
        enum PaletteImageFormat
        {
            IA8 = 0,
            RGB565 = 1,
            RGB5A3 = 2
        }

        public Palette(ref EndianBinaryReader reader)
        {
            mEntryCount = reader.ReadUInt16();
            mUnpacked = Convert.ToBoolean(reader.ReadByte());
            reader.ReadByte();
            mPaletteFormat = (PaletteImageFormat)reader.ReadUInt32();
            mPaletteDataAddres = reader.ReadUInt32();
        }

        ushort mEntryCount;
        bool mUnpacked;
        PaletteImageFormat mPaletteFormat;
        uint mPaletteDataAddres;
    }

    class ImageDecompressor
    {
        public static byte[] DecodeI4(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] buf = new byte[width * height * 4];
            byte[] data = reader.ReadBytes(width * height * 4);

            int i = 0;

            for (int yTile = 0; yTile < height; yTile += 8)
            {
                for (int xTile = 0; xTile < width; xTile += 8)
                {
                    for (int yPixel = yTile; yPixel < yTile + 8; yPixel++)
                    {
                        for (int xPixel = xTile; xPixel < yTile + 8; xPixel += 2)
                        {
                            if (xPixel >= width || yPixel >= height)
                                continue;

                            byte pix = Convert.ToByte((data[i] >> 4) * 0x11);

                            buf[(((yPixel * width) + xPixel) * 4)] = 0xFF;
                            buf[(((yPixel * width) + xPixel) * 4) + 1] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 2] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 3] = pix;

                            pix = Convert.ToByte((data[i] & 0xF) * 0x11);

                            buf[(((yPixel * width) + xPixel) * 4) + 4] = 0xFF;
                            buf[(((yPixel * width) + xPixel) * 4) + 5] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 6] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 7] = pix;

                            i++;
                        }
                    }
                }
            }

            return buf;
        }

        public static byte[] DecodeI8(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] buf = new byte[width * height * 4];
            byte[] data = reader.ReadBytes(width * height * 4);

            int i = 0;

            for (int yTile = 0; yTile < height; yTile += 4)
            {
                for (int xTile = 0; xTile < width; xTile += 8)
                {
                    for (int yPixel = yTile; yPixel < yTile + 4; yPixel++)
                    {
                        for (int xPixel = xTile; xPixel < yTile + 8; xPixel++)
                        {
                            if (xPixel >= width || yPixel >= height)
                                continue;

                            byte pix = data[i];

                            buf[(((yPixel * width) + xPixel) * 4) + 0] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 1] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 2] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 3] = 0xFF;

                            i++;
                        }
                    }
                }
            }

            return buf;
        }

        public static byte[] DecodeIA4(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] buf = new byte[width * height * 4];
            byte[] data = reader.ReadBytes(buf.Length);

            int i = 0;

            for (int yTile = 0; yTile < height; yTile += 4)
            {
                for (int xTile = 0; xTile < width; xTile += 8)
                {
                    for (int yPixel = yTile; yPixel < yTile + 4; yPixel++)
                    {
                        for (int xPixel = xTile; xPixel < yTile + 8; xPixel++)
                        {
                            if (xPixel >= width || yPixel >= height)
                                continue;

                            byte alpha = Convert.ToByte((data[i] >> 4) * 0x11);
                            byte pix = Convert.ToByte((data[i] & 0xF) * 0x11);

                            buf[(((yPixel * width) + xPixel) * 4)] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 1] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 2] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 3] = alpha;

                            i++;
                        }
                    }
                }
            }

            return buf;
        }

        public static byte[] DecodeIA8(ref EndianBinaryReader reader, int height, int width)
        {
            byte[] buf = new byte[width * height * 4];
            byte[] data = reader.ReadBytes(width * height * 4);

            int i = 0;

            for (int yTile = 0; yTile < height; yTile += 4)
            {
                for (int xTile = 0; xTile < width; xTile += 8)
                {
                    for (int yPixel = yTile; yPixel < yTile + 4; yPixel++)
                    {
                        for (int xPixel = xTile; xPixel < yTile + 8; xPixel++)
                        {
                            if (xPixel >= width || yPixel >= height)
                                continue;

                            byte pix = data[i];
                            i++;

                            byte alpha = data[i];
                            i++;

                            buf[(((yPixel * width) + xPixel) * 4)] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 1] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 2] = pix;
                            buf[(((yPixel * width) + xPixel) * 4) + 3] = alpha;
                        }
                    }
                }
            }

            return buf;
        }
    }
}
