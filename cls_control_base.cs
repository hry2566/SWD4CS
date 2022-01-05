
namespace SWD4CS
{
    public class cls_control_base
    {
        private cls_form form;
        private Control ctrl;
        private cls_selectbox selectBox;
        private DataGridView propertyList;
        private bool selectFlag = false;
        private bool changeFlag = false;
        private Point memPos;
        private int grid = 8;

        public cls_control_base(cls_form form, Control ctrl, Control parent, Control backPanel, DataGridView propertyList)
        {
            this.form = form;
            this.ctrl = ctrl;
            this.propertyList = propertyList;
            
            ctrl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ControlMouseMove);
            ctrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ControlMouseDown);
            backPanel.Click += new System.EventHandler(Backpanel_Click);
            ctrl.Click += new System.EventHandler(Ctrl_Click);

            selectBox = new cls_selectbox(ctrl, parent);
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

                Point newPos = new Point(pos.X - memPos.X + ctrl.Location.X, pos.Y - memPos.Y + ctrl.Location.Y);
                ctrl.Location = newPos;
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
            if (e.ToString() != "System.EventArgs")
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
                        form.SelectAllClear();
                        SetSelect(true);
                    }
                }
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
            if (flag)
            {
                propertyList.Rows.Clear();
                propertyList.Rows.Add("Name", ctrl.Name);
                propertyList.Rows.Add("Location.X", ctrl.Location.X);
                propertyList.Rows.Add("Location.Y", ctrl.Location.Y);
                propertyList.Rows.Add("Size.Width", ctrl.Size.Width);
                propertyList.Rows.Add("Size.Height", ctrl.Size.Height);
                propertyList.Rows.Add("TabIndex", ctrl.TabIndex);
                propertyList.Rows.Add("Text", ctrl.Text);
            }
            else
            {
                propertyList.Rows.Clear();
            }

            selectFlag = flag;
            selectBox.SetSelectBoxPos(selectFlag);
        }

        internal bool GetSelected()
        {
            return selectFlag;
        }
    }
}
