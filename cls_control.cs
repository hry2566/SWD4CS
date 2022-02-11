using System.Data;

namespace SWD4CS
{
    internal class cls_control
    {
        private cls_user_form form;
        internal string className;
        internal Control? ctrl;
        internal Control parent;
        private Control backPanel;
        private cls_selectbox? selectBox;
        private ListBox? toolList;
        private PropertyGrid propertyGrid;
        private TextBox? propertyCtrlName;
        private bool selectFlag;
        private bool changeFlag;
        private Point memPos;
        private int grid = 8;

        public cls_control(cls_user_form form, string className, Control parent, Control backPanel, ListBox? toolList, PropertyGrid propertyGrid, TextBox propertyCtrlName, int X, int Y)
        {
            this.form = form;
            this.className = className;
            this.parent = parent;
            this.backPanel = backPanel;
            this.toolList = toolList;
            this.propertyGrid = propertyGrid;
            this.propertyCtrlName = propertyCtrlName;

            if (Init(className))
            {
                if ((className == "TabPage" && parent == form) || (parent is StatusStrip))
                {
                    return;
                }

                this.ctrl!.Location = new System.Drawing.Point(X, Y);
                this.form.CtrlItems!.Add(this);
                parent.Controls.Add(this.form.CtrlItems[this.form.CtrlItems.Count - 1].ctrl);

                if (this.ctrl is TabControl)
                {
                    _ = new cls_control(form, "TabPage", this.ctrl!, backPanel!, toolList, propertyGrid!, propertyCtrlName, X, Y);
                    _ = new cls_control(form, "TabPage", this.ctrl!, backPanel!, toolList, propertyGrid!, propertyCtrlName, X, Y);
                }

                if (this.ctrl is TabPage)
                {
                    selectBox = new cls_selectbox(this, this.ctrl);
                    Selected = false;
                }
                else
                {
                    selectBox = new cls_selectbox(this, parent);
                    Selected = true;
                }

                this.ctrl!.Click += new System.EventHandler(Ctrl_Click);
                this.ctrl.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
                this.ctrl.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
                backPanel.Click += new System.EventHandler(Backpanel_Click);
            }
        }

