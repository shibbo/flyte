using flyte.io;
using System.ComponentModel;

namespace flyte.lyt.gc.blo1
{
    public class PAN1 : LayoutBase
    {
        public PAN1(ref EndianBinaryReader reader)
        {
            base.setType("Panel");

            mSectionSize = reader.ReadUInt32();
            mParamCount = reader.ReadByte();

            mVisible = reader.ReadByte();
            reader.ReadBytes(0x2);
            mName = reader.ReadString(4).Replace("\0", "");
            mLeft = reader.ReadInt16();
            mTop = reader.ReadInt16();
            mWidth = reader.ReadInt16();
            mHeight = reader.ReadInt16();

            int paramsLeft = mParamCount - 6;

            if (paramsLeft > 0)
            {
                mAngle = reader.ReadInt16();
                paramsLeft--;
            }

            if (paramsLeft > 0)
            {
                mAnchor = reader.ReadByte();
                paramsLeft--;
            }

            if (paramsLeft > 0)
            {
                mAlpha = reader.ReadByte();
                paramsLeft--;
            }

            if (paramsLeft > 0)
            {
                mInheritAlpha = reader.ReadByte();
                paramsLeft--;
            }

            reader.ReadAligned(0x4);
        }

        public uint mSectionSize;
        byte mParamCount;
        byte mVisible;
        short mLeft;
        short mTop;
        short mWidth;
        short mHeight;

        short mAngle;
        byte mAnchor;
        byte mAlpha;
        byte mInheritAlpha;

        [DisplayName("Type"), CategoryAttribute("General"), DescriptionAttribute("The element type."), ReadOnly(true)]
        public string Type
        {
            get { return base.getType(); }
        }

        [DisplayName("Is Visible"), CategoryAttribute("Attributes"), DescriptionAttribute("Sets the visibility of the element.")]
        public byte Visible
        {
            get { return mVisible; }
            set { mVisible = value; }
        }

        [DisplayName("Width"), CategoryAttribute("Scaling"), DescriptionAttribute("Width of the element.")]
        public short Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        [DisplayName("Height"), CategoryAttribute("Scaling"), DescriptionAttribute("Height of the element.")]
        public short Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
    }
}
