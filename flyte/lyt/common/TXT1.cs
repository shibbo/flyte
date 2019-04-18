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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace flyte.lyt.common
{
    class TXT1 : PAN1
    {
        public enum TextAlignment
        {
            Center = 0,
            Left = 1,
            Right = 2
        }

        public enum LineAlignment
        {
            None = 0,
            Left = 1,
            Center = 2,
            Right = 3
        }

        public enum BorderFormat
        {
            Standard = 0,
            DeleteBorder = 1,
            RenderInTwoCycles = 2
        }

        public TXT1(ref EndianBinaryReader reader, ref MAT1 materials, ref FNL1 fonts, uint version) : base(ref reader)
        {
            base.setType("Text Box");

            long startPos = reader.Pos() - 0x54;

            mTextLength = reader.ReadUInt16();
            mMaxTextLength = reader.ReadUInt16();
            mMaterialIndex = reader.ReadUInt16();
            mFontIndex = reader.ReadUInt16();

            byte alignment = reader.ReadByte();
            mHorizontalTextAligment = (TextAlignment)((alignment >> 2) & 0x3);
            mVerticalTextAlignment = (TextAlignment)(alignment & 0x3);

            byte lineAlignment = reader.ReadByte();
            mLineAlignment = (LineAlignment)lineAlignment;

            byte flag = reader.ReadByte();
            mPerCharTransform = Convert.ToBoolean((flag >> 4) & 0x1);
            mBorderFormat = (BorderFormat)((flag >> 2) & 0x3);
            mRestrictTextLength = Convert.ToBoolean((flag >> 1) & 0x1);
            mShadowEnabled = Convert.ToBoolean(flag & 0x1);

            reader.ReadByte(); // padding

            mItalicTilt = reader.ReadF32();
            mTextOffset = reader.ReadUInt32();

            mFontTopColor = reader.ReadRGBAColor8();
            mFontBottomColor = reader.ReadRGBAColor8();
            mFontSizeX = reader.ReadF32();
            mFontSizeY = reader.ReadF32();
            mCharSpace = reader.ReadF32();
            mLineSpace = reader.ReadF32();
            mNameOffset = reader.ReadUInt32();

            mShadowX = reader.ReadF32();
            mShadowY = reader.ReadF32();
            mShadowSizeX = reader.ReadF32();
            mShadowSizeY = reader.ReadF32();
            mShadowTopColor = reader.ReadRGBAColor8();
            mShadowBottomColor = reader.ReadRGBAColor8();
            mShadowItalic = reader.ReadF32();

            if (version == 0x8030000)
                reader.ReadUInt32(); // this might be something, but probably padding

            if (version != 0x3030000)
                mPerCharTransformOffset = reader.ReadUInt32();
            else
                mPerCharTransformOffset = 0;

            mMaterialName = materials.getMaterialNameFromIndex(mMaterialIndex);
            mFontName = fonts.getFontNameFromIndex(mFontIndex);

            // read the textbox text (which is mostly useless)
            mText = reader.ReadUTF16StringFrom(startPos + mTextOffset);
            mName = reader.ReadStringNTFrom(startPos + mNameOffset);

            // this is the last structure in the file, and we don't even get a count...
            if (mPerCharTransformOffset != 0)
            {
                reader.Seek(startPos + mPerCharTransformOffset);

                mTransforms = new List<PerCharTransform>();

                uint numEntries = (mSectionSize - mPerCharTransformOffset) / 0xC;

                for (int i = 0; i < numEntries; i++)
                    mTransforms.Add(new PerCharTransform(ref reader));
            }

            // and we are done
            reader.Seek(startPos + mSectionSize);
        }

        ushort mTextLength;
        ushort mMaxTextLength;
        ushort mMaterialIndex;
        ushort mFontIndex;

        TextAlignment mHorizontalTextAligment;
        TextAlignment mVerticalTextAlignment;
        LineAlignment mLineAlignment;

        bool mPerCharTransform;
        BorderFormat mBorderFormat;
        bool mRestrictTextLength;
        bool mShadowEnabled;

        float mItalicTilt;
        uint mTextOffset;
        RGBAColor8 mFontTopColor;
        RGBAColor8 mFontBottomColor;
        float mFontSizeX;
        float mFontSizeY;

        float mCharSpace;
        float mLineSpace;
        uint mNameOffset;

        float mShadowX;
        float mShadowY;
        float mShadowSizeX;
        float mShadowSizeY;
        RGBAColor8 mShadowTopColor;
        RGBAColor8 mShadowBottomColor;
        float mShadowItalic;

        uint mPerCharTransformOffset;

        string mText;
        string mMaterialName;
        string mFontName;

        List<PerCharTransform> mTransforms;

        [DisplayName("Text"), CategoryAttribute("Textbox Settings"), DescriptionAttribute("The string stored in the textbox.")]
        public string Text
        {
            get { return mText; }
            set
            {
                if (mRestrictTextLength)
                {
                    if (value.Length > mMaxTextLength)
                        MessageBox.Show("String cannot be more than " + mMaxTextLength + " characters long.");
                }
                else
                    mText = value;
            }
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
    }

    class PerCharTransform
    {
        public PerCharTransform(ref EndianBinaryReader reader)
        {
            mTimeOffset = reader.ReadF32();
            mTimeWidth = reader.ReadF32();
            mLoopType = reader.ReadByte();
            mVerticalOrigin = reader.ReadByte();
            mHasAnimInfo = reader.ReadByte() == 1;

            reader.ReadByte();
        }

        float mTimeOffset;
        float mTimeWidth;
        byte mLoopType;
        byte mVerticalOrigin;
        bool mHasAnimInfo;
    }

}
