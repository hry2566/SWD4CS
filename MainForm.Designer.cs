namespace SWD4CS;

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
        this.designTab = new System.Windows.Forms.TabControl();
        this.designPage = new System.Windows.Forms.TabPage();
        this.sourcePage = new System.Windows.Forms.TabPage();
        this.sourceTxtBox = new System.Windows.Forms.TextBox();
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tabPage3 = new System.Windows.Forms.TabPage();
        this.nameTxtBox = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.propertyBox = new System.Windows.Forms.PropertyGrid();
        this.tabPage4 = new System.Windows.Forms.TabPage();
        this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        this.designSplitContainer = new System.Windows.Forms.SplitContainer();
        this.otherCtlPanel = new System.Windows.Forms.FlowLayoutPanel();
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
        this.designTab.SuspendLayout();
        this.designPage.SuspendLayout();
        this.sourcePage.SuspendLayout();
        this.tabControl1.SuspendLayout();
        this.tabPage3.SuspendLayout();
        this.tabPage4.SuspendLayout();
        this.statusStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.designSplitContainer)).BeginInit();
        this.designSplitContainer.Panel1.SuspendLayout();
        this.designSplitContainer.Panel2.SuspendLayout();
        this.designSplitContainer.SuspendLayout();
        this.otherCtlPanel.SuspendLayout();
        this.SuspendLayout();
        //
        // mainWndSplitContainer
        //
        this.mainWndSplitContainer.Panel1.Controls.Add(this.ctrlsTab);
        this.mainWndSplitContainer.Panel2.Controls.Add(this.subWndSplitContainer);
        this.mainWndSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
        this.mainWndSplitContainer.Text =  "mainWndSplitContainer";
        this.mainWndSplitContainer.BackColor = System.Drawing.Color.WhiteSmoke;
        this.mainWndSplitContainer.Location = new System.Drawing.Point(0,31);
        this.mainWndSplitContainer.Size = new System.Drawing.Size(1003,608);
        this.mainWndSplitContainer.SplitterDistance = 199;
        this.mainWndSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        //
        // ctrlsTab
        //
        this.ctrlsTab.Controls.Add(this.tabPage1);
        this.ctrlsTab.Controls.Add(this.tabPage2);
        this.ctrlsTab.ItemSize = new System.Drawing.Size(59,20);
        this.ctrlsTab.SelectedIndex = 0;
        this.ctrlsTab.Text =  "ctrlsTab";
        this.ctrlsTab.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ctrlsTab.Size = new System.Drawing.Size(197,606);
        this.ctrlsTab.TabIndex = 1;
        this.ctrlsTab.SelectedIndexChanged += new System.EventHandler(ctrlsTab_SelectedIndexChanged);
        //
        // tabPage1
        //
        this.tabPage1.Controls.Add(this.ctrlLstBox);
        this.tabPage1.BackColor = Color.Transparent;
        this.tabPage1.Location = new System.Drawing.Point(4,24);
        this.tabPage1.TabIndex = 2;
        this.tabPage1.Text =  "ToolsBox";
        this.tabPage1.Size = new System.Drawing.Size(189,578);
        //
        // ctrlLstBox
        //
        this.ctrlLstBox.ItemHeight = 20;
        this.ctrlLstBox.Text =  "ctrlLstBox";
        this.ctrlLstBox.FormattingEnabled =  true;
        this.ctrlLstBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ctrlLstBox.Size = new System.Drawing.Size(189,564);
        this.ctrlLstBox.TabIndex = 3;
        //
        // tabPage2
        //
        this.tabPage2.Controls.Add(this.ctrlTreeView);
        this.tabPage2.BackColor = Color.Transparent;
        this.tabPage2.Location = new System.Drawing.Point(4,24);
        this.tabPage2.TabIndex = 1;
        this.tabPage2.Text =  "TreeView";
        this.tabPage2.Size = new System.Drawing.Size(189,578);
        //
        // ctrlTreeView
        //
        this.ctrlTreeView.ItemHeight = 18;
        this.ctrlTreeView.LineColor = System.Drawing.Color.Black;
        this.ctrlTreeView.Text =  "ctrlTreeView";
        this.ctrlTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ctrlTreeView.Size = new System.Drawing.Size(189,578);
        this.ctrlTreeView.TabIndex = 5;
        this.ctrlTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(ctrlTreeView_AfterSelect);
        //
        // subWndSplitContainer
        //
        this.subWndSplitContainer.Panel1.Controls.Add(this.designTab);
        this.subWndSplitContainer.Panel2.Controls.Add(this.tabControl1);
        this.subWndSplitContainer.Panel2.Controls.Add(this.nameTxtBox);
        this.subWndSplitContainer.Panel2.Controls.Add(this.label1);
        this.subWndSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        this.subWndSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
        this.subWndSplitContainer.Text =  "subWndSplitContainer";
        this.subWndSplitContainer.BackColor = System.Drawing.Color.WhiteSmoke;
        this.subWndSplitContainer.Size = new System.Drawing.Size(800,608);
        this.subWndSplitContainer.TabIndex = 6;
        this.subWndSplitContainer.SplitterDistance = 529;
        //
        // designTab
        //
        this.designTab.Controls.Add(this.designPage);
        this.designTab.Controls.Add(this.sourcePage);
        this.designTab.ItemSize = new System.Drawing.Size(54,20);
        this.designTab.SelectedIndex = 0;
        this.designTab.Text =  "designTab";
        this.designTab.Dock = System.Windows.Forms.DockStyle.Fill;
        this.designTab.Size = new System.Drawing.Size(527,606);
        this.designTab.TabIndex = 1;
        this.designTab.SelectedIndexChanged += new System.EventHandler(designTab_SelectedIndexChanged);
        //
        // designPage
        //
        this.designPage.Controls.Add(this.designSplitContainer);
        this.designPage.BackColor = Color.Transparent;
        this.designPage.Location = new System.Drawing.Point(4,24);
        this.designPage.TabIndex = 8;
        this.designPage.Text =  "Design";
        this.designPage.AutoScroll =  true;
        this.designPage.Size = new System.Drawing.Size(519,578);
        //
        // sourcePage
        //
        this.sourcePage.Controls.Add(this.sourceTxtBox);
        this.sourcePage.BackColor = Color.Transparent;
        this.sourcePage.Location = new System.Drawing.Point(4,24);
        this.sourcePage.TabIndex = 1;
        this.sourcePage.Text =  "Source";
        this.sourcePage.Size = new System.Drawing.Size(519,578);
        //
        // sourceTxtBox
        //
        this.sourceTxtBox.Multiline =  true;
        this.sourceTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.sourceTxtBox.Text =  "TextBox0";
        this.sourceTxtBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
        this.sourceTxtBox.ReadOnly =  true;
        this.sourceTxtBox.WordWrap =  false;
        this.sourceTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sourceTxtBox.Size = new System.Drawing.Size(519,578);
        this.sourceTxtBox.TabIndex = 10;
        //
        // tabControl1
        //
        this.tabControl1.Controls.Add(this.tabPage3);
        this.tabControl1.Controls.Add(this.tabPage4);
        this.tabControl1.ItemSize = new System.Drawing.Size(57,20);
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Text =  "TabControl2";
        this.tabControl1.Location = new System.Drawing.Point(4,32);
        this.tabControl1.Size = new System.Drawing.Size(269,602);
        this.tabControl1.TabIndex = 15;
        this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        //
        // tabPage3
        //
        this.tabPage3.Controls.Add(this.propertyBox);
        this.tabPage3.BackColor = Color.Transparent;
        this.tabPage3.Location = new System.Drawing.Point(4,24);
        this.tabPage3.TabIndex = 16;
        this.tabPage3.Text =  "Property";
        this.tabPage3.Size = new System.Drawing.Size(261,574);
        //
        // nameTxtBox
        //
        this.nameTxtBox.Text =  "TextBox3";
        this.nameTxtBox.Location = new System.Drawing.Point(58,6);
        this.nameTxtBox.Size = new System.Drawing.Size(206,27);
        this.nameTxtBox.TabIndex = 2;
        this.nameTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.nameTxtBox.TextChanged += new System.EventHandler(nameTxtBox_TextChanged);
        //
        // label1
        //
        this.label1.AutoSize =  true;
        this.label1.Text =  "Name";
        this.label1.BackColor = Color.Transparent;
        this.label1.Location = new System.Drawing.Point(8,9);
        this.label1.Size = new System.Drawing.Size(49,20);
        this.label1.TabIndex = 1;
        //
        // propertyBox
        //
        this.propertyBox.Text =  "PropertyGrid0";
        this.propertyBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.propertyBox.Size = new System.Drawing.Size(261,574);
        this.propertyBox.TabIndex = 19;
        //
        // tabPage4
        //
        this.tabPage4.Location = new System.Drawing.Point(4,24);
        this.tabPage4.TabIndex = 20;
        this.tabPage4.Text =  "Event";
        this.tabPage4.Size = new System.Drawing.Size(261,574);
        //
        // statusStrip1
        //
        this.statusStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
        this.statusStrip1.Location = new System.Drawing.Point(0,634);
        this.statusStrip1.Size = new System.Drawing.Size(1001,22);
        this.statusStrip1.TabIndex = 1;
        this.statusStrip1.Text =  "statusStrip1";
        //
        // designSplitContainer
        //
        this.designSplitContainer.Panel2.Controls.Add(this.otherCtlPanel);
        this.designSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        this.designSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
        this.designSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
        this.designSplitContainer.Text =  "SplitContainer2";
        this.designSplitContainer.BackColor = Color.Transparent;
        this.designSplitContainer.Size = new System.Drawing.Size(519,578);
        this.designSplitContainer.TabIndex = 22;
        this.designSplitContainer.SplitterDistance = 465;
        //
        // otherCtlPanel
        //
        this.otherCtlPanel.Text =  "FlowLayoutPanel0";
        this.otherCtlPanel.AutoScroll =  true;
        this.otherCtlPanel.BackColor = System.Drawing.Color.White;
        this.otherCtlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.otherCtlPanel.Size = new System.Drawing.Size(517,107);
        this.otherCtlPanel.TabIndex = 23;
     //
     // form
     //
        this.BackColor = System.Drawing.Color.WhiteSmoke;
        this.KeyPreview =  true;
        this.Size = new System.Drawing.Size(1019,703);
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text =  "SWD4CS";
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.KeyDown += new System.Windows.Forms.KeyEventHandler(MainForm_KeyDown);
        this.Controls.Add(this.mainWndSplitContainer);
        this.Controls.Add(this.statusStrip1);
        ((System.ComponentModel.ISupportInitialize)(this.mainWndSplitContainer)).EndInit();
        this.mainWndSplitContainer.Panel1.ResumeLayout(false);
        this.mainWndSplitContainer.Panel2.ResumeLayout(false);
        this.mainWndSplitContainer.ResumeLayout(false);
        this.ctrlsTab.ResumeLayout(false);
        this.tabPage1.ResumeLayout(false);
        this.tabPage2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.subWndSplitContainer)).EndInit();
        this.subWndSplitContainer.Panel1.ResumeLayout(false);
        this.subWndSplitContainer.Panel2.ResumeLayout(false);
        this.subWndSplitContainer.ResumeLayout(false);
        this.designTab.ResumeLayout(false);
        this.designPage.ResumeLayout(false);
        this.sourcePage.ResumeLayout(false);
        this.tabControl1.ResumeLayout(false);
        this.tabPage3.ResumeLayout(false);
        this.tabPage4.ResumeLayout(false);
        this.statusStrip1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.designSplitContainer)).EndInit();
        this.designSplitContainer.Panel1.ResumeLayout(false);
        this.designSplitContainer.Panel2.ResumeLayout(false);
        this.designSplitContainer.ResumeLayout(false);
        this.otherCtlPanel.ResumeLayout(false);
        this.ResumeLayout(false);
    } 

    #endregion 

    private System.Windows.Forms.SplitContainer mainWndSplitContainer;
    private System.Windows.Forms.TabControl ctrlsTab;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.ListBox ctrlLstBox;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.TreeView ctrlTreeView;
    private System.Windows.Forms.SplitContainer subWndSplitContainer;
    private System.Windows.Forms.TabControl designTab;
    private System.Windows.Forms.TabPage designPage;
    private System.Windows.Forms.TabPage sourcePage;
    private System.Windows.Forms.TextBox sourceTxtBox;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.TextBox nameTxtBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.PropertyGrid propertyBox;
    private System.Windows.Forms.TabPage tabPage4;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.SplitContainer designSplitContainer;
    private System.Windows.Forms.FlowLayoutPanel otherCtlPanel;
}

// private void ctrlsTab_SelectedIndexChanged(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void ctrlTreeView_AfterSelect(System.Object? sender, System.Windows.Forms.TreeViewEventArgs e)
// {
// 
// }

// private void designTab_SelectedIndexChanged(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void nameTxtBox_TextChanged(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void MainForm_KeyDown(System.Object? sender, System.Windows.Forms.KeyEventArgs e)
// {
//
// }

// private void Form1_KeyUp(System.Object? sender, System.Windows.Forms.KeyEventArgs e)
// {
//
// }

