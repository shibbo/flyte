using flyte.io;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt.gc.blo2
{
    class PIC2 : LayoutBase
    {
        public PIC2(ref EndianBinaryReader reader)
        {
            base.setType("Picture");

            long startPos = reader.Pos() - 0x4;

            mSectionSize = reader.ReadUInt32();
            mUnkString = reader.ReadString(4).Replace("\0", "");
            mParentSize = reader.ReadUInt32();
            mUnk10 = reader.ReadUInt16();
            mUnk12 = reader.ReadUInt16();
            mUnk14 = reader.ReadUInt16();
            mUnk16 = reader.ReadUInt16();
            mName = reader.ReadString(0x10).Replace("\0", "");
            mWidth = reader.ReadF32();
            mHeight = reader.ReadF32();
            mTop = reader.ReadF32();
            mLeft = reader.ReadF32();

            mUnkFloats = new float[0x6];

            for (int i = 0; i < 6; i++)
                mUnkFloats[i] = reader.ReadF32();

            reader.Seek(startPos + mSectionSize);
        }

        uint mSectionSize;
        string mUnkString;
        uint mParentSize;
        ushort mUnk10;
        ushort mUnk12;
        ushort mUnk14;
        ushort mUnk16;

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
