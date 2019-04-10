/*
    © 2019 - shibboleet
    flyte is free software: you can redistribute it and/or modify it under
    the terms of the GNU General Public License as published by the Free
    Software Foundation, either version 3 of the License, or (at your option)
    any later version.
    flyte is distributed in the hope that it will be useful, but WITHOUT ANY 
    WARRANTY; See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along 
    with flyte. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
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
