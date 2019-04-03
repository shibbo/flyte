using flyte.img.wii;
using flyte.io;
using System;
using System.Collections.Generic;

namespace flyte.lyt.wii
{
    class TXL1
    {
        public TXL1(ref EndianBinaryReader reader)
        {
            long startPos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mNumTextures = reader.ReadUInt16();
            mUnk0A = reader.ReadUInt16();

            // all offsets are relative to this point
            long curPos = reader.Pos();

            mStrings = new List<string>();

            for (ushort i = 0; i < mNumTextures; i++)
            {
                uint offset = reader.ReadUInt32();
                mStrings.Add(reader.ReadStringNTFrom(offset + curPos));
                reader.ReadUInt32();
            }

            reader.Seek(startPos + mSectionSize);
        }

        public bool doesImageExistWithExt(string name)
        {
            return mStrings.Contains(name);
        }

        uint mSectionSize;
        ushort mNumTextures;
        ushort mUnk0A;

        List<string> mStrings;
        List<TPL> mImages;
    }
}
