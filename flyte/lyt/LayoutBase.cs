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
using flyte.lyt.wii;
using flyte.utils;
using System.Collections.Generic;

namespace flyte.lyt
{
    public class LayoutBase
    {
        public enum LayoutVersion
        {
            GC = 0,
            Wii = 1,
            _3DS = 2,
            WiiU = 3,
            Switch = 4
        }
        public LayoutBase() { }

        public void addChild(LayoutBase child)
        {
            if (mChildren == null)
                mChildren = new List<LayoutBase>();

            mChildren.Add(child);
        }

        public void setLayoutVersion(LayoutVersion version) { mLayoutVersion = version; }
        public LayoutVersion getLayoutVersion() { return mLayoutVersion; }

        public List<LayoutBase> getChildren() { return mChildren; }
        public void setParent(LayoutBase parent) { mParent = parent; }
        public LayoutBase getParent() { return mParent; }

        public LayoutBase getRootPanel() { return mRootPanel; }
        public virtual LayoutBase getRootGroup() { return null; }

        public bool hasChildren() { return mChildren != null; }

        public string getType() { return mType; }
        public virtual void setType(string type) { mType = type; }

        public virtual List<string> getTextureNames() { return null; }
        public virtual List<string> getFontNames() { return null; }
        public virtual bool containsTextures() { return false; }
        public virtual bool containsFonts() { return false; }
        public virtual bool containsMaterials() { return false; }
        public virtual void addUserData(UserdataBase data) { }
        public virtual LayoutBase getLayoutParams() { return null; }
        public virtual List<string> getMaterialNames() { return null; }
        public virtual List<MaterialBase> getMaterials() { return null; }

        public virtual void write(ref EndianBinaryWriter writer) { }
        public virtual void draw() { }

        public RenderRectangle mRect;

        List<LayoutBase> mChildren;
        LayoutBase mParent;

        public LayoutBase mRootPanel;

        string mType;
        public string mName;
        LayoutVersion mLayoutVersion;
    }
}
