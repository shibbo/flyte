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

        public void write(ref EndianBinaryWriter writer)
        {
            writer.Write(0x666E6C31);
            long sectionSizePos = writer.Pos();
            writer.Write(mNumFonts);
            writer.Write(mUnk0A);

            // we already account for 0xC bytes: magic, section length, num fonts, and 2 bytes padding
            int sectionSize = 0xC;

            // starting offset for our first string
            int curOffsetLoc = mNumFonts * 0x8;
            sectionSize += mNumFonts * 0x8;

            // this is our first offset, no matter what
            writer.Write(curOffsetLoc);
            writer.Write(0); // unused

            // we write our offsets now (-1 since we already assigned our first one)
            for (int i = 0; i < mNumFonts - 1; i++)
            {
                // get our length (+1 for NT)
                int len = getFontNameFromIndex(i).Length + 1;
                curOffsetLoc += len;
                writer.Write(curOffsetLoc);
                sectionSize += len;
            }

            // now lets write our fonts
            for (int i = 0; i < mNumFonts; i++)
            {
                writer.Write(getFontNameFromIndex(i));
                writer.Write((byte)0); // null terminator
            }

            long remainder = (writer.Pos() % 0x4);

            if (remainder != 0)
            {
                int numBytes = 0x4 - (int)remainder;
                sectionSize += numBytes;
            }

            writer.WriteAligned(0x4);

            writer.WriteInt32At(sectionSizePos, sectionSize);
        }

        public List<string> getStrings() { return mStrings; }

        public string getFontNameFromIndex(int idx)
        {
            return mStrings[idx];
        }

        uint mSectionSize;
        ushort mNumFonts;
        ushort mUnk0A;

        List<string> mStrings;
    }
}
