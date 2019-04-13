using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using flyte.utils;

namespace flyte.lyt._3ds
{
    class WND1 : PAN1
    {
        public WND1(ref EndianBinaryReader reader, ref MAT1 materials) : base(ref reader)
        {
            base.setType("Window");

            long startPos = reader.Pos() - 0x4C;

            mContentOverflowLeft = reader.ReadF32();
            mContentOverflowRight = reader.ReadF32();
            mContentOverflowTop = reader.ReadF32();
            mContentOverflowBottom = reader.ReadF32();
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

            foreach (int offset in mFrameOffsets)
            {
                reader.Seek(startPos + offset);
                mFrames.Add(new WND1Frame(ref reader));
            }

            mMaterialName = materials.getMaterialNameFromIndex(mMaterialIndex);

            reader.Seek(startPos + mSectionSize);
        }

        float mContentOverflowLeft;
        float mContentOverflowRight;
        float mContentOverflowTop;
        float mContentOverflowBottom;
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

        string mMaterialName;

        List<UVCoordSet> mUVSets;
        List<int> mFrameOffsets;
        List<WND1Frame> mFrames;

        [DisplayName("Content Overflow Left"), CategoryAttribute("Window Settings"), DescriptionAttribute("The content overflow of the left side of the window.")]
        public float ContentOverFlowLeft
        {
            get { return mContentOverflowLeft; }
            set { mContentOverflowLeft = value; }
        }

        [DisplayName("Content Overflow Right"), CategoryAttribute("Window Settings"), DescriptionAttribute("The content overflow of the right side of the window.")]
        public float ContentOverFlowRight
        {
            get { return mContentOverflowRight; }
            set { mContentOverflowRight = value; }
        }

        [DisplayName("Content Overflow Top"), CategoryAttribute("Window Settings"), DescriptionAttribute("The content overflow of the top of the window.")]
        public float ContentOverFlowTop
        {
            get { return mContentOverflowTop; }
            set { mContentOverflowTop = value; }
        }

        [DisplayName("Content Overflow Bottom"), CategoryAttribute("Window Settings"), DescriptionAttribute("The content overflow of the bottom of the window.")]
        public float ContentOverFlowBottom
        {
            get { return mContentOverflowBottom; }
            set { mContentOverflowBottom = value; }
        }

        [DisplayName("Material"), CategoryAttribute("Window Settings"), DescriptionAttribute("The material name used with the window.")]
        public string Material
        {
            get { return mMaterialName; }
            set { mMaterialName = value; }
        }
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
