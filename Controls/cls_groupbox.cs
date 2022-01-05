
namespace SWD4CS
{
    public partial class cls_groupbox : GroupBox
    {
        private cls_form form;
        internal Control parentCtrl;
        private ListBox? toolList;
        private DataGridView? propertyList;
        private cls_selectbox? selectBox;
        private bool selectFlag = false;
        private bool changeFlag = false;
        private Point memPos;
        private int grid = 8;

        public cls_groupbox(cls_form form, Control parent, Control backPanel, ListBox toolList, DataGridView propertyList, int index, int X = 0, int Y = 0)
        {
            InitializeComponent(index, X, Y);

            this.form = form;
            this.parentCtrl = parent;
            this.toolList = toolList;
            this.propertyList = propertyList;
            this.form.CtrlItems!.Add(this);
            parent.Controls.Add(this.form.CtrlItems[this.form.CtrlItems.Count - 1] as cls_groupbox);

            backPanel.Click += new System.EventHandler(Backpanel_Click);
            this.Click += new System.EventHandler(Ctrl_Click);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);

            selectBox = new cls_selectbox(this, this.parentCtrl);
            SetSelect(true);
        }

        private void ControlMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && selectFlag)
            {
                memPos.X = (int)(e.X / grid) * grid;
                memPos.Y = (int)(e.Y / grid) * grid;
            }
        }

        private void ControlMouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && selectFlag)
            {
                Point pos = new Point(0, 0);
                pos.X = (int)(e.X / grid) * grid;
                pos.Y = (int)(e.Y / grid) * grid;

                Point newPos = new Point(pos.X - memPos.X + this.Location.X, pos.Y - memPos.Y + this.Location.Y);
                this.Location = newPos;
                SetSelect(true);
                changeFlag = false;
            }
            else
            {
                changeFlag = true;
            }
        }

        private void Ctrl_Click(object? sender, EventArgs e)
        {
            if (toolList!.Text == "")
            {
                MouseEventArgs me = (MouseEventArgs)e;

                if (me.Button == MouseButtons.Left)
                {
                    if (selectFlag && changeFlag)
                    {
                        SetSelect(false);
                    }
                    else
                    {
                        SelectAllClear();
                        SetSelect(true);
                    }
                }
            }
            else
            {
                MouseEventArgs me = (MouseEventArgs)e;

                // unselect
                SelectAllClear();

                // Add 
                int X = (int)(me.X / grid) * grid;
                int Y = (int)(me.Y / grid) * grid;

                form.AddControl(X, Y, this);

                toolList.SelectedIndex = -1;
            }
        }

        private void SelectAllClear()
        {
            form.SetSelect(false);

            for (int i = 0; i < form.CtrlItems!.Count; i++)
            {
                form.ControlCom(i, 0, 0);
            }
        }

        private void Backpanel_Click(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                if (selectFlag)
                {
                    SetSelect(false);
                }
            }
        }

        internal void SetSelect(bool flag)
        {
            selectFlag = flag;
            selectBox!.SetSelectBoxPos(selectFlag);

            // 選択されていたらプロパティ表示
            if (flag)
            {
                propertyList!.Rows.Clear();
                propertyList.Rows.Add("Name", this.Name);
                propertyList.Rows.Add("Location.X", this.Location.X);
                propertyList.Rows.Add("Location.Y", this.Location.Y);
                propertyList.Rows.Add("Size.Width", this.Size.Width);
                propertyList.Rows.Add("Size.Height", this.Size.Height);
                propertyList.Rows.Add("TabIndex", this.TabIndex);
                propertyList.Rows.Add("Text", this.Text);
            }
            else
            {
                propertyList!.Rows.Clear();
            }
        }

        internal bool GetSelected()
        {
            return selectFlag;
        }

        internal void Deleate(Control parent)
        {
            for(int i = 0; i < form.CtrlItems!.Count; i++)
            {
                // ****************************************************************************************
                // コントロール追加時に下記を編集すること
                // ****************************************************************************************
                if (form.CtrlItems[i] is cls_button)
                {
                    cls_button? ctrl = form.CtrlItems[i] as cls_button;

                    if (ctrl!.parentCtrl == this)
                    {
                        ctrl.ctrlBase.SetSelect(true);
                    }
                }
                else if (form.CtrlItems[i] is cls_label)
                {
                    cls_label? ctrl = form.CtrlItems[i] as cls_label;

                    if (ctrl!.parentCtrl == this)
                    {
                        ctrl.ctrlBase.SetSelect(true);
                    }
                }
                else if (form.CtrlItems[i] is cls_textbox)
                {
                    cls_textbox? ctrl = form.CtrlItems[i] as cls_textbox;

                    if (ctrl!.parentCtrl == this)
                    {
                        ctrl.ctrlBase.SetSelect(true);
                    }
                }
                else if (form.CtrlItems[i] is cls_listbox)
                {
                    cls_listbox? ctrl = form.CtrlItems[i] as cls_listbox;

                    if (ctrl!.parentCtrl == this)
                    {
                        ctrl.ctrlBase.SetSelect(true);
                    }
                }
                else if (form.CtrlItems[i] is cls_groupbox)
                {
                    cls_groupbox? ctrl = form.CtrlItems[i] as cls_groupbox;

                    if (ctrl!.parentCtrl == this)
                    {
                        ctrl.SetSelect(true);
                    }
                }
                else if (form.CtrlItems[i] is cls_tabcontrol)
                {
                    cls_tabcontrol? ctrl = form.CtrlItems[i] as cls_tabcontrol;

                    if (ctrl!.parentCtrl == this)
                    {
                        ctrl.SetSelect(true);
                    }
                }
                else if (form.CtrlItems[i] is cls_tabpage)
                {
                    cls_tabpage? ctrl = form.CtrlItems[i] as cls_tabpage;

                    if (ctrl!.parentCtrl == this)
                    {
                        ctrl.SetSelect(true);
                    }
                }

                // ****************************************************************************************
            }

            parent.Controls.Remove(parent);
            SetSelect(false);
            this.Dispose();
        }
    }
}
