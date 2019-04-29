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
using System.IO;
using flyte.io;

namespace flyte.archive._3ds
{
    /// <summary>
    /// A class that represents a DARC file.
    /// </summary>
    public class DARC : ArchiveBase
    {
        /// <summary>
        /// Constructs the structure of a DARC based on a stream.
        /// </summary>
        /// <param name="reader">The stream to read the data from.</param>
        public DARC(ref EndianBinaryReader reader) : base(ArchiveType.DARC)
        {
            if (reader.ReadString(4) != "darc")
            {
                Console.WriteLine("Bad header. Expecting darc.");
                return;
            }

            mBOM = reader.ReadUInt16();
            mHeaderLength = reader.ReadUInt16();
            mVersion = reader.ReadUInt32();
            mFileLength = reader.ReadUInt32();
            mFileTableOffset = reader.ReadUInt32();
            mFileTableLength = reader.ReadUInt32();
            mFileDataOffset = reader.ReadUInt32();

            // in order to read the number of darcs, we have to read the first entry, which is the root
            DARCFileEntry rootEntry = new DARCFileEntry(ref reader);
            uint numNodes = rootEntry.getSetting() - 1;

            mFileEntries = new List<DARCFileEntry>();

            for (int i = 0; i < numNodes; i++)
            {
                DARCFileEntry entry = new DARCFileEntry(ref reader);
                mFileEntries.Add(entry);
            }

            long tablePos = reader.Pos();

            string curDirectory = "";

            // now we set the names and their data
            foreach(DARCFileEntry entry in mFileEntries)
            {
                if (!entry.mIsDirectory)
                {
                    entry.setName(curDirectory + "/" + reader.ReadUTF16StringFrom(entry.getFileNameOffset() + tablePos));
                    entry.setData(reader.ReadBytesFrom(entry.getFileDataOffset(), (int)entry.getFileDataLength()));

                    File.WriteAllBytes(entry.getName(), entry.getData());
                }
                else
                {
                    entry.setName(reader.ReadUTF16StringFrom(entry.getFileNameOffset() + tablePos));
                    curDirectory = entry.getName();
                }
            }
        }

        public override Dictionary<string, byte[]> getLayoutFiles()
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            foreach(DARCFileEntry entry in mFileEntries)
            {
                if (!entry.mIsDirectory && entry.getName().Contains(".bclyt"))
                    files.Add(entry.getName(), entry.getData());
            }

            return files;
        }

        public override Dictionary<string, byte[]> getLayoutAnimations()
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            foreach (DARCFileEntry entry in mFileEntries)
            {
                if (!entry.mIsDirectory && entry.getName().Contains(".bclan"))
                    files.Add(entry.getName(), entry.getData());
            }

            return files;
        }

        public override Dictionary<string, byte[]> getLayoutImages()
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            foreach (DARCFileEntry entry in mFileEntries)
            {
                if (!entry.mIsDirectory && entry.getName().Contains(".bclim"))
                    files.Add(entry.getName(), entry.getData());
            }

            return files;
        }

        /// <summary>
        /// Returns the data from a given file, based on a given name.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <returns>The byte array from the file, null if the file wasn't found.</returns>
        public byte[] getFileDataByName(string name)
        {
            foreach(DARCFileEntry entry in mFileEntries)
            {
                if (entry.getName() == name && !entry.mIsDirectory)
                    return entry.getData();
            }

            return null;
        }

        /// <summary>
        /// Gets the file names, structured in a list.
        /// </summary>
        /// <returns>The list of file names.</returns>
        public override List<string> getFileNames()
        {
            List<string> names = new List<string>();

            foreach(DARCFileEntry entry in mFileEntries)
            {
                if (!entry.mIsDirectory)
                    names.Add(entry.getName());
            }

            return names;
        }

        ushort mBOM;
        ushort mHeaderLength;
        uint mVersion;
        uint mFileLength;
        uint mFileTableOffset;
        uint mFileTableLength;
        uint mFileDataOffset;

        List<DARCFileEntry> mFileEntries;

        /// <summary>
        /// Represents a file entry in a DARC.
        /// </summary>
        class DARCFileEntry
        {
            /// <summary>
            /// Constructs a file entry, which is a file in a DARC.
            /// </summary>
            /// <param name="reader">The stream to read data from.</param>
            public DARCFileEntry(ref EndianBinaryReader reader)
            {
                mFileNameOffset = reader.ReadUInt32();

                // C# is annoying here
                bool isDir = Convert.ToBoolean(mFileNameOffset & 0x01000000);

                if (isDir)
                {
                    mFileNameOffset = mFileNameOffset - 0x01000000;
                    mIsDirectory = true;
                }
                else
                    mIsDirectory = false;

                mFileDataOffset = reader.ReadUInt32();
                mFileLength = reader.ReadUInt32();
            }

            public uint getFileNameOffset() { return mFileNameOffset; }
            public uint getFileDataOffset() { return mFileDataOffset; }
            public uint getFileDataLength() { return mFileLength; }
            public byte[] getData() { return mData; }
            public string getName() { return mName; }
            public uint getSetting() { return mFileLength; }

            public void setData(byte[] d) { mData = d; }
            public void setName(string name) { mName = name; }

            uint mFileNameOffset;
            uint mFileDataOffset;
            // Files: File Length
            // Directories: Number of entries
            uint mFileLength;
            public bool mIsDirectory;

            string mName;
            byte[] mData;
        }
    }
}
