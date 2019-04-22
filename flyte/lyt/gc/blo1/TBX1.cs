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
using flyte.utils;

namespace flyte.lyt.gc.blo1
{
    class TBX1 : PAN1
    {
        public TBX1(ref EndianBinaryReader reader) : base(ref reader)
        {
            base.setType("Textbox");

            byte numParams = reader.ReadByte();

            byte type = reader.ReadByte();
            mFontName = reader.ReadStringLengthPrefix();

            mTopColor = reader.ReadRGBAColor8();
            mBottomColor = reader.ReadRGBAColor8();

            byte binding = reader.ReadByte();
            mHorizBinding = (byte)((binding >> 2) & 0x3);
            mVertBinding = (byte)((binding >> 0) & 0x3);

            mFontSpacing = reader.ReadInt16();
            mFontLeading = reader.ReadInt16();
            mFontWidth = reader.ReadInt16();
            mFontHeight = reader.ReadInt16();

            short stringLength = reader.ReadInt16();
            mText = reader.ReadString(stringLength);

            numParams -= 10;
            
            if (numParams > 0)
            {
                if (reader.ReadByte() != 0)
                    mConnectParent = 1;

                --numParams;
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

            reader.ReadAligned(0x4);
        }

        string mFontName;
        RGBAColor8 mTopColor;
        RGBAColor8 mBottomColor;

        byte mHorizBinding;
        byte mVertBinding;
        short mFontSpacing;
        short mFontLeading;
        short mFontWidth;
        short mFontHeight;

        string mText;
        byte mConnectParent;
        RGBAColor8 mFromColor;
        RGBAColor8 mToColor;

        [DisplayName("Text"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The text on the textbox.")]
        public string Text
        {
            get { return mText; }
            set { mText = value; }
        }

        [DisplayName("Font Name"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("Font name to use.")]
        public string FontName
        {
            get { return mFontName; }
            set { mFontName = value; }
        }

        [DisplayName("Font Spacing"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("Font spacing.")]
        public short FontSpacing
        {
            get { return mFontSpacing; }
            set { mFontSpacing = value; }
        }
    }
}
