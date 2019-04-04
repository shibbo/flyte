using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt.wii
{
    class GRP1 : LayoutBase
    {
        public GRP1(ref EndianBinaryReader reader)
        {
            long startPos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mGroupName = reader.ReadString(0x10).Replace("\0", "");
            mNumPanes = reader.ReadUInt16();
            reader.ReadUInt16(); // padding

            // root group never has any entries
            if (mNumPanes != 0)
            {
                mEntries = new List<string>();

                for (ushort i = 0; i < mNumPanes; i++)
                    mEntries.Add(reader.ReadString(0x10).Replace("\0", ""));
            }

            reader.Seek(startPos + mSectionSize);
        }

        uint mSectionSize;
        string mGroupName;
        ushort mNumPanes;

        List<string> mEntries;
    }
}
