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

namespace flyte.lyt.gc.blo2
{
    class MAT1
    {
        public MAT1(ref EndianBinaryReader reader)
        {
            Console.WriteLine("MAT1 not supported yet. Skipping...");

            long startPos = reader.Pos() - 0x4;

            uint sectionSize = reader.ReadUInt32();

            reader.Seek(startPos + sectionSize);
        }
    }
}
