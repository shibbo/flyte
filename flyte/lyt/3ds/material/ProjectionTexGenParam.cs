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

using System;
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
