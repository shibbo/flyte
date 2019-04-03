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

        public void setParent(LayoutBase parent) { mParent = parent; }
        public LayoutBase getParent() { return mParent; }

        List<LayoutBase> mChildren;
        LayoutBase mParent;
    }
}
