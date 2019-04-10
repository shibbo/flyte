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
using OpenTK.Graphics.OpenGL;
using flyte.lyt._3ds;
using static flyte.utils.Endian;
using System.Text;

namespace flyte
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
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
            dialog.Filter = "Archive Files (*.DARC;*.NARC;*.RARC;*.ARC;*.SZS;*.LZ)|*.DARC;*.NARC;*.RARC;*.ARC;*.SZS;*.LZ";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Clear();
                ProcessData(dialog.FileName);
                this.Text = "flyte v0.1 Alpha -- " + Path.GetFileName(dialog.FileName);
            }
        }

        private void ViewControl_Paint(object sender, PaintEventArgs e)
        {
            viewControl.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(-0.5f, -0.5f);
            GL.Vertex2(0.5f, -0.5f);
            GL.Vertex2(0.5f, 0.5f);
            GL.Vertex2(-0.5f, 0.5f);
            GL.End();

            viewControl.SwapBuffers();
        }

        private void ViewControl_Load(object sender, EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
        }

        private void CloseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
        }


        /// <summary>
        /// Process data in an input file that contains a layout.
        /// </summary>
        /// <param name="filename"></param>
        void ProcessData(string filename)
        {
            // now we need to decide what they just opened
            EndianBinaryReader reader = new EndianBinaryReader(File.Open(filename, FileMode.Open), Encoding.GetEncoding(932));

            string magic = "";

            // we have a Yaz0 compressed file
            if (reader.ReadStringFrom(0, 4) == "Yaz0")
            {
                // we have to close our reader so we can properly read this file as a Yaz0 stream
                reader.Close();
                MemoryStream ms = new Yaz0(File.Open(filename, FileMode.Open));

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
                return;
            }

            // get all of our needed files
            mLayoutFiles = mArchive.getLayoutFiles();
            mLayoutAnimFiles = mArchive.getLayoutAnimations();
            mLayoutImages = mArchive.getLayoutImages();
            mLayoutControls = mArchive.getLayoutControls();

            LayoutChooser layoutChooser = new LayoutChooser();
            layoutChooser.insertEntries(new List<string>(mLayoutFiles.Keys));
            layoutChooser.ShowDialog();

            string selectedFile = layoutChooser.getSelectedFile();

            if (selectedFile == null)
                return;

            string layoutType = Path.GetExtension(selectedFile);

            // now we have to init a layout reader
            EndianBinaryReader layoutReader;
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
                default:
                    MessageBox.Show("This format is not supported yet.");
                    break;
            }

            if (mMainLayout == null)
                return;

            // set our propertygrid with our LYT object
            mainPropertyGrid.SelectedObject = mMainLayout.getLayoutParams();

            // for now we can just write BRLYT's stuff here, since we have the root panel
            PAN1 pane = (PAN1)mMainLayout.getRootPanel();

            if (pane == null)
            {
                MessageBox.Show("Error, the root pane in this layout is not specified.");
                return;
            }

            // this should be RootPane
            TreeNode node = new TreeNode
            {
                Tag = pane,
                Name = pane.mName,
                Text = pane.mName,
            };

            // add our root node, and then start adding its children
            panelList.Nodes.Add(node);
            fillNodes(pane.getChildren());

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
    }
}
