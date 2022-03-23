using System.Reflection;

namespace SWD4CS
{
    public partial class MainForm : Form
    {
        private FILE_INFO fileInfo;
        internal PropertyGrid propertyGrid;
        internal TextBox propertyCtrlName;
        internal ListBox toolLstBox;
        internal TreeView ctrlTree;
        private string sourceFileName = "";


        public MainForm()
        {
            InitializeComponent();
            cls_controls.AddToolList(ctrlLstBox);

            propertyGrid = propertyBox;
            propertyCtrlName = nameTxtBox;
            toolLstBox = ctrlLstBox;
            this.ctrlTree = ctrlTreeView;
            userForm.Init(this, designePage);

            cls_file.ReadIni(this, "SWD4CS.ini", mainWndSplitContainer, subWndSplitContainer);
            RunCommandLine();

            Application.ApplicationExit += new EventHandler(AppExit);
        }
        private void AppExit(object? sender, EventArgs e)
        {
            cls_file.WriteIni(this, "SWD4CS.ini", mainWndSplitContainer, subWndSplitContainer);
        }
        private void RunCommandLine()
        {
            string[] cmds = System.Environment.GetCommandLineArgs();
            if (cmds.Length < 2) { return; }
            fileInfo = cls_file.CommandLine(cmds[1]);
            sourceFileName = fileInfo.source_FileName;
            logTxtBox.Text = "";
            userForm.AddControl(fileInfo.ctrlInfo);
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileInfo = cls_file.OpenFile();
            sourceFileName = fileInfo.source_FileName;
            logTxtBox.Text = "";
            userForm.AddControl(fileInfo.ctrlInfo);
        }
        internal void AddLog(string log)
        {
            if (logTxtBox.Text == "")
            {
                logTxtBox.Text = log;
            }
            else
            {
                logTxtBox.Text += "\r\n" + log;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.Delete && designeTab.SelectedIndex == 0)
            {
                userForm.RemoveSelectedItem();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            designeTab.SelectedIndex = 1;
            designeTab.SelectedIndex = 0;

            if (sourceFileName != "")
            {
                cls_file.SaveAs(sourceFileName, sourceTxtBox.Text);
            }
            else
            {
                cls_file.Save(sourceTxtBox.Text);
            }

            Close();
        }

        private void designeTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (designeTab.SelectedIndex)
            {
                case 1:
                    if (fileInfo.source_base == null)
                    {
                        fileInfo.source_base = cls_file.NewFile();
                    }
                    sourceTxtBox.Text = CreateSourcecCode();
                    break;
                case 2:
                    //textBox2.Text = CreateEventCode();
                    break;
            }
        }
        private string CreateSourcecCode()
        {
            string source = "";
            List<string> lstSuspend = new();
            List<string> lstResume = new();
            string space = "";

            if (fileInfo.source_base[0].IndexOf(";") == -1)
            {
                space = space.PadLeft(8);
            }
            else
            {
                space = space.PadLeft(4);
            }

            source = CreateCode_Instance(source, space);
            source = CreateCode_Suspend_Resume(source, lstSuspend, lstResume, space);

            // suspend
            for (int i = 0; i < lstSuspend.Count; i++)
            {
                source += lstSuspend[i];
            }

            source = CreateCode_Property(source, space);
            source = CreateCode_FormProperty(source, space);
            source = CreateCode_FormAddControl(source, space);

            // resume
            for (int i = 0; i < lstResume.Count; i++)
            {
                source += lstResume[i];
            }

            source = CreateCode_Declaration(source, space);

            if (fileInfo.source_base[0].IndexOf(";") == -1)
            {
                source += "    }\r\n";
            }
            source += "}\r\n";
            source += "\r\n";

            return source;
        }

        private string CreateCode_Declaration(string source, string space)
        {
            source += space + "}\r\n";
            source += "\r\n";
            source += space + "#endregion\r\n";
            source += "\r\n";

            // declaration
            for (int i = 0; i < userForm.CtrlItems.Count; i++)
            {
                string[] split = userForm.CtrlItems[i].ctrl!.GetType().ToString().Split(".");
                string dec = split[split.Length - 1];
                source += space + "private " + dec + " " + userForm.CtrlItems[i].ctrl!.Name + ";\r\n";

            }
            return source;
        }

