/*
    © 2019 - shibboleet
    flyte is free software: you can redistribute it and/or modify it under
    the terms of the GNU General Public License as published by the Free
    Software Foundation, either version 3 of the License, or (at your option)
    any later version.
    flyte is distributed in the hope that it will be useful, but WITHOUT ANY 
    WARRANTY; See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along 
    with flyte. If not, see http://www.gnu.org/licenses/.
*/

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
