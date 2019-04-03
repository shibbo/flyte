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
    public partial class LayoutChooser : Form
    {
        public LayoutChooser()
        {
            InitializeComponent();
        }

        public void insertEntries(List<string> strings)
        {
            foreach (string str in strings)
                fileList.Items.Add(str);
        }

        public string getSelectedFile()
        {
            return (string)fileList.SelectedItem;
        }

        private void FileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectLayoutButton.Enabled = true;
        }

        private void SelectLayoutButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
