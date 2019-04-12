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

namespace flyte.lyt._3ds.material
{
    class FontShadowParameter
    {
        public FontShadowParameter(ref EndianBinaryReader reader)
        {
            mBlackR = reader.ReadByte();
            mBlackG = reader.ReadByte();
            mBlackB = reader.ReadByte();
            mWhiteR = reader.ReadByte();
            mWhiteG = reader.ReadByte();
            mWhiteB = reader.ReadByte();
            mWhiteA = reader.ReadByte();
            reader.ReadByte();
        }

        byte mBlackR;
        byte mBlackG;
        byte mBlackB;
        byte mWhiteR;
        byte mWhiteG;
        byte mWhiteB;
        byte mWhiteA;
    }
}
