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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            bool isRootPaneSet = false;

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
                        mUserDataEntries.Add(new USD1(ref reader));
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
    }

    class LYT1 : LayoutBase
    {
        public LYT1(ref EndianBinaryReader reader)
        {
            if (reader.ReadString(4) != "lyt1")
            {
                Console.WriteLine("Bad magic. Expected LYT1.");
                return;
            }

            mSectionSize = reader.ReadUInt32();
            mOriginType = reader.ReadUInt32();
            mCanvasSizeX = reader.ReadF32();
            mCanvasSizeY = reader.ReadF32();
        }

        uint mSectionSize;
        uint mOriginType;
        float mCanvasSizeX;
        float mCanvasSizeY;
    }

}
