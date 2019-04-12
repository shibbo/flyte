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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static flyte.utils.Endian;

namespace flyte.lyt.wii
{
    class BRLYT : LayoutBase
    {
        public BRLYT(ref EndianBinaryReader reader) : base()
        {
            reader.SetEndianess(Endianess.Big);

            if (reader.ReadString(4) != "RLYT")
            {
                Console.WriteLine("Bad magic. Expected RLYT.");
                return;
            }

            mBOM = reader.ReadUInt16();
            mVersion = reader.ReadUInt16();
            mFileLength = reader.ReadUInt32();
            mHeaderLength = reader.ReadUInt16();
            mNumSections = reader.ReadUInt16();

            mLayoutParams = new LYT1(ref reader);

            // for panels
            LayoutBase prev = null;
            LayoutBase parent = null;

            bool isRootPaneSet = false;
            bool isRootGroupSet = false;

            // for groups
            LayoutBase previousGroup = null;
            LayoutBase groupParent = null;

            for (int i = 0; i < mNumSections; i++)
            {
                string magic = reader.ReadString(4);

                switch (magic)
                {
                    case "txl1":
                        mTextureList = new TXL1(ref reader);
                        break;
                    case "fnl1":
                        mFontList = new FNL1(ref reader);
                        break;
                    case "mat1":
                        mMaterialList = new MAT1(ref reader);
                        break;
                    case "pan1":
                        PAN1 panel = new PAN1(ref reader);
                        
                        // root panel *should* be the first thing in the list of elements
                        // so if it isn't, then the layout is wrong
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
                    case "pic1":
                        PIC1 pic = new PIC1(ref reader);

                        if (parent != null)
                        {
                            parent.addChild(pic);
                            pic.setParent(parent);
                        }

                        prev = pic;

                        break;
                    case "bnd1":
                        BND1 bnd = new BND1(ref reader);

                        if (parent != null)
                        {
                            parent.addChild(bnd);
                            bnd.setParent(parent);
                        }

                        prev = bnd;

                        break;
                    case "txt1":
                        TXT1 txt = new TXT1(ref reader, ref mMaterialList, ref mFontList);

                        if (parent != null)
                        {
                            parent.addChild(txt);
                            txt.setParent(parent);
                        }

                        prev = txt;

                        break;
                    case "usd1":
                        // user data is assigned to the previous read element
                        USD1 usd = new USD1(ref reader);

                        if (prev != null)
                            prev.addUserData(usd);

                        break;
                    case "wnd1":
                        WND1 window = new WND1(ref reader, ref mMaterialList);

                        if (parent != null)
                        {
                            parent.addChild(window);
                            window.setParent(parent);
                        }

                        prev = window;
                        break;
                    case "pas1":
                        if (prev != null)
                            parent = prev;

                        reader.ReadUInt32();
                        break;
                    case "pae1":
                        prev = parent;
                        parent = prev.getParent();

                        reader.ReadUInt32();
                        break;
                    case "grp1":
                        GRP1 group = new GRP1(ref reader);

                        if (!isRootGroupSet)
                        {
                            mRootGroup = group;
                            isRootGroupSet = true;
                        }

                        if (groupParent != null)
                        {
                            groupParent.addChild(group);
                            group.setParent(groupParent);
                        }

                        previousGroup = group;
                        break;
                    case "grs1":
                        if (previousGroup != null)
                             groupParent = previousGroup;

                        reader.ReadUInt32();
                        break;
                    case "gre1":
                        previousGroup = groupParent;
                        groupParent = previousGroup.getParent();

                        reader.ReadUInt32();
                        break;
                }
            }
        }

        public override bool containsTextures() { return mTextureList != null; }
        public override bool containsFonts() { return mFontList != null; }
        public override bool containsMaterials()
        {
            if (mMaterialList != null)
            {
                if (mMaterialList.getMaterialNames() != null)
                    return true;
            }

            return false;
        }

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

        public override List<string> getMaterialNames()
        {
            if (mMaterialList != null)
                return mMaterialList.getMaterialNames();

            return null;
        }

        public override LayoutBase getLayoutParams() { return mLayoutParams; }
        public override LayoutBase getRootGroup() { return mRootGroup; }

        ushort mBOM;
        ushort mVersion;
        uint mFileLength;
        ushort mHeaderLength;
        ushort mNumSections;

        LYT1 mLayoutParams;
        TXL1 mTextureList;
        FNL1 mFontList;
        MAT1 mMaterialList;

        GRP1 mRootGroup;
    }

    class LYT1 : LayoutBase
    {
        public LYT1(ref EndianBinaryReader reader)
        {
            if (reader.ReadString(4) != "lyt1")
            {
                Console.WriteLine("Bad magic. Expected lyt1.");
                return;
            }

            mSectionSize = reader.ReadUInt32();
            mIsCentered = Convert.ToBoolean(reader.ReadByte());
            mPadding = reader.ReadBytes(0x3);
            mWidth = reader.ReadF32();
            mHeight = reader.ReadF32();
        }

        uint mSectionSize;
        bool mIsCentered;
        byte[] mPadding; // supposed padding
        float mWidth;
        float mHeight;

        [DisplayName("Is Centered"), CategoryAttribute("General"), DescriptionAttribute("Centers the entire layout if true.")]
        public bool IsCentered
        {
            get { return mIsCentered; }
            set { mIsCentered = value; }
        }

        [DisplayName("Width"), CategoryAttribute("General"), DescriptionAttribute("Width of the layout.")]
        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        [DisplayName("Height"), CategoryAttribute("General"), DescriptionAttribute("Height of the layout.")]
        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
    }
}
