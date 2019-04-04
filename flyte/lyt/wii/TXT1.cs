using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using flyte.utils;

namespace flyte.lyt.wii
{
    class TXT1 : PAN1
    {
        public TXT1(ref EndianBinaryReader reader) : base(ref reader)
        {
            base.setType("Text Box");

            long startPos = reader.Pos() - 0x4C;

            mBufferLength = reader.ReadUInt16();
            mStringLength = reader.ReadUInt16();
            mMaterialIdx = reader.ReadUInt16();
            mFontNum = reader.ReadUInt16();
            mAnotherOrigin = reader.ReadByte();

            reader.ReadBytes(0x3);
            mTextOffset = reader.ReadUInt32();
            mTopColor = reader.ReadRGBAColor8();
            mBottomColor = reader.ReadRGBAColor8();
            mSizeX = reader.ReadF32();
            mSizeY = reader.ReadF32();
            mCharacterSize = reader.ReadF32();
            mLineSize = reader.ReadF32();

            // just in case...
            mString = reader.ReadUTF16StringFrom(startPos + mTextOffset);

            reader.Seek(startPos + mSectionSize);
        }

        ushort mBufferLength;
        ushort mStringLength;
        ushort mMaterialIdx;
        ushort mFontNum;
        byte mAnotherOrigin;

        uint mTextOffset;
        RGBAColor8 mTopColor;
        RGBAColor8 mBottomColor;
        float mSizeX;
        float mSizeY;
        float mCharacterSize;
        float mLineSize;
        string mString;
    }
}
