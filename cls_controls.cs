
using System.ComponentModel;

namespace SWD4CS
{
    internal class cls_controls
    {
        internal Control? ctrl;
        internal string? className;
        internal Component? nonCtrl = new();
        private cls_userform? form; FlowLayoutPanel? otherCtrlPanel; cls_selectbox? selectBox;
        private bool selectFlag = false; bool changeFlag = true;
        private Point memPos; int grid = 4;
        internal List<string> decHandler = new();
        internal List<string> decFunc = new();

        public cls_controls(cls_userform form, FlowLayoutPanel? otherCtrlPanel, string className, Control parent, int X, int Y, bool fileFlag = false)
        {
            this.form = form;
            this.otherCtrlPanel = otherCtrlPanel;

            if ((className == "TabPage" && parent == form) || (parent is StatusStrip)) { return; }
            if (!AddCtrl_Init(className)) { return; }

            this.className = className;
            this.ctrl!.Location = new System.Drawing.Point(X, Y);
            this.form.CtrlItems!.Add(this);

            if (this.nonCtrl.GetType() == typeof(Component))
            {
                parent.Controls.Add(this.ctrl);
                CreateSelectBox(parent);
            }
            else
            {
                otherCtrlPanel!.Controls.Add(this.ctrl);
                Selected = true;
            }

            if (this.ctrl is TabControl && !fileFlag)
            {
                _ = new cls_controls(form, otherCtrlPanel, "TabPage", this.ctrl!, 0, 0);
                _ = new cls_controls(form, otherCtrlPanel, "TabPage", this.ctrl!, 0, 0);
            }

            if (this.ctrl is TabPage) { CreateSelectBox(this.ctrl); }

            ctrl!.Click += Ctrl_Click;
            ctrl.MouseMove += ControlMouseMove;
            ctrl.MouseDown += ControlMouseDown;
        }


