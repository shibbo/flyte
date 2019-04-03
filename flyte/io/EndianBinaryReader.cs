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
using System.Collections.Generic;
using System.IO;
using System.Text;
using flyte.utils;

namespace flyte.io
{
    /// <summary>
    /// A binary reader that supports little endian and big endian.
    /// </summary>
    public class EndianBinaryReader : BinaryReader
    {
        /// <summary>
        /// Construct a reader using a stream.
        /// </summary>
        /// <param name="s">The stream to read data from.</param>
        public EndianBinaryReader(Stream s) : base(s) { }
        /// <summary>
        /// Construct a reader using a stream with a specific encoding.
        /// </summary>
        /// <param name="s">The stream to read data from.</param>
        /// <param name="e">The encoding for data to use.</param>
        public EndianBinaryReader(Stream s, Encoding e) : base(s, e) { }
        /// <summary>
        /// Construct a reader using a byte array. Will automatically be converted into a MemoryStream.
        /// </summary>
        /// <param name="input">The input bytes to create a reader with.</param>
        public EndianBinaryReader(byte[] input) : base(new MemoryStream(input)) { }

        /// <summary>
        /// Enumerator defining the possible endianess of the reader.
        /// </summary>
        public enum Endianess
        {
            Little = 0,
            Big = 1
        }
        /// <summary>
        /// The the endianess of the binary reader.
        /// </summary>
        /// <param name="e">The endianess to use.</param>
        public void SetEndianess(Endianess e)
        {
            mEndianess = e;
        }
        /// <summary>
        /// Get the position of the reader.
        /// </summary>
        /// <returns>The position of the binary reader.</returns>
        public long Pos()
        {
            return BaseStream.Position;
        }

        /// <summary>
        /// Move the current location of the binary reader to the specified position.
        /// </summary>
        /// <param name="newPos">The position to move the reader to.</param>
        public void Seek(long newPos)
        {
            BaseStream.Position = newPos;
        }

        /// <summary>
        /// Reads a signed 16-bit value from the stream.
        /// </summary>
        /// <returns>The read value, as a signed 16-bit value.</returns>
        public override short ReadInt16()
        {
            UInt16 val = base.ReadUInt16();

            if (mEndianess == Endianess.Big)
                return (Int16)((val >> 8) | (val << 8));
            else
                return (Int16)val;
        }

        /// <summary>
        /// Reads a signed 32-bit value from the stream.
        /// </summary>
        /// <returns>The read value, as a signed 32-bit value.</returns>
        public override int ReadInt32()
        {
            UInt32 val = base.ReadUInt32();

            if (mEndianess == Endianess.Big)
                return (Int32)((val >> 24) | ((val & 0xFF0000) >> 8) | ((val & 0xFF00) << 8) | (val << 24));
            else
                return (Int32)val;

        }

        /// <summary>
        /// Reads a unsigned 16-bit value from the stream.
        /// </summary>
        /// <returns>The read value, as a unsigned 16-bit value.</returns>
        public override ushort ReadUInt16()
        {
            UInt16 val = base.ReadUInt16();

            if (mEndianess == Endianess.Big)
                return (UInt16)((val >> 8) | (val << 8));
            else
                return (UInt16)val;

        }

        /// <summary>
        /// Reads a unsigned 32-bit value from the stream.
        /// </summary>
        /// <returns>The read value, as a unsigned 32-bit value.</returns>
        public override uint ReadUInt32()
        {
            UInt32 val = base.ReadUInt32();

            if (mEndianess == Endianess.Big)
                return (UInt32)((val >> 24) | ((val & 0xFF0000) >> 8) | ((val & 0xFF00) << 8) | (val << 24));
            else
                return (UInt32)val;

        }

        /// <summary>
        /// Reads an unsigned 32-bit integer from a given position in the stream.
        /// </summary>
        /// <param name="where">The offset to read from.</param>
        /// <returns>The value read from the given position on the stream, as a unsigned 32-bit value.</returns>
        public uint ReadUInt32From(long where)
        {
            long curPos = Pos();
            Seek(where);
            uint ret = ReadUInt32();
            Seek(curPos);
            return ret;
        }