        private string CreateCode_FormAddControl(string source, string space)
        {
            // AddControl
            for (int i = 0; i < userForm.CtrlItems.Count; i++)
            {
                if (userForm.CtrlItems[i].ctrl!.Parent == userForm)
                {
                    source += space + "    this.Controls.Add(this." + userForm.CtrlItems[i].ctrl!.Name + ");\r\n";
                }
            }
            return source;
        }

        private string CreateCode_FormProperty(string source, string space)
        {
            // form-property
            source += space + "    //\r\n";
            source += space + "    // form\r\n";
            source += space + "    //\r\n";


            foreach (PropertyInfo item in userForm!.memForm.GetType().GetProperties())
            {
                if (cls_controls.HideProperty(item.Name))
                {
                    Control? baseForm = new Form();
                    Control memForm = userForm.memForm as Control;

                    if (item.GetValue(memForm) != null && item.GetValue(baseForm) != null)
                    {
                        if (item.GetValue(memForm)!.ToString() != item.GetValue(baseForm)!.ToString())
                        {
                            string str1 = space + "    this." + item.Name;
                            string strProperty = cls_controls.Property2String(memForm, item);

                            if (strProperty != "")
                            {
                                source += str1 + strProperty + "\r\n";
                            }
                        }
                    }
                }
            }
            return source;
        }

        private string CreateCode_Property(string source, string space)
        {
            // Property
            for (int i = 0; i < userForm.CtrlItems.Count; i++)
            {
                string memCode = "";
                source += space + "    //\r\n";
                source += space + "    // " + userForm.CtrlItems[i].ctrl!.Name + "\r\n";
                source += space + "    //\r\n";

                source = CreateCode_AddControl(source, space, i);

                // Property
                foreach (PropertyInfo item in userForm.CtrlItems[i].ctrl!.GetType().GetProperties())
                {
                    if (cls_controls.HideProperty(item.Name))
                    {
                        Control? baseCtrl = userForm.CtrlItems[i].GetBaseCtrl();
                        if (item.GetValue(userForm.CtrlItems[i].ctrl) != null && item.GetValue(baseCtrl) != null)
                        {
                            if (item.GetValue(userForm.CtrlItems[i].ctrl)!.ToString() != item.GetValue(baseCtrl)!.ToString())
                            {
                                string str1 = space + "    this." + userForm.CtrlItems[i]!.ctrl!.Name + "." + item.Name;
                                string strProperty = cls_controls.Property2String(userForm.CtrlItems[i].ctrl!, item);
                                if (strProperty != "")
                                {
                                    if (item.Name != "SplitterDistance" && item.Name != "Anchor")
                                    {
                                        source += str1 + strProperty + "\r\n";
                                    }
                                    else
                                    {
                                        memCode += str1 + strProperty + "\r\n";
                                    }
                                }
                            }
                        }
                    }
                }
                if (memCode != "")
                {
                    source += memCode;
                }
            }
            return source;
        }

        private string CreateCode_AddControl(string source, string space, int i)
        {
            // AddControl
            for (int j = 0; j < userForm.CtrlItems.Count; j++)
            {
                if (userForm.CtrlItems[i].ctrl!.Name == userForm.CtrlItems[j].ctrl!.Parent.Name)
                {
                    source += space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Controls.Add(this." + userForm.CtrlItems[j].ctrl!.Name + ");\r\n";
                }
                else if (userForm.CtrlItems[i].ctrl!.Name == userForm.CtrlItems[j].ctrl!.Parent.Parent.Name)
                {
                    if (userForm.CtrlItems[j].ctrl!.Parent.Name.IndexOf("Panel1") > -1)
                    {
                        source += space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel1.Controls.Add(this." + userForm.CtrlItems[j].ctrl!.Name + ");\r\n";
                    }
                    else if (userForm.CtrlItems[j].ctrl!.Parent.Name.IndexOf("Panel2") > -1)
                    {
                        source += space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel2.Controls.Add(this." + userForm.CtrlItems[j].ctrl!.Name + ");\r\n";
                    }
                }
            }
            return source;
        }

