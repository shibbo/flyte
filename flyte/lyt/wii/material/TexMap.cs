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
using static flyte.utils.Bit;

namespace flyte.lyt.wii.material
{
    class TexMap
    {
        public TexMap(ref EndianBinaryReader reader)
        {
            mTextureNum = reader.ReadUInt16();

            byte v1 = reader.ReadByte();
            byte v2 = reader.ReadByte();

            mWrapS = (int)ExtractBits(v1, 2, 30);
            mWrapT = (int)ExtractBits(v2, 2, 30);
            mMinFilter = (int)(ExtractBits(v1, 3, 27) + 1) & 7;
            mMagFilter = (int)(ExtractBits(v2, 1, 29) + 1) & 1;
        }

        ushort mTextureNum;
        int mWrapS;
        int mWrapT;
        int mMinFilter;
        int mMagFilter;

        string mTextureName;
    }
}
