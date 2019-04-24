using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;
using flyte.utils;

namespace flyte.img._3ds
{
    class BCLIM : ImageContainerBase
    {
        public BCLIM(ref EndianBinaryReader reader)
        {
            reader.SetEndianess(Endian.Endianess.Little);
            
            while(true)
            {
                string a = reader.ReadString(4);

                if (a == "CLIM")
                    break;
            }

            // BOM
            reader.ReadUInt16();
            // total length but we already have that
            reader.ReadUInt32();
            mTileWidth = (byte)(2 << reader.ReadByte());
            mTileHeight = (byte)(2 << reader.ReadByte());
            mLength = reader.ReadUInt32();
            mCount = reader.ReadUInt32();
            
            mImage = new BCLIMImage(ref reader);
        }

        byte mTileWidth;
        byte mTileHeight;
        uint mLength;
        uint mCount;

        BCLIMImage mImage;
    }

    class BCLIMImage : ImageBase
    {
        public BCLIMImage(ref EndianBinaryReader reader)
        {
            // oh noes
            if (reader.ReadString(4) != "imag")
                return;

            reader.ReadUInt32();
            mWidth = reader.ReadUInt16();
            mHeight = reader.ReadUInt16();
            mFormat = reader.ReadInt32();
            mDataLength = reader.ReadUInt32();
        }

        ushort mWidth;
        ushort mHeight;
        int mFormat;
        uint mDataLength;
    }

}
