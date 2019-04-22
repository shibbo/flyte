using flyte.io;
using flyte.utils;
using System;
using System.ComponentModel;

namespace flyte.lyt.gc.blo1
{
    class BLO1 : LayoutBase
    {
        public BLO1(ref EndianBinaryReader reader)
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
                    case "PAN1":
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
                    case "PIC1":
                        PIC1 pic = new PIC1(ref reader);

                        if (parent != null)
                        {
                            parent.addChild(pic);
                            pic.setParent(parent);
                        }

                        prev = pic;

                        break;
                    case "WIN1":
                        WIN1 window = new WIN1(ref reader);

                        if (parent != null)
                        {
                            parent.addChild(window);
                            window.setParent(parent);
                        }

                        prev = window;

                        break;
                    case "TBX1":
                        TBX1 text = new TBX1(ref reader);

                        if (parent != null)
                        {
                            parent.addChild(text);
                            text.setParent(parent);
                        }

                        prev = text;

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
                        reader.ReadAligned(0x20);
                        break;
                    default:
                        Console.WriteLine("Section " + magic + " not supported.");
                        break;
                }

            }

        }

        public override LayoutBase getLayoutParams() { return mInfo; }

        uint mSectionCount;
        INF1 mInfo;
    }

    class INF1 : LayoutBase
    {
        public INF1(ref EndianBinaryReader reader)
        {
            if (reader.ReadString(4) != "INF1")
                return;

            reader.ReadUInt32();
            mLayoutWidth = reader.ReadUInt16();
            mLayoutHeight = reader.ReadUInt16();
            mTintColor = reader.ReadRGBAColor8();
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
