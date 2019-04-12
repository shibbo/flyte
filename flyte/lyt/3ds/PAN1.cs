using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using static flyte.utils.Bit;

namespace flyte.lyt._3ds
{
    class PAN1 : LayoutBase
    {
        public PAN1(ref EndianBinaryReader reader) : base()
        {
            base.setType("Panel");

            mSectionSize = reader.ReadUInt32();

            mFlags = reader.ReadByte();
            mIsVisible = Convert.ToBoolean(ExtractBits(mFlags, 1, 0));
            mInfluencedAlpha = Convert.ToBoolean(ExtractBits(mFlags, 1, 1));
            mLocationAdjust = Convert.ToBoolean(ExtractBits(mFlags, 1, 2));

            mOrigin = reader.ReadByte();
            mAlpha = reader.ReadByte();
            mPaneMagFlags = reader.ReadByte();
            mIgnorePartsMagnify = Convert.ToBoolean(ExtractBits(mPaneMagFlags, 1, 0));
            mAdjustToPartsBounds = Convert.ToBoolean(ExtractBits(mPaneMagFlags, 1, 1));

            mName = reader.ReadString(0x18).Replace("\0", "");
            mPosX = reader.ReadF32();
            mPosY = reader.ReadF32();
            mPosZ = reader.ReadF32();
            mRotX = reader.ReadF32();
            mRotY = reader.ReadF32();
            mRotZ = reader.ReadF32();
            mScaleX = reader.ReadF32();
            mScaleY = reader.ReadF32();
            mHeight = reader.ReadF32();
            mWidth = reader.ReadF32();
        }

        public override void addUserData(UserdataBase data)
        {
            if (mUserDatas == null)
                mUserDatas = new List<UserdataBase>();

            mUserDatas.Add(data);
        }

        public uint mSectionSize;
        byte mFlags;
        byte mOrigin;
        byte mAlpha;
        byte mPaneMagFlags;
        float mPosX;
        float mPosY;
        float mPosZ;
        float mRotX;
        float mRotY;
        float mRotZ;
        float mScaleX;
        float mScaleY;
        float mHeight;
        float mWidth;

        bool mIsVisible;
        bool mInfluencedAlpha;
        bool mLocationAdjust;
        bool mIgnorePartsMagnify;
        bool mAdjustToPartsBounds;

        List<UserdataBase> mUserDatas;
    }
}
