using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        float mWidth;
        float mHeight;

        bool mIsVisible;
        bool mInfluencedAlpha;
        bool mLocationAdjust;
        bool mIgnorePartsMagnify;
        bool mAdjustToPartsBounds;

        List<UserdataBase> mUserDatas;

        [DisplayName("Name"), CategoryAttribute("General"), DescriptionAttribute("The name of the element.")]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        // non-editable
        [DisplayName("Type"), CategoryAttribute("General"), DescriptionAttribute("The element type."), ReadOnly(true)]
        public string Type
        {
            get { return base.getType(); }
        }

        [DisplayName("Is Visible"), CategoryAttribute("Flags"), DescriptionAttribute("The visibility of this element.")]
        public bool IsVisible
        {
            get { return mIsVisible; }
            set { mIsVisible = value; }
        }

        [DisplayName("Has Alpha Influence"), CategoryAttribute("Flags"), DescriptionAttribute("If the element has an alpha influence.")]
        public bool HasAlphaInfluence
        {
            get { return mInfluencedAlpha; }
            set { mInfluencedAlpha = value; }
        }

        [DisplayName("Location Adjust"), CategoryAttribute("Flags"), DescriptionAttribute("Unknown.")]
        public bool LocationAdjust
        {
            get { return mLocationAdjust; }
            set { mLocationAdjust = value; }
        }

        [DisplayName("Alpha"), CategoryAttribute("Color"), DescriptionAttribute("The alpha color of this element.")]
        public byte Alpha
        {
            get { return mAlpha; }
            set { mAlpha = value; }
        }

        [DisplayName("Translation X"), CategoryAttribute("Positioning"),
            DescriptionAttribute("The X position of this element, relative to its parent element.")]
        public float PositionX
        {
            get { return mPosX; }
            set { mPosX = value; }
        }

        [DisplayName("Translation Y"), CategoryAttribute("Positioning"),
            DescriptionAttribute("The Y position of this element, relative to its parent element.")]
        public float PositionY
        {
            get { return mPosY; }
            set { mPosY = value; }
        }

        [DisplayName("Translation Z"), CategoryAttribute("Positioning"),
            DescriptionAttribute("The Z position of this element, relative to its parent element.")]
        public float PositionZ
        {
            get { return mPosZ; }
            set { mPosZ = value; }
        }

        [DisplayName("Rotation X"), CategoryAttribute("Positioning"),
            DescriptionAttribute("The X rotation of this element, relative to its parent element.")]
        public float RotationX
        {
            get { return mRotX; }
            set { mRotX = value; }
        }

        [DisplayName("Rotation Y"), CategoryAttribute("Positioning"),
            DescriptionAttribute("The Y rotation of this element, relative to its parent element.")]
        public float RotationY
        {
            get { return mRotY; }
            set { mRotY = value; }
        }

        [DisplayName("Rotation Z"), CategoryAttribute("Positioning"),
            DescriptionAttribute("The Z rotation of this element, relative to its parent element.")]
        public float RotationZ
        {
            get { return mRotZ; }
            set { mRotZ = value; }
        }

        [DisplayName("Scale X"), CategoryAttribute("Scaling"),
            DescriptionAttribute("The X scale of this element, relative to its parent element.")]
        public float ScaleX
        {
            get { return mScaleX; }
            set { mRotZ = value; }
        }

        [DisplayName("Scale Y"), CategoryAttribute("Scaling"),
            DescriptionAttribute("The Y scale of this element, relative to its parent element.")]
        public float ScaleY
        {
            get { return mScaleY; }
            set { mRotY = value; }
        }

        [DisplayName("Width"), CategoryAttribute("Scaling"),
            DescriptionAttribute("The width of this element.")]
        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        [DisplayName("Height"), CategoryAttribute("Scaling"),
            DescriptionAttribute("The height of this element.")]
        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
    }
}
