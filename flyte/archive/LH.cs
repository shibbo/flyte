using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.archive
{
    // this is almost a direct port of the C++ code so lol
    struct LHContext
    {
        byte[] buf1;
        byte[] buf2;
    }

    static class LHFunctions
    {
        static uint getDecompressedSize(byte[] data)
        {
            uint outSize = (uint)(data[1] | (data[2] << 8) | (data[3] << 16));

            if (outSize == 0)
                outSize = (uint)(data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24));

            return outSize;
        }

        static uint loadLHPiece(byte[] buf, byte[] data, byte unk)
        {
            uint r0, r4, r6, r7, r9, r10, r11, r12, r30;
            uint inOffset, dataSize, copiedAmount;

            r6 = (uint)1 << unk;
            r7 = 2;
            r9 = 1;
            r10 = 0;
            r11 = 0;
            r12 = r6 - 1;
            r30 = r6 << 1;

            if (unk <= 8)
            {
                r6 = data[0];
                inOffset = 1;
                copiedAmount = 1;
            }
            else
            {
                r6 = (uint)(data[0] | (data[1] << 8));
                inOffset = 2;
                copiedAmount = 2;
            }

            dataSize = (r6 + 1) << 2;
            goto startLoop;

        loop:
            r6 = (uint)unk + 7;
            r6 = (r6 - r11) >> 3;

            if (r11 < unk)
            {
                for (int i = 0; i < r6; i++)
                {
                    r4 = data[inOffset];
                    r10 <<= 8;
                    r10 |= r4;
                    copiedAmount++;
                    inOffset++;
                }
                r11 += (r6 << 3);
            }

            if (r9 < r30)
            {
                r0 = r11 - unk;
                r9++;
                r0 = Convert.ToUInt32((int)r10 >> (int)r0);
                r0 &= r12;
                buf[r7] = Convert.ToByte(r0 >> 8);
                buf[r7 + 1] = Convert.ToByte(r0 & 0xFF);
                r7 += 2;
            }

            r11 -= unk;

        startLoop:
            if (copiedAmount < dataSize)
                goto loop;

            return copiedAmount;
        }
    }

    class LH
    {
        public LH() { throw new NotImplementedException(); }
    }
}
