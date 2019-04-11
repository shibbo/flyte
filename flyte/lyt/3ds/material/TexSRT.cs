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
    class TexSRT
    {
        public TexSRT(ref EndianBinaryReader reader)
        {
            mTransX = reader.ReadF32();
            mTransY = reader.ReadF32();
            mRotation = reader.ReadF32();
            mScaleX = reader.ReadF32();
            mScaleY = reader.ReadF32();
        }

        public void Write(ref EndianBinaryWriter writer)
        {
            writer.Write(mTransX);
            writer.Write(mTransY);
            writer.Write(mRotation);
            writer.Write(mScaleX);
            writer.Write(mScaleY);
        }

        float mTransX;
        float mTransY;
        float mRotation;
        float mScaleX;
        float mScaleY;
    }
}
