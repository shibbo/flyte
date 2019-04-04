using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt.wii
{
    class USD1 : LayoutBase
    {
        public USD1(ref EndianBinaryReader reader)
        {
            base.setType("User Data");

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

            switch(mType)
            {
                // string
                case 0:
                    mValString = reader.ReadString(mSetting);
                    break;
                 // Int32
                case 1:
                    mValsInt = new List<int>();

                    for (int i = 0; i < mSetting; i++)
                        mValsInt.Add(reader.ReadInt32());
                    break;
                 // Float
                case 2:
                    mValsFloat = new List<float>();

                    for (int i = 0; i < mSetting; i++)
                        mValsFloat.Add(reader.ReadF32());
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
