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
using System.ComponentModel;

namespace flyte.lyt.wii.material
{
    public class TexSRT
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

        [DisplayName("X"), CategoryAttribute("General"), DescriptionAttribute("X position.")]
        public float PosX
        {
            get { return mTransX; }
            set { mTransY = value; }
        }

        [DisplayName("Y"), CategoryAttribute("General"), DescriptionAttribute("Y position.")]
        public float PosY
        {
            get { return mTransY; }
            set { mTransY = value; }
        }

        [DisplayName("Rotation"), CategoryAttribute("General"), DescriptionAttribute("Rotation of the SRT.")]
        public float Rotation
        {
            get { return mRotation; }
            set { mRotation = value; }
        }

        [DisplayName("Scale X"), CategoryAttribute("General"), DescriptionAttribute("X scale.")]
        public float ScaleX
        {
            get { return mScaleX; }
            set { mScaleY = value; }
        }

        [DisplayName("Scale Y"), CategoryAttribute("General"), DescriptionAttribute("Y scale.")]
        public float ScaleY
        {
            get { return mScaleY; }
            set { mScaleY = value; }
        }
    }
}
