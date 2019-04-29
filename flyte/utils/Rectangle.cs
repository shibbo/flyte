using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.utils
{
    public class RenderRectangle
    {
        public RenderRectangle(int left, int top, int right, int bottom)
        {
            mLeft = left;
            mTop = top;
            mRight = right;
            mBottom = bottom;
        }

        public int mLeft;
        public int mTop;
        public int mRight;
        public int mBottom;
    }
}