        private string CreateCode_Suspend_Resume(string source, List<string> lstSuspend, List<string> lstResume, string space)
        {
            // Suspend & resume
            for (int i = 0; i < userForm.CtrlItems.Count; i++)
            {
                source += space + "    this." + userForm.CtrlItems[i].ctrl!.Name + " = new System.Windows.Forms." + userForm.CtrlItems[i].className + "();\r\n";
                if (userForm.CtrlItems[i].className == "DataGridView" ||
                    userForm.CtrlItems[i].className == "PictureBox" ||
                    userForm.CtrlItems[i].className == "SplitContainer")
                {
                    lstSuspend.Add(space + "    ((System.ComponentModel.ISupportInitialize)(this." + userForm.CtrlItems[i].ctrl!.Name + ")).BeginInit();\r\n");
                    lstResume.Add(space + "    ((System.ComponentModel.ISupportInitialize)(this." + userForm.CtrlItems[i].ctrl!.Name + ")).EndInit();\r\n");
                }
                if (userForm.CtrlItems[i].className == "GroupBox" ||
                         userForm.CtrlItems[i].className == "Panel" ||
                         userForm.CtrlItems[i].className == "StatusStrip" ||
                         userForm.CtrlItems[i].className == "TabControl" ||
                         userForm.CtrlItems[i].className == "TabPage")
                {
                    lstSuspend.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".SuspendLayout();\r\n");
                    lstResume.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".ResumeLayout(false);\r\n");
                }
                if (userForm.CtrlItems[i].className == "SplitContainer")
                {
                    lstSuspend.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel1.SuspendLayout();\r\n");
                    lstSuspend.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel2.SuspendLayout();\r\n");
                    lstSuspend.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".SuspendLayout();\r\n");
                    lstResume.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel1.ResumeLayout(false);\r\n");
                    lstResume.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel2.ResumeLayout(false);\r\n");
                    lstResume.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".ResumeLayout(false);\r\n");
                }
            }
            lstSuspend.Add(space + "    this.SuspendLayout();\r\n");
            lstResume.Add(space + "    this.ResumeLayout(false);\r\n");

            return source;
        }

        private string CreateCode_Instance(string source, string space)
        {
            // Instance
            for (int i = 0; i < fileInfo.source_base.Count; i++)
            {
                source += fileInfo.source_base[i] + "\r\n";
            }
            source += space + "{\r\n";

            return source;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userForm.RemoveSelectedItem();
        }

        private void nameTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (propertyGrid!.SelectedObject != null && propertyGrid.SelectedObject is Form == false)
            {
                Control? ctrl = propertyGrid.SelectedObject as Control;
                ctrl!.Name = propertyCtrlName!.Text;
            }
        }

        private void ctrlsTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ctrlsTab.SelectedIndex == 1)
            {
                ControlViewShow(userForm);
            }
        }
        private void ControlViewShow(cls_userform form)
        {
            ctrlTreeView.Nodes.Clear();
            TreeNode NodeRoot = new("Form");
            cls_treenode[] itemNode = Array.Empty<cls_treenode>();

            for (int i = 0; i < form.CtrlItems.Count; i++)
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
                    else
                    {
                        itemNode[itemNode.Length - 1] = new cls_treenode(form.CtrlItems[i].ctrl!.Name);
                    }
                }
                else
                {
                    for (int j = 0; j < itemNode.Length; j++)
                    {
                        cls_treenode? retNode = itemNode[j].Search(form.CtrlItems[i].ctrl!.Parent.Name);
                        if (retNode != null)
                        {
                            retNode.Add(form.CtrlItems[i].ctrl!.Name, form.CtrlItems[i].className!);
                            break;
                        }
                    }
                }
            }

            if (itemNode.Length > 0)
            {
                NodeRoot.Nodes.AddRange(itemNode);
            }

            ctrlTreeView.Nodes.Add(NodeRoot);
            ctrlTreeView.TopNode.Expand();
        }

        private void ctrlTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (ctrlTree.SelectedNode == null) { return; }
            string ctrlName = ctrlTree.SelectedNode.Text;

            if (ctrlName == "Form")
            {
                userForm.SelectAllClear();
                userForm.SetSelect(true);
            }
            else
            {
                for (int i = 0; i < userForm.CtrlItems.Count; i++)
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
    }
}