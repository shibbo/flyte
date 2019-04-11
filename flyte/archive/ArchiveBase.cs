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

using System.Collections.Generic;

namespace flyte.archive
{
    /// <summary>
    /// Defines the archive types that are supported.
    /// </summary>
    public enum ArchiveType
    {
        DARC,
        NARC,
        RARC,
        U8,
        SARC
    }
    /// <summary>
    /// A class that represents as a base for all supported archive types.
    /// </summary>
    public class ArchiveBase
    {
        /// <summary>
        /// Only constructor. Defines the archive type for easy lookup.
        /// </summary>
        /// <param name="type"></param>
        public ArchiveBase(ArchiveType type)
        {
            mType = type;
        }

        public virtual List<string> getFileNames() { return mFileNames; }
        public ArchiveType getType() { return mType; }
        public void setFileNames(List<string> names) { mFileNames = names; }
        public virtual Dictionary<string, byte[]> getLayoutFiles() { return null; }
        public virtual Dictionary<string, byte[]> getLayoutAnimations() { return null; }
        public virtual Dictionary<string, byte[]> getLayoutImages() { return null; }
        public virtual Dictionary<string, byte[]> getLayoutControls() { return null; }
        public virtual bool isStringTableObfuscated() { return false; }

        ArchiveType mType;
        List<string> mFileNames;
    }
}
