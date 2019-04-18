using System;
using System.Windows.Forms;
using flyte.lyt.wii;
using flyte.lyt.wii.material;

namespace flyte.ui
{
    public partial class MaterialEditor : Form
    {
        public MaterialEditor(ref Material material)
        {
            InitializeComponent();
            texSRTGrid.PropertySort = PropertySort.Categorized;

            mMat = material;

            int idx = 0;

            // texture SRTs
            if (mMat.getTextureSRTs() != null)
            {
                foreach (TexSRT srt in mMat.getTextureSRTs())
                {
                    texSRTList.Items.Add("SRT " + idx);
                    idx++;
                }
            }

            if (mMat.getSwapTable() == null)
                tevSwapList.Enabled = false;
        }

        Material mMat;

        private void TevSwapList_SelectedIndexChanged(object sender, EventArgs e)
        {
            tevSwapGrid.SelectedObject = mMat.getSwapTable().getSwapModeAtIndex(tevSwapList.SelectedIndex);
        }

        private void TexSRTList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (texSRTList.SelectedIndex == -1)
                return;

            texSRTGrid.SelectedObject = mMat.getTextureSRTs()[texSRTList.SelectedIndex];
        }
    }
}
