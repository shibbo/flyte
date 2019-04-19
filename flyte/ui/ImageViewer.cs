using flyte.img;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace flyte.ui
{
    public partial class ImageViewer : Form
    {
        public ImageViewer()
        {
            InitializeComponent();
        }

        public void setImage(ImageBase image)
        {
            imageBox.Image = image.getImageBitmap();
            imageGrid.SelectedObject = image;
        }
    }
}
