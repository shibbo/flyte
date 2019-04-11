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
using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace flyte.lyt._3ds
{
    class TXT1 : PAN1
    {
        public TXT1(ref EndianBinaryReader reader) : base(ref reader)
        {
            base.setType("Text Box");

            long startPos = reader.Pos() - 0x4C;

            mBufferLength = reader.ReadUInt16();
            mStringLength = reader.ReadUInt16();
            mMaterialIdx = reader.ReadUInt16();
            mFontNum = reader.ReadUInt16();
            mAnotherOrigin = reader.ReadByte();
            mAlignment = reader.ReadByte();

            reader.ReadBytes(0x2);
            mTextOffset = reader.ReadUInt32();
            mTopColor = reader.ReadRGBAColor8();
            mBottomColor = reader.ReadRGBAColor8();
            mSizeX = reader.ReadF32();
            mSizeY = reader.ReadF32();
            mCharacterSize = reader.ReadF32();
            mLineSize = reader.ReadF32();

            if (mStringLength != 0)
            {
                byte[] str = reader.ReadBytesFrom(startPos + mTextOffset, mStringLength - 2);
                mString = Encoding.GetEncoding(1201).GetString(str);
            }

            reader.Seek(startPos + mSectionSize);
        }

        ushort mBufferLength;
        ushort mStringLength;
        ushort mMaterialIdx;
        ushort mFontNum;
        byte mAnotherOrigin;
        byte mAlignment;

        uint mTextOffset;
        RGBAColor8 mTopColor;
        RGBAColor8 mBottomColor;
        float mSizeX;
        float mSizeY;
        float mCharacterSize;
        float mLineSize;
        string mString;

        string mMaterialName;
        string mFontName;

        public ushort getMaterialIndex() { return mMaterialIdx; }
        public ushort getFontNum() { return mFontNum; }

        public void setMaterialName(string name) { mMaterialName = name; }
        public void setFontName(string name) { mFontName = name; }

        [DisplayName("Text"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The string stored in the textbox.")]
        public string Text
        {
            get { return mString; }
            set
            {
                if (value.Length > 0x10)
                    MessageBox.Show("String cannot be more than 16 characters long.");
                else
                    mString = value;
            }
        }

        [DisplayName("Alignment"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The alignment of text for each line of text in the textbox.")]
        public byte Alignment
        {
            get { return mAlignment; }
            set { mAlignment = value; }
        }

        [DisplayName("Material"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The material name used with the textbox.")]
        public string Material
        {
            get { return mMaterialName; }
            set { mMaterialName = value; }
        }

        [DisplayName("Font"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The font to use with the textbox text.")]
        public string Font
        {
            get { return mFontName; }
            set { mFontName = value; }
        }

        [DisplayName("Size X"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The X size of the textbox.")]
        public float SizeX
        {
            get { return mSizeX; }
            set { mSizeX = value; }
        }

        [DisplayName("Size Y"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The Y size of the textbox.")]
        public float SizeY
        {
            get { return mSizeY; }
            set { mSizeY = value; }
        }

        [DisplayName("Character Size"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The character size of the text in the textbox.")]
        public float CharacterSize
        {
            get { return mCharacterSize; }
            set { mCharacterSize = value; }
        }

        [DisplayName("Line Size"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The line size of each line of text in the textbox.")]
        public float LineSize
        {
            get { return mLineSize; }
            set { mLineSize = value; }
        }
    }
}