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
using System;
using System.Collections.Generic;

namespace flyte.archive.wii
{
    /// <summary>
    /// Represents a RARC archive.
    /// </summary>
    class RARC : ArchiveBase
    {
        /// <summary>
        /// Constructs a representation of a RARC archive.
        /// </summary>
        /// <param name="reader">The stream to read the data.</param>
        public RARC(ref EndianBinaryReader reader) : base(ArchiveType.RARC)
        {
            // header
            if (reader.ReadString(4) != "RARC")
            {
                Console.WriteLine("Bad header. Expected RARC.");
                return;
            }

            mFileLength = reader.ReadUInt32();
            mHeaderLength = reader.ReadUInt32();
            mFileDataOffset = reader.ReadUInt32() + 0x20;
            mFileDataLength = reader.ReadUInt32();
            mUnk14 = reader.ReadUInt32();
            mUnk18 = reader.ReadUInt32();
            mUnk1C = reader.ReadUInt32();

            // info block
            mInfoBlock = new RARCInfoBlock(ref reader);

            mNodes = new List<RARCNode>();

            for (int i = 0; i < mInfoBlock.getNumNodes(); i++)
            {
                RARCNode node = new RARCNode(ref reader);
                mNodes.Add(node);
            }

            mDirectories = new List<RARCDirectory>();

            for (int i = 0; i < mInfoBlock.getNumDirs(); i++)
            {
                RARCDirectory dir = new RARCDirectory(ref reader);
                mDirectories.Add(dir);
            }

            uint strTableOffs = mInfoBlock.getStrTableOffset();
            
            // now that we have all the info we need, we can assign the strings to our nodes
            foreach(RARCNode node in mNodes)
            {
                string name = reader.ReadStringNTFrom(node.getNameOffset() + strTableOffs);
                node.setName(name);
            }

            // now we do it for the files as well, and take care of their file data
            foreach (RARCDirectory dir in mDirectories)
            {
                // make sure its a file
                if (dir.getDataLength() != 0)
                {
                    string name = reader.ReadStringNTFrom(dir.getStringOffset() + strTableOffs);
                    dir.setName(name);

                    byte[] data = reader.ReadBytesFrom(dir.getDataOffset() + mFileDataOffset, (int)dir.getDataLength());
                    dir.setData(data);
                }
            }

            uint curIdx = 0;
            // now that we have both finished, we can assign directory names to file nodes
            foreach(RARCNode node in mNodes)
            {
                curIdx = node.mFirstDirIndex;

                while (curIdx < node.mNumDirectories + node.mFirstDirIndex)
                {
                    setName((int)curIdx, node.getName());
                    curIdx++;
                }
            }
        }

        /// <summary>
        /// Gets the data from a defined file name.
        /// </summary>
        /// <param name="name">The file name to find the data for.</param>
        /// <returns>The file data. NULL if the file was not found.</returns>
        public override byte[] getDataByName(string name)
        {
            foreach (RARCDirectory dir in mDirectories)
            {
                if (dir.getType() == 0x1100)
                {
                    if (dir.getName() == name)
                        return dir.getData();
                }
            }

            return null;
        }

        /// <summary>
        /// Set a node's name, with a directory name prepended.
        /// </summary>
        /// <param name="idx">The file index.</param>
        /// <param name="name">The directory name.</param>
        public void setName(int idx, string name)
        {
            RARCDirectory curDir = mDirectories[idx];

            // directories dont get this special treatment
            if (curDir.getType() != 0x1100)
                return;

            curDir.setName(name + "/" + curDir.getName());
            mDirectories[idx] = curDir;
        }

        public override List<string> getFileNames()
        {
            List<string> names = new List<string>();

            foreach (RARCDirectory dir in mDirectories)
            {
                if (dir.getName() != null)
                    names.Add(dir.getName());
            }

            return names;
        }

        public override Dictionary<string, byte[]> getLayoutFiles()
        {
            Dictionary<string, byte[]> dict = new Dictionary<string, byte[]>();

            foreach (RARCDirectory dir in mDirectories)
            {
                if (dir.getType() != 0x1100)
                    continue;

                if (dir.getName().Contains(".brlyt"))
                    dict.Add(dir.getName(), dir.getData());
            }

            return dict;
        }

        public override Dictionary<string, byte[]> getLayoutAnimations()
        {
            Dictionary<string, byte[]> dict = new Dictionary<string, byte[]>();

            foreach (RARCDirectory dir in mDirectories)
            {
                if (dir.getType() != 0x1100)
                    continue;

                if (dir.getName().Contains(".brlan"))
                    dict.Add(dir.getName(), dir.getData());
            }

            return dict;
        }

