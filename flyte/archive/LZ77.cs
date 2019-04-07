using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.archive
{
    class LZ77
    {
        public LZ77(ref EndianBinaryReader reader)
        {
            uint curSize = 0;
            int pos, copylen;
            byte first, second, third, fourth;

            uint thing = reader.ReadUInt32();
            mDecompressedSize = thing >> 8;
            mDecompressedData = new byte[mDecompressedSize];

            while (curSize < mDecompressedSize && reader.Pos() < reader.BaseStream.Length)
            {
                byte flags = reader.ReadByte();

                for (int x = 7; x >= 0; x--)
                {
                    if (curSize >= mDecompressedSize)
                        break;

                    byte val = Convert.ToByte(flags & (1 << x));

                    if (val > 0)
                    {
                        first = reader.ReadByte();
                        second = reader.ReadByte();

                        if (first < 0x20)
                        {
                            third = reader.ReadByte();

                            if (first >= 0x10)
                            {
                                fourth = reader.ReadByte();

                                pos = (((third & 0xF) << 8) | fourth) + 1;
                                copylen = ((second << 4) | ((first & 0xF) << 12) | (third >> 4)) + 273;
                            }
                            else
                            {
                                pos = (((second & 0xF) << 8) | third) + 1;
                                copylen = (((first & 0xF) << 4) | (second >> 4)) + 17;
                            }
                        }
                        else
                        {
                            pos = (((first & 0xF) << 8) | second) + 1;
                            copylen = (first >> 4) + 1;
                        }

                        int start = (int)curSize - pos;
                        int end = (int)curSize - pos + copylen;

                        byte[] copyBuff = getBytesFrom(start, end);
                        int copyBuffLen = copyBuff.Length;

                        for (int j = 0; j < copylen; j++)
                        {
                            mDecompressedData[curSize] = copyBuff[j % copyBuffLen];
                            curSize++;
                        }
                    }
                    else
                    {
                        byte otherVal = reader.ReadByte();
                        mDecompressedData[curSize] = otherVal;
                        curSize++;
                    }
                }
            }
        }

        byte[] getBytesFrom(int start, int end)
        {
            byte[] ret = new byte[end - start];

            int curIdx = 0;

            for (int i = start; i < end; i++)
            {
                ret[curIdx] = mDecompressedData[i];
                curIdx++;
            }

            return ret;
        }

        public byte[] getData() { return mDecompressedData; }

        uint mDecompressedSize;
        byte[] mDecompressedData;
    }
}