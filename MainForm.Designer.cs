namespace SWD4CS
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainWndSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ctrlsTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ctrlLstBox = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ctrlTreeView = new System.Windows.Forms.TreeView();
            this.subWndSplitContainer = new System.Windows.Forms.SplitContainer();
            this.designeTab = new System.Windows.Forms.TabControl();
            this.designePage = new System.Windows.Forms.TabPage();
            this.userForm = new SWD4CS.cls_userform();
            this.sourcePage = new System.Windows.Forms.TabPage();
            this.sourceTxtBox = new System.Windows.Forms.TextBox();
            this.eventsPage = new System.Windows.Forms.TabPage();
            this.eventTxtBox = new System.Windows.Forms.TextBox();
            this.logPage = new System.Windows.Forms.TabPage();
            this.logTxtBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.nameTxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.propertyBox = new System.Windows.Forms.PropertyGrid();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.evtGridView = new SWD4CS.cls_user_datagridview();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainWndSplitContainer)).BeginInit();
            this.mainWndSplitContainer.Panel1.SuspendLayout();
            this.mainWndSplitContainer.Panel2.SuspendLayout();
            this.mainWndSplitContainer.SuspendLayout();
            this.ctrlsTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subWndSplitContainer)).BeginInit();
            this.subWndSplitContainer.Panel1.SuspendLayout();
            this.subWndSplitContainer.Panel2.SuspendLayout();
            this.subWndSplitContainer.SuspendLayout();
            this.designeTab.SuspendLayout();
            this.designePage.SuspendLayout();
            this.sourcePage.SuspendLayout();
            this.eventsPage.SuspendLayout();
            this.logPage.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.evtGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainWndSplitContainer
            // 
            this.mainWndSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainWndSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainWndSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainWndSplitContainer.Location = new System.Drawing.Point(0, 31);
            this.mainWndSplitContainer.Name = "mainWndSplitContainer";
            // 
            // mainWndSplitContainer.Panel1
            // 
            this.mainWndSplitContainer.Panel1.Controls.Add(this.ctrlsTab);
            // 
            // mainWndSplitContainer.Panel2
            // 
            this.mainWndSplitContainer.Panel2.Controls.Add(this.subWndSplitContainer);
            this.mainWndSplitContainer.Size = new System.Drawing.Size(1003, 608);
            this.mainWndSplitContainer.SplitterDistance = 199;
            this.mainWndSplitContainer.TabIndex = 0;
            // 
            // ctrlsTab
            // 
            this.ctrlsTab.Controls.Add(this.tabPage1);
            this.ctrlsTab.Controls.Add(this.tabPage2);
            this.ctrlsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlsTab.Location = new System.Drawing.Point(0, 0);
            this.ctrlsTab.Name = "ctrlsTab";
            this.ctrlsTab.SelectedIndex = 0;
            this.ctrlsTab.Size = new System.Drawing.Size(195, 604);
            this.ctrlsTab.TabIndex = 0;
            this.ctrlsTab.SelectedIndexChanged += new System.EventHandler(this.ctrlsTab_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ctrlLstBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(187, 571);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ToolsBox";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ctrlLstBox
            // 
            this.ctrlLstBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlLstBox.FormattingEnabled = true;
            this.ctrlLstBox.ItemHeight = 20;
            this.ctrlLstBox.Location = new System.Drawing.Point(3, 3);
            this.ctrlLstBox.Name = "ctrlLstBox";
            this.ctrlLstBox.Size = new System.Drawing.Size(181, 565);
            this.ctrlLstBox.Sorted = true;
            this.ctrlLstBox.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ctrlTreeView);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(187, 571);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TreeView";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ctrlTreeView
            // 
            this.ctrlTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlTreeView.Location = new System.Drawing.Point(3, 3);
            this.ctrlTreeView.Name = "ctrlTreeView";
            this.ctrlTreeView.Size = new System.Drawing.Size(181, 565);
            this.ctrlTreeView.TabIndex = 0;
            this.ctrlTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ctrlTreeView_AfterSelect);
            // 
            // subWndSplitContainer
            // 
            this.subWndSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.subWndSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subWndSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.subWndSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.subWndSplitContainer.Name = "subWndSplitContainer";
            // 
            // subWndSplitContainer.Panel1
            // 
            this.subWndSplitContainer.Panel1.AutoScroll = true;
            this.subWndSplitContainer.Panel1.BackColor = System.Drawing.Color.White;
            this.subWndSplitContainer.Panel1.Controls.Add(this.designeTab);
            // 
            // subWndSplitContainer.Panel2
            // 
            this.subWndSplitContainer.Panel2.Controls.Add(this.tabControl1);
            this.subWndSplitContainer.Size = new System.Drawing.Size(800, 608);
            this.subWndSplitContainer.SplitterDistance = 525;
            this.subWndSplitContainer.TabIndex = 0;
            // 
            // designeTab
            // 
            this.designeTab.Controls.Add(this.designePage);
            this.designeTab.Controls.Add(this.sourcePage);
            this.designeTab.Controls.Add(this.eventsPage);
            this.designeTab.Controls.Add(this.logPage);
            this.designeTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.designeTab.Location = new System.Drawing.Point(0, 0);
            this.designeTab.Name = "designeTab";
            this.designeTab.SelectedIndex = 0;
            this.designeTab.Size = new System.Drawing.Size(521, 604);
            this.designeTab.TabIndex = 1;
            this.designeTab.SelectedIndexChanged += new System.EventHandler(this.designeTab_SelectedIndexChanged);
            // 
            // designePage
            // 
            this.designePage.AutoScroll = true;
            this.designePage.Controls.Add(this.userForm);
            this.designePage.Location = new System.Drawing.Point(4, 29);
            this.designePage.Name = "designePage";
            this.designePage.Padding = new System.Windows.Forms.Padding(3);
            this.designePage.Size = new System.Drawing.Size(513, 571);
            this.designePage.TabIndex = 0;
            this.designePage.Text = "Designe";
            this.designePage.UseVisualStyleBackColor = true;
            // 
            // userForm
            // 
            this.userForm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.userForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userForm.Location = new System.Drawing.Point(16, 16);
            this.userForm.Name = "userForm";
            this.userForm.Size = new System.Drawing.Size(480, 400);
            this.userForm.TabIndex = 0;
            // 
            // sourcePage
            // 
            this.sourcePage.Controls.Add(this.sourceTxtBox);
            this.sourcePage.Location = new System.Drawing.Point(4, 29);
            this.sourcePage.Name = "sourcePage";
            this.sourcePage.Padding = new System.Windows.Forms.Padding(3);
            this.sourcePage.Size = new System.Drawing.Size(513, 571);
            this.sourcePage.TabIndex = 1;
            this.sourcePage.Text = "Source";
            this.sourcePage.UseVisualStyleBackColor = true;
            // 
            // sourceTxtBox
            // 
            this.sourceTxtBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.sourceTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceTxtBox.Location = new System.Drawing.Point(3, 3);
            this.sourceTxtBox.Multiline = true;
            this.sourceTxtBox.Name = "sourceTxtBox";
            this.sourceTxtBox.ReadOnly = true;
            this.sourceTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.sourceTxtBox.Size = new System.Drawing.Size(507, 565);
            this.sourceTxtBox.TabIndex = 0;
            this.sourceTxtBox.WordWrap = false;
            // 
            // eventsPage
            // 
            this.eventsPage.Controls.Add(this.eventTxtBox);
            this.eventsPage.Location = new System.Drawing.Point(4, 29);
            this.eventsPage.Name = "eventsPage";
            this.eventsPage.Padding = new System.Windows.Forms.Padding(3);
            this.eventsPage.Size = new System.Drawing.Size(513, 571);
            this.eventsPage.TabIndex = 2;
            this.eventsPage.Text = "Events";
            this.eventsPage.UseVisualStyleBackColor = true;
            // 
            // eventTxtBox
            // 
            this.eventTxtBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.eventTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventTxtBox.Location = new System.Drawing.Point(3, 3);
            this.eventTxtBox.Multiline = true;
            this.eventTxtBox.Name = "eventTxtBox";
            this.eventTxtBox.ReadOnly = true;
            this.eventTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.eventTxtBox.Size = new System.Drawing.Size(507, 565);
            this.eventTxtBox.TabIndex = 0;
            this.eventTxtBox.WordWrap = false;
            // 
            // logPage
            // 
            this.logPage.Controls.Add(this.logTxtBox);
            this.logPage.Location = new System.Drawing.Point(4, 29);
            this.logPage.Name = "logPage";
            this.logPage.Padding = new System.Windows.Forms.Padding(3);
            this.logPage.Size = new System.Drawing.Size(513, 571);
            this.logPage.TabIndex = 3;
            this.logPage.Text = "log";
            this.logPage.UseVisualStyleBackColor = true;
            // 
            // logTxtBox
            // 
            this.logTxtBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.logTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTxtBox.Location = new System.Drawing.Point(3, 3);
            this.logTxtBox.Multiline = true;
            this.logTxtBox.Name = "logTxtBox";
            this.logTxtBox.ReadOnly = true;
            this.logTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTxtBox.Size = new System.Drawing.Size(507, 565);
            this.logTxtBox.TabIndex = 0;
            this.logTxtBox.WordWrap = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(267, 604);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.nameTxtBox);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.propertyBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(259, 571);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Property";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // nameTxtBox
            // 
            this.nameTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTxtBox.Location = new System.Drawing.Point(58, 6);
            this.nameTxtBox.Name = "nameTxtBox";
            this.nameTxtBox.Size = new System.Drawing.Size(195, 27);
            this.nameTxtBox.TabIndex = 2;
            this.nameTxtBox.TextChanged += new System.EventHandler(this.nameTxtBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // propertyBox
            // 
            this.propertyBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyBox.Location = new System.Drawing.Point(3, 39);
            this.propertyBox.Name = "propertyBox";
            this.propertyBox.Size = new System.Drawing.Size(253, 526);
            this.propertyBox.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.evtGridView);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(259, 567);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Events";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // evtGridView
            // 
            this.evtGridView.AllowUserToAddRows = false;
            this.evtGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.evtGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.evtGridView.Location = new System.Drawing.Point(3, 3);
            this.evtGridView.Name = "evtGridView";
            this.evtGridView.RowHeadersWidth = 51;
            this.evtGridView.RowTemplate.Height = 29;
            this.evtGridView.Size = new System.Drawing.Size(253, 561);
            this.evtGridView.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 642);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1003, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1003, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.deleteToolStripMenuItem.Text = "Delete (Alt + Del)";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click_1);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 664);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.mainWndSplitContainer);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SWD4CS";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.mainWndSplitContainer.Panel1.ResumeLayout(false);
            this.mainWndSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainWndSplitContainer)).EndInit();
            this.mainWndSplitContainer.ResumeLayout(false);
            this.ctrlsTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.subWndSplitContainer.Panel1.ResumeLayout(false);
            this.subWndSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.subWndSplitContainer)).EndInit();
            this.subWndSplitContainer.ResumeLayout(false);
            this.designeTab.ResumeLayout(false);
            this.designePage.ResumeLayout(false);
            this.sourcePage.ResumeLayout(false);
            this.sourcePage.PerformLayout();
            this.eventsPage.ResumeLayout(false);
            this.eventsPage.PerformLayout();
            this.logPage.ResumeLayout(false);
            this.logPage.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.evtGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SplitContainer mainWndSplitContainer;
        private TabControl ctrlsTab;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private SplitContainer subWndSplitContainer;
        private TabControl tabControl1;
        private TabPage tabPage3;
        private TextBox nameTxtBox;
        private Label label1;
        private PropertyGrid propertyBox;
        private TabPage tabPage4;
        private StatusStrip statusStrip1;
        private ListBox ctrlLstBox;
        private TreeView ctrlTreeView;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private TabControl designeTab;
        private TabPage designePage;
        private TabPage sourcePage;
        private TabPage eventsPage;
        private cls_userform userForm;
        private TabPage logPage;
        private TextBox logTxtBox;
        private TextBox sourceTxtBox;
        private cls_user_datagridview evtGridView;
        private TextBox eventTxtBox;
    }
}