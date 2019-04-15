using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using flyte.utils;

namespace flyte.lyt.common
{
    class PIC1 : PAN1
    {
       public PIC1(ref EndianBinaryReader reader, ref MAT1 materials, uint versionMajor, uint versionMinor) : base(ref reader)
        {
            base.setType("Picture");

            mTopLeftColor = reader.ReadRGBAColor8();
            mTopRightColor = reader.ReadRGBAColor8();
            mBottomLeftColor = reader.ReadRGBAColor8();
            mBottomRightColor = reader.ReadRGBAColor8();

            mMaterialIndex = reader.ReadUInt16();
            mUVCount = reader.ReadByte();
            reader.ReadByte();

            if (versionMajor == 8 && versionMinor == 2)
                reader.ReadUInt32();

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

        string mMaterialName;
        List<UVCoordSet> mUVs;
    }
}
