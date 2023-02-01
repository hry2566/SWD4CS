namespace SWD4CS;

public partial class MainForm : Form
{
    cls_userform? userForm;
    private MenuStrip? menuStrip1; FILE_INFO fileInfo; string sourceFileName = "";
    private ToolStripMenuItem? fileToolStripMenuItem; ToolStripMenuItem? openToolStripMenuItem;
    private ToolStripMenuItem? saveToolStripMenuItem; ToolStripMenuItem? closeToolStripMenuItem;
    private ToolStripMenuItem? editToolStripMenuItem; ToolStripMenuItem? deleteToolStripMenuItem;
    internal ListBox? toolLstBox;
    internal PropertyGrid? propertyGrid;
    internal TextBox? propertyCtrlName;
    internal cls_user_datagridview? eventView;
    internal TreeView? ctrlTree;

    public MainForm()
    {
        InitializeComponent();
        Private2Internal_Controls();
        Init_Manual_Add_Controls();
        cls_controls.AddToolList(ctrlLstBox);
        Run_CommandLine();
    }
    private void Private2Internal_Controls()
    {
        propertyGrid = propertyBox;
        propertyCtrlName = nameTxtBox;
        toolLstBox = ctrlLstBox;
        ctrlTree = ctrlTreeView;
    }

