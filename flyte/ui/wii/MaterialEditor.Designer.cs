namespace flyte.ui
{
    partial class MaterialEditor
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
            this.tevStageTab = new System.Windows.Forms.TabPage();
            this.tevSwapBox = new System.Windows.Forms.GroupBox();
            this.tevSwapList = new System.Windows.Forms.ListBox();
            this.tevSwapGrid = new System.Windows.Forms.PropertyGrid();
            this.tevStageBox = new System.Windows.Forms.GroupBox();
            this.chanControlPage = new System.Windows.Forms.TabPage();
            this.texCoordsPage = new System.Windows.Forms.TabPage();
            this.texMapPage = new System.Windows.Forms.TabPage();
            this.texSRTsGroup = new System.Windows.Forms.GroupBox();
            this.texSRTGrid = new System.Windows.Forms.PropertyGrid();
            this.texSRTList = new System.Windows.Forms.ListBox();
            this.texMapsGroup = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.texMapList = new System.Windows.Forms.ListBox();
            this.texMapGrid = new System.Windows.Forms.PropertyGrid();
            this.tevStageTab.SuspendLayout();
            this.tevSwapBox.SuspendLayout();
            this.texMapPage.SuspendLayout();
            this.texSRTsGroup.SuspendLayout();
            this.texMapsGroup.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tevStageTab
            // 
            this.tevStageTab.Controls.Add(this.tevSwapBox);
            this.tevStageTab.Controls.Add(this.tevStageBox);
            this.tevStageTab.Location = new System.Drawing.Point(4, 22);
            this.tevStageTab.Name = "tevStageTab";
            this.tevStageTab.Size = new System.Drawing.Size(792, 424);
            this.tevStageTab.TabIndex = 4;
            this.tevStageTab.Text = "TEV";
            this.tevStageTab.UseVisualStyleBackColor = true;
            // 
            // tevSwapBox
            // 
            this.tevSwapBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tevSwapBox.Controls.Add(this.tevSwapList);
            this.tevSwapBox.Controls.Add(this.tevSwapGrid);
            this.tevSwapBox.Location = new System.Drawing.Point(399, 3);
            this.tevSwapBox.Name = "tevSwapBox";
            this.tevSwapBox.Size = new System.Drawing.Size(385, 418);
            this.tevSwapBox.TabIndex = 1;
            this.tevSwapBox.TabStop = false;
            this.tevSwapBox.Text = "TEV Swap Table";
            // 
            // tevSwapList
            // 
            this.tevSwapList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tevSwapList.FormattingEnabled = true;
            this.tevSwapList.Items.AddRange(new object[] {
            "Swap Mode 0",
            "Swap Mode 1",
            "Swap Mode 2",
            "Swap Mode 3"});
            this.tevSwapList.Location = new System.Drawing.Point(6, 19);
            this.tevSwapList.Name = "tevSwapList";
            this.tevSwapList.Size = new System.Drawing.Size(373, 69);
            this.tevSwapList.TabIndex = 2;
            this.tevSwapList.SelectedIndexChanged += new System.EventHandler(this.TevSwapList_SelectedIndexChanged);
            // 
            // tevSwapGrid
            // 
            this.tevSwapGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tevSwapGrid.Location = new System.Drawing.Point(6, 94);
            this.tevSwapGrid.Name = "tevSwapGrid";
            this.tevSwapGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.tevSwapGrid.Size = new System.Drawing.Size(373, 324);
            this.tevSwapGrid.TabIndex = 1;
            this.tevSwapGrid.ToolbarVisible = false;
            // 
            // tevStageBox
            // 
            this.tevStageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tevStageBox.Location = new System.Drawing.Point(3, 3);
            this.tevStageBox.Name = "tevStageBox";
            this.tevStageBox.Size = new System.Drawing.Size(390, 418);
            this.tevStageBox.TabIndex = 0;
            this.tevStageBox.TabStop = false;
            this.tevStageBox.Text = "TEV Stages";
            // 
            // chanControlPage
            // 
            this.chanControlPage.Location = new System.Drawing.Point(4, 22);
            this.chanControlPage.Name = "chanControlPage";
            this.chanControlPage.Size = new System.Drawing.Size(792, 424);
            this.chanControlPage.TabIndex = 3;
            this.chanControlPage.Text = "Channel Control";
            this.chanControlPage.UseVisualStyleBackColor = true;
            // 
            // texCoordsPage
            // 
            this.texCoordsPage.Location = new System.Drawing.Point(4, 22);
            this.texCoordsPage.Name = "texCoordsPage";
            this.texCoordsPage.Size = new System.Drawing.Size(792, 424);
            this.texCoordsPage.TabIndex = 2;
            this.texCoordsPage.Text = "Texture Coordinates";
            this.texCoordsPage.UseVisualStyleBackColor = true;
            // 
            // texMapPage
            // 
            this.texMapPage.Controls.Add(this.texSRTsGroup);
            this.texMapPage.Controls.Add(this.texMapsGroup);
            this.texMapPage.Location = new System.Drawing.Point(4, 22);
            this.texMapPage.Name = "texMapPage";
            this.texMapPage.Padding = new System.Windows.Forms.Padding(3);
            this.texMapPage.Size = new System.Drawing.Size(792, 424);
            this.texMapPage.TabIndex = 0;
            this.texMapPage.Text = "Texture Maps";
            this.texMapPage.UseVisualStyleBackColor = true;
            // 
            // texSRTsGroup
            // 
            this.texSRTsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.texSRTsGroup.Controls.Add(this.texSRTGrid);
            this.texSRTsGroup.Controls.Add(this.texSRTList);
            this.texSRTsGroup.Location = new System.Drawing.Point(374, 3);
            this.texSRTsGroup.Name = "texSRTsGroup";
            this.texSRTsGroup.Size = new System.Drawing.Size(415, 410);
            this.texSRTsGroup.TabIndex = 1;
            this.texSRTsGroup.TabStop = false;
            this.texSRTsGroup.Text = "Texture SRTs";
            // 
            // texSRTGrid
            // 
            this.texSRTGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.texSRTGrid.Location = new System.Drawing.Point(6, 133);
            this.texSRTGrid.Name = "texSRTGrid";
            this.texSRTGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.texSRTGrid.Size = new System.Drawing.Size(401, 271);
            this.texSRTGrid.TabIndex = 2;
            this.texSRTGrid.ToolbarVisible = false;
            // 
            // texSRTList
            // 
            this.texSRTList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.texSRTList.FormattingEnabled = true;
            this.texSRTList.Location = new System.Drawing.Point(6, 19);
            this.texSRTList.Name = "texSRTList";
            this.texSRTList.Size = new System.Drawing.Size(401, 108);
            this.texSRTList.TabIndex = 1;
            this.texSRTList.SelectedIndexChanged += new System.EventHandler(this.TexSRTList_SelectedIndexChanged);
            // 
            // texMapsGroup
            // 
            this.texMapsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.texMapsGroup.Controls.Add(this.texMapGrid);
            this.texMapsGroup.Controls.Add(this.texMapList);
            this.texMapsGroup.Location = new System.Drawing.Point(3, 3);
            this.texMapsGroup.Name = "texMapsGroup";
            this.texMapsGroup.Size = new System.Drawing.Size(365, 410);
            this.texMapsGroup.TabIndex = 0;
            this.texMapsGroup.TabStop = false;
            this.texMapsGroup.Text = "Texture Maps";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.texMapPage);
            this.tabControl1.Controls.Add(this.texCoordsPage);
            this.tabControl1.Controls.Add(this.chanControlPage);
            this.tabControl1.Controls.Add(this.tevStageTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // texMapList
            // 
            this.texMapList.FormattingEnabled = true;
            this.texMapList.Location = new System.Drawing.Point(6, 19);
            this.texMapList.Name = "texMapList";
            this.texMapList.Size = new System.Drawing.Size(353, 108);
            this.texMapList.TabIndex = 0;
            // 
            // texMapGrid
            // 
            this.texMapGrid.Location = new System.Drawing.Point(6, 133);
            this.texMapGrid.Name = "texMapGrid";
            this.texMapGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.texMapGrid.Size = new System.Drawing.Size(353, 271);
            this.texMapGrid.TabIndex = 1;
            this.texMapGrid.ToolbarVisible = false;
            // 
            // MaterialEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "MaterialEditor";
            this.Text = "MaterialEditor";
            this.tevStageTab.ResumeLayout(false);
            this.tevSwapBox.ResumeLayout(false);
            this.texMapPage.ResumeLayout(false);
            this.texSRTsGroup.ResumeLayout(false);
            this.texMapsGroup.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tevStageTab;
        private System.Windows.Forms.GroupBox tevSwapBox;
        private System.Windows.Forms.ListBox tevSwapList;
        private System.Windows.Forms.PropertyGrid tevSwapGrid;
        private System.Windows.Forms.GroupBox tevStageBox;
        private System.Windows.Forms.TabPage chanControlPage;
        private System.Windows.Forms.TabPage texCoordsPage;
        private System.Windows.Forms.TabPage texMapPage;
        private System.Windows.Forms.GroupBox texSRTsGroup;
        private System.Windows.Forms.ListBox texSRTList;
        private System.Windows.Forms.GroupBox texMapsGroup;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.PropertyGrid texSRTGrid;
        private System.Windows.Forms.PropertyGrid texMapGrid;
        private System.Windows.Forms.ListBox texMapList;
    }
}