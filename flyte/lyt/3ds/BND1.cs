using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds
{
    class BND1 : PAN1
    {
        public BND1(ref EndianBinaryReader reader) : base(ref reader)
        {
            base.setType("Bounding Box");
        }
    }
}
