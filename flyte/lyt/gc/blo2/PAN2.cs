using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt.gc.blo2
{
    class PAN2 : LayoutBase
    {
        public PAN2(ref EndianBinaryReader reader)
        {
            base.setType("Panel");

            mSectionSize = reader.ReadUInt32();
            mUnkString = reader.ReadString(4).Replace("\0", "");
            mUnk0C = reader.ReadUInt16();
            mUnk0E = reader.ReadUInt16();
            mName = reader.ReadString(0x10).Replace("\0", "");
            mWidth = reader.ReadF32();
            mHeight = reader.ReadF32();
            mTop = reader.ReadF32();
            mLeft = reader.ReadF32();

            mUnkFloats = new float[0x6];

            for (int i = 0; i < 6; i++)
                mUnkFloats[i] = reader.ReadF32();
        }

        uint mSectionSize;
        string mUnkString;
        ushort mUnk0C;
        ushort mUnk0E; // seems to be "RE" in ASCII

        float mWidth;
        float mHeight;
        float mTop;
        float mLeft;

        float[] mUnkFloats;

        [DisplayName("Type"), CategoryAttribute("General"), DescriptionAttribute("The element type."), ReadOnly(true)]
        public string Type
        {
            get { return base.getType(); }
        }

        [DisplayName("Width"), CategoryAttribute("Scaling"), DescriptionAttribute("Width of the element.")]
        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        [DisplayName("Height"), CategoryAttribute("Scaling"), DescriptionAttribute("Height of the element.")]
        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
    }
}
