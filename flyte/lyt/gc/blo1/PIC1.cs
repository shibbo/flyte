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

using flyte.io;
using flyte.utils;
using System.ComponentModel;

namespace flyte.lyt.gc.blo1
{
    class PIC1 : PAN1
    {
        public PIC1(ref EndianBinaryReader reader) : base(ref reader)
        {
            base.setType("Picture");

            byte numParams = reader.ReadByte();

            byte resourceType = reader.ReadByte();
            mTextureName = reader.ReadStringLengthPrefix();
            resourceType = reader.ReadByte();
            mPaletteName = reader.ReadStringLengthPrefix();
            mBinding = reader.ReadByte();
            numParams -= 3;

            if (numParams > 0)
            {
                byte src = reader.ReadByte();
                mMirror = (byte)(src & 0x3);
                mRotate90 = (byte)(src & 0x4);

                numParams--;
            }

            if (numParams > 0)
            {
                byte src = reader.ReadByte();
                mWrapS = (byte)((src >> 2) & 0x3);
                mWrapT = (byte)(src & 0x3);

                numParams--;
            }

            if (numParams > 0)
            {
                mFromColor = reader.ReadRGBAColor8();
                numParams--;
            }

            if (numParams > 0)
            {
                mToColor = reader.ReadRGBAColor8();
                numParams--;
            }

            mColors = new RGBAColor8[4];

            for (int i = 0; i < 4; ++i)
            {
                if (numParams > 0)
                {
                    mColors[i] = reader.ReadRGBAColor8();
                    numParams--;
                }
            }

            reader.ReadAligned(0x4);
        }

        public string getTextureName() { return mTextureName; }

        string mTextureName;
        string mPaletteName;

        byte mBinding;
        byte mMirror;
        byte mRotate90;
        byte mWrapS;
        byte mWrapT;
        RGBAColor8 mFromColor;
        RGBAColor8 mToColor;
        RGBAColor8[] mColors;

        [DisplayName("Texture Name"), CategoryAttribute("Picture Settings"), DescriptionAttribute("Texture name to use.")]
        public string TextureName
        {
            get { return mTextureName; }
            set { mTextureName = value; }
        }
    }
}
