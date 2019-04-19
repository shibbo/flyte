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
            Wii = 0,
            _3DS = 1,
            WiiU = 2,
            Switch = 3
        }

        public virtual Bitmap getImageBitmap() { return null; }
        public virtual void setType(ImagePlatform platform) { mType = platform; }

        ImagePlatform mType;
    }
}
