using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using flyte.lyt.common.material;
using flyte.utils;
using static flyte.utils.Bit;

namespace flyte.lyt.common
{
    class MAT1
    {
        public MAT1(ref EndianBinaryReader reader, uint version)
        {
            long startPos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();

            if (version == 0x8030000)
                mMaterialCount = reader.ReadUInt32();
            else
            {
                mMaterialCount = reader.ReadUInt16();
                reader.ReadBytes(0x2);
            }

            mSectionOffsets = new uint[mMaterialCount];

            for (int i = 0; i < mMaterialCount; i++)
                mSectionOffsets[i] = reader.ReadUInt32();

            mMaterials = new List<Material>();

            foreach (int offset in mSectionOffsets)
            {
                reader.Seek(offset + startPos);
                mMaterials.Add(new Material(ref reader, version));
            }

            reader.Seek(startPos + mSectionSize);
        }

        public string getMaterialNameFromIndex(int idx)
        {
            return mMaterials[idx].getName();
        }

        public List<Material> getMaterials() { return mMaterials; }

        public List<string> getMaterialNames()
        {
            List<string> strs = new List<string>();

            foreach (Material mat in mMaterials)
                strs.Add(mat.getName());

            return strs;
        }

        uint mSectionSize;
        uint mMaterialCount;

        uint[] mSectionOffsets;

        List<Material> mMaterials;
    }

    class Material
    {
        public Material(ref EndianBinaryReader reader, uint version)
        {
            mName = reader.ReadString(0x1C).Replace("\0", "");

            // Switch BFLYT
            if (version == 0x8030000)
            {
                mFlags = reader.ReadUInt32();
                mUnk = reader.ReadUInt32();

                mBlackColor = reader.ReadRGBAColor8();
                mWhiteColor = reader.ReadRGBAColor8();
            }
            // WiiU BFLYT
            else
            {
                mBlackColor = reader.ReadRGBAColor8();
                mWhiteColor = reader.ReadRGBAColor8();
                mFlags = reader.ReadUInt32();
            }

            mTexMapCount = Convert.ToUInt32(mFlags & 0x3);
            mTexMtxCount = Convert.ToUInt32((mFlags >> 2) & 0x3);
            mTexCoordGenCount = Convert.ToUInt32((mFlags >> 4) & 0x3);
            mTevStageCount = Convert.ToUInt32((mFlags >> 6) & 0x7);
            mHasAlphaCompare = Convert.ToBoolean((mFlags >> 9) & 0x1);
            mHasBlendMode = Convert.ToBoolean((mFlags >> 10) & 0x1);
            mUseTextureOnly = Convert.ToBoolean((mFlags >> 11) & 0x1);
            mSeperateBlendMode = Convert.ToBoolean((mFlags >> 12) & 0x1);
            mHasIndParam = Convert.ToBoolean((mFlags >> 14) & 0x1);
            mProjTexGenParamCount = Convert.ToUInt32((mFlags >> 15) & 0x3);
            mHasFontShadowParam = Convert.ToBoolean((mFlags >> 17) & 0x1);
            mThresholdingAlphaInterpolation = Convert.ToBoolean((mFlags >> 18) & 0x1);

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

        public string getName() { return mName; }

        string mName;
        RGBAColor8 mBlackColor;
        RGBAColor8 mWhiteColor;
        uint mUnk;
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
        bool mThresholdingAlphaInterpolation;

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