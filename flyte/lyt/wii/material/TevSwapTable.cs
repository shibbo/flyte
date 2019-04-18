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
using System.ComponentModel;

namespace flyte.lyt.wii.material
{
    public class SwapMode
    {
        public SwapMode(ref EndianBinaryReader reader)
        {
            byte val = reader.ReadByte();
            r = (val >> 2) & 0x3;
            g = (val >> 4) & 0x3;
            b = (val >> 6) & 0x3;
            a = (val >> 8) & 0x3;
        }

        public int r;
        public int g;
        public int b;
        public int a;

        [DisplayName("Red"), CategoryAttribute("General"), DescriptionAttribute("Red color.")]
        public int Red
        {
            get { return r; }
            set { r = value; }
        }

        [DisplayName("Green"), CategoryAttribute("General"), DescriptionAttribute("Green color.")]
        public int Green
        {
            get { return g; }
            set { g = value; }
        }

        [DisplayName("Blue"), CategoryAttribute("General"), DescriptionAttribute("Blue color.")]
        public int Blue
        {
            get { return b; }
            set { b = value; }
        }

        [DisplayName("Alpha"), CategoryAttribute("General"), DescriptionAttribute("Alpha color.")]
        public int Alpha
        {
            get { return a; }
            set { a = value; }
        }
    }
    public class TevSwapTable
    {
        public TevSwapTable(ref EndianBinaryReader reader)
        {
            mSwapMode = new SwapMode[4];

            for (int i = 0; i < 4; i++)
                mSwapMode[i] = new SwapMode(ref reader);
        }

        public SwapMode getSwapModeAtIndex(int idx)
        {
            if (idx < 4)
                return mSwapMode[idx];
            else
                return null;
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
