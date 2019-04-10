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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static flyte.utils.Endian;

namespace flyte.lyt._3ds
{
    class BCLYT : LayoutBase
    {
        public BCLYT(ref EndianBinaryReader reader)
        {
            reader.SetEndianess(Endianess.Little);

            if (reader.ReadString(4) != "CLYT")
            {
                Console.WriteLine("Bad magic. Expected CLYT.");
                return;
            }

            mBOM = reader.ReadUInt16();
            mHeaderLength = reader.ReadUInt16();
            mRevision = reader.ReadUInt32();
            mFileSize = reader.ReadUInt32();
            mSectionCount = reader.ReadUInt32();

            mLYT1 = new LYT1(ref reader);

            string magic = "";

            for (uint i = 0; i < mSectionCount; i++)
            {
                magic = reader.ReadString(4);

                switch (magic)
                {
                    case "txl1":
                        break;
                    case "fnt1":
                        break;
                }
            }
        }

        ushort mBOM;
        ushort mHeaderLength;
        uint mRevision;
        uint mFileSize;
        uint mSectionCount;

        LYT1 mLYT1;
    }

    class LYT1
    {
        public LYT1(ref EndianBinaryReader reader)
        {
            if (reader.ReadString(4) != "lyt1")
            {
                Console.WriteLine("Bad magic. Expected LYT1.");
                return;
            }

            mSectionSize = reader.ReadUInt32();
            mOriginType = reader.ReadUInt32();
            mCanvasSizeX = reader.ReadF32();
            mCanvasSizeY = reader.ReadF32();
        }

        uint mSectionSize;
        uint mOriginType;
        float mCanvasSizeX;
        float mCanvasSizeY;
    }

}