    // *****************************************************************************
    // Event Function
    // *****************************************************************************
    private void deleteToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        userForm!.RemoveSelectedItem();
    }
    private void closeToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        Close();
    }
    private void saveToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        designTab.SelectedIndex = 0;
        designTab.SelectedIndex = 1;
        designTab.SelectedIndex = 0;

        if (sourceFileName != "") { cls_file.SaveAs(sourceFileName, sourceTxtBox.Text); }
        else { cls_file.Save(sourceTxtBox.Text); }
    }
    private void openToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        fileInfo = cls_file.OpenFile();
        if (fileInfo.source_FileName != "")
        {
            sourceFileName = fileInfo.source_FileName;
            userForm!.viewName = fileInfo.formName;
            userForm!.Add_Controls(fileInfo.ctrlInfo);
        }
    }
    private void designTab_SelectedIndexChanged(System.Object? sender, System.EventArgs e)
    {
        switch (designTab.SelectedIndex)
        {
            case 1:
                if (fileInfo.source_base == null) { fileInfo.source_base = cls_file.NewFile(); }
                sourceTxtBox.Text = cls_create_code.Create_SourceCode(fileInfo, userForm!);
                break;
        }
    }
    private void nameTxtBox_TextChanged(System.Object? sender, System.EventArgs e)
    {
        if (propertyGrid!.SelectedObject != null && propertyGrid.SelectedObject is Form == false)
        {
            if (propertyGrid.SelectedObject is Control)
            {
                Control? ctrl = propertyGrid.SelectedObject as Control;
                ctrl!.Name = propertyCtrlName!.Text;
            }
            else
            {
                int index = search_nonCtrl();
                if (index != -1) { userForm!.CtrlItems[index].ctrl!.Name = propertyCtrlName!.Text; }
            }
        }
    }

    private void ctrlsTab_SelectedIndexChanged(System.Object? sender, System.EventArgs e)
    {
        if (ctrlsTab.SelectedIndex == 1) { Show_ControlView(userForm); }
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt && e.KeyCode == Keys.Delete && designTab.SelectedIndex == 0) { userForm!.RemoveSelectedItem(); }
    }

    // *****************************************************************************
    // private Function
    // *****************************************************************************
    private void Run_CommandLine()
    {
        string[] cmds = System.Environment.GetCommandLineArgs();
        if (cmds.Length < 2) { return; }
        fileInfo = cls_file.CommandLine(cmds[1]);
        sourceFileName = fileInfo.source_FileName;
        userForm!.Add_Controls(fileInfo.ctrlInfo);
    }

    private void ctrlTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (ctrlTree!.SelectedNode == null) { return; }
        string ctrlName = ctrlTree.SelectedNode.Text;
        if (ctrlName == "Form")
        {
            userForm!.SelectAllClear();
            userForm.SetSelect(true);
        }
        else
        {
            for (int i = 0; i < userForm!.CtrlItems.Count; i++)
            {
                if (userForm.CtrlItems[i].ctrl!.Name == ctrlName && !userForm.CtrlItems[i].Selected)
                {
                    userForm.SelectAllClear();
                    userForm.CtrlItems[i].Selected = true;
                    break;
                }
            }
        }
        ctrlTree.Focus();
    }

    private void Show_ControlView(cls_userform? form)
    {
        ctrlTreeView.Nodes.Clear();
        TreeNode NodeRoot = new("Form");
        cls_treenode[] itemNode = Array.Empty<cls_treenode>();

        for (int i = 0; i < form!.CtrlItems.Count; i++)
        {
            if (form.CtrlItems[i].ctrl!.Parent == form)
            {
                Array.Resize(ref itemNode, itemNode.Length + 1);
                if (form.CtrlItems[i].className == "SplitContainer")
                {
                    itemNode[itemNode.Length - 1] = new cls_treenode(form.CtrlItems[i].ctrl!.Name + ".Panel1");
                    Array.Resize(ref itemNode, itemNode.Length + 1);
                    itemNode[itemNode.Length - 1] = new cls_treenode(form.CtrlItems[i].ctrl!.Name + ".Panel2");
                }
                else { itemNode[itemNode.Length - 1] = new cls_treenode(form.CtrlItems[i].ctrl!.Name); }
            }
            else
            {
                for (int j = 0; j < itemNode.Length; j++)
                {
                    cls_treenode? retNode;

                    if (form.CtrlItems[i].ctrl!.Parent!.Name.IndexOf(".Panel1") > -1)
                    {
                        retNode = itemNode[j].Search(form.CtrlItems[i].ctrl!.Parent!.Parent!.Name + ".Panel1");
                    }
                    else if (form.CtrlItems[i].ctrl!.Parent!.Name.IndexOf(".Panel2") > -1)
                    {
                        retNode = itemNode[j].Search(form.CtrlItems[i].ctrl!.Parent!.Parent!.Name + ".Panel2");
                    }
                    else { retNode = itemNode[j].Search(form.CtrlItems[i].ctrl!.Parent!.Name); }

                    if (retNode != null)
                    {
                        retNode.Add(form.CtrlItems[i].ctrl!.Name, form.CtrlItems[i].className!);
                        break;
                    }
                }
            }
        }

        if (itemNode.Length > 0) { NodeRoot.Nodes.AddRange(itemNode); }
        ctrlTreeView.Nodes.Add(NodeRoot);
        ctrlTreeView.TopNode.Expand();
    }

    private int search_nonCtrl()
    {
        for (int i = 0; i < userForm!.CtrlItems.Count; i++)
        {
            if (userForm.CtrlItems[i].nonCtrl == propertyGrid!.SelectedObject) { return i; }
        }
        return -1;
    }

    private void Init_Manual_Add_Controls()
    {
        this.userForm = new SWD4CS.cls_userform();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.eventView = new SWD4CS.cls_user_datagridview();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        // 
        // userForm
        // 
        this.userForm.Location = new System.Drawing.Point(16, 16);
        this.userForm.Size = new System.Drawing.Size(480, 400);
        this.userForm.TabIndex = 0;
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
        // evtGridView
        // 
        this.eventView.AllowUserToAddRows = false;
        this.eventView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.eventView.Dock = System.Windows.Forms.DockStyle.Fill;
        this.eventView.Location = new System.Drawing.Point(3, 3);
        this.eventView.Name = "evtGridView";
        this.eventView.RowHeadersWidth = 51;
        this.eventView.RowTemplate.Height = 29;
        this.eventView.Size = new System.Drawing.Size(253, 561);
        this.eventView.TabIndex = 0;
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
        this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
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
        this.deleteToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
        this.deleteToolStripMenuItem.Text = "Delete (Alt + Del)";
        this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);

        this.designSplitContainer.Panel1.Controls.Add(this.userForm);
        this.Controls.Add(this.menuStrip1);
        this.tabPage4.Controls.Add(this.eventView);
        this.designSplitContainer.Panel1.AutoScroll = true;

        this.userForm!.Init(this, designSplitContainer.Panel1, otherCtlPanel);
    }
}
