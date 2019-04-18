using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using flyte.lyt._3ds.material;
using flyte.utils;
using static flyte.utils.Bit;

namespace flyte.lyt._3ds
{
    class MAT1 : LayoutBase
    {
        public MAT1(ref EndianBinaryReader reader)
        {
            long startPos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mMaterialCount = reader.ReadUInt32();

            mSectionOffsets = new uint[mMaterialCount];

            for (int i = 0; i < mMaterialCount; i++)
                mSectionOffsets[i] = reader.ReadUInt32();

            mMaterials = new List<MaterialBase>();

            foreach(int offset in mSectionOffsets)
            {
                reader.Seek(offset + startPos);
                mMaterials.Add(new Material(ref reader));
            }

            reader.Seek(startPos + mSectionSize);
        }

        public string getMaterialNameFromIndex(int idx)
        {
            return mMaterials[idx].getName();
        }

        public override List<MaterialBase> getMaterials()
        {
            return mMaterials;
        }

        public override List<string> getMaterialNames()
        {
            List<string> strs = new List<string>();

            foreach (Material mat in mMaterials)
                strs.Add(mat.getName());

            return strs;
        }

        uint mSectionSize;
        uint mMaterialCount;

        uint[] mSectionOffsets;

        List<MaterialBase> mMaterials;
    }

    class Material : MaterialBase
    {
        public Material(ref EndianBinaryReader reader)
        {
            base.setType(Type._3DS);

            mName = reader.ReadString(0x14).Replace("\0", "");
            mTevColor = reader.ReadRGBAColor8();

            mTevConstantColors = new RGBAColor8[0x6];

            for (int i = 0; i < 6; i++)
                mTevConstantColors[i] = reader.ReadRGBAColor8();

            mFlags = reader.ReadUInt32();

            mTexMapCount = ExtractBits(mFlags, 2, 0);
            mTexMtxCount = ExtractBits(mFlags, 2, 2);
            mTexCoordGenCount = ExtractBits(mFlags, 2, 4);
            mTevStageCount = ExtractBits(mFlags, 2, 6);
            mHasAlphaCompare = Convert.ToBoolean(ExtractBits(mFlags, 1, 9));
            mHasBlendMode = Convert.ToBoolean(ExtractBits(mFlags, 1, 10));
            mUseTextureOnly = Convert.ToBoolean(ExtractBits(mFlags, 1, 11));
            mSeperateBlendMode = Convert.ToBoolean(ExtractBits(mFlags, 1, 12));
            mHasIndParam = Convert.ToBoolean(ExtractBits(mFlags, 1, 14));
            mProjTexGenParamCount = ExtractBits(mFlags, 2, 15);
            mHasFontShadowParam = Convert.ToBoolean(ExtractBits(mFlags, 1, 17));

            mTexMaps = new List<TexMap>();

            for (int i = 0; i < mTexMapCount; i++)
                mTexMaps.Add(new TexMap(ref reader));

            mTexSRTs = new List<TexSRT>();

            for (int i = 0; i < mTexMtxCount; i++)
                mTexSRTs.Add(new TexSRT(ref reader));

            mTexCoords = new List<TexCoordGen>();

            for (int i = 0; i < mTexCoordGenCount; i++)
                mTexCoords.Add(new TexCoordGen(ref reader));

            mTevStages = new List<TevStage>();

            for (int i = 0; i < mTevStageCount; i++)
                mTevStages.Add(new TevStage(ref reader));

            if (mHasAlphaCompare)
                mAlphaCompare = new AlphaCompare(ref reader);
            if (mHasBlendMode)
                mBlendMode_Blend = new BlendMode(ref reader);
            if (mSeperateBlendMode)
                mBlendMode_Logic = new BlendMode(ref reader);
            if (mHasIndParam)
                mIndParameter = new IndirectParameter(ref reader);

            mProjTexGenParams = new List<ProjectionTexGenParam>();

            for (int i = 0; i < mProjTexGenParamCount; i++)
                mProjTexGenParams.Add(new ProjectionTexGenParam(ref reader));

            if (mHasFontShadowParam)
                mFontShadowParam = new FontShadowParameter(ref reader);
        }

        string mName;
        RGBAColor8 mTevColor;
        RGBAColor8[] mTevConstantColors;
        uint mFlags;

        uint mTexMapCount;
        uint mTexMtxCount;
        uint mTexCoordGenCount;
        uint mTevStageCount;
        bool mHasAlphaCompare;
        bool mHasBlendMode;
        bool mUseTextureOnly;
        bool mSeperateBlendMode;
        bool mHasIndParam;
        uint mProjTexGenParamCount;
        bool mHasFontShadowParam;

        List<TexMap> mTexMaps;
        List<TexSRT> mTexSRTs;
        List<TexCoordGen> mTexCoords;
        List<TevStage> mTevStages;
        AlphaCompare mAlphaCompare;
        BlendMode mBlendMode_Blend;
        BlendMode mBlendMode_Logic;
        IndirectParameter mIndParameter;
        List<ProjectionTexGenParam> mProjTexGenParams;
        FontShadowParameter mFontShadowParam;
    }
}