        private void Backpanel_Click(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                if (Selected)
                {
                    Selected = false;
                }
            }
        }

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
            if (e.Button == MouseButtons.Left && Selected)
            {
                Point pos = new((int)(e.X / grid) * grid, (int)(e.Y / grid) * grid);
                Point newPos = new(pos.X - memPos.X + ctrl!.Location.X, pos.Y - memPos.Y + ctrl.Location.Y);

                ctrl.Location = newPos;
                Selected = true;
                changeFlag = false;
            }
            else
            {
                changeFlag = true;
            }
        }

        private void Ctrl_Click(object? sender, EventArgs e)
        {
            if (e.ToString() != "System.EventArgs")
            {
                MouseEventArgs me = (MouseEventArgs)e;

                if (toolList!.Text == "")
                {
                    SetSelected(me);
                }
                else
                {
                    AddControls(me);
                }
            }
        }

        private void AddControls(MouseEventArgs me, SplitterPanel? splitpanel = null)
        {
            int X = (int)(me.X / grid) * grid;
            int Y = (int)(me.Y / grid) * grid;

            form.SelectAllClear();

            if ((this.ctrl is TabControl && toolList!.Text == "TabPage") || (this.ctrl is TabControl == false && toolList!.Text != "TabPage"))
            {
                if (splitpanel == null)
                {
                    _ = new cls_control(form, toolList!.Text, this.ctrl!, backPanel!, toolList, propertyGrid!, propertyCtrlName!, X, Y);
                }
                else
                {
                    _ = new cls_control(form, toolList!.Text, splitpanel!, backPanel!, toolList, propertyGrid!, propertyCtrlName!, X, Y);
                }
            }
            toolList!.SelectedIndex = -1;
        }

        private void SetSelected(MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left)
            {
                if (Selected && changeFlag)
                {
                    Selected = false;
                }
                else
                {
                    if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                    {
                        form.SelectAllClear();
                    }
                    Selected = true;
                }
            }
        }

        internal void Delete()
        {
            Selected = false;
            parent.Controls.Remove(ctrl);
        }

        internal bool Selected
        {
            set
            {
                if (selectBox != null)
                {
                    selectFlag = value;
                    selectBox!.SetSelectBoxPos(value);
                }
                ShowProperty(value);
            }
            get
            {
                return selectFlag;
            }
        }

        private void ShowProperty(bool flag)
        {
            if (flag)
            {
                propertyGrid!.SelectedObject = this.ctrl;
                propertyCtrlName!.Text = this.ctrl!.Name;
            }
            else
            {
                propertyGrid!.SelectedObject = null;
                propertyCtrlName!.Text = "";
            }
        }

        internal static bool HideProperty(string itemName)
        {
            if (itemName != "AccessibilityObject" &&
                itemName != "BindingContext" &&
                itemName != "Parent" &&
                itemName != "TopLevelControl" &&
                itemName != "DataSource" &&
                itemName != "FirstDisplayedCell" &&
                itemName != "Item" &&
                itemName != "TopItem" &&
                itemName != "Rtf" &&
                itemName != "ParentForm" &&
                itemName != "SelectedTab" &&
                itemName != "Top" &&
                itemName != "Left" &&
                itemName != "Right" &&
                itemName != "Bottom" &&
                itemName != "Width" &&
                itemName != "Height" &&
                itemName != "CanSelect" &&
                itemName != "Created" &&
                itemName != "IsHandleCreated" &&
                itemName != "PreferredSize" &&
                itemName != "Visible" &&
                itemName != "Enable" &&
                itemName != "ClientSize" &&
                itemName != "UseVisualStyleBackColor" &&
                itemName != "PreferredHeight" &&
                itemName != "ColumnCount" &&
                itemName != "FirstDisplayedScrollingColumnIndex" &&
                itemName != "FirstDisplayedScrollingRowIndex" &&
                itemName != "NewRowIndex" &&
                itemName != "RowCount" &&
                itemName != "HasChildren" &&
                itemName != "PreferredWidth" &&
                itemName != "SingleMonthSize" &&
                itemName != "TextLength" &&
                itemName != "SelectedIndex" &&
                itemName != "TabCount" &&
                itemName != "VisibleCount" &&
                itemName != "DesktopLocation" &&
                itemName != "AutoScale" &&
                itemName != "CanFocus" &&
                itemName != "IsMirrored" &&
                itemName != "SelectionStart" &&
                itemName != "" &&
                itemName != "" &&
                itemName != "" &&
                itemName != "" &&
                itemName != "" &&
                itemName != "" &&
                itemName != "" &&
                itemName != ""
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SplitContainerPanelClick(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            SplitterPanel? panel = sender as SplitterPanel;

            if (e.ToString() == "System.EventArgs")
            {
                return;
            }

            if (toolList!.Text == "")
            {
                SetSelected(me);
            }
            else
            {
                AddControls(me, panel);
            }
        }

        private void CreatePickBox(Control ctrl)
        {
            Button pickbox = new();
            pickbox = new();
            pickbox.Size = new System.Drawing.Size(24, 24);
            pickbox.Text = "▼";
            pickbox.Click += new System.EventHandler(Ctrl_Click);
            pickbox.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
            pickbox.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
            ctrl.Controls.Add(pickbox);
        }


        private bool Init(string className)
        {
            form.cnt_Control++;

            // ****************************************************************************************
            // コントロール追加時に下記を編集すること
            // ****************************************************************************************
            switch (className)
            {
                case "Button":
                    this.ctrl = new Button();
                    this.ctrl.Size = new System.Drawing.Size(96, 32);
                    this.ctrl!.Name = className + form.cnt_Button;
                    form.cnt_Button++;
                    break;
                case "Label":
                    this.ctrl = new Label();
                    this.ctrl!.Name = className + form.cnt_Label;
                    this.ctrl!.AutoSize = true;
                    form.cnt_Label++;
                    break;
                case "GroupBox":
                    this.ctrl = new GroupBox();
                    this.ctrl.Size = new System.Drawing.Size(250, 125);
                    this.ctrl!.Name = className + form.cnt_GroupBox;
                    form.cnt_GroupBox++;
                    break;
                case "TextBox":
                    this.ctrl = new TextBox();
                    this.ctrl!.Name = className + form.cnt_TextBox;
                    form.cnt_TextBox++;
                    break;
                case "ListBox":
                    this.ctrl = new ListBox();
                    this.ctrl.Size = new System.Drawing.Size(120, 104);
                    this.ctrl!.Name = className + form.cnt_ListBox;
                    ListBox? listbox = this.ctrl as ListBox;
                    listbox!.Items.Add("ListBox");
                    form.cnt_ListBox++;
                    break;
                case "TabControl":
                    this.ctrl = new TabControl();
                    this.ctrl.Size = new System.Drawing.Size(250, 125);
                    this.ctrl!.Name = className + form.cnt_TabControl;
                    form.cnt_TabControl++;
                    break;
                case "TabPage":
                    this.ctrl = new TabPage();
                    this.ctrl.Size = new System.Drawing.Size(250, 125);
                    this.ctrl!.Name = className + form.cnt_TabPage;
                    form.cnt_TabPage++;
                    break;
                case "CheckBox":
                    this.ctrl = new CheckBox();
                    this.ctrl!.Name = className + form.cnt_CheckBox;
                    this.ctrl!.AutoSize = true;
                    form.cnt_CheckBox++;
                    break;
                case "ComboBox":
                    this.ctrl = new ComboBox();
                    this.ctrl!.Name = className + form.cnt_ComboBox;
                    form.cnt_ComboBox++;
                    break;
                case "SplitContainer":
                    this.ctrl = new SplitContainer();
                    this.ctrl.Size = new System.Drawing.Size(120, 32);
                    this.ctrl!.Name = className + form.cnt_SplitContainer;
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
                    form.cnt_SplitContainer++;
                    break;
                case "DataGridView":
                    this.ctrl = new DataGridView();
                    this.ctrl.Size = new System.Drawing.Size(304, 192);
                    this.ctrl!.Name = className + form.cnt_DataGridView;
                    DataTable table = new DataTable();
                    table.Columns.Add("Column1");
                    table.Columns.Add("Column2");
                    DataGridView? datagridview = this.ctrl as DataGridView;
                    datagridview!.DataSource = table;
                    form.cnt_DataGridView++;
                    break;
                case "Panel":
                    this.ctrl = new Panel();
                    this.ctrl.Size = new System.Drawing.Size(304, 192);
                    this.ctrl!.Name = className + form.cnt_Panel;
                    Panel? panel = this.ctrl as Panel;
                    panel!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    form.cnt_Panel++;
                    break;
                case "CheckedListBox":
                    this.ctrl = new CheckedListBox();
                    this.ctrl.Size = new System.Drawing.Size(152, 112);
                    this.ctrl!.Name = className + form.cnt_CheckedListBox;
                    CheckedListBox? checkedlistbox = this.ctrl as CheckedListBox;
                    checkedlistbox!.Items.Add("CheckedListBox");
                    form.cnt_CheckedListBox++;
                    break;
                case "LinkLabel":
                    this.ctrl = new LinkLabel();
                    this.ctrl.Size = new System.Drawing.Size(120, 32);
                    this.ctrl!.Name = className + form.cnt_LinkLabel;
                    this.ctrl!.AutoSize = true;
                    form.cnt_LinkLabel++;
                    break;
                case "PictureBox":
                    this.ctrl = new PictureBox();
                    this.ctrl.Size = new System.Drawing.Size(125, 62);
                    this.ctrl!.Name = className + form.cnt_PictureBox;
                    PictureBox? picbox = this.ctrl as PictureBox;
                    picbox!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    form.cnt_PictureBox++;
                    break;
                case "ProgressBar":
                    this.ctrl = new ProgressBar();
                    this.ctrl.Size = new System.Drawing.Size(125, 29);
                    this.ctrl!.Name = className + form.cnt_ProgressBar;
                    ProgressBar? prgressbar = this.ctrl as ProgressBar;
                    prgressbar!.Value = 50;
                    form.cnt_ProgressBar++;
                    break;
                case "RadioButton":
                    this.ctrl = new RadioButton();
                    this.ctrl.Size = new System.Drawing.Size(125, 29);
                    this.ctrl!.Name = className + form.cnt_RadioButton;
                    this.ctrl!.AutoSize = true;
                    form.cnt_RadioButton++;
                    break;
                case "RichTextBox":
                    this.ctrl = new RichTextBox();
                    this.ctrl.Size = new System.Drawing.Size(125, 120);
                    this.ctrl!.Name = className + form.cnt_RichTextBox;
                    form.cnt_RichTextBox++;
                    break;
                case "StatusStrip":
                    this.ctrl = new StatusStrip();
                    this.ctrl.Size = new System.Drawing.Size(125, 120);
                    this.ctrl!.Name = className + form.cnt_StatusStrip;
                    form.cnt_StatusStrip++;
                    break;
                case "HScrollBar":
                    this.ctrl = new HScrollBar();
                    this.ctrl.Size = new System.Drawing.Size(120, 32);
                    this.ctrl!.Name = className + form.cnt_HScrollBar;
                    CreatePickBox(this.ctrl);
                    form.cnt_HScrollBar++;
                    break;
                case "VScrollBar":
                    this.ctrl = new VScrollBar();
                    this.ctrl.Size = new System.Drawing.Size(32, 120);
                    this.ctrl!.Name = className + form.cnt_VScrollBar;
                    CreatePickBox(this.ctrl);
                    form.cnt_VScrollBar++;
                    break;
                case "MonthCalendar":
                    this.ctrl = new MonthCalendar();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form.cnt_MonthCalendar;
                    CreatePickBox(this.ctrl);
                    form.cnt_MonthCalendar++;
                    break;
                case "ListView":
                    this.ctrl = new ListView();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form.cnt_ListView;
                    CreatePickBox(this.ctrl);
                    form.cnt_ListView++;
                    break;
                case "TreeView":
                    this.ctrl = new TreeView();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form.cnt_TreeView;
                    CreatePickBox(this.ctrl);
                    form.cnt_TreeView++;
                    break;
                default:
                    return false;
            }
            // ****************************************************************************************

            this.ctrl!.Text = this.ctrl!.Name;
            this.ctrl!.TabIndex = form.cnt_Control;
            return true;
        }
    }
}
