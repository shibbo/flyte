using flyte.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt.gc.blo2
{
    class FNT1
    {
        public FNT1(ref EndianBinaryReader reader)
        {
            long startPos = reader.Pos() - 0x4;

            uint sectionSize = reader.ReadUInt32();

            ushort count = reader.ReadUInt16();

            if (count == 0)
            {
                reader.Seek(startPos + sectionSize);
                return;
            }

            reader.ReadUInt16();
            reader.ReadUInt32();

            long offsetStartPos = reader.Pos();

            reader.ReadUInt16();

            mFontNames = new List<string>();

            for (int i = 0; i < count; i++)
            {
                int offs = reader.ReadInt16() + 1;
                mFontNames.Add(reader.ReadStringLengthPrefixFrom(offs + offsetStartPos));
            }

            reader.Seek(startPos + sectionSize);
        }

        public List<string> getStrings() { return mFontNames; }

        List<string> mFontNames;
    }
}