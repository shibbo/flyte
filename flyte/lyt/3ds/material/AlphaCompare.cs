using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds.material
{
    class AlphaCompare
    {
        public AlphaCompare(ref EndianBinaryReader reader)
        {
            mCompareMode = reader.ReadByte();
            mRef = reader.ReadF32();
            reader.ReadBytes(0x3);
        }

        byte mCompareMode;
        float mRef;
    }
}
