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
            mSections = new List<LayoutBase>();

            LayoutBase prev = null;
            LayoutBase parent = null;
            
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
                        mSections.Add(panel);

                        if (parent != null)
                        {
                            parent.addChild(panel);
                            panel.setParent(parent);
                        }

                        prev = panel;
                        break;
                    case "pic1":
                        // pictures have to have a panel parent
                        PIC1 pic = new PIC1(ref reader);
                        mSections.Add(pic);

                        if (parent != null)
                        {
                            parent.addChild(pic);
                            pic.setParent(parent);
                        }

                        prev = pic;

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

        List<LayoutBase> mSections;
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
