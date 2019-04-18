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

using System;
using System.IO;
using System.Text;
using static flyte.utils.Endian;
using flyte.utils;

namespace flyte.io
{
    public class EndianBinaryWriter : BinaryWriter
    {
        public EndianBinaryWriter(Stream s) : base(s) { }

        public EndianBinaryWriter(Stream s, Encoding e) : base(s, e) { }

        public long Pos() { return this.BaseStream.Position; }

        public void SetEndianess(Endianess endianess) { mEndianess = endianess; }

        // byte is straightforward and already implemented

        public override void Write(short value)
        {
            ushort val = (ushort)value;

            if (mEndianess == Endianess.Little)
                base.Write((short)((val >> 8) | (val << 8)));
            else
                base.Write(value);
        }

        public override void Write(int value)
        {
            uint val = (uint)value;

            if (mEndianess == Endianess.Little)
                base.Write((int)((val >> 24) | ((val & 0xFF0000) >> 8) | ((val & 0xFF00) << 8) | (val << 24)));
            else
                base.Write(value);
        }

        public override void Write(ushort value)
        {
            if (mEndianess == Endianess.Little)
                base.Write((ushort)((value >> 8) | (value << 8)));
            else
                base.Write(value);
        }

        public override void Write(uint value)
        {
            if (mEndianess == Endianess.Little)
                base.Write((uint)((value >> 24) | ((value & 0xFF0000) >> 8) | ((value & 0xFF00) << 8) | (value << 24)));
            else
                base.Write(value);
        }

        public override void Write(byte[] bytes)
        {
            if (mEndianess == Endianess.Little)
                Array.Reverse(bytes);

            base.Write(bytes);
        }

        public override void Write(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            base.Write(bytes);
        }

        public void WriteStringNT(string str)
        {
            base.Write(str);
            base.Write((byte)0);
        }

        public void WriteRGBAColor8(RGBAColor8 color)
        {
            base.Write(color.r);
            base.Write(color.g);
            base.Write(color.b);
            base.Write(color.a);
        }

        public void WriteRGBAColor16(RGBAColor16 color)
        {
            base.Write(color.r);
            base.Write(color.g);
            base.Write(color.b);
            base.Write(color.a);
        }

        Endianess mEndianess;
    }
}
