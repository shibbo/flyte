using flyte.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt.gc.blo2
{
    class TEX1
    {
        public TEX1(ref EndianBinaryReader reader)
        {
            long startPos = reader.Pos() - 0x4;

            uint sectionSize = reader.ReadUInt32();

            ushort count = reader.ReadUInt16();

            if (count == 0)
            {
                reader.Seek(startPos + sectionSize);
                return;
            }

            reader.ReadUInt16(); // constant (0xFFFF)
            reader.ReadUInt32(); // constant (0x10 (header size?))

            // all offsets are relative to this
            long offsetStartPos = reader.Pos();

            reader.ReadUInt16(); // the above count, again?

            mTextureNames = new List<string>();

            for (int i = 0; i < count; i++)
            {
                // the offset itself points to the resource type (0x2)
                // since this is a constant, we don't care, so we jump to the byte after it
                int offs = reader.ReadInt16() + 1;
                mTextureNames.Add(reader.ReadStringLengthPrefixFrom(offs + offsetStartPos));
            }

            reader.Seek(startPos + sectionSize);
        }

        public List<string> getStrings() { return mTextureNames; }

        List<string> mTextureNames;
    }
}
