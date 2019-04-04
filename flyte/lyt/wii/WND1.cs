using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using flyte.utils;

namespace flyte.lyt.wii
{
    class WND1 : PAN1
    {
        public WND1(ref EndianBinaryReader reader) : base (ref reader)
        {
            base.setType("Window");

            long startPos = reader.Pos() - 0x4C;

            mCoord1 = reader.ReadF32();
            mCoord2 = reader.ReadF32();
            mCoord3 = reader.ReadF32();
            mCoord4 = reader.ReadF32();
            mFrameCount = reader.ReadByte();
            mFlag = reader.ReadByte();
            reader.ReadUInt16(); // padding
            mWindowContentOffset = reader.ReadUInt32();
            mWindowFrameOffset = reader.ReadUInt32();

            mTopLeftColor = reader.ReadRGBAColor8();
            mTopRightColor = reader.ReadRGBAColor8();
            mBottomLeftColor = reader.ReadRGBAColor8();
            mBottomRightColor = reader.ReadRGBAColor8();
            mMaterialIndex = reader.ReadUInt16();
            mNumUVSets = reader.ReadByte();
            reader.ReadByte(); // padding

            mUVSets = new List<UVCoordSet>();

            for (byte i = 0; i < mNumUVSets; i++)
                mUVSets.Add(reader.ReadUVCoordSet());

            // now we read our window frames
            reader.Seek(startPos + mWindowFrameOffset);

            mFrameOffsets = new List<int>();

            for (byte i = 0; i < mFrameCount; i++)
                mFrameOffsets.Add(reader.ReadInt32());

            mFrames = new List<WND1Frame>();

            foreach(int offset in mFrameOffsets)
            {
                reader.Seek(startPos + offset);
                mFrames.Add(new WND1Frame(ref reader));
            }

            reader.Seek(startPos + mSectionSize);
        }

        float mCoord1;
        float mCoord2;
        float mCoord3;
        float mCoord4;
        byte mFrameCount;
        byte mFlag;

        uint mWindowContentOffset;
        uint mWindowFrameOffset;
        RGBAColor8 mTopLeftColor;
        RGBAColor8 mTopRightColor;
        RGBAColor8 mBottomLeftColor;
        RGBAColor8 mBottomRightColor;
        ushort mMaterialIndex;
        byte mNumUVSets;

        List<UVCoordSet> mUVSets;
        List<int> mFrameOffsets;
        List<WND1Frame> mFrames;
    }

    class WND1Frame
    {
        public WND1Frame(ref EndianBinaryReader reader)
        {
            mMaterialIndex = reader.ReadUInt16();
            mFlipType = reader.ReadByte();
            reader.ReadByte();
        }

        ushort mMaterialIndex;
        byte mFlipType;
    }
}
