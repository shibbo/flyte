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
using System.Linq;
using static flyte.utils.Endian;
using static flyte.utils.Hash;

namespace flyte.archive.common
{
    /// <summary>
    /// Class that represents a SARC archive. (Sead ARChive)
    /// </summary>
    public class SARC : ArchiveBase
    {
        /// <summary>
        /// Constructs a representation of a SARC archive.
        /// </summary>
        /// <param name="reader">The stream to read the SARC data from.</param>
        public SARC(ref EndianBinaryReader reader) : base(ArchiveType.SARC)
        {
            if (reader.ReadString(4) != "SARC")
            {
                Console.WriteLine("Bad magic. Expected SARC.");
                return;
            }

            if (reader.ReadUInt16() != 0x14)
                reader.SetEndianess(Endianess.Big);

            mBOM = reader.ReadUInt16();
            mFileSize = reader.ReadUInt32();
            mDataOffset = reader.ReadUInt32();
            mVersion = reader.ReadUInt16();
            reader.ReadUInt16();

            mAllocTable = new SARCFileAllocationTable(ref reader);
            mFileNameTable = new SARCFileNameTable(ref reader, mAllocTable.getNodeCount());

            // now that we have our tables, we can assign data to strings for opening
            mFileData = new Dictionary<string, byte[]>();

            List<SFATNode> nodes = mAllocTable.getNodes();

            if (mFileNameTable.isObfuscated())
            {
                for (int i = 0; i < mAllocTable.getNodeCount(); i++)
                {
                    uint dataLen = nodes[i].mDataEnd - nodes[i].mDataBegin;
                    byte[] data = reader.ReadBytesFrom(mDataOffset + nodes[i].mDataBegin, (int)dataLen);
                    mFileData.Add("hash_" + nodes[i].mFileHash.ToString("X"), data);
                }
            }
            else
            {
                for (int i = 0; i < mAllocTable.getNodeCount(); i++)
                {
                    uint dataLen = nodes[i].mDataEnd - nodes[i].mDataBegin;
                    string name = mFileNameTable.getNames()[i];

                    byte[] data = reader.ReadBytesFrom(mDataOffset + nodes[i].mDataBegin, (int)dataLen);
                    mFileData.Add(name, data);
                }
            }
        }

        public override bool isStringTableObfuscated()
        {
            return mFileNameTable.isObfuscated();
        }

        public override List<string> getFileNames()
        {
            return new List<string>(mFileData.Keys);
        }

        public override Dictionary<string, byte[]> getLayoutFiles()
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            foreach(KeyValuePair<string, byte[]> pair in mFileData)
            {
                if (mFileNameTable.isObfuscated())
                {
                    // attempt to see if we have the de-obfuscated string
                    uint hash = uint.Parse(pair.Key.Replace("hash_", ""), System.Globalization.NumberStyles.HexNumber);
                    string deobfus = GetStringFromHash(hash);

                    if (deobfus != "")
                    {
                        if (deobfus.Contains(".bflyt"))
                            files.Add(deobfus, pair.Value);
                    }
                    else
                    {
                        // this is me being nice
                        if (pair.Value[0] == 'C' && pair.Value[1] == 'L' && pair.Value[2] == 'Y' && pair.Value[3] == 'T')
                            files.Add(pair.Key + ".bclyt", pair.Value);

                        if (pair.Value[0] == 'F' && pair.Value[1] == 'L' && pair.Value[2] == 'Y' && pair.Value[3] == 'T')
                            files.Add(pair.Key + ".bflyt", pair.Value);
                    }
                }
                else
                {
                    if (pair.Key.Contains(".bflyt"))
                    {
                        files.Add(pair.Key, pair.Value);
                    }
                }
            }

            return files;
        }

        public override Dictionary<string, byte[]> getLayoutAnimations()
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            foreach (KeyValuePair<string, byte[]> pair in mFileData)
            {
                if (pair.Key.Contains(".bflan") || mFileNameTable.isObfuscated())
                    files.Add(pair.Key, pair.Value);
            }

