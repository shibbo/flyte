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
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using flyte.img.wii;

namespace flyte
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            MemoryStream ms = new Yaz0(File.Open("CounterCoin.szs", FileMode.Open));
            EndianBinaryReader reader = new EndianBinaryReader(ms);
            NARC narc = new NARC(ref reader);

            File.WriteAllBytes("CounterCoin.bclyt", narc.getDataByString("CounterCoin.bclyt"));
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archive Files (*.DARC;*.NARC;*.RARC;*.ARC;*.SZS)|*.DARC;*.NARC;*.RARC;*.ARC;*.SZS";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Clear();
                ProcessData(dialog.FileName);
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

        void ProcessData(string filename)
        {
            // now we need to decide what they just opened
            EndianBinaryReader reader = new EndianBinaryReader(File.Open(filename, FileMode.Open));

            string magic = "";

            // we have a Yaz0 compressed file
            if (reader.ReadString(4) == "Yaz0")
            {
                // we have to close our reader so we can properly read this file as a Yaz0 stream
                reader.Close();
                MemoryStream ms = new Yaz0(File.Open(filename, FileMode.Open));

                reader = new EndianBinaryReader(ms);
                magic = reader.ReadString(4);
                reader.Seek(0);
            }
            else
            {
                // it is not yaz0 compressed, so we go back so we can see the magic
                reader.Seek(0);
                magic = reader.ReadString(4);
                reader.Seek(0);
            }

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

                    if (mArchive.isStringTableObfuscated())
                        MessageBox.Show("This file has obfuscated file names. The editor attempted to find layout files, but cannot supply names.");
                    break;
                case "RARC":
                    reader.SetEndianess(EndianBinaryReader.Endianess.Big);
                    mArchive = new RARC(ref reader);
                    break;
                case "U?8-":
                    reader.SetEndianess(EndianBinaryReader.Endianess.Big);
                    mArchive = new U8(ref reader);
                    break;
                default:
                    MessageBox.Show("Error. Unsupported format with magic: " + magic);
                    break;
            }

            reader.Close();

            if (mArchive == null)
            {
                MessageBox.Show("Format not supported.");
                return;
            }

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

            EndianBinaryReader layoutReader;

            switch (layoutType)
            {
                case ".brlyt":
                    byte[] data = mLayoutFiles[selectedFile];
                    layoutReader = new EndianBinaryReader(data);
                    mMainLayout = new BRLYT(ref layoutReader);
                    layoutReader.Close();
                    break;
                default:
                    MessageBox.Show("soz, i dont support that yet");
                    break;
            }

            if (mMainLayout == null)
                return;

            // for now we can just write BRLYT's stuff here
            PAN1 pane = (PAN1)mMainLayout.getRootPanel();

            // this should be RootPane
            TreeNode node = new TreeNode
            {
                Tag = pane.mName,
                Name = pane.mName,
                Text = pane.mName,
            };

            panelList.Nodes.Add(node);

            fillNodes(pane.getChildren());
        }

        void Clear()
        {
            mLayoutFiles = null;
            mLayoutAnimFiles = null;
            mLayoutImages = null;
            mLayoutControls = null;
            mMainLayout = null;
            mArchive = null;

            panelList.Nodes.Clear();
        }

        void fillNodes(List<LayoutBase> nodes)
        {
            foreach (LayoutBase node in nodes)
            {
                TreeNode[] parentNodes = panelList.Nodes.Find(node.getParent().mName, true);

                for (int i = 0; i < parentNodes.Length; i++)
                {
                    TreeNode parentNode = parentNodes[i];

                    TreeNode newNode = new TreeNode();
                    newNode.Tag = node.mName;
                    newNode.Name = node.mName;
                    newNode.Text = node.mName + " (" + node.getType() + ")";

                    parentNode.Nodes.Add(newNode);
                }

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
    }
}
