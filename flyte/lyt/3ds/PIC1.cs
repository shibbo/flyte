using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds
{
    class PIC1 : PAN1
    {
        public PIC1(ref EndianBinaryReader reader) : base(ref reader)
        {
            base.setType("Picture");

            long pos = reader.Pos() - 0x4C;
            reader.Seek(pos + base.mSectionSize);
        }
    }
}
