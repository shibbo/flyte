using flyte.io;
using flyte.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt.wii
{
    class MAT1
    {
        public MAT1(ref EndianBinaryReader reader)
        {
            long basePos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mNumMaterials = reader.ReadUInt16();
            mUnk0A = reader.ReadUInt16();

            mMaterials = new List<Material>();

            long offsStartPos = reader.Pos();

            for (ushort i = 0; i < mNumMaterials; i++)
            {
                uint offset = reader.ReadUInt32();
                reader.Seek(offset + basePos);
                mMaterials.Add(new Material(ref reader));

                reader.Seek(offsStartPos + (i * 4));
            }

            reader.Seek(basePos + mSectionSize);
        }

        uint mSectionSize;
        ushort mNumMaterials;
        ushort mUnk0A;

        List<Material> mMaterials;
    }

    class Material
    {
        public Material(ref EndianBinaryReader reader)
        {
            mMaterialName = reader.ReadString(0x14).Replace("\0", "");

            mForeColor = reader.ReadRGBAColor16();
            mBackColor = reader.ReadRGBAColor16();
            mColorReg3 = reader.ReadRGBAColor16();
            mTevColor1 = reader.ReadRGBAColor8();
            mTevColor2 = reader.ReadRGBAColor8();
            mTevColor3 = reader.ReadRGBAColor8();
            mTevColor4 = reader.ReadRGBAColor8();
            mFlags = reader.ReadUInt32();
        }

        string mMaterialName;
        RGBAColor16 mForeColor;
        RGBAColor16 mBackColor;
        RGBAColor16 mColorReg3;
        RGBAColor8 mTevColor1;
        RGBAColor8 mTevColor2;
        RGBAColor8 mTevColor3;
        RGBAColor8 mTevColor4;
        uint mFlags;
    }
}
