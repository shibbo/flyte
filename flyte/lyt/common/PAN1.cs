using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using flyte.io;
using flyte.utils;

namespace flyte.lyt.common
{
    class PAN1 : LayoutBase
    {
        public enum OriginX
        {
            Center = 0,
            Left = 1,
            Right = 2
        }

        public enum OriginY
        {
            Center = 0,
            Top = 1,
            Bottom = 2
        }

        public PAN1(ref EndianBinaryReader reader) : base()
        {
            base.setType("Panel");

            mSectionSize = reader.ReadUInt32();

            byte flags = reader.ReadByte();
            mIsVisible = Convert.ToBoolean(flags & 0x1);
            mInfluencedAlpha = Convert.ToBoolean(flags & 0x2);

            byte origin = reader.ReadByte();
            mOriginX = (OriginX)((origin >> 6) & 0x3);
            mOriginY = (OriginY)((origin >> 4) & 0x3);
            mParentOriginX = (OriginX)((origin >> 2) & 0x3);
            mParentOriginY = (OriginY)(origin & 0x3);

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

        public override void draw()
        {
            GL.PushMatrix();
            GL.Translate(mTransX, mTransY, 0);
            GL.Scale(mScaleX, mScaleY, 1);

            RenderRectangle.OriginH originH = RenderRectangle.OriginH.CENTER;
            RenderRectangle.OriginV originV = RenderRectangle.OriginV.CENTER;

            switch (mOriginX)
            {
                case OriginX.Left:
                    originH = RenderRectangle.OriginH.LEFT;
                    break;
                case OriginX.Right:
                    originH = RenderRectangle.OriginH.RIGHT;
                    break;
                case OriginX.Center:
                    originH = RenderRectangle.OriginH.CENTER;
                    break;
            }

            switch (mOriginY)
            {
                case OriginY.Top:
                    originV = RenderRectangle.OriginV.TOP;
                    break;
                case OriginY.Bottom:
                    originV = RenderRectangle.OriginV.BOTTOM;
                    break;
                case OriginY.Center:
                    originV = RenderRectangle.OriginV.CENTER;
                    break;
            }

            RenderRectangle.DrawRect((int)mWidth, (int)mHeight, originH, originV);

            // no children to draw
            if (base.getChildren() == null)
            {
                GL.PopMatrix();
                return;
            }

            foreach (LayoutBase child in base.getChildren())
                child.draw();

            GL.PopMatrix();
        }

        public uint mSectionSize;
        bool mInfluencedAlpha;
        bool mIsVisible;
        OriginX mOriginX;
        OriginY mOriginY;
        OriginX mParentOriginX;
        OriginY mParentOriginY;
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
        public OriginX HorizontalX
        {
            get { return mOriginX; }
            set { mOriginX = value; }
        }

        [DisplayName("Y Origin"), CategoryAttribute("Origin"), DescriptionAttribute("The Y origin of the element.")]
        public OriginY HorizontalY
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
