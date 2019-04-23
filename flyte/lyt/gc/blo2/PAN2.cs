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

using System.ComponentModel;
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
