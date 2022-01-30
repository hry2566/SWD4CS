using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace SWD4CS
{
    internal class cls_control
    {
        private cls_form form;
        public string className;
        public Control? ctrl;
        public Control parent;
        private Control backPanel;
        private cls_selectbox? selectBox;
        private ListBox? toolList;
        private DataGridView propertyList;
        private bool selectFlag;
        private bool changeFlag;
        private Point memPos;
        private int grid = 8;

        public cls_control(cls_form form, string className, Control parent, Control backPanel, ListBox? toolList, DataGridView propertyList, int X, int Y)
        {
            this.form = form;
            this.className = className;
            this.parent = parent;
            this.backPanel = backPanel;
            this.toolList = toolList;
            this.propertyList = propertyList;

            if (Init(className))
            {
                if (className == "TabPage" && parent == form)
                {
                    return;
                }

                this.ctrl!.Location = new System.Drawing.Point(X, Y);
                this.form.CtrlItems!.Add(this);
                parent.Controls.Add(this.form.CtrlItems[this.form.CtrlItems.Count - 1].ctrl);

                if (this.ctrl is TabControl)
                {
                    _ = new cls_control(form, "TabPage", this.ctrl!, backPanel!, toolList, propertyList!, X, Y);
                    _ = new cls_control(form, "TabPage", this.ctrl!, backPanel!, toolList, propertyList!, X, Y);
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
                cls_form.EnableDoubleBuffering(this.ctrl);
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
            MouseEventArgs me = (MouseEventArgs)e;

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
                AddControls(me);
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
                    _ = new cls_control(form, toolList!.Text, this.ctrl!, backPanel!, toolList, propertyList!, X, Y);
                }
                else
                {
                    _ = new cls_control(form, toolList!.Text, splitpanel!, backPanel!, toolList, propertyList!, X, Y);
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
                    form.SelectAllClear();
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

        private void ShowProperty(bool value)
        {
            DataTable table = new();

            propertyList.Columns.Clear();
            table.Columns.Add("Property");
            table.Columns.Add("Value");

            if (value)
            {
                foreach (PropertyInfo item in this.ctrl!.GetType().GetProperties())
                {
                    if (HideProperty(item.Name))
                    {
                        DataRow row = table.NewRow();

                        row[0] = item.Name;
                        row[1] = item.GetValue(this.ctrl);
                        table.Rows.Add(row);
                    }
                }
            }
            propertyList.DataSource = table;
            propertyList.Sort(propertyList.Columns[0], ListSortDirection.Ascending);
        }

        internal static bool HideProperty(string itemName)
        {
            if (itemName != "AccessibilityObject" &&
                itemName != "AccessibleDefaultActionDescription" &&
                itemName != "AutoScrollOffset" &&
                itemName != "BindingContext" &&
                itemName != "Bottom" &&
                itemName != "Bounds" &&
                itemName != "CanFocus" &&
                itemName != "CanSelect" &&
                itemName != "Capture" &&
                itemName != "ClientRectangle" &&
                itemName != "ClientSize" &&
                itemName != "CompanyName" &&
                itemName != "Container" &&
                itemName != "ContainsFocus" &&
                itemName != "Controls" &&
                itemName != "Created" &&
                itemName != "DataBindings" &&
                itemName != "DeviceDpi" &&
                itemName != "DisplayRectangle" &&
                itemName != "Disposing" &&
                itemName != "Focused" &&
                itemName != "Handle" &&
                itemName != "HasChildren" &&
                itemName != "Height" &&
                itemName != "ImeMode" &&
                itemName != "InvokeRequired" &&
                itemName != "IsAccessible" &&
                itemName != "IsAncestorSiteInDesignMode" &&
                itemName != "IsDisposed" &&
                itemName != "IsHandleCreated" &&
                itemName != "IsMirrored" &&
                itemName != "LayoutEngine" &&
                itemName != "Left" &&
                itemName != "Parent" &&
                itemName != "PreferredSize" &&
                itemName != "ProductName" &&
                itemName != "ProductVersion" &&
                itemName != "RecreatingHandle" &&
                itemName != "Region" &&
                itemName != "Right" &&
                itemName != "Site" &&
                itemName != "Top" &&
                itemName != "TopLevelControl" &&
                itemName != "Width" &&
                itemName != "WindowTarget" &&
                itemName != "Visible" &&
                itemName != "TextLength" &&
                itemName != "RowCount" &&
                itemName != "TabCount" &&
                itemName != "PreferredWidth" &&
                itemName != "PreferredHeight" &&
                itemName != "SelectedIndex" &&
                itemName != "SelectedItem" &&
                itemName != "SelectedText" &&
                itemName != "SelectedValue" &&
                itemName != "SelectionLength" &&
                itemName != "SelectionStart" &&
                itemName != "CustomTabOffsets" &&
                itemName != "FormatInfo" &&
                itemName != "SelectedIndices" &&
                itemName != "SelectedItems" &&
                itemName != "SelectedTab" &&
                itemName != "CanUndo" &&
                itemName != "ParentForm" &&
                itemName != "Item" &&
                itemName != "AdjustedTopLeftHeaderBorderStyle" &&
                itemName != "AdvancedCellBorderStyle" &&
                itemName != "AdvancedColumnHeadersBorderStyle" &&
                itemName != "AdvancedRowHeadersBorderStyle" &&
                itemName != "AutoGenerateColumns" &&
                itemName != "ColumnCount" &&
                itemName != "CurrentCell" &&
                itemName != "CurrentCellAddress" &&
                itemName != "CurrentRow" &&
                itemName != "EditingControl" &&
                itemName != "EditingPanel" &&
                itemName != "FirstDisplayedCell" &&
                itemName != "FirstDisplayedScrollingColumnHiddenWidth" &&
                itemName != "FirstDisplayedScrollingColumnIndex" &&
                itemName != "FirstDisplayedScrollingRowIndex" &&
                itemName != "HorizontalScrollingOffset" &&
                itemName != "IsCurrentCellDirty" &&
                itemName != "IsCurrentCellInEditMode" &&
                itemName != "IsCurrentRowDirty" &&
                itemName != "NewRowIndex" &&
                itemName != "Rows" &&
                itemName != "SelectedCells" &&
                itemName != "SelectedRows" &&
                itemName != "SortedColumn" &&
                itemName != "SortOrder" &&
                itemName != "TopLeftHeaderCell" &&
                itemName != "VerticalScrollingOffset" &&
                itemName != "VirtualMode" &&
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
                    this.ctrl.Size = new System.Drawing.Size(80, 32);
                    this.ctrl!.Name = className + form.cnt_Label;
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
                    this.ctrl.Size = new System.Drawing.Size(120, 32);
                    this.ctrl!.Name = className + form.cnt_TextBox;
                    form.cnt_TextBox++;
                    break;
                case "ListBox":
                    this.ctrl = new ListBox();
                    this.ctrl.Size = new System.Drawing.Size(120, 104);
                    this.ctrl!.Name = className + form.cnt_ListBox;
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
                    this.ctrl.Size = new System.Drawing.Size(120, 32);
                    this.ctrl!.Name = className + form.cnt_CheckBox;
                    form.cnt_CheckBox++;
                    break;
                case "ComboBox":
                    this.ctrl = new ComboBox();
                    this.ctrl.Size = new System.Drawing.Size(120, 32);
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
