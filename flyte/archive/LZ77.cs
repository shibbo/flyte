using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

/* DOES NOT WORK */
namespace flyte.archive
{
    class LZ77
    {
        public LZ77(ref EndianBinaryReader reader)
        {
            throw new NotImplementedException();

            uint curSize = 0;
            int pos, copylen;
            byte first, second, third, fourth;

            uint thing = reader.ReadUInt32();

            Console.WriteLine(thing);

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

                        byte[] copyBuff = getBytesFrom((int)curSize - pos, (int)curSize - pos + copylen);
                        int copyBuffLen = copyBuff.Length;

                        for (int j = 0; j < copylen; j++)
                            mDecompressedData[reader.Pos() - 4] = copyBuff[j % copyBuffLen];

                        curSize += (uint)copylen;
                    }
                    else
                    {
                        byte otherVal = reader.ReadByte();
                        mDecompressedData[reader.Pos() - 4] = otherVal;
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
