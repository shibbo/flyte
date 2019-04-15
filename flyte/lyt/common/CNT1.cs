using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flyte.io;

namespace flyte.lyt.common
{
    class CNT1 : LayoutBase
    {
        public CNT1(ref EndianBinaryReader reader, uint versionMajor)
        {
            long startPos = reader.Pos() - 4;

            base.setType("Container");

            mSectionSize = reader.ReadUInt32();

            if (versionMajor < 5)
            {
                mPaneNamesOffset = reader.ReadUInt32();
                mPaneCount = reader.ReadUInt16();
                mAnimCount = reader.ReadUInt16();
            }
            else
            {
                mControlUserNameOffset = reader.ReadUInt32();
                mPaneNamesOffset = reader.ReadUInt32();
                mPaneCount = reader.ReadUInt16();
                mAnimCount = reader.ReadUInt16();
                mPaneParamNamesOffset = reader.ReadUInt32();
                mAnimParamNamesOffset = reader.ReadUInt32();
            }

            mName = reader.ReadStringNT();

            if (mControlUserNameOffset != 0)
                mControlName = reader.ReadStringNTFrom(mControlUserNameOffset + startPos);
            else
                mControlName = mName;

            reader.Seek(startPos + mPaneNamesOffset);

            mPanelNames = new List<string>();

            for (int i = 0; i < mPaneCount; i++)
                mPanelNames.Add(reader.ReadString(0x18).Replace("\0", ""));

            long animStart = reader.Pos();
            // we should be directly at the panel animation names
            mAnimNames = new List<string>();

            for (int i = 0; i < mAnimCount; i++)
                mAnimNames.Add(reader.ReadStringNTFrom(animStart + reader.ReadUInt32()));

            reader.Seek(startPos + mPaneParamNamesOffset);

            long paramStart = reader.Pos();

            mPanelParamNames = new List<string>();

            for (int i = 0; i < mPaneCount; i++)
                mPanelParamNames.Add(reader.ReadStringNTFrom(paramStart + reader.ReadUInt32()));

            reader.Seek(startPos + mAnimParamNamesOffset);

            paramStart = reader.Pos();

            mAnimParamNames = new List<string>();

            for (int i = 0; i < mAnimCount; i++)
                mAnimParamNames.Add(reader.ReadStringNTFrom(paramStart + reader.ReadUInt32()));

            reader.Seek(startPos + mSectionSize);
        }

        uint mSectionSize;
        uint mPaneNamesOffset;
        ushort mPaneCount;
        ushort mAnimCount;
        uint mControlUserNameOffset;
        uint mPaneParamNamesOffset;
        uint mAnimParamNamesOffset;

        string mControlName;

        List<string> mPanelNames;
        List<string> mAnimNames;
        List<string> mPanelParamNames;
        List<string> mAnimParamNames;
    }
}
