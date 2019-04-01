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
using System.IO;
using System.Windows.Forms;
using flyte.io;
using flyte.io._3ds;
using flyte.io.common;
using flyte.io.wii;
namespace flyte
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            MemoryStream memStream = new Yaz0(File.Open("SpinGuidance.arc", FileMode.Open));
            EndianBinaryReader reader = new EndianBinaryReader(memStream);
            reader.SetEndianess(EndianBinaryReader.Endianess.Big);

            RARC rarc = new RARC(ref reader);
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
                listBox1.Items.Clear();

                // now we need to decide what they just opened
                EndianBinaryReader reader = new EndianBinaryReader(File.Open(dialog.FileName, FileMode.Open));

                string magic = "";

                // we have a Yaz0 compressed file
                if (reader.ReadString(4) == "Yaz0")
                {
                    // we have to close our reader so we can properly read this file as a Yaz0 stream
                    reader.Close();
                    MemoryStream ms = new Yaz0(File.Open(dialog.FileName, FileMode.Open));

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

                if (mArchive != null)
                {
                    string txt = String.Format("Opened file {0} -- Format: {1}", dialog.FileName, magic);
                    statusLabel.Text = txt;

                    foreach (string str in mArchive.getFileNames())
                        listBox1.Items.Add(str);
                }
            }
        }

        ArchiveBase mArchive;
    }
}
