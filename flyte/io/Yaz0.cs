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

// Borrowed from WhiteholeCS by StapleButter

namespace flyte.io
{
    /// <summary>
    /// Implementation of Yaz0 compression.
    /// </summary>
    public class Yaz0 : MemoryStream
    {
        /// <summary>
        /// Constructs a stream for an inputted Yaz0 file.
        /// </summary>
        /// <param name="s">The stream to read the data from.</param>
        public Yaz0(Stream s) : base(1)
        {
            mStream = s;
            mStream.Position = 0;
            byte[] buffer = new byte[mStream.Length];
            mStream.Read(buffer, 0, (int)mStream.Length);

            Yaz0.Decompress(ref buffer);
            // we first move the buffer back to the beginning to write it to our stream
            Position = 0;
            Write(buffer, 0, buffer.Length);
            // now that we moved the buffer, we have to go back to the beginning for any readers
            // that read the decompressed data
            Position = 0;
        }

        /// <summary>
        /// Decompress data from a Yaz0.
        /// </summary>
        /// <param name="data">The data source to insert into, and to read from.</param>
        public static void Decompress(ref byte[] data)
        {
            if (data[0] != 'Y' || data[1] != 'a' || data[2] != 'z' || data[3] != '0')
                return;

            int fullsize = (data[4] << 24) | (data[5] << 16) | (data[6] << 8) | data[7];
            byte[] output = new byte[fullsize];

            int inpos = 16, outpos = 0;
            while (outpos < fullsize)
            {
                byte block = data[inpos++];

                for (int i = 0; i < 8; i++)
                {
                    if ((block & 0x80) != 0)
                    {
                        // copy one plain byte
                        output[outpos++] = data[inpos++];
                    }
                    else
                    {
                        // copy N compressed bytes
                        byte b1 = data[inpos++];
                        byte b2 = data[inpos++];

                        int dist = ((b1 & 0xF) << 8) | b2;
                        int copysrc = outpos - (dist + 1);

                        int nbytes = b1 >> 4;
                        if (nbytes == 0) nbytes = data[inpos++] + 0x12;
                        else nbytes += 2;

                        for (int j = 0; j < nbytes; j++)
                            output[outpos++] = output[copysrc++];
                    }

                    block <<= 1;
                    if (outpos >= fullsize || inpos >= data.Length)
                        break;
                }
            }

            Array.Resize(ref data, fullsize);
            output.CopyTo(data, 0);
        }

        private Stream mStream;
    }
}
