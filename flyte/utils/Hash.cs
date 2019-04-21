using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace flyte.utils
{
    public class Hash
    {
        public static void InitHashList()
        {
            mHashes = new Dictionary<uint, string>();

            string[] strings = File.ReadAllLines("resource/hashes.txt");

            foreach(string str in strings)
            {
                string name = str.Split('=')[0];
                string hashStr = str.Split('=')[1].Replace("0x", "").ToUpper();
                uint hash = Convert.ToUInt32(hashStr, 16);

                // sometimes there are duplicates
                if (mHashes.ContainsKey(hash))
                    continue;

                mHashes.Add(hash, name);
            }
        }

        public static string GetStringFromHash(uint hash)
        {
            if (mHashes.ContainsKey(hash))
                return mHashes[hash];
            else
                return "";
        }

        static Dictionary<uint, string> mHashes;
    }
}
