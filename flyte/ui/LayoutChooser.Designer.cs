namespace flyte.ui
{
    partial class LayoutChooser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileList = new System.Windows.Forms.ListBox();
            this.selectLayoutButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileList
            // 
            this.fileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileList.FormattingEnabled = true;
            this.fileList.Location = new System.Drawing.Point(4, 3);
            this.fileList.Name = "fileList";
            this.fileList.Size = new System.Drawing.Size(304, 251);
            this.fileList.TabIndex = 0;
            this.fileList.SelectedIndexChanged += new System.EventHandler(this.FileList_SelectedIndexChanged);
            // 
            // selectLayoutButton
            // 
            this.selectLayoutButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectLayoutButton.Enabled = false;
            this.selectLayoutButton.Location = new System.Drawing.Point(121, 260);
            this.selectLayoutButton.Name = "selectLayoutButton";
            this.selectLayoutButton.Size = new System.Drawing.Size(75, 24);
            this.selectLayoutButton.TabIndex = 1;
            this.selectLayoutButton.Text = "Select";
            this.selectLayoutButton.UseVisualStyleBackColor = true;
            this.selectLayoutButton.Click += new System.EventHandler(this.SelectLayoutButton_Click);
            // 
            // LayoutChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 290);
            this.Controls.Add(this.selectLayoutButton);
            this.Controls.Add(this.fileList);
            this.Name = "LayoutChooser";
            this.Text = "Choose a Layout";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox fileList;
        private System.Windows.Forms.Button selectLayoutButton;
    }
}