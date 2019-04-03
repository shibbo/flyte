using flyte.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.utils;

namespace flyte.lyt.wii
{
    class PIC1 : LayoutBase
    {
        public PIC1(ref EndianBinaryReader reader) : base()
        {
            long startPos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mFlag_0 = reader.ReadByte();
            mFlag_1 = reader.ReadByte();
            mAlpha = reader.ReadByte();
            reader.ReadByte();
            mName = reader.ReadString(0x10).Replace("\0", "");
            mUserInfo = reader.ReadString(0x8);
            mTransX = reader.ReadF32();
            mTransY = reader.ReadF32();
            mTransZ = reader.ReadF32();
            mRotX = reader.ReadF32();
            mRotY = reader.ReadF32();
            mRotZ = reader.ReadF32();
            mScaleX = reader.ReadF32();
            mScaleY = reader.ReadF32();
            mScaleZ = reader.ReadF32();
            mWidth = reader.ReadF32();
            mHeight = reader.ReadF32();

            mTopLeftColor = reader.ReadRGBAColor8();
            mTopRightColor = reader.ReadRGBAColor8();
            mBottomLeftColor = reader.ReadRGBAColor8();
            mBottomRightColor = reader.ReadRGBAColor8();
            mMaterialIndex = reader.ReadUInt16();
            mNumUVSets = reader.ReadByte();
            mUnk5F = reader.ReadByte();

            mUVCoordinates = new List<UVCoordSet>();

            for (byte i = 0; i < mNumUVSets; i++)
                mUVCoordinates.Add(reader.ReadUVCoordSet());

            reader.Seek(startPos + mSectionSize);
        }

        uint mSectionSize;
        byte mFlag_0;
        byte mFlag_1;
        byte mAlpha;
        string mName;
        string mUserInfo;
        float mTransX;
        float mTransY;
        float mTransZ;
        float mRotX;
        float mRotY;
        float mRotZ;
        float mScaleX;
        float mScaleY;
        float mScaleZ;
        float mWidth;
        float mHeight;

        RGBAColor8 mTopLeftColor;
        RGBAColor8 mTopRightColor;
        RGBAColor8 mBottomLeftColor;
        RGBAColor8 mBottomRightColor;
        ushort mMaterialIndex;
        byte mNumUVSets;
        byte mUnk5F;

        List<UVCoordSet> mUVCoordinates;
    }
}