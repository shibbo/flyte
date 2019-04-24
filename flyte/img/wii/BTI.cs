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
using flyte.utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace flyte.img
{
    class BTI : ImageContainerBase
    {
        public BTI(ref EndianBinaryReader reader)
        {
            mImage = new BTIImage(ref reader);
        }

        public Bitmap getImageBitmap(int imageIndex)
        {
            return mImage.getImageBitmap();
        }

        public override ImageBase getImage(int imageIndex)
        {
            return mImage;
        }

        BTIImage mImage;
    }

    class BTIImage : ImageBase
    {
        public enum FilterType : byte
        {
            Near = 0,
            Linear = 1
        }

        public BTIImage(ref EndianBinaryReader reader)
        {
            base.setType(ImagePlatform.GC);

            reader.SetEndianess(Endian.Endianess.Big);

            mFormat = (ImageDecoder.ImageFormat)reader.ReadByte();
            mAlphaEnabled = reader.ReadByte() != 0;
            mWidth = reader.ReadUInt16();
            mHeight = reader.ReadUInt16();
            mWrapS = reader.ReadByte();
            mWrapT = reader.ReadByte();
            mPaletteFormat = reader.ReadUInt16();
            mPaletteCount = reader.ReadUInt16();
            mPaletteDataOffset = reader.ReadUInt32();
            reader.ReadUInt32();
            mMagFilter = (FilterType)reader.ReadByte();
            mMinFilter = (FilterType)reader.ReadByte();
            reader.ReadUInt16();
            mImageCount = reader.ReadByte();

            // this isn't a usual thing to see
            if (mImageCount != 0x1)
                Console.WriteLine("Multiple images alert!!");

            reader.ReadByte();
            reader.ReadUInt16();
            mImageDataOffset = reader.ReadUInt32();

            reader.Seek(mImageDataOffset);

            mOutImg = null;
            bool unsupported = false;
            Console.WriteLine("Format: " + mFormat);

            if (mWidth % 2 != 0)
                mWidth += (ushort)(2 - (mWidth % 2));

            if (mHeight % 2 != 0)
                mHeight += (ushort)(2 - (mHeight % 2));

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

            // so we just do our extraction here lol
            mBitmap = new Bitmap(mWidth, mHeight);
            var img = mBitmap.LockBits(new Rectangle(0, 0, mBitmap.Width, mBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(mOutImg, 0, img.Scan0, mOutImg.Length);
            mBitmap.UnlockBits(img);
        }

        public override Bitmap getImageBitmap() { return mBitmap; }

        ImageDecoder.ImageFormat mFormat;
        bool mAlphaEnabled;
        ushort mWidth;
        ushort mHeight;
        byte mWrapS;
        byte mWrapT;
        ushort mPaletteFormat;
        ushort mPaletteCount;
        uint mPaletteDataOffset;
        FilterType mMagFilter;
        FilterType mMinFilter;
        byte mImageCount;
        uint mImageDataOffset;

        byte[] mOutImg;
        Bitmap mBitmap;

        [DisplayName("Format"), CategoryAttribute("General"), DescriptionAttribute("The image format.")]
        public ImageDecoder.ImageFormat Format
        {
            get { return mFormat; }
            set { mFormat = value; }
        }

        [DisplayName("Is Alpha Enabled"), CategoryAttribute("General"), DescriptionAttribute("Is the alpha component enabled?")]
        public bool AlphaEnabled
        {
            get { return mAlphaEnabled; }
            set { mAlphaEnabled = value; }
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

        [DisplayName("Wrap S"), CategoryAttribute("Wrap"), DescriptionAttribute("Wrap S.")]
        public byte WrapS
        {
            get { return mWrapS; }
            set { mWrapS = value; }
        }

        [DisplayName("Wrap T"), CategoryAttribute("Wrap"), DescriptionAttribute("Wrap T.")]
        public byte WrapT
        {
            get { return mWrapT; }
            set { mWrapT = value; }
        }

        [DisplayName("MagFilter"), CategoryAttribute("Filter"), DescriptionAttribute("MagFilter")]
        public FilterType MagFilter
        {
            get { return mMagFilter; }
            set { mMagFilter = value; }
        }

        [DisplayName("MinFilter"), CategoryAttribute("Filter"), DescriptionAttribute("MinFilter")]
        public FilterType MinFilter
        {
            get { return mMinFilter; }
            set { mMinFilter = value; }
        }
    }
}
