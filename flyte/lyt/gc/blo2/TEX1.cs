/*
    © 2019 - shibboleet
    flyte is free software: you can redistribute it and/or modify it under
    the terms of the GNU General Public License as published by the Free
    Software Foundation, either version 3 of the License, or (at your option)
    any later version.
    flyte is distributed in the hope that it will be useful, but WITHOUT ANY 
    WARRANTY; See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along 
    with flyte. If not, see http://www.gnu.org/licenses/.
*/

using flyte.io;
using System.Collections.Generic;

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
