using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds.material
{
    class IndirectParameter
    {
        public IndirectParameter(ref EndianBinaryReader reader)
        {
            mRotation = reader.ReadF32();
            mScaleX = reader.ReadF32();
            mScaleY = reader.ReadF32();
        }

        float mRotation;
        float mScaleX;
        float mScaleY;
    }
}
