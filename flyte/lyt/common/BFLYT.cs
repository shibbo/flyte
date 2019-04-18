using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt.common
{
    class BFLYT : LayoutBase
    {
        public BFLYT(ref EndianBinaryReader reader) : base()
        {
            if (reader.ReadString(4) != "FLYT")
            {
                Console.WriteLine("Bad magic. Expected FLYT.");
                return;
            }

            mBOM = reader.ReadUInt16();

            if (mBOM == 0xFEFF)
                reader.SetEndianess(utils.Endian.Endianess.Little);
            else
                reader.SetEndianess(utils.Endian.Endianess.Big);

            mHeaderSize = reader.ReadUInt16();
            mVersion = reader.ReadUInt32();

            mVersionMajor = mVersion >> 24;
            mVersionMinor = (mVersion >> 16) & 0xFF;
            mVersionMicro = (mVersion >> 8) & 0xFF;
            mVersionMicro2 = mVersion & 0xFF;

            string str = String.Format("Version: {0}.{1}.{2}.{3}", mVersionMajor, mVersionMinor, mVersionMicro, mVersionMicro2);
            Console.WriteLine(str);

            mFileSize = reader.ReadUInt32();
            mNumSections = reader.ReadUInt16();
            reader.ReadUInt16();

            mLayoutParams = new LYT1(ref reader);

            // for panels
            LayoutBase prev = null;
            LayoutBase parent = null;

            bool isRootPaneSet = false;
            bool isRootGroupSet = false;

            // for groups
            LayoutBase previousGroup = null;
            LayoutBase groupParent = null;

            string magic = "";

            for (ushort i = 0; i < mNumSections - 1; i++)
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
                        mMaterialList = new MAT1(ref reader, mVersion);
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
                        PIC1 pic = new PIC1(ref reader, ref mMaterialList, mVersionMajor, mVersionMinor);

                        if (parent != null)
                        {
                            parent.addChild(pic);
                            pic.setParent(parent);
                        }

                        prev = pic;
                        break;
                    case "txt1":
                        TXT1 txt = new TXT1(ref reader, ref mMaterialList, ref mFontList, mVersion);

                        if (parent != null)
                        {
                            parent.addChild(txt);
                            txt.setParent(parent);
                        }

                        prev = txt;
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
                    case "prt1":
                        PRT1 prt = new PRT1(ref reader);

                        if (parent != null)
                        {
                            parent.addChild(prt);
                            prt.setParent(parent);
                        }

                        prev = prt;
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
                    case "cnt1":
                        CNT1 cnt = new CNT1(ref reader, mVersionMajor);

                        if (parent != null)
                        {
                            parent.addChild(cnt);
                            cnt.setParent(parent);
                        }

                        prev = cnt;
                        break;
                    case "usd1":
                        USD1 usd = new USD1(ref reader);

                        if (prev != null)
                            prev.addUserData(usd);

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
                        GRP1 group = new GRP1(ref reader, mVersionMajor);

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
        public override List<MaterialBase> getMaterials() { return mMaterialList.getMaterials(); }


        ushort mBOM;
        ushort mHeaderSize;
        uint mVersion;
        uint mFileSize;
        ushort mNumSections;

        LYT1 mLayoutParams;
        TXL1 mTextureList;
        FNL1 mFontList;
        MAT1 mMaterialList;

        GRP1 mRootGroup;

        uint mVersionMajor;
        uint mVersionMinor;
        uint mVersionMicro;
        uint mVersionMicro2;
    }

    class LYT1 : LayoutBase
    {
        public LYT1(ref EndianBinaryReader reader) : base()
        {
            long startPos = reader.Pos();

            if (reader.ReadString(4) != "lyt1")
            {
                Console.WriteLine("Bad magic. Expected lyt1.");
                return;
            }

            mSectionSize = reader.ReadUInt32();
            mDrawnFromCenter = reader.ReadByte();
            reader.ReadBytes(0x3);
            mWidth = reader.ReadF32();
            mHeight = reader.ReadF32();
            mMaxPartsWidth = reader.ReadF32();
            mMaxPartsHeight = reader.ReadF32();
            mName = reader.ReadStringNT();

            reader.Seek(startPos + mSectionSize);
        }

        uint mSectionSize;
        byte mDrawnFromCenter;
        float mWidth;
        float mHeight;
        float mMaxPartsWidth;
        float mMaxPartsHeight;

        [DisplayName("Name"), CategoryAttribute("General"),
            DescriptionAttribute("The name of the layout.")]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        [DisplayName("Width"), CategoryAttribute("General"),
            DescriptionAttribute("The width of the entire canvas.")]
        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        [DisplayName("Height"), CategoryAttribute("General"),
            DescriptionAttribute("The height of the entire canvas.")]
        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }

        [DisplayName("MaxPartsWidth"), CategoryAttribute("General"),
            DescriptionAttribute("The width of the entire canvas.")]
        public float MaxPartsWidth
        {
            get { return mMaxPartsWidth; }
            set { mMaxPartsWidth = value; }
        }

        [DisplayName("MaxPartsHeight"), CategoryAttribute("General"),
            DescriptionAttribute("The height of the entire canvas.")]
        public float MaxPartsHeight
        {
            get { return mMaxPartsHeight; }
            set { mMaxPartsHeight = value; }
        }
    }
}
