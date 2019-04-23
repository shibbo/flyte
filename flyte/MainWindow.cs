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
using System.IO;
using System.Windows.Forms;
using flyte.archive;
using flyte.archive._3ds;
using flyte.archive.common;
using flyte.archive.wii;
using flyte.io;
using flyte.lyt;
using flyte.lyt.wii;
using flyte.ui;
using flyte.lyt._3ds;
using static flyte.utils.Endian;
using static flyte.utils.Hash;
using System.Text;
using flyte.img.wii;
using flyte.lyt.common;
using flyte.img;
using flyte.lyt.gc;
using flyte.lyt.gc.blo1;
using flyte.lyt.gc.blo2;

namespace flyte
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            InitHashList();
            layoutPropertyGrid.PropertySort = PropertySort.Categorized;
            mainPropertyGrid.PropertySort = PropertySort.Categorized;
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archive Files (*.DARC;*.NARC;*.RARC;*.ARC;*.SZS;*.LZ;*.LYARC;*.PACK)|*.DARC;*.NARC;*.RARC;*.ARC;*.SZS;*.LZ;*.LYARC;*.PACK";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Clear();
                bool ret = ProcessData(dialog.FileName, null);

                if (ret)
                    this.Text = "flyte v0.3 Alpha -- " + Path.GetFileName(dialog.FileName);
            }
        }

        private void CloseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
        }


        /// <summary>
        /// Process data in an input file that contains a layout.
        /// </summary>
        /// <param name="filename"></param>
        bool ProcessData(string filename, byte[] inData)
        {
            EndianBinaryReader reader = null;

            // now we need to decide what they just opened
            if (inData == null && filename != "")
                reader = new EndianBinaryReader(File.Open(filename, FileMode.Open), Encoding.GetEncoding(932));
            else
                reader = new EndianBinaryReader(inData);

            string magic = "";

            // we have a Yaz0 compressed file
            if (reader.ReadStringFrom(0, 4) == "Yaz0")
            {
                // we have to close our reader so we can properly read this file as a Yaz0 stream
                reader.Close();

                MemoryStream ms = null;

                if (inData == null)
                    ms = new Yaz0(File.Open(filename, FileMode.Open));
                else
                    ms = new Yaz0(new MemoryStream(inData));

                reader = new EndianBinaryReader(ms);
                magic = reader.ReadStringFrom(0, 4);
            }
            // we have a LZ compressed file
            else if (reader.ReadByteFrom(0) == 0x11)
            {
                LZ77 lzFile = new LZ77(ref reader);
                byte[] lzData = lzFile.getData();
                // close our current reader to open a new one with our input data
                reader.Close();
                reader = new EndianBinaryReader(lzData);
                magic = reader.ReadStringFrom(0, 4);
            }
            // no compression
            else
            {
                // it is not yaz0 compressed, so we see the magic
                magic = reader.ReadStringFrom(0, 4);
            }

            // now we have to check our magic to see what kind of file it is
            switch (magic)
            {
                case "darc":
                    mArchive = new DARC(ref reader);
                    break;
                case "NARC":
                    mArchive = new NARC(ref reader);
                    break;
                case "SARC":
                    mArchive = new SARC(ref reader);
                    break;
                case "RARC":
                    reader.SetEndianess(Endianess.Big);
                    mArchive = new RARC(ref reader);
                    break;
                case "U?8-":
                    reader.SetEndianess(Endianess.Big);
                    mArchive = new U8(ref reader);
                    break;
                default:
                    MessageBox.Show("Error. Unsupported format with magic: " + magic);
                    break;
            }

            string layoutType = "";

            // some files have their string table nullified, which makes the names obfuscated
            // I've only seen this in SARCs from MK7, but there's probably more
            if (mArchive != null)
            {
                if (mArchive.isStringTableObfuscated())
                    MessageBox.Show("This file has obfuscated file names. The editor attempted to find layout files, but cannot supply names.");
            }

            reader.Close();

            if (mArchive == null)
            {
                MessageBox.Show("Format not supported.");
                return false;
            }

            // the only familiar format with archives in archives is SARC and RARC
            if (mArchive.getType() == ArchiveType.SARC || mArchive.getType() == ArchiveType.RARC)
            {
                List<string> names = mArchive.getArchiveFileNames();

                if (names.Count != 0)
                {
                    DialogResult res = MessageBox.Show("This archive has another archive inside of it.\nDo you wish to choose one of the found archives to select a layout?", "Internal Archive", MessageBoxButtons.YesNo);

                    if (res == DialogResult.Yes)
                    {
                        LayoutChooser archiveChooser = new LayoutChooser();
                        archiveChooser.insertEntries(names);
                        archiveChooser.ShowDialog();

                        // if this worked, we dont need to do anything
                        bool result = ProcessData(archiveChooser.getSelectedFile(), mArchive.getDataByName(archiveChooser.getSelectedFile()));

                        if (result)
                            return true;
                        else
                        {
                            MessageBox.Show("Failed to get the internal file.");
                            return false;
                        }
                    }
                }
            }

            // get all of our needed files
            mLayoutFiles = mArchive.getLayoutFiles();
            mLayoutAnimFiles = mArchive.getLayoutAnimations();
            mLayoutImages = mArchive.getLayoutImages();
            mLayoutControls = mArchive.getLayoutControls();

            if (mLayoutFiles.Count == 0)
            {
                MessageBox.Show("This file contains no layouts.");
                return false;
            }

            LayoutChooser layoutChooser = new LayoutChooser();
            layoutChooser.insertEntries(new List<string>(mLayoutFiles.Keys));
            layoutChooser.ShowDialog();

            string selectedFile = layoutChooser.getSelectedFile();

            if (selectedFile == null)
                return false;

            string[] sections = selectedFile.Split('/');
            mMainRoot = "";

            // remove "lyt" part and the file name
            // this will be our main root of the entire opened file
            for (int i = 0; i < sections.Length - 2; i++)
                mMainRoot += sections[i] + "/";

            if (layoutType == "")
                layoutType = Path.GetExtension(selectedFile);

            // now we have to init a layout reader
            EndianBinaryReader layoutReader = null;
            byte[] data;

            switch (layoutType)
            {
                case ".brlyt":
                    data = mLayoutFiles[selectedFile];
                    layoutReader = new EndianBinaryReader(data);
                    mMainLayout = new BRLYT(ref layoutReader);
                    layoutReader.Close();
                    break;
                case ".bclyt":
                    data = mLayoutFiles[selectedFile];
                    layoutReader = new EndianBinaryReader(data);
                    mMainLayout = new BCLYT(ref layoutReader);
                    break;
                case ".bflyt":
                    data = mLayoutFiles[selectedFile];
                    layoutReader = new EndianBinaryReader(data);
                    mMainLayout = new BFLYT(ref layoutReader);
                    break;
                case ".blo":
                    data = mLayoutFiles[selectedFile];
                    layoutReader = new EndianBinaryReader(data);

                    if (layoutReader.ReadStringFrom(4, 4) == "blo1")
                        mMainLayout = new BLO1(ref layoutReader);
                    else
                        mMainLayout = new BLO2(ref layoutReader);
                    break;
                default:
                    MessageBox.Show("This format is not supported yet.");
                    break;
            }

            layoutReader.Close();

            if (mMainLayout == null)
                return false;

            // set our propertygrid with our LYT object
            mainPropertyGrid.SelectedObject = mMainLayout.getLayoutParams();

            if (mMainLayout.getRootPanel() == null)
            {
                MessageBox.Show("Error, the root pane in this layout is not specified.");
                return false;
            }

            LayoutBase pane = null;
            LayoutBase group = null;

            // now we have to grab our root panel, which is different on each console
            // so we have to specifically get the one we want
            // the same applies to our root group
            pane = mMainLayout.getRootPanel();

            // this should be RootPane
            TreeNode n1 = new TreeNode
            {
                Tag = pane,
                Name = pane.mName,
                Text = pane.mName,
            };

            panelList.Nodes.Add(n1);
            fillNodes(pane.getChildren());

            // now for our groups
            group = mMainLayout.getRootGroup();

            if (group != null)
            {
                TreeNode n1_1 = new TreeNode
                {
                    Tag = group,
                    Name = group.mName,
                    Text = group.mName,
                };

                panelList.Nodes.Add(n1_1);
                fillNodes(group.getChildren());
            }

            // now for textures and fonts
            // but it is possible for either one to not exist
            if (mMainLayout.containsTextures())
            {
                foreach (string str in mMainLayout.getTextureNames())
                    texturesList.Items.Add(str);
            }

            if (mMainLayout.containsFonts())
            {
                foreach (string str in mMainLayout.getFontNames())
                    fontsList.Items.Add(str);
            }
            
            // and our materials
            if (mMainLayout.containsMaterials())
            {
                foreach (string str in mMainLayout.getMaterialNames())
                    materialList.Items.Add(str);
            }

            return true;
        }

        /// <summary>
        /// Clears all UI elements, such as the listboxes, tree nodes, etc
        /// </summary>
        void Clear()
        {
            mLayoutFiles = null;
            mLayoutAnimFiles = null;
            mLayoutImages = null;
            mLayoutControls = null;
            mMainLayout = null;
            mArchive = null;

            panelList.SelectedNode = null;
            layoutPropertyGrid.SelectedObject = null;
            mainPropertyGrid.SelectedObject = null;
            panelList.Nodes.Clear();
            texturesList.Items.Clear();
            fontsList.Items.Clear();
            materialList.Items.Clear();
        }

        /// <summary>
        /// Fills the panelList TreeView control with layout elements.
        /// </summary>
        /// <param name="nodes"></param>
        void fillNodes(List<LayoutBase> nodes)
        {
            if (nodes == null)
                return;

            foreach (LayoutBase node in nodes)
            {
                TreeNode[] parentNodes = panelList.Nodes.Find(node.getParent().mName, true);

                for (int i = 0; i < parentNodes.Length; i++)
                {
                    TreeNode parentNode = parentNodes[i];

                    TreeNode newNode = new TreeNode();
                    newNode.Tag = node;
                    newNode.Name = node.mName;
                    newNode.Text = node.mName;

                    parentNode.Nodes.Add(newNode);
                }

                // if the node has children, we have to iterate through those as well
                if (node.hasChildren())
                    fillNodes(node.getChildren());
            }
        }

        ArchiveBase mArchive;

        Dictionary<string, byte[]> mLayoutFiles;
        Dictionary<string, byte[]> mLayoutAnimFiles;
        Dictionary<string, byte[]> mLayoutImages;
        Dictionary<string, byte[]> mLayoutControls;

        LayoutBase mMainLayout;
        string mMainRoot;

        private void LoadAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutChooser layoutChooser = new LayoutChooser();
            layoutChooser.insertEntries(new List<string>(mLayoutAnimFiles.Keys));
            layoutChooser.ShowDialog();
        }

        private void ControlToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo
        }

        private void PanelList_AfterSelect(object sender, TreeViewEventArgs e)
        {
           if (panelList.SelectedNode != null && panelList.SelectedNode.Tag != null)
           {
                layoutPropertyGrid.SelectedObject = panelList.SelectedNode.Tag;     
           }
        }

        private void LayoutViewer_Paint(object sender, PaintEventArgs e)
        {
            /* todo -- drawing */
        }

        private void MaterialList_DoubleClick(object sender, EventArgs e)
        {
            if (mMainLayout == null)
                return;

            MaterialEditor editor = null;

            List<MaterialBase> mats = mMainLayout.getMaterials();

            if (mats == null)
                return;

            switch (mats[materialList.SelectedIndex].getType())
            {
                case MaterialBase.Type.Wii:
                    lyt.wii.Material mat = (lyt.wii.Material)mMainLayout.getMaterials()[materialList.SelectedIndex];
                    editor = new MaterialEditor(ref mat);
                    break;
                default:
                    MessageBox.Show("Unsupported material format.");
                    break;
            }

            if (editor != null)
                editor.Show();
        }

        private void TexturesList_DoubleClick(object sender, EventArgs e)
        {
            if (texturesList.Items.Count == 0)
                return;

            string ext = Path.GetExtension(texturesList.GetItemText(texturesList.SelectedItem));

            string[] exts = { ".tpl" };
            bool isSupported = false;

            for (int i = 0; i < exts.Length; i++)
                isSupported = ext == exts[i] ? true : false;

            if (!isSupported)
            {
                MessageBox.Show("This image format does not support viewing yet.");
                return;
            }

            byte[] data = mArchive.getLayoutImages()[mMainRoot + "timg/" + texturesList.GetItemText(texturesList.SelectedItem)];

            EndianBinaryReader reader = new EndianBinaryReader(data);

            ImageContainerBase container = null;

            // no need for a default case since it will return if the format isnt supported
            switch (ext)
            {
                case ".tpl":
                    container = new TPL(ref reader);
                    break;
            }

            if (container == null)
                return;

            ImageViewer viewer = new ImageViewer();

            viewer.setImage(container.getImage(0));
            viewer.Show();
        }
    }
}
