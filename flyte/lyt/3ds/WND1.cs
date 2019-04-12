using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds
{
    class WND1 : PAN1
    {
        public WND1(ref EndianBinaryReader reader, ref MAT1 materials) : base(ref reader)
        {
            base.setType("Window");

            long startPos = reader.Pos() - 0x4C;
            reader.Seek(startPos + mSectionSize);
        }
    }
}
