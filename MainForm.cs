using System.ComponentModel;

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

            for (int i = 0; i < userForm!.CtrlItems.Count; i++)
            {
                if (ReferenceEquals(userForm.CtrlItems[i].ctrl, propertyGrid!.SelectedObject) || ReferenceEquals(userForm.CtrlItems[i].nonCtrl, propertyGrid!.SelectedObject))
                {
                    change_EventsName(userForm.CtrlItems[i].ctrl!.Name, i);
                    userForm.CtrlItems[i].ctrl!.Name = propertyCtrlName!.Text;
                    break;
                }
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
            foreach (cls_controls item in userForm!.CtrlItems)
            {
                if (item.ctrl!.Name == ctrlName && !item.Selected)
                {
                    userForm.SelectAllClear();
                    item.Selected = true;
                    break;
                }
            }
        }
        ctrlTree.Focus();
    }

    private void Show_ControlView(cls_userform? form)
    {
        ctrlTreeView.Nodes.Clear();
        TreeNode NodeRoot = new TreeNode("Form");
        var itemNode = new List<cls_treenode>();

        if (form != null)
        {
            foreach (var formCtrl in form.CtrlItems)
            {
                if (formCtrl.ctrl!.Parent == form)
                {
                    if (formCtrl.className == "SplitContainer")
                    {
                        itemNode.Add(new cls_treenode(formCtrl.ctrl.Name + ".Panel1"));
                        itemNode.Add(new cls_treenode(formCtrl.ctrl.Name + ".Panel2"));
                    }
                    else { itemNode.Add(new cls_treenode(formCtrl.ctrl.Name)); }
                }
                else
                {
                    foreach (var node in itemNode)
                    {
                        cls_treenode? retNode;
                        if (formCtrl.ctrl.Parent.Name.IndexOf(".Panel1") > -1)
                        {
                            retNode = node.Search(formCtrl.ctrl.Parent.Parent.Name + ".Panel1");
                        }
                        else if (formCtrl.ctrl.Parent.Name.IndexOf(".Panel2") > -1)
                        {
                            retNode = node.Search(formCtrl.ctrl.Parent.Parent.Name + ".Panel2");
                        }
                        else { retNode = node.Search(formCtrl.ctrl.Parent.Name); }

                        if (retNode != null) { retNode.Add(formCtrl.ctrl.Name, formCtrl.className!); break; }
                    }
                }
            }
        }

        if (itemNode.Count > 0) { NodeRoot.Nodes.AddRange(itemNode.ToArray()); }
        ctrlTreeView.Nodes.Add(NodeRoot);
        ctrlTreeView.TopNode.Expand();
    }

    private void change_EventsName(string oldName, int index)
    {
        for (int i = 0; i < userForm!.CtrlItems[index].decHandler.Count; i++)
        {
            string newHandler = userForm!.CtrlItems[index].decHandler[i].Replace("." + oldName + ".", "." + propertyCtrlName!.Text + ".");
            newHandler = newHandler.Replace("(" + oldName + "_", "(" + propertyCtrlName!.Text + "_");
            userForm!.CtrlItems[index].decHandler[i] = newHandler;

            string newFunc = userForm!.CtrlItems[index].decFunc[i].Replace(" " + oldName + "_", " " + propertyCtrlName!.Text + "_");
            userForm!.CtrlItems[index].decFunc[i] = newFunc;
        }
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
