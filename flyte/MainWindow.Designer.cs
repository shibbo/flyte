namespace flyte
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelList = new System.Windows.Forms.TreeView();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.layoutPage = new System.Windows.Forms.TabPage();
            this.mainPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.materialsPage = new System.Windows.Forms.TabPage();
            this.materialsToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.materialList = new System.Windows.Forms.ListBox();
            this.elementsPage = new System.Windows.Forms.TabPage();
            this.elementActionsToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.layoutPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.texturesPage = new System.Windows.Forms.TabPage();
            this.texturesList = new System.Windows.Forms.ListBox();
            this.fontsPage = new System.Windows.Forms.TabPage();
            this.fontsList = new System.Windows.Forms.ListBox();
            this.layoutViewer = new OpenTK.GLControl();
            this.mainMenuStrip.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.layoutPage.SuspendLayout();
            this.materialsPage.SuspendLayout();
            this.materialsToolStrip.SuspendLayout();
            this.elementsPage.SuspendLayout();
            this.elementActionsToolStrip.SuspendLayout();
            this.texturesPage.SuspendLayout();
            this.fontsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.layoutToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(934, 24);
            this.mainMenuStrip.TabIndex = 1;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.extractToolStripMenuItem,
            this.closeFileToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Enabled = false;
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.extractToolStripMenuItem.Text = "Extract";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.ExtractToolStripMenuItem_Click);
            // 
            // closeFileToolStripMenuItem
            // 
            this.closeFileToolStripMenuItem.Name = "closeFileToolStripMenuItem";
            this.closeFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.closeFileToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.closeFileToolStripMenuItem.Text = "Close File";
            this.closeFileToolStripMenuItem.Click += new System.EventHandler(this.CloseFileToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.QuitToolStripMenuItem_Click);
            // 
            // layoutToolStripMenuItem
            // 
            this.layoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem});
            this.layoutToolStripMenuItem.Name = "layoutToolStripMenuItem";
            this.layoutToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.layoutToolStripMenuItem.Text = "Layout";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadAnimationToolStripMenuItem,
            this.controlToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // loadAnimationToolStripMenuItem
            // 
            this.loadAnimationToolStripMenuItem.Name = "loadAnimationToolStripMenuItem";
            this.loadAnimationToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.loadAnimationToolStripMenuItem.Text = "Animation";
            this.loadAnimationToolStripMenuItem.Click += new System.EventHandler(this.LoadAnimationToolStripMenuItem_Click);
            // 
            // controlToolStripMenuItem
            // 
            this.controlToolStripMenuItem.Name = "controlToolStripMenuItem";
            this.controlToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.controlToolStripMenuItem.Text = "Control";
            this.controlToolStripMenuItem.Click += new System.EventHandler(this.ControlToolStripMenuItem_Click);
            // 
            // panelList
            // 
            this.panelList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelList.Location = new System.Drawing.Point(3, 31);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(263, 178);
            this.panelList.TabIndex = 4;
            this.panelList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.PanelList_AfterSelect);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mainTabControl.Controls.Add(this.layoutPage);
            this.mainTabControl.Controls.Add(this.materialsPage);
            this.mainTabControl.Controls.Add(this.elementsPage);
            this.mainTabControl.Controls.Add(this.texturesPage);
            this.mainTabControl.Controls.Add(this.fontsPage);
            this.mainTabControl.Location = new System.Drawing.Point(0, 24);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(274, 540);
            this.mainTabControl.TabIndex = 5;
            // 
            // layoutPage
            // 
            this.layoutPage.Controls.Add(this.mainPropertyGrid);
            this.layoutPage.Location = new System.Drawing.Point(4, 22);
            this.layoutPage.Name = "layoutPage";
            this.layoutPage.Padding = new System.Windows.Forms.Padding(3);
            this.layoutPage.Size = new System.Drawing.Size(266, 514);
            this.layoutPage.TabIndex = 3;
            this.layoutPage.Text = "Layout";
            this.layoutPage.UseVisualStyleBackColor = true;
            // 
            // mainPropertyGrid
            // 
            this.mainPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.mainPropertyGrid.Name = "mainPropertyGrid";
            this.mainPropertyGrid.Size = new System.Drawing.Size(260, 508);
            this.mainPropertyGrid.TabIndex = 0;
            this.mainPropertyGrid.ToolbarVisible = false;
            // 
            // materialsPage
            // 
            this.materialsPage.Controls.Add(this.materialsToolStrip);
            this.materialsPage.Controls.Add(this.materialList);
            this.materialsPage.Location = new System.Drawing.Point(4, 22);
            this.materialsPage.Name = "materialsPage";
            this.materialsPage.Size = new System.Drawing.Size(266, 514);
            this.materialsPage.TabIndex = 4;
            this.materialsPage.Text = "Materials";
            this.materialsPage.UseVisualStyleBackColor = true;
            // 
            // materialsToolStrip
            // 
            this.materialsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.materialsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.materialsToolStrip.Location = new System.Drawing.Point(0, 0);
            this.materialsToolStrip.Name = "materialsToolStrip";
            this.materialsToolStrip.Size = new System.Drawing.Size(266, 25);
            this.materialsToolStrip.TabIndex = 1;
            this.materialsToolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Enabled = false;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(77, 22);
            this.toolStripButton1.Text = "Edit Material";
            // 
            // materialList
            // 
            this.materialList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialList.FormattingEnabled = true;
            this.materialList.Location = new System.Drawing.Point(3, 28);
            this.materialList.Name = "materialList";
            this.materialList.Size = new System.Drawing.Size(255, 485);
            this.materialList.TabIndex = 0;
            this.materialList.DoubleClick += new System.EventHandler(this.MaterialList_DoubleClick);
            // 
            // elementsPage
            // 
            this.elementsPage.Controls.Add(this.elementActionsToolStrip);
            this.elementsPage.Controls.Add(this.layoutPropertyGrid);
            this.elementsPage.Controls.Add(this.panelList);
            this.elementsPage.Location = new System.Drawing.Point(4, 22);
            this.elementsPage.Name = "elementsPage";
            this.elementsPage.Padding = new System.Windows.Forms.Padding(3);
            this.elementsPage.Size = new System.Drawing.Size(266, 514);
            this.elementsPage.TabIndex = 0;
            this.elementsPage.Text = "Elements";
            this.elementsPage.UseVisualStyleBackColor = true;
            // 
            // elementActionsToolStrip
            // 
            this.elementActionsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.elementActionsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5});
            this.elementActionsToolStrip.Location = new System.Drawing.Point(3, 3);
            this.elementActionsToolStrip.Name = "elementActionsToolStrip";
            this.elementActionsToolStrip.Size = new System.Drawing.Size(260, 25);
            this.elementActionsToolStrip.TabIndex = 6;
            this.elementActionsToolStrip.Text = "toolStrip2";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Enabled = false;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(33, 22);
            this.toolStripButton2.Text = "Add";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Enabled = false;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(44, 22);
            this.toolStripButton3.Text = "Delete";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Enabled = false;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(61, 22);
            this.toolStripButton4.Text = "Duplicate";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Enabled = false;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(38, 22);
            this.toolStripButton5.Text = "Clear";
            // 
            // layoutPropertyGrid
            // 
            this.layoutPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutPropertyGrid.Location = new System.Drawing.Point(3, 215);
            this.layoutPropertyGrid.Name = "layoutPropertyGrid";
            this.layoutPropertyGrid.Size = new System.Drawing.Size(263, 299);
            this.layoutPropertyGrid.TabIndex = 5;
            this.layoutPropertyGrid.ToolbarVisible = false;
            // 
            // texturesPage
            // 
            this.texturesPage.Controls.Add(this.texturesList);
            this.texturesPage.Location = new System.Drawing.Point(4, 22);
            this.texturesPage.Name = "texturesPage";
            this.texturesPage.Padding = new System.Windows.Forms.Padding(3);
            this.texturesPage.Size = new System.Drawing.Size(266, 514);
            this.texturesPage.TabIndex = 1;
            this.texturesPage.Text = "Textures";
            this.texturesPage.UseVisualStyleBackColor = true;
            // 
            // texturesList
            // 
            this.texturesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texturesList.FormattingEnabled = true;
            this.texturesList.Location = new System.Drawing.Point(3, 3);
            this.texturesList.Name = "texturesList";
            this.texturesList.Size = new System.Drawing.Size(260, 508);
            this.texturesList.TabIndex = 0;
            this.texturesList.DoubleClick += new System.EventHandler(this.TexturesList_DoubleClick);
            // 
            // fontsPage
            // 
            this.fontsPage.Controls.Add(this.fontsList);
            this.fontsPage.Location = new System.Drawing.Point(4, 22);
            this.fontsPage.Name = "fontsPage";
            this.fontsPage.Size = new System.Drawing.Size(266, 514);
            this.fontsPage.TabIndex = 2;
            this.fontsPage.Text = "Fonts";
            this.fontsPage.UseVisualStyleBackColor = true;
            // 
            // fontsList
            // 
            this.fontsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fontsList.FormattingEnabled = true;
            this.fontsList.Location = new System.Drawing.Point(0, 0);
            this.fontsList.Name = "fontsList";
            this.fontsList.Size = new System.Drawing.Size(266, 514);
            this.fontsList.TabIndex = 0;
            // 
            // layoutViewer
            // 
            this.layoutViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutViewer.BackColor = System.Drawing.Color.Black;
            this.layoutViewer.Location = new System.Drawing.Point(273, 24);
            this.layoutViewer.Name = "layoutViewer";
            this.layoutViewer.Size = new System.Drawing.Size(661, 540);
            this.layoutViewer.TabIndex = 6;
            this.layoutViewer.VSync = false;
            this.layoutViewer.Load += new System.EventHandler(this.LayoutViewer_Load);
            this.layoutViewer.Paint += new System.Windows.Forms.PaintEventHandler(this.LayoutViewer_Paint_1);
            this.layoutViewer.Resize += new System.EventHandler(this.LayoutViewer_Resize);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 566);
            this.Controls.Add(this.layoutViewer);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainWindow";
            this.Text = "flyte v0.3 Alpha";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.layoutPage.ResumeLayout(false);
            this.materialsPage.ResumeLayout(false);
            this.materialsPage.PerformLayout();
            this.materialsToolStrip.ResumeLayout(false);
            this.materialsToolStrip.PerformLayout();
            this.elementsPage.ResumeLayout(false);
            this.elementsPage.PerformLayout();
            this.elementActionsToolStrip.ResumeLayout(false);
            this.elementActionsToolStrip.PerformLayout();
            this.texturesPage.ResumeLayout(false);
            this.fontsPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.TreeView panelList;
        private System.Windows.Forms.ToolStripMenuItem closeFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage elementsPage;
        private System.Windows.Forms.TabPage texturesPage;
        private System.Windows.Forms.TabPage fontsPage;
        private System.Windows.Forms.ListBox texturesList;
        private System.Windows.Forms.ListBox fontsList;
        private System.Windows.Forms.PropertyGrid layoutPropertyGrid;
        private System.Windows.Forms.TabPage layoutPage;
        private System.Windows.Forms.TabPage materialsPage;
        private System.Windows.Forms.PropertyGrid mainPropertyGrid;
        private System.Windows.Forms.ToolStrip materialsToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ListBox materialList;
        private System.Windows.Forms.ToolStrip elementActionsToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private OpenTK.GLControl layoutViewer;
    }
}

