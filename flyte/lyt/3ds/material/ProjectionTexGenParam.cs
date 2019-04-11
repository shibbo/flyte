using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt._3ds.material
{
    class ProjectionTexGenParam
    {
        public ProjectionTexGenParam(ref EndianBinaryReader reader)
        {
            mPosX = reader.ReadF32();
            mPosY = reader.ReadF32();
            mScaleX = reader.ReadF32();
            mScaleY = reader.ReadF32();

            byte flag = reader.ReadByte();
            mIsFittingLayoutSize = Convert.ToBoolean(flag & 0x1);
            mIsFittingPaneSize = Convert.ToBoolean(flag & 0x2);
            mIsAdjustPRojectionSR = Convert.ToBoolean(flag & 0x3);

            reader.ReadBytes(0x3); // padding
        }

        float mPosX;
        float mPosY;
        float mScaleX;
        float mScaleY;

        bool mIsFittingLayoutSize;
        bool mIsFittingPaneSize;
        bool mIsAdjustPRojectionSR;
    }
}
