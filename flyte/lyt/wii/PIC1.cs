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
using System.Collections.Generic;
using flyte.utils;

namespace flyte.lyt.wii
{
    class PIC1 : PAN1
    {
        public PIC1(ref EndianBinaryReader reader, ref MAT1 materials) : base(ref reader)
        {
            base.setType("Picture");

            mTopLeftColor = reader.ReadRGBAColor8();
            mTopRightColor = reader.ReadRGBAColor8();
            mBottomLeftColor = reader.ReadRGBAColor8();
            mBottomRightColor = reader.ReadRGBAColor8();
            mMaterialIndex = reader.ReadUInt16();
            mNumUVSets = reader.ReadByte();
            mUnk5F = reader.ReadByte();

            mUVCoordinates = new List<UVCoordSet>();

            for (byte i = 0; i < mNumUVSets; i++)
                mUVCoordinates.Add(reader.ReadUVCoordSet());

            mMaterialName = materials.getMaterialNameFromIndex(mMaterialIndex);
        }

        RGBAColor8 mTopLeftColor;
        RGBAColor8 mTopRightColor;
        RGBAColor8 mBottomLeftColor;
        RGBAColor8 mBottomRightColor;
        ushort mMaterialIndex;
        byte mNumUVSets;
        byte mUnk5F;

        List<UVCoordSet> mUVCoordinates;
        string mMaterialName;
    }
}