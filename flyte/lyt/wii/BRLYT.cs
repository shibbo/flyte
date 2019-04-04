using flyte.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt.wii
{
    class BRLYT : LayoutBase
    {
        public BRLYT(ref EndianBinaryReader reader) : base()
        {
            reader.SetEndianess(EndianBinaryReader.Endianess.Big);

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
            mGroups = new List<GRP1>();

            // for panels
            LayoutBase prev = null;
            LayoutBase parent = null;

            bool isRootPaneSet = false;

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

                        if (parent != null)
                        {
                            parent.addChild(usd);
                            usd.setParent(parent);
                        }

                        prev = usd;

                        break;
                    case "wnd1":
                        WND1 window = new WND1(ref reader);

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
                        mGroups.Add(group);

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

        ushort mBOM;
        ushort mVersion;
        uint mFileLength;
        ushort mHeaderLength;
        ushort mNumSections;

        LYT1 mLayoutParams;
        TXL1 mTextureList;
        FNL1 mFontList;
        MAT1 mMaterialList;

        List<GRP1> mGroups;
    }

    class LYT1
    {
        public LYT1(ref EndianBinaryReader reader)
        {
            if (reader.ReadString(4) != "lyt1")
            {
                Console.WriteLine("Bad magic. Expected lyt1.");
                return;
            }

            mSectionSize = reader.ReadUInt32();
            mIsCentered = reader.ReadByte();
            mPadding = reader.ReadBytes(0x3);
            mWidth = reader.ReadF32();
            mHeight = reader.ReadF32();
        }

        uint mSectionSize;
        byte mIsCentered;
        byte[] mPadding; // supposed padding
        float mWidth;
        float mHeight;
    }
}
