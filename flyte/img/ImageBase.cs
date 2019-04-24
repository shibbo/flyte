using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyte.img
{
    public class ImageContainerBase
    {
        public virtual ImageBase getImage(int index) { return null; }
    }

    public class ImageBase
    {
        public enum ImagePlatform
        {
            GC = 0,
            Wii = 1,
            _3DS = 2,
            WiiU = 3,
            Switch = 4
        }

        public virtual Bitmap getImageBitmap() { return null; }
        public virtual void setType(ImagePlatform platform) { mType = platform; }

        ImagePlatform mType;
    }
}
