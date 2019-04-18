/*
    © 2019 - shibboleet
    flyte is free software: you can redistribute it and/or modify it under
    the terms of the GNU General Public License as published by the Free
    Software Foundation, either version 3 of the License, or (at your option)
    any later version.
    flyte is distributed in the hope that it will be useful, but WITHOUT ANY 
    WARRANTY; See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along 
    with flyte. If not, see http://www.gnu.org/licenses/.
*/

using flyte.io;
using flyte.utils;
using System.Collections.Generic;
using System;
using static flyte.utils.Bit;
using flyte.lyt.wii.material;

namespace flyte.lyt.wii
{
    public class MAT1 : LayoutBase
    {
        public MAT1(ref EndianBinaryReader reader)
        {
            long basePos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mNumMaterials = reader.ReadUInt16();
            mUnk0A = reader.ReadUInt16();

            mMaterials = new List<MaterialBase>();

            List<uint> offsets = new List<uint>(mNumMaterials);

            for (ushort i = 0; i < mNumMaterials; i++)
                offsets.Add(reader.ReadUInt32());

            foreach(uint offset in offsets)
            {
                reader.Seek(offset + basePos);
                mMaterials.Add(new Material(ref reader));
            }

            reader.Seek(basePos + mSectionSize);
        }

        public string getMaterialNameFromIndex(int idx)
        {
            return mMaterials[idx].getName();
        }

        public override List<MaterialBase> getMaterials() { return mMaterials; }

        public override List<string> getMaterialNames()
        {
            List<string> strs = new List<string>();

            foreach (Material mat in mMaterials)
                strs.Add(mat.getName());

            return strs;
        }

        uint mSectionSize;
        ushort mNumMaterials;
        ushort mUnk0A;

        public List<MaterialBase> mMaterials;
    }

    public class Material : MaterialBase
    {
        public Material(ref EndianBinaryReader reader)
        {
            base.setType(Type.Wii);

            mMaterialName = reader.ReadString(0x14).Replace("\0", "");

            mForeColor = reader.ReadRGBAColor16();
            mBackColor = reader.ReadRGBAColor16();
            mColorReg3 = reader.ReadRGBAColor16();
            mTevColor1 = reader.ReadRGBAColor8();
            mTevColor2 = reader.ReadRGBAColor8();
            mTevColor3 = reader.ReadRGBAColor8();
            mTevColor4 = reader.ReadRGBAColor8();
            mFlags = reader.ReadUInt32();

            mHasMaterialColor = Convert.ToBoolean(ExtractBits(mFlags, 1, 4));
            mHasChannelControl = Convert.ToBoolean(ExtractBits(mFlags, 1, 6));
            mHasBlendMode = Convert.ToBoolean(ExtractBits(mFlags, 1, 7));
            mHasAlphaCompare = Convert.ToBoolean(ExtractBits(mFlags, 1, 8));
            mTevStageCount = ExtractBits(mFlags, 5, 9);
            mIndTexStageCount = ExtractBits(mFlags, 3, 14);
            mIndTexSRTCount = ExtractBits(mFlags, 2, 17);
            mHasTevSwapTable = Convert.ToBoolean(ExtractBits(mFlags, 1, 19));
            mTexCoordGenCount = ExtractBits(mFlags, 4, 20);
            mTexSRTCount = ExtractBits(mFlags, 4, 24);
            mTexMapCount = ExtractBits(mFlags, 4, 28);

            mTexMaps = new List<TexMap>();

            for (int i = 0; i < mTexMapCount; i++)
                mTexMaps.Add(new TexMap(ref reader));

            mTexSRTs = new List<TexSRT>();

            for (int i = 0; i < mTexSRTCount; i++)
                mTexSRTs.Add(new TexSRT(ref reader));

            mTexCoordGens = new List<TexCoordGen>();

            for (int i = 0; i < mTexCoordGenCount; i++)
                mTexCoordGens.Add(new TexCoordGen(ref reader));

            if (mHasChannelControl)
                mChanCtrl = new ChanCtrl(ref reader);

            if (mHasMaterialColor)
                mMaterialColor = reader.ReadRGBAColor8();

            if (mHasTevSwapTable)
                mTevSwapTable = new TevSwapTable(ref reader);

            mIndTexSRT = new List<IndTexSRT>();

            for (int i = 0; i < mIndTexSRTCount; i++)
                mIndTexSRT.Add(new IndTexSRT(ref reader));

            mIndTexStages = new List<IndTexStage>();

            for (int i = 0; i < mIndTexStageCount; i++)
                mIndTexStages.Add(new IndTexStage(ref reader));

            mTevStages = new List<TevStage>();

            for (int i = 0; i < mTevStageCount; i++)
                mTevStages.Add(new TevStage(ref reader));

            if (mHasAlphaCompare)
                mAlphaCompare = new AlphaCompare(ref reader);

            if (mHasBlendMode)
                mBlendMode = new BlendMode(ref reader);
        }

        public List<TexSRT> getTextureSRTs() { return mTexSRTs; }
        public TevSwapTable getSwapTable() { return mTevSwapTable; }

        RGBAColor16 mForeColor;
        RGBAColor16 mBackColor;
        RGBAColor16 mColorReg3;
        RGBAColor8 mTevColor1;
        RGBAColor8 mTevColor2;
        RGBAColor8 mTevColor3;
        RGBAColor8 mTevColor4;
        uint mFlags;

        bool mHasMaterialColor;
        bool mHasChannelControl;
        bool mHasBlendMode;
        bool mHasAlphaCompare;
        uint mTevStageCount;
        uint mIndTexStageCount;
        uint mIndTexSRTCount;
        bool mHasTevSwapTable;
        uint mTexCoordGenCount;
        uint mTexSRTCount;
        uint mTexMapCount;

        AlphaCompare mAlphaCompare;
        BlendMode mBlendMode;
        ChanCtrl mChanCtrl;
        List<TevStage> mTevStages;
        List<IndTexStage> mIndTexStages;
        List<IndTexSRT> mIndTexSRT;
        TevSwapTable mTevSwapTable;
        List<TexCoordGen> mTexCoordGens;
        List<TexSRT> mTexSRTs;
        List<TexMap> mTexMaps;

        RGBAColor8 mMaterialColor;
    }
}
