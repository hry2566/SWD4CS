
namespace SWD4CS
{
    public partial class cls_tabpage : TabPage
    {
        private cls_form form;
        internal Control parentCtrl;
        private ListBox? toolList;
        private bool selectFlag = false;
        private int grid = 8;

        public cls_tabpage(cls_form form, Control parent, Control backPanel, ListBox toolList, DataGridView propertyList, int index, int X = 0, int Y = 0)
        {
            InitializeComponent(index, X, Y);

            this.form = form;
            this.parentCtrl = parent;
            this.toolList = toolList;
            this.form.CtrlItems!.Add(this);
            parent.Controls.Add(this.form.CtrlItems[this.form.CtrlItems.Count - 1] as cls_tabpage);

            backPanel.Click += new System.EventHandler(Backpanel_Click);
            this.Click += new System.EventHandler(Ctrl_Click);
            cls_tabcontrol? tabctrl = parent as cls_tabcontrol;
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(tabctrl!.ControlMouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(tabctrl!.ControlMouseDown);
        }

        private void Ctrl_Click(object? sender, EventArgs e)
        {
            if (toolList!.Text == "")
            {
                MouseEventArgs me = (MouseEventArgs)e;

                if (me.Button == MouseButtons.Left)
                {
                    cls_tabcontrol? paremt = this.Parent as cls_tabcontrol;

                    if (paremt!.selectFlag && paremt.changeFlag)
                    {
                        paremt.SetSelect(false);
                    }
                    else
                    {
                        paremt.SelectAllClear();
                        paremt.SetSelect(true);
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
