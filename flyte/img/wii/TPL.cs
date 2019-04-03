using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.img.wii
{
    class TPL
    {
        public TPL(ref EndianBinaryReader reader)
        {
            reader.SetEndianess(EndianBinaryReader.Endianess.Big);
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

            foreach(ImageOffset offset in mImageOffsets)
            {
                reader.Seek(offset.mImgHeader);
                TPLImage image = new TPLImage(ref reader);
                mImages.Add(image);
            }
        }

        uint mIdentifier;
        uint mNumImages;
        uint mImageTableOffset;

        List<ImageOffset> mImageOffsets;
        List<TPLImage> mImages;
    }

    struct ImageOffset
    {
        public uint mImgHeader;
        public uint mPaletteHeader;
    }

    class TPLImage
    {
        enum ImageFormat
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
    }
}
