using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using flyte.utils;

namespace flyte.lyt.common
{
    class WND1 : PAN1
    {
        public WND1(ref EndianBinaryReader reader, ref MAT1 materials) : base (ref reader)
        {
            base.setType("Window");

            long startPos = reader.Pos() - 0x54;

            mInflationLeft = reader.ReadInt16();
            mInflationRight = reader.ReadInt16();
            mInflationTop = reader.ReadInt16();
            mInflationBottom = reader.ReadInt16();
            mFrameSizeLeft = reader.ReadInt16();
            mFrameSizeRight = reader.ReadInt16();
            mFrameSizeTop = reader.ReadInt16();
            mFrameSizeBottom = reader.ReadInt16();
            mFrameCount = reader.ReadByte();
            mFlag = reader.ReadByte();
            reader.ReadUInt16();
            mContentOffset = reader.ReadUInt32();
            mFrameOffsetTableOffset = reader.ReadUInt32();

            reader.Seek(mContentOffset + startPos);
            mContent = new WindowContent(ref reader, ref materials);

            reader.Seek(mFrameOffsetTableOffset + startPos);

            mFrames = new List<WindowFrame>();

            uint[] offsets = new uint[mFrameCount];

            for (int i = 0; i < mFrameCount; i++)
                offsets[i] = reader.ReadUInt32();

            foreach(int offset in offsets)
            {
                reader.Seek(offset + startPos);
                mFrames.Add(new WindowFrame(ref reader, ref materials));
            }

            reader.Seek(startPos + mSectionSize);
        }

        short mInflationLeft;
        short mInflationRight;
        short mInflationTop;
        short mInflationBottom;
        short mFrameSizeLeft;
        short mFrameSizeRight;
        short mFrameSizeTop;
        short mFrameSizeBottom;
        byte mFrameCount;
        byte mFlag;

        uint mContentOffset;
        uint mFrameOffsetTableOffset;

        List<WindowFrame> mFrames;

        WindowContent mContent;
    }

    class WindowContent
    {
        public WindowContent(ref EndianBinaryReader reader, ref MAT1 materials)
        {
            mTopLeftColor = reader.ReadRGBAColor8();
            mTopRightColor = reader.ReadRGBAColor8();
            mBottomLeftColor = reader.ReadRGBAColor8();
            mBottomRightColor = reader.ReadRGBAColor8();
            mMaterialIndex = reader.ReadUInt16();
            mUVCount = reader.ReadByte();
            reader.ReadByte();

            mUVs = new List<UVCoordSet>();

            for (byte i = 0; i < mUVCount; i++)
                mUVs.Add(reader.ReadUVCoordSet());

            mMaterialName = materials.getMaterialNameFromIndex(mMaterialIndex);
        }

        RGBAColor8 mTopLeftColor;
        RGBAColor8 mTopRightColor;
        RGBAColor8 mBottomLeftColor;
        RGBAColor8 mBottomRightColor;
        ushort mMaterialIndex;
        byte mUVCount;

        List<UVCoordSet> mUVs;

        string mMaterialName;
    }

    class WindowFrame
    {
        public WindowFrame(ref EndianBinaryReader reader, ref MAT1 materials)
        {
            mMaterialIndex = reader.ReadUInt16();
            mFlip = reader.ReadByte();

            reader.ReadByte();

            mMaterialName = materials.getMaterialNameFromIndex(mMaterialIndex);
        }

        ushort mMaterialIndex;
        byte mFlip;

        string mMaterialName;
    }
}
