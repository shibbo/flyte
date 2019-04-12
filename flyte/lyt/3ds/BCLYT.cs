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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static flyte.utils.Endian;

namespace flyte.lyt._3ds
{
    class BCLYT : LayoutBase
    {
        public BCLYT(ref EndianBinaryReader reader)
        {
            reader.SetEndianess(Endianess.Little);

            if (reader.ReadString(4) != "CLYT")
            {
                Console.WriteLine("Bad magic. Expected CLYT.");
                return;
            }

            mBOM = reader.ReadUInt16();
            mHeaderLength = reader.ReadUInt16();
            mRevision = reader.ReadUInt32();
            mFileSize = reader.ReadUInt32();
            mSectionCount = reader.ReadUInt32();

            mLayoutParams = new LYT1(ref reader);

            mUserDataEntries = new List<USD1>();

            LayoutBase prev = null;
            LayoutBase parent = null;

            // for groups
            LayoutBase previousGroup = null;
            LayoutBase groupParent = null;

            bool isRootPaneSet = false;
            bool isRootGroupSet = false;

            string magic = "";

            for (uint i = 0; i < mSectionCount; i++)
            {
                magic = reader.ReadString(4);

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
                        TXT1 txt = new TXT1(ref reader);
                        if (parent != null)
                        {
                            parent.addChild(txt);
                            txt.setParent(parent);
                        }

                        prev = txt;
                        break;
                    case "usd1":
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
                    case "pts1":
                        MessageBox.Show("PTS1 found! Do tell shibboleet about this...");
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
                    default:
                        Console.WriteLine("Unsupported magic " + magic);
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
        ushort mHeaderLength;
        uint mRevision;
        uint mFileSize;
        uint mSectionCount;

        LYT1 mLayoutParams;
        TXL1 mTextureList;
        FNL1 mFontList;
        MAT1 mMaterialList;

        List<USD1> mUserDataEntries;

        GRP1 mRootGroup;
    }

    class LYT1 : LayoutBase
    {
        public enum OriginType
        {
            Classic = 0,
            Normal = 1
        }

        public LYT1(ref EndianBinaryReader reader)
        {
            if (reader.ReadString(4) != "lyt1")
            {
                Console.WriteLine("Bad magic. Expected LYT1.");
                return;
            }

            mSectionSize = reader.ReadUInt32();
            mOriginType = (OriginType)reader.ReadUInt32();
            mCanvasSizeX = reader.ReadF32();
            mCanvasSizeY = reader.ReadF32();
        }

        uint mSectionSize;
        OriginType mOriginType;
        float mCanvasSizeX;
        float mCanvasSizeY;

        [DisplayName("Origin Type"), CategoryAttribute("General"),
            DescriptionAttribute("The origin type for the entire canvas.")]
        public OriginType Origin
        {
            get { return mOriginType; }
            set { mOriginType = value; }
        }

        [DisplayName("Canvas Size X"), CategoryAttribute("General"),
            DescriptionAttribute("The X size of the entire canvas.")]
        public float CanvasSizeX
        {
            get { return mCanvasSizeX; }
            set { mCanvasSizeX = value; }
        }

        [DisplayName("Canvas Size Y"), CategoryAttribute("General"),
            DescriptionAttribute("The Y size of the entire canvas.")]
        public float CanvasSizeY
        {
            get { return mCanvasSizeY; }
            set { mCanvasSizeY = value; }
        }
    }

}
