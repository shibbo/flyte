using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds.material
{
    class TevStage
    {
        public TevStage(ref EndianBinaryReader reader)
        {
            mRGBMode = reader.ReadByte();
            mAlphaMode = reader.ReadByte();

            reader.ReadUInt16();
        }

        byte mRGBMode;
        byte mAlphaMode;
    }
}
