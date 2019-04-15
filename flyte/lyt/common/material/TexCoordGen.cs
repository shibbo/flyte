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

namespace flyte.lyt.common.material
{
    class TexCoordGen
    {
        enum MatrixType
        {
            Matrix2x4 = 0
        }

        enum TextureGenerationType
        {
            Tex0 = 0,
            Tex1 = 1,
            Tex2 = 2,
            Ortho = 3,
            PaneBased = 4,
            PerspectiveProj = 5
        }


        public TexCoordGen(ref EndianBinaryReader reader)
        {
            mGenType = (MatrixType)reader.ReadByte();
            mSource = (TextureGenerationType)reader.ReadByte();

            mUnk = reader.ReadBytes(0xE);
        }

        public void Write(ref EndianBinaryWriter writer)
        {
            writer.Write((byte)mGenType);
            writer.Write((byte)mSource);
            writer.Write(mUnk);
        }

        MatrixType mGenType;
        TextureGenerationType mSource;

        byte[] mUnk;
    }
}
