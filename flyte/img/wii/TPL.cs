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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.ComponentModel;
using flyte.io;
using static flyte.utils.Endian;

namespace flyte.img.wii
{
    class TPL : ImageContainerBase
    {
        public TPL(ref EndianBinaryReader reader)
        {
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

            mImages = new List<ImageBase>();
            mPalettes = new List<Palette>();

            foreach (ImageOffset offset in mImageOffsets)
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
        
        /// <summary>
        /// Gets an image from the image list and returns the Bitmap.
        /// </summary>
        /// <param name="imageIndex">The index of the image to return.</param>
        /// <returns>A bitmap representation of the image.</returns>
        public Bitmap getImageBitmap(int imageIndex)
        {
            return mImages[imageIndex].getImageBitmap();
        }

        public override ImageBase getImage(int imageIndex)
        {
            return mImages[imageIndex];
        }

        uint mIdentifier;
        uint mNumImages;
        uint mImageTableOffset;

        List<ImageOffset> mImageOffsets;
        List<ImageBase> mImages;
        List<Palette> mPalettes;
    }

    struct ImageOffset
    {
        public uint mImgHeader;
        public uint mPaletteHeader;
    }

    class TPLImage : ImageBase
    {
        public enum ImageFormat
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

        public TPLImage(ref EndianBinaryReader reader)
        {
            base.setType(ImagePlatform.Wii);

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

            reader.Seek(mImageDataAddr);

            mOutImg = null;
            bool unsupported = false;

            Console.WriteLine("Format: " + mFormat);

            if (mWidth % 4 != 0)
                mWidth += (ushort)(4 - (mWidth % 4));

            if (mHeight % 4 != 0)
                mHeight += (ushort)(4 - (mHeight % 4));

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
                case ImageFormat.RGB565:
                    mOutImg = ImageDecompressor.DecodeRGB565(ref reader, mHeight, mWidth);
                    break;
                case ImageFormat.RGB5A3:
                    mOutImg = ImageDecompressor.DecodeRGB5A3(ref reader, mHeight, mWidth);
                    break;
                default:
                    Console.WriteLine("Format " + mFormat + " not supported...");
                    unsupported = true;
                    break;
            }

            if (unsupported)
                return;
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

        [DisplayName("Format"), CategoryAttribute("General"), DescriptionAttribute("The image format.")]
        public ImageFormat Format
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

        [DisplayName("Min Filter"), CategoryAttribute("Filter"), DescriptionAttribute("Min filter.")]
        public uint MinFilter
        {
            get { return mMinFilter; }
            set { mMinFilter = value; }
        }

        [DisplayName("Mag Filter"), CategoryAttribute("Filter"), DescriptionAttribute("Mag filter.")]
        public uint MagFilter
        {
            get { return mMagFilter; }
            set { mMagFilter = value; }
        }

        [DisplayName("Minimum LOD"), CategoryAttribute("LOD"), DescriptionAttribute("Minimum LOD.")]
        public byte MinLOD
        {
            get { return mMinLOD; }
            set { mMinLOD = value; }
        }

        [DisplayName("Maximum LOD"), CategoryAttribute("LOD"), DescriptionAttribute("Maximum LOD.")]
        public byte MaxLOD
        {
            get { return mMaxLOD; }
            set { mMaxLOD = value; }
        }

        [DisplayName("LOD Bias"), CategoryAttribute("LOD"), DescriptionAttribute("LOD Bias.")]
        public float LODBias
        {
            get { return mLODBias; }
            set { mLODBias = value; }
        }

        [DisplayName("Edge LOD Enabled"), CategoryAttribute("LOD"), DescriptionAttribute("Edge LOD.")]
        public byte EdgeLODEnabled
        {
            get { return mEdgeLODEnable; }
            set { mEdgeLODEnable = value; }
        }

        [DisplayName("Wrap S"), CategoryAttribute("Wrap"), DescriptionAttribute("Wrap S.")]
        public uint WrapS
        {
            get { return mWrapS; }
            set { mWrapS = value; }
        }

        [DisplayName("Wrap T"), CategoryAttribute("Wrap"), DescriptionAttribute("Wrap T.")]
        public uint WrapT
        {
            get { return mWrapT; }
            set { mWrapT = value; }
        }
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

                            // does this have alpha?
                            if ((val & 0x8000) == 0x8000)
                            {
                                r = (byte)(val & 0x1F);
                                r = (byte)(r << 3 | r >> 2);

                                g = (byte)((val & 0x3E0) >> 5);
                                g = (byte)(g << 3 | g >> 2);

                                b = (byte)((val & 0x7C00) >> 10);
                                b = (byte)(b << 3 | b >> 2);

                                a = 0xFF;
                            }
                            else
                            {
                                r = (byte)((val & 0xF0) >> 4);
                                r = (byte)((r << 4) | r);

                                g = (byte)((val & 0xF00) >> 8);
                                g = (byte)((g << (8 - 4)) | g);

                                b = (byte)((val & 0x7000) >> 12);
                                b = (byte)(b << 5 | (b << 3) | (b >> 1));

                                a = (byte)(val & 0xF);
                                a = (byte)((a << 4) | a);
                            }

                            image[output++] = r;
                            image[output++] = g;
                            image[output++] = b;
                            image[output] = a;
                        }
                    }
                }
            }

            return image;
        }
    }
}