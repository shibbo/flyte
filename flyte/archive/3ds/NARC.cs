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
using flyte.io;

namespace flyte.archive._3ds
{
    /// <summary>
    /// A class that represents a NARC archive.
    /// </summary>
    public class NARC : ArchiveBase
    {
        /// <summary>
        /// Constructs a representation of a NARC archive.
        /// </summary>
        /// <param name="reader">The stream to read data from.</param>
        public NARC(ref EndianBinaryReader reader) : base(ArchiveType.NARC)
        {
            if (reader.ReadString(4) != "NARC")
            {
                Console.WriteLine("Error reading header. Excpected NARC.");
                return;
            }

            mBOM = reader.ReadUInt16();
            mVersion = reader.ReadUInt16();
            mFileSize = reader.ReadUInt32();
            mHeaderSize = reader.ReadUInt16();
            mNumSections = reader.ReadUInt16();

            for (int i = 0; i < mNumSections; i++)
            {
                string curSection = reader.ReadString(4);

                switch (curSection)
                {
                    case "BTAF":
                        mAllocationBlock = new NARCFileAllocationBlock(ref reader);
                        break;
                    case "BTNF":
                        mFileTable = new NARCFileTable(ref reader);
                        break;
                    case "GMIF":
                        break;
                    default:
                        Console.WriteLine("Unsupported section, " + curSection);
                        break;
                }
            }

            long dataOffset = reader.Pos();

            uint sectionSize = reader.ReadUInt32();

            // now that we have all of our data, we can finally build our dictionary for filenames and data
            mFileDict = new Dictionary<string, byte[]>();

            int curIdx = 0;
            // go through each GMIF offset and get our data
            foreach(GMIFOffset offset in mAllocationBlock.getOffsets())
            {
                byte[] data = reader.ReadBytesFrom(offset.mStart, (int)offset.mLength);
                mFileDict.Add(getStringFromIndex(curIdx), data);

                curIdx++;
            }
        }

        public NARCFileAllocationBlock getAllocationBlock() { return mAllocationBlock; }

        /// <summary>
        /// Get an index in the dictionary from a name.
        /// </summary>
        /// <param name="name">The string to search for the index of.</param>
        /// <returns>The index if the string exists in the dictionary. -1 if the string was not found.</returns>
        public int getIndexFromName(string name)
        {
            int curIndex = 0;

            foreach(NARCFileTableEntry entry in mFileTable.mFileEntries)
            {
                if (entry.mName == name)
                    return curIndex;

                curIndex++;
            }

            return -1;
        }

        /// <summary>
        /// Get a string from a given index.
        /// </summary>
        /// <param name="idx">The index of the string to search for.</param>
        /// <returns>The string if the index exists. "" if the index does not exist.</returns>
        public string getStringFromIndex(int idx)
        {
            if (idx > mFileTable.mFileEntries.Count)
                return "";
            else
                return mFileTable.mFileEntries[idx].mName;
        }

        /// <summary>
        /// Gets the file data based on a string.
        /// </summary>
        /// <param name="str">The string to search data for.</param>
        /// <returns>The byte array of data if the string exists in the dictionary. NULL if it does not exist.</returns>
        public byte[] getDataByString(string str)
        {
            if (mFileDict.ContainsKey(str))
                return mFileDict[str];
            else
                return null;
        }

        public override List<string> getFileNames()
        {
            return new List<string>(mFileDict.Keys);
        }

        ushort mBOM;
        ushort mVersion;
        uint mFileSize;
        ushort mHeaderSize;
        ushort mNumSections;

        long mDataOffset;

        NARCFileAllocationBlock mAllocationBlock;
        NARCFileTable mFileTable;

        Dictionary<string, byte[]> mFileDict;
    }

    /// <summary>
    /// A class that represents the BTAF block in a NARC archive.
    /// </summary>
    public class NARCFileAllocationBlock
    {
        /// <summary>
        /// Constructs a representation of a BTAF block in a NARC archive.
        /// </summary>
        /// <param name="reader">The stream to read data from.</param>
        public NARCFileAllocationBlock(ref EndianBinaryReader reader)
        {
            // we don't need to check the magic since the switch case already validated it
            mHeaderSize = reader.ReadUInt32();
            mFileCount = reader.ReadUInt32();

            mOffsets = new List<GMIFOffset>();

            for (int i = 0; i < mFileCount; i++)
            {
                GMIFOffset offset = new GMIFOffset();
                offset.mStart = reader.ReadUInt32();
                offset.mEnd = reader.ReadUInt32();
                offset.mLength = offset.mEnd - offset.mStart;

                mOffsets.Add(offset);
            }
        }

