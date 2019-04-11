using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds.material
{
    class TexMap
    {
        enum WrapMode
        {
            Clamp = 0,
            Repeat = 1,
            Mirror = 2
        }

        enum FilterMode
        {
            Near = 0,
            Linear = 1
        }

        public TexMap(ref EndianBinaryReader reader)
        {
            mTextureIndex = reader.ReadUInt16();

            byte val1 = reader.ReadByte();
            byte val2 = reader.ReadByte();

            mWrapSMode = (WrapMode)(val1 & 0x3);
            mMinFilterMode = (FilterMode)((val1 >> 2) & 0x3);

            mWrapTMode = (WrapMode)(val2 & 0x3);
            mMaxFilterMode = (FilterMode)((val2 >> 2) & 0x3);
        }

        ushort mTextureIndex;

        WrapMode mWrapSMode;
        WrapMode mWrapTMode;
        FilterMode mMinFilterMode;
        FilterMode mMaxFilterMode;
    }
}