        // ********************************************************************************************
        // events Function 
        // ********************************************************************************************
        private void ControlMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Selected)
            {
                memPos.X = (int)(e.X / grid) * grid;
                memPos.Y = (int)(e.Y / grid) * grid;
            }
        }

        private void ControlMouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || !Selected) { changeFlag = true; return; }

            Point pos = new Point((int)(e.X / grid) * grid, (int)(e.Y / grid) * grid);
            Point newPos = new Point(pos.X - memPos.X + ctrl!.Location.X, pos.Y - memPos.Y + ctrl.Location.Y);
            ctrl.Location = newPos;
            Selected = true;
            changeFlag = false;
        }

        private void Ctrl_Click(object? sender, EventArgs e)
        {
            if (e.ToString() != "System.EventArgs")
            {
                MouseEventArgs me = (MouseEventArgs)e;
                if (form!.mainForm!.toolLstBox!.Text == "")
                {
                    SetSelected(me);
                    foreach (TreeNode n in form.mainForm.ctrlTree!.Nodes)
                    {
                        TreeNode ret = FindNode(n, this.ctrl!.Name);
                        if (ret != null) { form!.mainForm.ctrlTree.SelectedNode = ret; break; }
                    }
                }
                else
                {
                    if (this.nonCtrl!.GetType() == typeof(Component)) { AddControls(me); }
                }
            }
        }

        private void SplitContainerPanelClick(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            SplitterPanel? panel = sender as SplitterPanel;

            if (e.ToString() == "System.EventArgs") { return; }
            if (form!.mainForm!.toolLstBox!.Text == "") { SetSelected(me); }
            else { AddControls(me, panel); }
        }

        // ********************************************************************************************
        // internal Function 
        // ********************************************************************************************
        internal bool Selected
        {
            set
            {
                selectFlag = value;
                if (selectBox != null) { selectBox!.SetSelectBoxPos(value); }
                else
                {
                    if (value) { this.ctrl!.BackColor = System.Drawing.SystemColors.ActiveCaption; }
                    else { this.ctrl!.BackColor = System.Drawing.Color.Transparent; }
                }
                ShowProperty(value);
                form!.mainForm!.eventView!.ShowEventList(value, this);
            }
            get { return selectFlag; }
        }

        internal void Delete()
        {
            Selected = false;
            ctrl!.Parent!.Controls.Remove(ctrl);
        }

        internal static string SetFormProperty(Control? cls_ctrl, string? propertyName, string? propertyValue)
        {
            return cls_properties.SetProperty(cls_ctrl!, propertyName!, propertyValue!);
        }

        internal static string SetCtrlProperty(cls_controls? cls_ctrl, string? propertyName, string? propertyValue)
        {
            Component? ctrl = cls_ctrl!.nonCtrl!.GetType() == typeof(Component) ? cls_ctrl!.ctrl : cls_ctrl!.nonCtrl;
            string ret = cls_properties.SetProperty(ctrl!, propertyName!, propertyValue!);
            return ret;
        }

        // ********************************************************************************************
        // private Function 
        // ********************************************************************************************
        private TreeNode FindNode(TreeNode treeNode, string ctrlName)
        {
            if (ctrlName == treeNode.Text) { return treeNode; }

            TreeNode ret;
            foreach (TreeNode tn in treeNode.Nodes)
            {
                ret = FindNode(tn, ctrlName);
                if (ret != null) { return ret; }
            }
            return null!;
        }

        private void CreateSelectBox(Control parent)
        {
            Control? ctl = this.ctrl!;
            bool isTabPage = this.ctrl is TabPage;
            if (!isTabPage)
            {
                bool isFlowOrTable = (parent is FlowLayoutPanel) || (parent is TableLayoutPanel);
                ctl = isFlowOrTable ? this.ctrl : parent;
            }
            if (ctl is TabControl || ctl is SplitContainer || ctl is StatusStrip || ctl is FlowLayoutPanel || ctl is TableLayoutPanel) { return; }
            selectBox = new cls_selectbox(this, ctl!);
            Selected = !isTabPage;
        }

        private void SetSelected(MouseEventArgs mouseEvent)
        {
            if (mouseEvent.Button != MouseButtons.Left) { return; }
            bool isControlPressed = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            // if (Selected && changeFlag) { Selected = false; }
            if (Selected && changeFlag) { Selected = false; }
            else if (!isControlPressed) { form!.SelectAllClear(); Selected = true; }
            else { Selected = true; }
        }

        private void ShowProperty(bool flag)
        {
            if (flag)
            {
                Component? Comp = this.nonCtrl!.GetType() == typeof(Component) ? Comp = this.ctrl : this.nonCtrl;
                form!.mainForm!.propertyGrid!.SelectedObject = Comp;
                form.mainForm.propertyCtrlName!.Text = this.ctrl!.Name;
            }
            else
            {
                form!.mainForm!.propertyGrid!.SelectedObject = null;
                form.mainForm!.propertyCtrlName!.Text = "";
            }
        }

        private void CreateTrancePanel(Control ctrl)
        {
            cls_transparent_panel trancepanel = new();
            trancepanel.Dock = DockStyle.Fill;
            trancepanel.BackColor = Color.FromArgb(0, 0, 0, 0);
            trancepanel.Click += new System.EventHandler(Ctrl_Click);
            trancepanel.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
            trancepanel.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
            ctrl.Controls.Add(trancepanel);
            trancepanel.BringToFront();
            trancepanel.Invalidate();
        }

        private void CreatePickBox(Control ctrl)
        {
            Button pickbox = new();
            pickbox.Size = new System.Drawing.Size(24, 24);
            pickbox.Text = "▼";
            pickbox.Click += new System.EventHandler(Ctrl_Click);
            pickbox.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
            pickbox.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
            ctrl.Controls.Add(pickbox);
        }

        private void AddControls(MouseEventArgs mouseEvent, SplitterPanel? splitPanel = null)
        {
            int x = (int)(mouseEvent.X / grid) * grid;
            int y = (int)(mouseEvent.Y / grid) * grid;
            form!.SelectAllClear();

            bool isTabControl = this.ctrl is TabControl;
            bool isTabPageSelected = form!.mainForm!.toolLstBox!.Text == "TabPage";

            if ((isTabControl && isTabPageSelected) || (!isTabControl && !isTabPageSelected))
            {
                Control? parentPanel = splitPanel == null ? this.ctrl : splitPanel;
                _ = new cls_controls(form, otherCtrlPanel, form!.mainForm!.toolLstBox!.Text, parentPanel!, x, y);
            }

            form!.mainForm!.toolLstBox!.SelectedIndex = 0;
        }

        // ********************************************************************************************
        // コントロール追加時に編集する箇所
        // ********************************************************************************************
        private bool AddCtrl_Init(string className)
        {
            switch (className)
            {
                case "Button":
                    this.ctrl = new Button();
                    break;
                case "Label":
                    this.ctrl = new Label();
                    this.ctrl!.AutoSize = true;
                    break;
                case "GroupBox":
                    this.ctrl = new GroupBox();
                    break;
                case "TextBox":
                    this.ctrl = new TextBox();
                    break;
                case "ListBox":
                    this.ctrl = new ListBox();
                    ListBox? listbox = this.ctrl as ListBox;
                    listbox!.Items.Add("ListBox");
                    break;
                case "TabControl":
                    this.ctrl = new TabControl();
                    break;
                case "TabPage":
                    this.ctrl = new TabPage();
                    break;
                case "CheckBox":
                    this.ctrl = new CheckBox();
                    this.ctrl!.AutoSize = true;
                    break;
                case "ComboBox":
                    this.ctrl = new ComboBox();
                    break;
                case "SplitContainer":
                    this.ctrl = new SplitContainer();
                    this.ctrl.Size = new System.Drawing.Size(250, 125);
                    SplitContainer? splitcontainer = this.ctrl as SplitContainer;
                    splitcontainer!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    splitcontainer.Panel1.Name = this.ctrl.Name + ".Panel1";
                    splitcontainer.Panel1.Click += new System.EventHandler(this.SplitContainerPanelClick);
                    splitcontainer.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
                    splitcontainer.Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
                    splitcontainer.Panel2.Name = this.ctrl.Name + ".Panel2";
                    splitcontainer.Panel2.Click += new System.EventHandler(this.SplitContainerPanelClick);
                    splitcontainer.Panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
                    splitcontainer.Panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
                    break;
                case "DataGridView":
                    this.ctrl = new DataGridView();
                    break;
                case "Panel":
                    this.ctrl = new Panel();
                    Panel? panel = this.ctrl as Panel;
                    panel!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    break;
                case "CheckedListBox":
                    this.ctrl = new CheckedListBox();
                    CheckedListBox? checkedlistbox = this.ctrl as CheckedListBox;
                    checkedlistbox!.Items.Add("CheckedListBox");
                    break;
                case "LinkLabel":
                    this.ctrl = new LinkLabel();
                    this.ctrl!.AutoSize = true;
                    break;
                case "PictureBox":
                    this.ctrl = new PictureBox();
                    PictureBox? picbox = this.ctrl as PictureBox;
                    picbox!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    break;
                case "ProgressBar":
                    this.ctrl = new ProgressBar();
                    ProgressBar? prgressbar = this.ctrl as ProgressBar;
                    prgressbar!.Value = 50;
                    break;
                case "RadioButton":
                    this.ctrl = new RadioButton();
                    this.ctrl!.AutoSize = true;
                    break;
                case "RichTextBox":
                    this.ctrl = new RichTextBox();
                    break;
                case "StatusStrip":
                    this.ctrl = new StatusStrip();
                    break;
                case "HScrollBar":
                    this.ctrl = new HScrollBar();
                    CreateTrancePanel(this.ctrl);
                    break;
                case "VScrollBar":
                    this.ctrl = new VScrollBar();
                    CreateTrancePanel(this.ctrl);
                    break;
                case "MonthCalendar":
                    this.ctrl = new MonthCalendar();
                    CreateTrancePanel(this.ctrl);
                    break;
                case "ListView":
                    this.ctrl = new ListView();
                    CreateTrancePanel(this.ctrl);
                    break;
                case "TreeView":
                    this.ctrl = new TreeView();
                    CreateTrancePanel(this.ctrl);
                    break;
                case "MaskedTextBox":
                    this.ctrl = new MaskedTextBox();
                    break;
                case "PropertyGrid":
                    this.ctrl = new PropertyGrid();
                    CreateTrancePanel(this.ctrl);
                    break;
                case "DateTimePicker":
                    this.ctrl = new DateTimePicker();
                    CreatePickBox(this.ctrl);
                    break;
                case "DomainUpDown":
                    this.ctrl = new DomainUpDown();
                    break;
                case "FlowLayoutPanel":
                    this.ctrl = new FlowLayoutPanel();
                    FlowLayoutPanel? flwlayoutpnl = this.ctrl as FlowLayoutPanel;
                    flwlayoutpnl!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    break;
                case "Splitter":
                    this.ctrl = new Splitter();
                    Splitter? splitter = this.ctrl as Splitter;
                    splitter!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    break;
                case "TableLayoutPanel":
                    this.ctrl = new TableLayoutPanel();
                    TableLayoutPanel? tbllaypnl = this.ctrl as TableLayoutPanel;
                    tbllaypnl!.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
                    break;
                case "TrackBar":
                    this.ctrl = new TrackBar();
                    break;
                case "Timer":
                    this.nonCtrl = new System.Windows.Forms.Timer();
                    break;
                case "ColorDialog":
                    this.nonCtrl = new System.Windows.Forms.ColorDialog();
                    break;
                case "OpenFileDialog":
                    this.nonCtrl = new System.Windows.Forms.OpenFileDialog();
                    break;
                case "FontDialog":
                    this.nonCtrl = new System.Windows.Forms.FontDialog();
                    break;
                case "FolderBrowserDialog":
                    this.nonCtrl = new System.Windows.Forms.FolderBrowserDialog();
                    break;
                case "SaveFileDialog":
                    this.nonCtrl = new System.Windows.Forms.SaveFileDialog();
                    break;
                case "ImageList":
                    this.nonCtrl = new System.Windows.Forms.ImageList();
                    break;
                default:
                    return false;
            }

            if (this.ctrl == null)
            {
                Label lbl = new Label();
                lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.ctrl = lbl;
                this.ctrl.AutoSize = true;
            }

            this.ctrl.Name = className + form!.cnt_Control;
            if (className != "DateTimePicker") { this.ctrl!.Text = this.ctrl!.Name; }
            this.ctrl!.TabIndex = form!.cnt_Control;
            form.cnt_Control++;
            return true;
        }

        internal static void AddToolList(ListBox ctrlLstBox)
        {
            ctrlLstBox.Items.Add("");
            ctrlLstBox.Items.Add("Button");
            ctrlLstBox.Items.Add("CheckBox");
            ctrlLstBox.Items.Add("CheckedListBox");
            ctrlLstBox.Items.Add("ComboBox");
            ctrlLstBox.Items.Add("DataGridView");
            ctrlLstBox.Items.Add("DateTimePicker");
            ctrlLstBox.Items.Add("DomainUpDown");
            ctrlLstBox.Items.Add("FlowLayoutPanel");
            ctrlLstBox.Items.Add("GroupBox");
            ctrlLstBox.Items.Add("HScrollBar");
            ctrlLstBox.Items.Add("Label");
            ctrlLstBox.Items.Add("LinkLabel");
            ctrlLstBox.Items.Add("ListBox");
            ctrlLstBox.Items.Add("ListView");
            ctrlLstBox.Items.Add("MaskedTextBox");
            ctrlLstBox.Items.Add("MonthCalendar");
            ctrlLstBox.Items.Add("Panel");
            ctrlLstBox.Items.Add("PictureBox");
            ctrlLstBox.Items.Add("ProgressBar");
            ctrlLstBox.Items.Add("PropertyGrid");
            ctrlLstBox.Items.Add("RadioButton");
            ctrlLstBox.Items.Add("RichTextBox");
            ctrlLstBox.Items.Add("SplitContainer");
            ctrlLstBox.Items.Add("Splitter");
            ctrlLstBox.Items.Add("StatusStrip");
            ctrlLstBox.Items.Add("TabControl");
            ctrlLstBox.Items.Add("TableLayoutPanel");
            ctrlLstBox.Items.Add("TabPage");
            ctrlLstBox.Items.Add("TextBox");
            ctrlLstBox.Items.Add("TrackBar");
            ctrlLstBox.Items.Add("TreeView");
            ctrlLstBox.Items.Add("VScrollBar");
            ctrlLstBox.Items.Add("");
            ctrlLstBox.Items.Add("ColorDialog");
            ctrlLstBox.Items.Add("FolderBrowserDialog");
            ctrlLstBox.Items.Add("FontDialog");
            ctrlLstBox.Items.Add("ImageList");
            ctrlLstBox.Items.Add("OpenFileDialog");
            ctrlLstBox.Items.Add("SaveFileDialog");
            ctrlLstBox.Items.Add("Timer");

            ctrlLstBox.SelectedIndex = 0;
        }
    }
}
