using flyte.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.lyt.wii
{
    class BND1 : PAN1
    {
        public BND1(ref EndianBinaryReader reader) : base(ref reader)
        {
            base.setType("Bounding Panel");
        }
    }
}
