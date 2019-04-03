using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt.wii
{
    class PAN1 : LayoutBase
    {
        enum OriginType
        {
            Left = 0,
            Top = 0,
            Center = 1,
            Right = 2,
            Bottom = 2
        }

        public PAN1(ref EndianBinaryReader reader) : base()
        {
            long startPos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mFlags = reader.ReadByte();
            mIsVisible = Convert.ToBoolean(mFlags & 0x1);
            mInfluencedAlpha = Convert.ToBoolean(mFlags & 0x2);
            mIsWideScreen = Convert.ToBoolean(mFlags & 0x4);

            mOrigin = reader.ReadByte();
            mHorizontalOrigin = (OriginType)(mOrigin % 3);
            mVerticalOrigin = (OriginType)(mOrigin / 3);

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
            mWidth = reader.ReadF32();
            mHeight = reader.ReadF32();
        }

        uint mSectionSize;
        byte mFlags;
        byte mOrigin;
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
        float mWidth;
        float mHeight;

        bool mIsVisible;
        bool mInfluencedAlpha;
        bool mIsWideScreen;

        OriginType mHorizontalOrigin;
        OriginType mVerticalOrigin;
    }
}
