using flyte.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.utils;

namespace flyte.lyt.wii
{
    class PIC1 : PAN1
    {
        public PIC1(ref EndianBinaryReader reader) : base(ref reader)
        {
            base.setType("Picture");

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
        }

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