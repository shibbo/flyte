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
    class TevStage
    {
        public TevStage(ref EndianBinaryReader reader)
        {
            // we read a bytearray and just read from that
            // there are too many operations running around and debugging would suck
            byte[] data = reader.ReadBytes(0x10);

            // TEV order
            mTexCoord = data[0];
            mColor = data[1];
            mTexMap = data[2] | (int)(ExtractBits(data[3], 1, 31) << 8);

            // Swap modes
            mRasSwapMode = (int)ExtractBits(data[3], 2, 29);
            mTexSwapMode = (int)ExtractBits(data[3], 2, 27);

            // Color In
            mColorInA = (int)ExtractBits(data[4], 4, 28);
            mColorInB = (int)ExtractBits(data[4], 4, 24);
            mColorInC = (int)ExtractBits(data[5], 4, 28);
            mColorInD = (int)ExtractBits(data[5], 4, 24);

            // Color OP
            mColorOP = (int)ExtractBits(data[6], 4, 28);
            mColorBias = (int)ExtractBits(data[6], 2, 26);
            mColorScale = (int)ExtractBits(data[6], 2, 24);
            mColorClamp = (int)ExtractBits(data[7], 1, 31);
            mColorOutReg = (int)ExtractBits(data[7], 2, 29);

            // Alpha In
            mAlphaInA = (int)ExtractBits(data[8], 4, 28);
            mAlphaInB = (int)ExtractBits(data[8], 4, 24);
            mAlphaInC = (int)ExtractBits(data[9], 4, 28);
            mAlphaInD = (int)ExtractBits(data[9], 4, 24);

            // Alpha
            mAlphaOP = (int)ExtractBits(data[10], 4, 28);
            mAlphaBias = (int)ExtractBits(data[10], 2, 26);
            mAlphaScale = (int)ExtractBits(data[10], 2, 24);
            mAlphaClamp = (int)ExtractBits(data[11], 1, 31);
            mAlphaOutReg = (int)ExtractBits(data[11], 2, 29);

            // Constants
            mColorConst = (int)ExtractBits(data[7], 5, 24);
            mAlphaConst = (int)ExtractBits(data[11], 5, 24);

            // Indirect stuff
            mIndStage = data[12];
            mIndFormat = (int)ExtractBits(data[15], 2, 30);
            mIndBias = (int)ExtractBits(data[13], 3, 29);
            mIndMtx = (int)ExtractBits(data[13], 4, 25);
            mIndWrapS = (int)ExtractBits(data[14], 3, 29);
            mIndWrapT = (int)ExtractBits(data[14], 3, 26);
            mIndAddPrev = (int)ExtractBits(data[15], 1, 29);
            mIndUtcLod = (int)ExtractBits(data[15], 1, 28);
            mIndAlphaSel = (int)ExtractBits(data[15], 2, 26);
        }

        int mTexCoord;
        int mColor;
        int mTexMap;

        int mRasSwapMode;
        int mTexSwapMode;

        int mColorInA;
        int mColorInB;
        int mColorInC;
        int mColorInD;

        int mColorOP;
        int mColorBias;
        int mColorScale;
        int mColorClamp;
        int mColorOutReg;

        int mAlphaInA;
        int mAlphaInB;
        int mAlphaInC;
        int mAlphaInD;

        int mAlphaOP;
        int mAlphaBias;
        int mAlphaScale;
        int mAlphaClamp;
        int mAlphaOutReg;

        int mColorConst;
        int mAlphaConst;

        int mIndStage;
        int mIndFormat;
        int mIndBias;
        int mIndMtx;
        int mIndWrapS;
        int mIndWrapT;
        int mIndAddPrev;
        int mIndUtcLod;
        int mIndAlphaSel;
    }
}
