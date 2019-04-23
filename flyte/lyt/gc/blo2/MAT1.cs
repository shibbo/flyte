using flyte.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
