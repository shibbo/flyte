using flyte.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt.common
{
    class PRT1 : PAN1
    {
        public PRT1(ref EndianBinaryReader reader) : base(ref reader)
        {
            long pos = reader.Pos() - 0x54;

            base.setType("Part");

            reader.Seek(pos + mSectionSize);
        }
    }

    class PRTProperty
    {

    }
}