        public List<GMIFOffset> getOffsets() { return mOffsets; }

        uint mHeaderSize;
        uint mFileCount;

        List<GMIFOffset> mOffsets;
    }

    /// <summary>
    /// Class that represents a BTNF table in a NARC archive.
    /// </summary>
    class NARCFileTable
    {
        /// <summary>
        /// Constructs a representation of a BTNF table in a NARC archive.
        /// </summary>
        /// <param name="reader">The stream to read data from.</param>
        public NARCFileTable(ref EndianBinaryReader reader)
        {
            mSectionSize = reader.ReadUInt32();

            // here is where this format gets trippy
            // there is a root folder, but it's not "."
            // so we need to read this root, and then iterate through our string table, 
            // then go back to define the rest
            long root = reader.Pos();

            uint offsetToRoot = reader.ReadUInt32();

            reader.Seek(offsetToRoot + root);

            mFileEntries = new List<NARCFileTableEntry>();
            mDirectoryCount = 1; // we have to include the root dir

            while (true)
            {
                NARCFileTableEntry entry = new NARCFileTableEntry();
                entry.mIsDirectory = false;

                byte val = reader.ReadByte();
                byte strLength;

                // sometimes there's two bytes of padding after a null terminator
                // time to see if it is one of those...
                if (val == 0)
                    val = reader.ReadByte();

                // 0xFF marks the end of the file table
                if (val == 0xFF)
                    break;

                // bit 1 defines if it is a directory
                // the other bits define the string length for the file / directory
                bool isDirectory = Convert.ToBoolean(val >> 7);

                if (isDirectory)
                    strLength = Convert.ToByte(val & 0x7F);
                else
                    strLength = val;

                entry.mName = reader.ReadString(strLength);

                // if the entry is a directory, we have to keep track of our directory count
                // we also read the directory index
                if (isDirectory)
                {
                    entry.mDirectoryIndex = reader.ReadInt16();
                    entry.mIsDirectory = true;
                    mDirectoryCount++;
                }

                mFileEntries.Add(entry);
            }

            // we need to get the position to come back to, to read the next section
            long nextSectionPos = (-(reader.Pos() - (root - 0x8) - mSectionSize)) + reader.Pos();
            // but first, we have to go back to the root to read all of our folder entries
            reader.Seek(root);

            mDirectoryTable = new NARCDirectoryTable();

            mDirectoryTable.mTableEntries = new List<NARCDirectoryTableEntry>();

            for (int i = 0; i < mDirectoryCount; i++)
            {
                NARCDirectoryTableEntry entry = new NARCDirectoryTableEntry();
                entry.mDirectoryStartPosition = reader.ReadUInt32();
                entry.mFirstFilePosition = reader.ReadUInt16();
                entry.mDirectoryCount = reader.ReadUInt16();

                mDirectoryTable.mTableEntries.Add(entry);
            }

            reader.Seek(nextSectionPos);
        }

        uint mSectionSize;

        uint mDirectoryCount;

        NARCDirectoryTable mDirectoryTable;
        public List<NARCFileTableEntry> mFileEntries;
    }

    /// <summary>
    /// Structure that represents a NARC file table entry.
    /// </summary>
    public struct NARCFileTableEntry
    {
        public string mName;
        public short mDirectoryIndex;
        public bool mIsDirectory;
    }

    /// <summary>
    /// Structure that stores directory table entries in a list.
    /// </summary>
    public struct NARCDirectoryTable
    {
        public List<NARCDirectoryTableEntry> mTableEntries;
    }

    /// <summary>
    /// Structure that represents a directory table entry.
    /// </summary>
    public struct NARCDirectoryTableEntry
    {
        public uint mDirectoryStartPosition;
        public ushort mFirstFilePosition;
        public ushort mDirectoryCount;
    }

    /// <summary>
    /// Structure that represents a GMIF table offset for file data.
    /// </summary>
    public struct GMIFOffset
    {
        public uint mStart;
        public uint mEnd;

        public uint mLength;
    }

    /// <summary>
    /// Structure that represents a directory entry in a file table.
    /// </summary>
    public struct DirectoryEntry
    {
        uint mStartOffset;
        ushort mFirstFilePos;
        ushort mDirectoryCount;
    }

}