        public float ReadF32()
        {
            byte[] src = ReadBytes(4);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(src);

            float ret = BitConverter.ToSingle(src, 0);
            return ret;
        }

        /// <summary>
        /// Reads a length-defined string.
        /// </summary>
        /// <param name="length">The length of the string to read.</param>
        /// <returns>The read string.</returns>
        public string ReadString(int length)
        {
            return Encoding.ASCII.GetString(ReadBytes(length));
        }
        
        /// <summary>
        /// Reads a null-terminated (NT) string.
        /// </summary>
        /// <returns>The string, stripped of the null terminator.</returns>
        public string ReadStringNT()
        {
            string ret = "";
            char curChar;

            // keep iterating until a null terminator (0) is found
            while ((curChar = ReadChar()) != '\0')
                ret += curChar;

            return ret;
        }

        /// <summary>
        /// Reads a UTF-16 encoded string.
        /// </summary>
        /// <returns>The decoded string.</returns>
        public string ReadUTF16String()
        {
            List<byte> chars = new List<byte>();

            while (true)
            {
                ushort val = ReadUInt16();

                if (val == 0)
                    return Encoding.ASCII.GetString(chars.ToArray());
                else
                    chars.Add((byte)val);
            }
        }

        /// <summary>
        /// Reads a length-prefixed string from a given offset in the stream.
        /// </summary>
        /// <param name="where">The offset to read from.</param>
        /// <param name="len">The length of the string.</param>
        /// <returns>The string that was read.</returns>
        public string ReadStringFrom(long where, int len)
        {
            long curPos = Pos();
            Seek(where);
            string ret = ReadString(len);
            Seek(curPos);
            return ret;
        }

        /// <summary>
        /// Reads a null-terminated (NT) string from a given offset from the stream.
        /// </summary>
        /// <param name="where">The position of the NT string in the stream.</param>
        /// <returns>The read string, stripped of the null terminator.</returns>
        public string ReadStringNTFrom(long where)
        {
            long curPos = Pos();
            Seek(where);
            string ret = ReadStringNT();
            Seek(curPos);
            return ret;
        }

        /// <summary>
        /// Reads a UTF-16 encoded string.
        /// </summary>
        /// <param name="where">The position of the UTF-16 encoded string in the stream.</param>
        /// <returns>The decoded string.</returns>
        public string ReadUTF16StringFrom(long where)
        {
            long curPos = Pos();
            Seek(where);
            List<byte> chars = new List<byte>();

            while (true)
            {
                ushort val = ReadUInt16();

                if (val == 0)
                {
                    Seek(curPos);
                    return Encoding.ASCII.GetString(chars.ToArray());
                }
                else
                    chars.Add((byte)val); // casting to byte will remove the period, which is a part of UTF-16
            }
        }

        /// <summary>
        /// Reads bytes from a given location in the stream.
        /// </summary>
        /// <param name="where">Where the bytes are located in the stream.</param>
        /// <param name="num">The number of bytes in the array.</param>
        /// <returns>The read byte array.</returns>
        public byte[] ReadBytesFrom(long where, int num)
        {
            long curPos = Pos();
            Seek(where);
            byte[] ret = ReadBytes(num);
            Seek(curPos);
            return ret;
        }

        public RGBAColor8 ReadRGBAColor8()
        {
            RGBAColor8 ret = new RGBAColor8
            {
                r = ReadByte(),
                g = ReadByte(),
                b = ReadByte(),
                a = ReadByte()
            };

            return ret;
        }

        public RGBAColor16 ReadRGBAColor16()
        {
            RGBAColor16 ret = new RGBAColor16
            {
                r = ReadInt16(),
                g = ReadInt16(),
                b = ReadInt16(),
                a = ReadInt16()
            };

            return ret;
        }

        public UVCoordSet ReadUVCoordSet()
        {
            UVCoordSet ret = new UVCoordSet
            {
                topLeftU = ReadF32(),
                topLeftV = ReadF32(),
                topRightU = ReadF32(),
                topRightV = ReadF32(),
                bottomLeftU = ReadF32(),
                bottomLeftV = ReadF32(),
                bottomRightU = ReadF32(),
                bottomRightV = ReadF32()
            };

            return ret;
        }

        Endianess mEndianess;
    }
}
