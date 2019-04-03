using flyte.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt.wii
{
    class FNL1
    {
        public FNL1(ref EndianBinaryReader reader)
        {
            long startPos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mNumFonts = reader.ReadUInt16();
            mUnk0A = reader.ReadUInt16();

            // all offsets are relative to this point
            long curPos = reader.Pos();

            mStrings = new List<string>();

            for (ushort i = 0; i < mNumFonts; i++)
            {
                uint offset = reader.ReadUInt32();
                mStrings.Add(reader.ReadStringNTFrom(offset + curPos));
                reader.ReadUInt32();
            }

            reader.Seek(startPos + mSectionSize);
        }

        uint mSectionSize;
        ushort mNumFonts;
        ushort mUnk0A;

        List<string> mStrings;
    }
}
