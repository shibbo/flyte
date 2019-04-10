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
    class BlendMode
    {
        public BlendMode(ref EndianBinaryReader reader)
        {
            mSource = reader.ReadByte();
            mSourceFactor = reader.ReadByte();
            mDestFactor = reader.ReadByte();
            mOP = reader.ReadByte();
        }

        byte mSource;
        byte mSourceFactor;
        byte mDestFactor;
        byte mOP;
    }
}
