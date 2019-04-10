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
using System;
using System.Collections.Generic;

namespace flyte.lyt._3ds
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

        public List<string> getStrings() { return mStrings; }

        public bool doesImageExistWithExt(string name)
        {
            return mStrings.Contains(name);
        }

        uint mSectionSize;
        ushort mNumTextures;
        ushort mUnk0A;

        List<string> mStrings;
    }
}
