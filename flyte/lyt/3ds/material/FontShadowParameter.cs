using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds.material
{
    class FontShadowParameter
    {
        public FontShadowParameter(ref EndianBinaryReader reader)
        {
            mBlackR = reader.ReadByte();
            mBlackG = reader.ReadByte();
            mBlackB = reader.ReadByte();
            mWhiteR = reader.ReadByte();
            mWhiteG = reader.ReadByte();
            mWhiteB = reader.ReadByte();
            mWhiteA = reader.ReadByte();
            reader.ReadByte();
        }

        byte mBlackR;
        byte mBlackG;
        byte mBlackB;
        byte mWhiteR;
        byte mWhiteG;
        byte mWhiteB;
        byte mWhiteA;
    }
}
