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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using flyte.io;

namespace flyte.lyt.wii
{
    class PAN1 : LayoutBase
    {
        public enum OriginType
        {
            Left = 0,
            Top = 0,
            Center = 1,
            Right = 2,
            Bottom = 2
        }

        public PAN1(ref EndianBinaryReader reader) : base()
        {
            base.setType("Panel");

            mSectionSize = reader.ReadUInt32();
            mFlags = reader.ReadByte();
            // extract our flags
            mIsVisible = Convert.ToBoolean(mFlags & 0x1);
            mInfluencedAlpha = Convert.ToBoolean(mFlags & 0x2);
            mIsWideScreen = Convert.ToBoolean(mFlags & 0x4);

            mOrigin = reader.ReadByte();
            mHorizontalOrigin = (OriginType)(mOrigin % 3);
            mVerticalOrigin = (OriginType)(mOrigin / 3);

            mAlpha = reader.ReadByte();
            reader.ReadByte();
            mName = reader.ReadString(0x10).Replace("\0", "");
            mUserData = reader.ReadString(0x8);
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

        bool mIsVisible;
        bool mInfluencedAlpha;
        bool mIsWideScreen;

        OriginType mHorizontalOrigin;
        OriginType mVerticalOrigin;

        List<UserdataBase> mUserDatas;

        #region  PropertyGrid Stuff
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

        [DisplayName("Horizontal Origin"), CategoryAttribute("Origin"), DescriptionAttribute("The horizonal origin of the element.")]
        public OriginType HorizontalOrigin
        {
            get { return mHorizontalOrigin; }
            set { mHorizontalOrigin = value; }
        }

        [DisplayName("Vertical Origin"), CategoryAttribute("Origin"), DescriptionAttribute("The vertical origin of the element.")]
        public OriginType VerticalOrigin
        {
            get { return mVerticalOrigin; }
            set { mVerticalOrigin = value; }
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

        [DisplayName("Is Widescreen"), CategoryAttribute("Flags"), DescriptionAttribute("If the element is in widescreen.")]
        public bool IsWideScreen
        {
            get { return mIsWideScreen; }
            set { mIsWideScreen = value; }
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
        #endregion
    }
}