            return files;
        }

        public override Dictionary<string, byte[]> getLayoutImages()
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            foreach (KeyValuePair<string, byte[]> pair in mFileData)
            {
                if (mFileNameTable.isObfuscated())
                {
                    // attempt to see if we have the de-obfuscated string
                    uint hash = uint.Parse(pair.Key.Replace("hash_", ""), System.Globalization.NumberStyles.HexNumber);
                    string deobfus = GetStringFromHash(hash);

                    if (deobfus != "")
                    {
                        if (deobfus.Contains(".bntx") || deobfus.Contains(".bflim"))
                            files.Add(deobfus, pair.Value);
                    }
                }
                else
                {
                    if (pair.Key.Contains(".bntx") || pair.Key.Contains(".bflim") || mFileNameTable.isObfuscated())
                        files.Add(pair.Key, pair.Value);
                }
               
            }

            return files;
        }

        public override List<string> getArchiveFileNames()
        {
            List<string> strs = new List<string>();

            string[] exts = { ".arc", ".lyarc", ".sarc", ".pack", ".szs" };

            foreach (KeyValuePair<string, byte[]> pair in mFileData)
            {
                for (int i = 0; i < exts.Length; i++)
                {
                    if (pair.Key.Contains(exts[i]))
                        strs.Add(pair.Key);
                }
            }

            return strs;
        }

        public override byte[] getDataByName(string name)
        {
            foreach (KeyValuePair<string, byte[]> pair in mFileData)
            {
                if (pair.Key == name)
                    return pair.Value;
            }

            return null;
        }

        ushort mBOM;
        uint mFileSize;
        uint mDataOffset;
        ushort mVersion;

        SARCFileAllocationTable mAllocTable;
        SARCFileNameTable mFileNameTable;

        Dictionary<string, byte[]> mFileData;
    }

    /// <summary>
    /// Represents a SFAT section in a SARC.
    /// </summary>
    public class SARCFileAllocationTable
    {
        /// <summary>
        /// Constructs a representation of a SFAT table in a SARC.
        /// </summary>
        /// <param name="reader">The stream to read the data from.</param>
        public SARCFileAllocationTable(ref EndianBinaryReader reader)
        {
            if (reader.ReadString(4) != "SFAT")
            {
                Console.WriteLine("Invalid header. Expected SFAT.");
                return;
            }

            mHeaderLength = reader.ReadUInt16();
            mNodeCount = reader.ReadUInt16();
            mHashKey = reader.ReadUInt32();

            mSFATNodes = new List<SFATNode>();

            for (int i = 0; i < mNodeCount; i++)
            {
                SFATNode node = new SFATNode();
                node.mFileHash = reader.ReadUInt32();
                node.mFileAttributes = reader.ReadUInt32();
                node.mDataBegin = reader.ReadUInt32();
                node.mDataEnd = reader.ReadUInt32();

                mSFATNodes.Add(node);
            }
        }

        public uint getHashKey() { return mHashKey; }
        public ushort getNodeCount() { return mNodeCount; }
        public List<SFATNode> getNodes() { return mSFATNodes; }

        ushort mHeaderLength;
        ushort mNodeCount;
        uint mHashKey;

        List<SFATNode> mSFATNodes;
    }

    /// <summary>
    /// Represents a SFAT node in a SARC's SFAT.
    /// </summary>
    public struct SFATNode
    {
        public uint mFileHash;
        public uint mFileAttributes;
        public uint mDataBegin;
        public uint mDataEnd;
    }

    /// <summary>
    /// Represents a SFNT (File Name Table) in a SARC.
    /// </summary>
    public class SARCFileNameTable
    {
        /// <summary>
        /// Constructs a representation of a File Name Table in a SARC.
        /// </summary>
        /// <param name="reader">The stream to read data from.</param>
        /// <param name="numFiles">The number of files to read, since the section does not supply this.</param>
        public SARCFileNameTable(ref EndianBinaryReader reader, ushort numFiles)
        {
            if (reader.ReadString(4) != "SFNT")
            {
                Console.WriteLine("Bad header. Expected SFNT.");
                return;
            }

            mHeaderSize = reader.ReadUInt32();

            if (reader.ReadUInt32From(reader.Pos()) == 0)
                return;

            mNames = new List<string>();
            
            for (int i = 0; i < numFiles; i++)
            {
                mNames.Add(reader.ReadStringNT());

                long val = 0x4 - (reader.Pos() % 0x4);

                if (val < 0)
                    val = -val;

                reader.ReadBytes((int)val);
            }
        }

        /// <summary>
        /// When the SFNT table is empty, that means the file names are obfucsated by their hashes.
        /// This is the function that can tell if a SARC's filenames are obfuscated.
        /// </summary>
        /// <returns>True if obfuscated, False if not.</returns>
        public bool isObfuscated() { return mNames == null; }
        public int getFileCount() { return mNames.Count; }
        public List<string> getNames() { return mNames; }
        /// <summary>
        /// Sees if the file name has a hash in the SFNT.
        /// </summary>
        /// <param name="name">The string to check.</param>
        /// <param name="key">The hash key.</param>
        /// <returns>The hash of the matching name. Null if the name doesn't exist.</returns>
        public uint getHashByName(string name, uint key)
        {
            foreach(string str in mNames)
            {
                if (str == name)
                    return SARCFunctions.GetHash(name, key);
            }

            return 0;
        }

        /// <summary>
        /// Gets a string by a given hash.
        /// </summary>
        /// <param name="hash">The hash to find in the file name table.</param>
        /// <param name="key">The hash key.</param>
        /// <returns>The string that matches the given hash.</returns>
        public string getStringFromHash(uint hash, uint key)
        {
            foreach(string str in mNames)
            {
                uint genHash = SARCFunctions.GetHash(str, key);

                if (genHash == hash)
                    return str;
            }

            return "";
        }

        uint mHeaderSize;

        List<string> mNames;
    }

    /// <summary>
    /// A static class that has helpful SARC functions.
    /// </summary>
    public static class SARCFunctions
    {
        /// <summary>
        /// Gets a hash based on a string and a key.
        /// </summary>
        /// <param name="name">The string to get the hash from.</param>
        /// <param name="key">The hash key.</param>
        /// <returns>The hash generated from the name and key.</returns>
        public static uint GetHash(string name, uint key)
        {
            uint result = 0;

            for (int i = 0; i < name.Length; i++)
            {
                result = name[i] + result * key;
            }
            return result;
        }
    }
}
