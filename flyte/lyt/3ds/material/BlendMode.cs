using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds.material
{
    class BlendMode
    {
        enum BlendFactor
        {
            Factor0 = 0,
            Factor1 = 1,
            DestColor = 2,
            DestInvColor = 3,
            SourceAlpha = 4,
            SourceInvAlpha = 5,
            DestAlpha = 6,
            DestInvAlpha = 7,
            SourceColor = 8,
            SourceInvColor = 9
        }

        enum BlendOp
        {
            Disable = 0,
            Add = 1,
            Subtract = 2,
            ReverseSubtract = 3,
            SelectMin = 4,
            SelectMax = 5
        }

        enum LogicOp
        {
            Disable = 0,
            NoOp = 1,
            Clear = 2,
            Set = 3,
            Copy = 4,
            InvCopy = 5,
            Inv = 6,
            And = 7,
            Nand = 8,
            Or = 9,
            Nor = 10,
            Xor = 11,
            Equiv = 12,
            RevAnd = 13,
            InvAd = 14,
            RevOr = 15,
            InvOr = 16
        }

        public BlendMode(ref EndianBinaryReader reader)
        {
            mBlendOp = (BlendOp)reader.ReadByte();
            mSourceFactor = (BlendFactor)reader.ReadByte();
            mDestFactor = (BlendFactor)reader.ReadByte();
            mLogicOp = (LogicOp)reader.ReadByte();
        }

        BlendOp mBlendOp;
        BlendFactor mSourceFactor;
        BlendFactor mDestFactor;
        LogicOp mLogicOp;
    }
}
