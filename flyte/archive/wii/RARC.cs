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
using flyte.io;

namespace flyte.archive.wii
{
    public class RARC : ArchiveBase
    {
        public RARC(ref EndianBinaryReader reader) : base(ArchiveType.RARC)
        {
            uint magic = reader.ReadUInt32();

            if (magic != 0x52415243)
                return;

            // we just skip everything we don't need for packing
            reader.Seek(0xC);

            mFileDataOffset = reader.ReadUInt32() + 0x20;
            reader.Seek(0x20);
            mNumDirNodes = reader.ReadUInt32();
            mDirNodesOffset = reader.ReadUInt32() + 0x20;
            reader.ReadUInt32();
            mFileEntriesOffset = reader.ReadUInt32() + 0x20;
            reader.ReadUInt32();
            mStringTableOffset = reader.ReadUInt32() + 0x20;

            mDirEntries = new Dictionary<uint, DirectoryEntry>();
            mFileEntries = new Dictionary<uint, FileEntry>();

            DirectoryEntry root = new DirectoryEntry();
            root.ID = 0;
            root.ParentDir = 0xFFFFFFFF;

            reader.Seek(mDirNodesOffset + 0x6);

            ushort stringOffset = reader.ReadUInt16();
            root.Name = reader.ReadStringNTFrom(stringOffset + mStringTableOffset);
            root.FullName = "/" + root.Name;

            mDirEntries.Add(0, root);

            for (uint i = 0; i < mNumDirNodes; i++)
            {
                DirectoryEntry parentDir = mDirEntries[i];

                reader.Seek(mDirNodesOffset + (i * 0x10) + 10);

                ushort numEntries = reader.ReadUInt16();
                uint firstIdx = reader.ReadUInt32();

                for (uint j = 0; j < numEntries; j++)
                {
                    uint entryoffset = mFileEntriesOffset + ((j + firstIdx) * 0x14);
                    reader.Seek(entryoffset);

                    uint fileid = reader.ReadUInt16();
                    reader.ReadBytes(0x4);
                    uint nameOffset = reader.ReadUInt16();
                    uint dataOffset = reader.ReadUInt32();
                    uint dataSize = reader.ReadUInt32();

                    string name = reader.ReadStringNTFrom(mStringTableOffset + nameOffset);

                    // root stuff
                    if (name == "." || name == "..")
                        continue;

                    string fullName = parentDir.FullName + "/" + name;

                    if (fileid == 0xFFFF)
                    {
                        DirectoryEntry dir = new DirectoryEntry
                        {
                            EntryOffset = entryoffset,
                            ID = dataOffset,
                            ParentDir = i,
                            NameOffset = nameOffset,
                            Name = name,
                            FullName = fullName
                        };

                        mDirEntries.Add(dataOffset, dir);
                    }
                    else
                    {
                        FileEntry file = new FileEntry
                        {
                            EntryOffset = entryoffset,
                            ID = fileid,
                            ParentDir = i,
                            NameOffset = nameOffset,
                            DataOffset = dataOffset,
                            DataSize = dataSize,
                            Name = name,
                            FullName = fullName
                        };

                        // now we add our file data
                        file.Data = reader.ReadBytesFrom(dataOffset + mFileDataOffset, (int)dataSize);

                        mFileEntries.Add(fileid, file);
                    }
                }
            }
        }

        public override Dictionary<string, byte[]> getLayoutFiles()
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            foreach(KeyValuePair<uint, FileEntry> pair in mFileEntries)
            {
                if (pair.Value.Name.Contains(".brlyt") | pair.Value.Name.Contains(".blo"))
                    files.Add(pair.Value.FullName, pair.Value.Data);
            }

            return files;
        }

        public override Dictionary<string, byte[]> getLayoutAnimations()
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            foreach (KeyValuePair<uint, FileEntry> pair in mFileEntries)
            {
                if (pair.Value.Name.Contains(".brlan"))
                    files.Add(pair.Value.FullName, pair.Value.Data);
            }

            return files;
        }

        public override Dictionary<string, byte[]> getLayoutImages()
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            foreach (KeyValuePair<uint, FileEntry> pair in mFileEntries)
            {
                if (pair.Value.Name.Contains(".tpl") || pair.Value.Name.Contains(".bti"))
                    files.Add(pair.Value.FullName, pair.Value.Data);
            }

            return files;
        }

        public override byte[] getDataByName(string name)
        {
            byte[] ret = null;

            foreach (KeyValuePair<uint, FileEntry> pair in mFileEntries)
            {
                if (pair.Value.FullName == name)
                    return pair.Value.Data;
            }

            return ret;
        }

        public override List<string> getArchiveFileNames()
        {
            List<string> strs = new List<string>();


            foreach (KeyValuePair<uint, FileEntry> pair in mFileEntries)
            {
                if (pair.Value.Name.Contains(".arc"))
                    strs.Add(pair.Value.FullName);
            }

            return strs;
        }

        private class FileEntry
        {
            public uint EntryOffset;

            public uint ID;
            public uint NameOffset;
            public uint DataOffset;
            public uint DataSize;

            public uint ParentDir;

            public string Name;
            public string FullName;
            public byte[] Data;
        }

        private class DirectoryEntry
        {
            public uint EntryOffset;

            public uint ID;
            public uint NameOffset;

            public uint ParentDir;

            public string Name;
            public string FullName;
        }

        uint mFileDataOffset;
        uint mNumDirNodes;
        uint mDirNodesOffset;
        uint mFileEntriesOffset;
        uint mStringTableOffset;

        private Dictionary<uint, FileEntry> mFileEntries;
        private Dictionary<uint, DirectoryEntry> mDirEntries;
    }
}