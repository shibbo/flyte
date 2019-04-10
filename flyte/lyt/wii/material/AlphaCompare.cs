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
    class AlphaCompare
    {
        public AlphaCompare(ref EndianBinaryReader reader)
        {
            byte val = reader.ReadByte();
            mComp0 = (byte)ExtractBits(val, 4, 28);
            mComp1 = (byte)ExtractBits(val, 4, 24);
            mOP = reader.ReadByte();
            mRef1 = reader.ReadByte();
            mRef2 = reader.ReadByte();
        }

        public void Write(ref EndianBinaryWriter writer)
        {
            writer.Write((byte)(mComp0 | (mComp1 << 4)));
            writer.Write(mOP);
            writer.Write(mRef1);
            writer.Write(mRef2);
        }

        byte mComp0;
        byte mComp1;
        byte mOP;
        byte mRef1;
        byte mRef2;
    }
}
