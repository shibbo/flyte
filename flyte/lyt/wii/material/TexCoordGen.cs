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

namespace flyte.lyt.wii.material
{
    class TexCoordGen
    {
        public TexCoordGen(ref EndianBinaryReader reader)
        {
            mGenType = reader.ReadByte();
            mSource = reader.ReadByte();
            mMtx = reader.ReadByte();
            reader.ReadByte();
        }

        public void Write(ref EndianBinaryWriter writer)
        {
            writer.Write(mGenType);
            writer.Write(mSource);
            writer.Write(mMtx);
            writer.Write((byte)0);
        }

        byte mGenType;
        byte mSource;
        byte mMtx;
    }
}
