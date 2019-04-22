using flyte.io;
using flyte.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt.gc.blo1
{
    class WIN1 : PAN1
    {
        public WIN1(ref EndianBinaryReader reader) : base(ref reader)
        {
            base.setType("Window");

            byte numParams = reader.ReadByte();

            mTransX = reader.ReadInt16();
            mTransY = reader.ReadInt16();
            mWidth = reader.ReadInt16();
            mHeight = reader.ReadInt16();

            mTextures = new WindowTexture[4];

            for (int i = 0; i < 4; i++)
            {
                byte type = reader.ReadByte();
                mTextures[i].name = reader.ReadStringLengthPrefix();
            }

            byte resourceType = reader.ReadByte();
            mPaletteName = reader.ReadStringLengthPrefix();

            byte src = reader.ReadByte();

            for (int i = 0; i < 4; i++)
            {
                mTextures[i].mirror = (byte)((src >> (6 - (i * 2))) & 0x3);
            }

            for (int i = 0; i < 4; i++)
            {
                mTextures[i].color = reader.ReadRGBAColor8();
            }

            numParams -= 14;

            if (numParams > 0)
            {
                byte type = reader.ReadByte();
                mContentTexture = reader.ReadStringLengthPrefix();
                numParams--;
            }

            if (numParams > 0)
            {
                mFromColor = reader.ReadRGBAColor8();
                numParams--;
            }

            if (numParams > 0)
            {
                mToColor = reader.ReadRGBAColor8();
                numParams--;
            }

            reader.ReadAligned(0x4);
        }

        short mTransX;
        short mTransY;
        short mWidth;
        short mHeight;

        WindowTexture[] mTextures;
        string mPaletteName;

        string mContentTexture;
        RGBAColor8 mFromColor;
        RGBAColor8 mToColor;
    }

    struct WindowTexture
    {
        public string name;
        public byte mirror;
        public RGBAColor8 color;
    }

}
