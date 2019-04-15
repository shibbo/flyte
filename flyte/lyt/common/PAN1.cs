using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using flyte.io;

namespace flyte.lyt.common
{
    class PAN1 : LayoutBase
    {
        public enum Origin
        {
            Center = 0,
            Left = 1,
            Right = 2
        }

        public PAN1(ref EndianBinaryReader reader) : base()
        {
            base.setType("Panel");

            mSectionSize = reader.ReadUInt32();

            byte flags = reader.ReadByte();
            mIsVisible = Convert.ToBoolean(flags & 0x1);
            mInfluencedAlpha = Convert.ToBoolean(flags & 0x2);

            byte origin = reader.ReadByte();
            mOriginX = (Origin)((origin >> 6) & 0x3);
            mOriginY = (Origin)((origin >> 4) & 0x3);
            mParentOriginX = (Origin)((origin >> 2) & 0x3);
            mParentOriginY = (Origin)(origin & 0x3);

            mAlpha = reader.ReadByte();
            mUnk0B = reader.ReadByte();
            mName = reader.ReadString(0x18).Replace("\0", "");
            mUserData = reader.ReadString(0x8).Replace("\0", "");

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

        public uint mSectionSize;
        bool mInfluencedAlpha;
        bool mIsVisible;
        Origin mOriginX;
        Origin mOriginY;
        Origin mParentOriginX;
        Origin mParentOriginY;
        byte mAlpha;
        byte mUnk0B;
        string mUserData;
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

        [DisplayName("X Origin"), CategoryAttribute("Origin"), DescriptionAttribute("The X origin of the element.")]
        public Origin HorizontalX
        {
            get { return mOriginX; }
            set { mOriginX = value; }
        }

        [DisplayName("Y Origin"), CategoryAttribute("Origin"), DescriptionAttribute("The Y origin of the element.")]
        public Origin HorizontalY
        {
            get { return mOriginY; }
            set { mOriginY = value; }
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
            get { return mTransX; }
            set { mTransX = value; }
        }

        [DisplayName("Translation Y"), CategoryAttribute("Positioning"),
            DescriptionAttribute("The Y position of this element, relative to its parent element.")]
        public float PositionY
        {
            get { return mTransY; }
            set { mTransY = value; }
        }

        [DisplayName("Translation Z"), CategoryAttribute("Positioning"),
            DescriptionAttribute("The Z position of this element, relative to its parent element.")]
        public float PositionZ
        {
            get { return mTransZ; }
            set { mTransZ = value; }
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

        [DisplayName("User Data"), CategoryAttribute("Other"),
            DescriptionAttribute("User data for the element.")]
        public string UserData
        {
            get { return mUserData; }
            set
            {
                if (value.Length > 0x8)
                    MessageBox.Show("User data cannot be more than 8 characters in length.");
                else
                    mUserData = value;
            }
        }
    }
}
