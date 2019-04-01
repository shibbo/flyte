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
using System.Collections.Generic;

namespace flyte.io.wii
{
    /// <summary>
    /// Class that represents a U8 archive.
    /// </summary>
    class U8 : ArchiveBase
    {
        /// <summary>
        /// Constructs a representation of a U8 archive.
        /// </summary>
        /// <param name="reader">The stream to read data from.</param>
        public U8(ref EndianBinaryReader reader) : base(ArchiveType.U8)
        {
            if (reader.ReadString(4) != "U?8-")
            {
                Console.WriteLine("Error: Bad U8 magic.");
                return;
            }

            mFirstNodeOffset = reader.ReadUInt32();
            mFullSize = reader.ReadUInt32();
            mDataOffset = reader.ReadUInt32();
            reader.ReadBytes(0x10);

            // first value is always a root node
            mRootNode = new ArchiveNode(ref reader);
            int nodeCount = mRootNode.getSetting2();

            mNodes = new List<ArchiveNode>(nodeCount);
            // we subtract one because of our exclusion of the root node
            for (int i = 0; i < nodeCount - 1; i++)
            {
                ArchiveNode node = new ArchiveNode(ref reader);
                mNodes.Add(node);
            }

            // grab our string table position
            long strTablePos = reader.Pos();
            mStringTable = new List<string>(nodeCount);
            // set our root node string since it's not a part of the list
            mRootNode.setString(reader.ReadStringNTFrom(strTablePos + mRootNode.getStringPoolIdx()));
            // go through each node in the file and assign the name
            // we also assign the file's data here as well
            foreach (ArchiveNode node in mNodes)
            {
                node.setString(reader.ReadStringNTFrom(strTablePos + node.getStringPoolIdx()));
                // only files have data we want
                if (node.getNodeType() == ArchiveNode.NodeType.File)
                    node.setData(reader.ReadBytesFrom(node.getSetting1(), node.getSetting2()));
            }
        }

        /// <summary>
        /// Gets file data based on a name.
        /// </summary>
        /// <param name="name">The file name to get data from.</param>
        /// <returns>The data in a byte array. NULL if the file was not found.</returns>
        public byte[] getDataByName(string name)
        {
            foreach (ArchiveNode node in mNodes)
            {
                // be sure that the data is from a file, not a directory
                if (node.getString() == name && node.getNodeType() == ArchiveNode.NodeType.File)
                    return node.getData();
            }

            return null;
        }

        /// <summary>
        /// Gets a list of all the file names in the archive.
        /// </summary>
        /// <returns>A list of file name strings.</returns>
        public List<string> getOnlyFileNames()
        {
            List<string> names = new List<string>();

            foreach (ArchiveNode node in mNodes)
            {
                if (node.getNodeType() == ArchiveNode.NodeType.File)
                    names.Add(node.getString());
            }

            return names;
        }

        /// <summary>
        /// Gets a list of all the directory names in the archive.
        /// </summary>
        /// <returns>A list of directory name strings.</returns>
        public List<string> getOnlyDirectoryNames()
        {
            List<string> names = new List<string>();

            foreach (ArchiveNode node in mNodes)
            {
                if (node.getNodeType() == ArchiveNode.NodeType.Directory)
                    names.Add(node.getString());
            }

            return names;
        }

        public override List<string> getFileNames()
        {
            List<string> names = new List<string>();

            foreach (ArchiveNode node in mNodes)
            {
                names.Add(node.getString());
            }

            return names;
        }

        uint mFirstNodeOffset;
        uint mFullSize;
        uint mDataOffset;
        ArchiveNode mRootNode;
        List<ArchiveNode> mNodes;
        List<string> mStringTable;
    }

    /// <summary>
    /// Class that represents a archive node in a U8 archive.
    /// </summary>
    class ArchiveNode
    {
        /// <summary>
        /// Enumerator for the node type.
        /// </summary>
        public enum NodeType
        {
            File = 0,
            Directory = 1
        }

        /// <summary>
        /// Constructs a representation of an archive node in a U8.
        /// </summary>
        /// <param name="reader">The stream to read data from.</param>
        public ArchiveNode(ref EndianBinaryReader reader)
        {
            int data = reader.ReadInt32();
            mType = (NodeType)(data >> 24);
            mStrPoolIdx = data & 0x00ffffff;
            mSetting1 = reader.ReadInt32();
            mSetting2 = reader.ReadInt32();
        }

        public NodeType getNodeType() { return mType; }
        public int getStringPoolIdx() { return mStrPoolIdx; }
        public int getSetting1() { return mSetting1; }
        public int getSetting2() { return mSetting2; }

        public byte[] getData() { return mFileData; }
        public string getString() { return mName; }

        public void setString(string str) { mName = str; }
        public void setData(byte[] d) { mFileData = d; }

        NodeType mType;
        int mStrPoolIdx;
        string mName;
        // File: Offset
        // Directories: Parent ID
        int mSetting1;
        // File: Size
        // Directories: Last ID
        int mSetting2;

        byte[] mFileData;
    }
}
