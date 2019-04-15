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

namespace flyte.lyt.common.material
{
    class TexMap
    {
        enum WrapMode
        {
            Clamp = 0,
            Repeat = 1,
            Mirror = 2
        }

        enum FilterMode
        {
            Near = 0,
            Linear = 1
        }

        public TexMap(ref EndianBinaryReader reader)
        {
            mTextureIndex = reader.ReadUInt16();

            byte val1 = reader.ReadByte();
            byte val2 = reader.ReadByte();

            mWrapSMode = (WrapMode)(val1 & 0x3);
            mMinFilterMode = (FilterMode)((val1 >> 2) & 0x3);

            mWrapTMode = (WrapMode)(val2 & 0x3);
            mMaxFilterMode = (FilterMode)((val2 >> 2) & 0x3);
        }

        ushort mTextureIndex;

        WrapMode mWrapSMode;
        WrapMode mWrapTMode;
        FilterMode mMinFilterMode;
        FilterMode mMaxFilterMode;
    }
}
