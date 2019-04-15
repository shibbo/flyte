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
    class AlphaCompare
    {
        public AlphaCompare(ref EndianBinaryReader reader)
        {
            mCompareMode = reader.ReadByte();
            reader.ReadBytes(0x3);
            mValue = reader.ReadUInt32();
        }

        byte mCompareMode;
        uint mValue;
    }
}
