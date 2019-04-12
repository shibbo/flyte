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
using System.ComponentModel;
using flyte.io;

namespace flyte.lyt._3ds
{
    class GRP1 : LayoutBase
    {
        public GRP1(ref EndianBinaryReader reader)
        {
            long startPos = reader.Pos() - 4;

            mSectionSize = reader.ReadUInt32();
            mName = reader.ReadString(0x10).Replace("\0", "");
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
        ushort mNumPanes;

        List<string> mEntries;

        [DisplayName("Name"), CategoryAttribute("General"), DescriptionAttribute("The name of the group.")]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
    }
}
