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

using System.Collections.Generic;
using flyte.io;

namespace flyte.lyt.wii
{
    class USD1 : UserdataBase
    {
        public USD1(ref EndianBinaryReader reader)
        {
            long curPos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mNumEntries = reader.ReadUInt16();
            reader.ReadUInt16();

            mEntries = new List<USD1Entry>();

            for (ushort i = 0; i < mNumEntries; i++)
                mEntries.Add(new USD1Entry(ref reader));

            reader.Seek(curPos + mSectionSize);
        }

        uint mSectionSize;
        ushort mNumEntries;

        List<USD1Entry> mEntries;
    }

    class USD1Entry
    {
        public USD1Entry(ref EndianBinaryReader reader)
        {
            long curPos = reader.Pos();

            mNameOffset = reader.ReadUInt32();
            mName = reader.ReadStringNTFrom(curPos + mNameOffset);
            mDataOffset = reader.ReadUInt32();
            mSetting = reader.ReadUInt16();
            mType = reader.ReadByte();
            mUnk0B = reader.ReadByte();

            switch (mType)
            {
                // string
                case 0:
                    mValString = reader.ReadStringFrom(curPos + mDataOffset, mSetting);
                    break;
                // Int32
                case 1:
                    mValsInt = new List<int>();

                    for (int i = 0; i < mSetting; i++)
                        mValsInt.Add(reader.ReadInt32From(mDataOffset + curPos));
                    break;
                // Float
                case 2:
                    mValsFloat = new List<float>();

                    for (int i = 0; i < mSetting; i++)
                        mValsFloat.Add(reader.ReadF32From(mDataOffset + curPos));
                    break;
            }
        }

        uint mNameOffset;
        uint mDataOffset;
        ushort mSetting;
        byte mType;
        byte mUnk0B;

        string mName;

        // i dont feel like implementing something better
        List<int> mValsInt;
        List<float> mValsFloat;
        string mValString;
    }
}
