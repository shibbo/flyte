using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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

        public enum OriginH
        {
            LEFT,
            CENTER,
            RIGHT
        }

        public enum OriginV
        {
            TOP,
            CENTER,
            BOTTOM
        }

        public static void DrawRect(int width, int height, OriginH originH, OriginV originV)
        {
            int l, r, t, b;

            if (originH == OriginH.LEFT)
            {
                l = 0;
                r = width;
            }
            else if (originH == OriginH.RIGHT)
            {
                l = -width;
                r = 0;
            }
            else
            {
                l = -width / 2;
                r = width / 2;
            }

            if (originV == OriginV.TOP)
            {
                t = 0;
                b = height;
            }
            else if (originV == OriginV.BOTTOM)
            {
                t = -height;
                b = 0;
            }
            else
            {
                t = -height / 2;
                b = height / 2;
            }

            GL.Begin(PrimitiveType.LineLoop);
            GL.LineWidth(1.5f);
            GL.Vertex2(l, t);
            GL.Vertex2(r, t);
            GL.Vertex2(r, b);
            GL.Vertex2(l, b);
            GL.End();
        }

        public int mLeft;
        public int mTop;
        public int mRight;
        public int mBottom;
    }
}
