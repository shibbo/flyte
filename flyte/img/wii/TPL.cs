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
using flyte.utils;

namespace flyte.img.wii
{
    class TPL : ImageContainerBase
    {
        public TPL(ref EndianBinaryReader reader)
        {
            reader.SetEndianess(Endian.Endianess.Big);
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
        public TPLImage(ref EndianBinaryReader reader)
        {
            base.setType(ImagePlatform.Wii);

            mHeight = reader.ReadUInt16();
            mWidth = reader.ReadUInt16();
            mFormat = (ImageDecoder.ImageFormat)reader.ReadUInt32();
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
                case ImageDecoder.ImageFormat.I4:
                    mOutImg = ImageDecoder.DecodeI4(ref reader, mHeight, mWidth);
                    break;
                case ImageDecoder.ImageFormat.I8:
                    mOutImg = ImageDecoder.DecodeI8(ref reader, mHeight, mWidth);
                    break;
                case ImageDecoder.ImageFormat.IA4:
                    mOutImg = ImageDecoder.DecodeIA4(ref reader, mHeight, mWidth);
                    break;
                case ImageDecoder.ImageFormat.IA8:
                    mOutImg = ImageDecoder.DecodeIA8(ref reader, mHeight, mWidth);
                    break;
                case ImageDecoder.ImageFormat.RGB565:
                    mOutImg = ImageDecoder.DecodeRGB565(ref reader, mHeight, mWidth);
                    break;
                case ImageDecoder.ImageFormat.RGB5A3:
                    mOutImg = ImageDecoder.DecodeRGB5A3(ref reader, mHeight, mWidth);
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
        ImageDecoder.ImageFormat mFormat;
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
        public ImageDecoder.ImageFormat Format
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
}