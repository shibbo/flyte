using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        List<LayoutBase> mChildren;
        LayoutBase mParent;

        public LayoutBase mRootPanel;

        string mType;
        public string mName;
    }
}
