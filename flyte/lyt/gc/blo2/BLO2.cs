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

namespace flyte.lyt.gc.blo2
{
    class BLO2 : LayoutBase
    {
        public BLO2(ref EndianBinaryReader reader)
        {
            reader.SetEndianess(Endian.Endianess.Big);

            reader.Seek(0xC);
            mSectionCount = reader.ReadUInt32();
            reader.ReadBytes(0x10);

            mInfo = new INF1(ref reader);

            string magic = "";
            LayoutBase prev = null;
            LayoutBase parent = null;

            bool isRootPaneSet = false;
            
            for (int i = 0; i < mSectionCount; i++)
            {
                magic = reader.ReadString(4);

                switch (magic)
                {
                    case "TEX1":
                        mTextureList = new TEX1(ref reader);
                        break;
                    case "FNT1":
                        mFontList = new FNT1(ref reader);
                        break;
                    case "MAT1":
                        mMaterialList = new MAT1(ref reader);
                        break;
                    case "PAN2":
                        PAN2 panel = new PAN2(ref reader);

                        if (!isRootPaneSet)
                        {
                            mRootPanel = panel;
                            isRootPaneSet = true;
                        }

                        if (parent != null)
                        {
                            parent.addChild(panel);
                            panel.setParent(parent);
                        }

                        prev = panel;
                        break;
                    case "PIC2":
                        PIC2 pic = new PIC2(ref reader);

                        if (parent != null)
                        {
                            parent.addChild(pic);
                            pic.setParent(parent);
                        }

                        prev = pic;
                        break;
                    case "TBX2":
                        TBX2 txt = new TBX2(ref reader);

                        if (parent != null)
                        {
                            parent.addChild(txt);
                            txt.setParent(parent);
                        }

                        prev = txt;
                        break;
                    case "WIN2":
                        WIN2 win = new WIN2(ref reader);

                        if (parent != null)
                        {
                            parent.addChild(win);
                            win.setParent(parent);
                        }

                        prev = win;
                        break;
                    case "BGN1":
                        if (prev != null)
                            parent = prev;

                        reader.ReadUInt32();
                        break;
                    case "END1":
                        prev = parent;
                        parent = prev.getParent();

                        reader.ReadUInt32();
                        break;
                    case "EXT1":
                        reader.ReadUInt32();
                        reader.ReadAligned(0x10);
                        break;
                    default:
                        Console.WriteLine("Section " + magic + " not supported.");
                        break;
                }
            }
        }

        public override bool containsTextures() { return mTextureList != null; }
        public override bool containsFonts()
        {
            if (mFontList != null && mFontList.getStrings() != null)
                return mFontList.getStrings().Count != 0;
            else
                return false;
        }
        public override bool containsMaterials() { return false; }

        public override List<string> getTextureNames()
        {
            if (mTextureList != null)
                return mTextureList.getStrings();
            else
                return null;
        }

        public override List<string> getFontNames()
        {
            if (mFontList != null)
                return mFontList.getStrings();
            else
                return null;
        }

        public override LayoutBase getLayoutParams() { return mInfo; }

        uint mSectionCount;
        INF1 mInfo;
        TEX1 mTextureList;
        FNT1 mFontList;
        MAT1 mMaterialList;
    }

    class INF1 : LayoutBase
    {
        public INF1(ref EndianBinaryReader reader)
        {
            if (reader.ReadString(4) != "INF1")
                return;

            // section size, 0x20
            reader.ReadUInt32();
            mLayoutWidth = reader.ReadUInt16();
            mLayoutHeight = reader.ReadUInt16();
            mTintColor = reader.ReadRGBAColor8();

            reader.ReadAligned(0x20);
        }

        ushort mLayoutWidth;
        ushort mLayoutHeight;
        RGBAColor8 mTintColor;

        [DisplayName("Layout Width"), CategoryAttribute("General"), DescriptionAttribute("Layout width.")]
        public ushort Width
        {
            get { return mLayoutWidth; }
            set { mLayoutWidth = value; }
        }

        [DisplayName("Layout Height"), CategoryAttribute("General"), DescriptionAttribute("Layout height.")]
        public ushort Height
        {
            get { return mLayoutHeight; }
            set { mLayoutHeight = value; }
        }
    }
}