        public override Dictionary<string, byte[]> getLayoutImages()
        {
            Dictionary<string, byte[]> dict = new Dictionary<string, byte[]>();

            foreach (RARCDirectory dir in mDirectories)
            {
                if (dir.getType() != 0x1100)
                    continue;

                if (dir.getName().Contains(".tpl"))
                    dict.Add(dir.getName(), dir.getData());
            }

            return dict;
        }

        public override Dictionary<string, byte[]> getLayoutControls()
        {
            Dictionary<string, byte[]> dict = new Dictionary<string, byte[]>();

            foreach (RARCDirectory dir in mDirectories)
            {
                if (dir.getType() != 0x1100)
                    continue;

                if (dir.getName().Contains(".brctr"))
                    dict.Add(dir.getName(), dir.getData());
            }

            return dict;
        }

        public override List<string> getArchiveFileNames()
        {
            List<string> strs = new List<string>();

            string[] exts = { ".arc", ".lyarc", ".sarc", ".pack", ".szs" };

            foreach (RARCNode dir in mNodes)
            {
                for (int i = 0; i < exts.Length; i++)
                {
                    if (dir.getName().Contains(exts[i]))
                        strs.Add(dir.getName());
                }
            }

            return strs;
        }

        uint mFileLength;
        uint mHeaderLength;
        uint mFileDataOffset;
        uint mFileDataLength;
        uint mUnk14;
        uint mUnk18;
        uint mUnk1C;

        RARCInfoBlock mInfoBlock;

        List<RARCNode> mNodes;
        List<RARCDirectory> mDirectories;
    }
    
    class RARCInfoBlock
    {
        public RARCInfoBlock(ref EndianBinaryReader reader)
        {
            long startPos = reader.Pos();

            mNumNodes = reader.ReadUInt32();
            mFirstNodeOffset = reader.ReadUInt32();
            mNumDirs = reader.ReadUInt32();
            mFirstDirOffset = reader.ReadUInt32();
            mStrTableLength = reader.ReadUInt32();
            mStrTableOffset = reader.ReadUInt32() + (uint)startPos;
            mNumDirsThatAreFiles = reader.ReadUInt16();
            mUnk18 = reader.ReadInt16();
            mUnk1C = reader.ReadUInt32();
        }

        public uint getNumNodes() { return mNumNodes; }
        public uint getNumDirs() { return mNumDirs; }
        public uint getStrTableOffset() { return mStrTableOffset; }

        uint mNumNodes;
        uint mFirstNodeOffset;
        uint mNumDirs;
        uint mFirstDirOffset;
        uint mStrTableLength;
        uint mStrTableOffset;
        ushort mNumDirsThatAreFiles;
        short mUnk18;
        uint mUnk1C;
    }

    class RARCNode
    {
        public RARCNode(ref EndianBinaryReader reader)
        {
            mIdentifier = reader.ReadString(4).Replace(" ", "");
            mNameOffset = reader.ReadUInt32();
            mHash = reader.ReadUInt16();
            mNumDirectories = reader.ReadUInt16();
            mFirstDirIndex = reader.ReadUInt32();
        }

        public string getName() { return mName; }
        public uint getNameOffset() { return mNameOffset; }
        public void setName(string name) { mName = name; }

        string mIdentifier;
        uint mNameOffset;
        ushort mHash;
        public ushort mNumDirectories;
        public uint mFirstDirIndex;

        string mName;
    }

    class RARCDirectory
    {
        public RARCDirectory(ref EndianBinaryReader reader)
        {
            mDirIndex = reader.ReadInt16();
            mHash = reader.ReadUInt16();
            mType = reader.ReadUInt16();
            mStrTableOffset = reader.ReadUInt16();
            mFileDataOffset = reader.ReadUInt32();
            mFileDataLength = reader.ReadUInt32();
            mUnk10 = reader.ReadUInt32();
        }

        public uint getDataOffset() { return mFileDataOffset; }
        public uint getDataLength() { return mFileDataLength; }
        public byte[] getData() { return mData; }
        public string getName() { return mName; }
        public ushort getStringOffset() { return mStrTableOffset; }
        public ushort getType() { return mType; }
        public bool isDirectory() { return mType != 0x1100; }
        public void setData(byte[] data) { mData = data; }
        public void setName(string name) { mName = name; }

        short mDirIndex;
        ushort mHash;
        ushort mType;
        ushort mStrTableOffset;
        uint mFileDataOffset;
        uint mFileDataLength;
        uint mUnk10;

        string mName;
        byte[] mData;
    }
}
