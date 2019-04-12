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

using flyte.lyt.wii;
using System.Collections.Generic;

namespace flyte.lyt
{
    class LayoutBase
    {
        public LayoutBase() { }

        public void addChild(LayoutBase child)
        {
            if (mChildren == null)
                mChildren = new List<LayoutBase>();

            mChildren.Add(child);
        }

        public List<LayoutBase> getChildren() { return mChildren; }
        public void setParent(LayoutBase parent) { mParent = parent; }
        public LayoutBase getParent() { return mParent; }

        public LayoutBase getRootPanel() { return mRootPanel; }

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

        List<LayoutBase> mChildren;
        LayoutBase mParent;

        public LayoutBase mRootPanel;

        string mType;
        public string mName;
    }
}
