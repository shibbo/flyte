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
using System;

namespace flyte.lyt.wii.material
{
    struct SwapMode
    {
        public int r;
        public int g;
        public int b;
        public int a;
    }
    class TevSwapTable
    {
        public TevSwapTable(ref EndianBinaryReader reader)
        {
            mSwapMode = new SwapMode[4];

            for (int i = 0; i < 4; i++)
            {
                byte val = reader.ReadByte();
                mSwapMode[i].r = (val >> 2) & 0x3;
                mSwapMode[i].g = (val >> 4) & 0x3;
                mSwapMode[i].b = (val >> 6) & 0x3;
                mSwapMode[i].a = (val >> 8) & 0x3;
            }
        }

        public void Write(ref EndianBinaryWriter writer)
        {
            for (int i = 0; i < 4; i++)
            {
                byte val = 0;

                val |= Convert.ToByte(mSwapMode[i].r);
                val |= Convert.ToByte(mSwapMode[i].g << 2);
                val |= Convert.ToByte(mSwapMode[i].b << 4);
                val |= Convert.ToByte(mSwapMode[i].a << 6);
                writer.Write(val);
            }
        }

        SwapMode[] mSwapMode;
    }
}
