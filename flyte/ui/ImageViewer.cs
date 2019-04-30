using flyte.img;
using System;
using System.Drawing.Imaging;
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

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageBox.Image != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "PNG Image (*.png;)|*png";

                if (dialog.ShowDialog() == DialogResult.OK)
                    imageBox.Image.Save(dialog.FileName, ImageFormat.Png);
            }
            else
                MessageBox.Show("Failed to save image, as there is no image present.");
        }

        private void CopyImageToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageBox.Image != null)
                Clipboard.SetImage(imageBox.Image);
        }
    }
}
