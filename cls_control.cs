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
                this.Name = className + this.TabIndex.ToString();
                this.Text = className + this.TabIndex.ToString();
                this.Location = new System.Drawing.Point(X, Y);

                this.form.CtrlItems!.Add(this);
                parent.Controls.Add(this.form.CtrlItems[this.form.CtrlItems.Count - 1].ctrl);

                if (this.ctrl is TabControl)
                {
                    _ = new cls_control(form, "TabPage", this.ctrl!, backPanel!, toolList, propertyList!, X, Y);
                    _ = new cls_control(form, "TabPage", this.ctrl!, backPanel!, toolList, propertyList!, X, Y);
                }

                if (this.ctrl is TabPage == false)
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
                Point pos = new Point(0, 0);
                pos.X = (int)(e.X / grid) * grid;
                pos.Y = (int)(e.Y / grid) * grid;

                Point newPos = new Point(pos.X - memPos.X + ctrl!.Location.X, pos.Y - memPos.Y + ctrl.Location.Y);
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
            if (e.ToString() == "System.EventArgs")
            {
                return;
            }

            MouseEventArgs me = (MouseEventArgs)e;

            if (toolList!.Text == "")
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
            else
            {
                // unselect
                form.SelectAllClear();

                // Add 
                int X = (int)(me.X / grid) * grid;
                int Y = (int)(me.Y / grid) * grid;

                if ((this.ctrl is TabControl && toolList!.Text == "TabPage") || (this.ctrl is TabControl == false && toolList!.Text != "TabPage"))
                {
                    cls_control ctrl = new cls_control(form, toolList!.Text, this.ctrl!, backPanel!, toolList, propertyList!, X, Y);
                }

                toolList.SelectedIndex = -1;
            }

        }

        public void Delete()
        {
            Selected = false;
            parent.Controls.Remove(ctrl);
        }

        // ********************************************************************************
        // プロパティ
        // ********************************************************************************

        public string Name
        {
            set
            {
                ctrl!.Name = value;

            }
            get
            {
                return ctrl!.Name;
            }
        }
        public string Text
        {
            set
            {
                ctrl!.Text = value;

            }
            get
            {
                return ctrl!.Text;
            }
        }
        public System.Drawing.Point Location
        {
            set
            {
                ctrl!.Location = value;

            }
            get
            {
                return ctrl!.Location;
            }
        }
        public System.Drawing.Size Size
        {
            set
            {
                ctrl!.Size = value;

            }
            get
            {
                return ctrl!.Size;
            }
        }

        public bool Selected
        {
            set
            {
                if (selectBox != null)
                {
                    selectFlag = value;
                    selectBox!.SetSelectBoxPos(value);
                }


                if (value)
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
            get
            {
                return selectFlag;
            }
        }

        public int Left
        {
            set
            {
                ctrl!.Location = new System.Drawing.Point(value, ctrl!.Location.Y);
            }
            get
            {
                return ctrl!.Location.X;
            }
        }
        public int Top
        {
            set
            {
                ctrl!.Location = new System.Drawing.Point(ctrl!.Location.X, value);
            }
            get
            {
                return ctrl!.Location.Y;
            }
        }

        public int Width
        {
            set
            {
                ctrl!.Size = new System.Drawing.Size(value, ctrl!.Size.Height);
            }
            get
            {
                return ctrl!.Size.Width;
            }
        }

        public int Height
        {
            set
            {
                ctrl!.Size = new System.Drawing.Size(ctrl!.Size.Width, value);
            }
            get
            {
                return ctrl!.Size.Height;
            }
        }

        public int TabIndex
        {
            set
            {
                ctrl!.TabIndex = value;
            }
            get
            {
                return ctrl!.TabIndex;
            }
        }

        // ****************************************************************************************
        // コントロール追加時に下記を編集すること
        // ****************************************************************************************
        private bool Init(string className)
        {
            switch (className)
            {
                case "Button":
                    this.ctrl = new Button();
                    this.Size = new System.Drawing.Size(96, 32);
                    this.TabIndex = form.cnt_Button;
                    form.cnt_Button++;
                    return true;
                case "Label":
                    this.ctrl = new Label();
                    this.Size = new System.Drawing.Size(80, 32);
                    this.TabIndex = form.cnt_Label;
                    form.cnt_Label++;
                    return true;
                case "GroupBox":
                    this.ctrl = new GroupBox();
                    this.Size = new System.Drawing.Size(250, 125);
                    this.TabIndex = form.cnt_GroupBox;
                    form.cnt_GroupBox++;
                    return true;
                case "TextBox":
                    this.ctrl = new TextBox();
                    this.Size = new System.Drawing.Size(120, 32);
                    this.TabIndex = form.cnt_TextBox;
                    form.cnt_TextBox++;
                    return true;
                case "ListBox":
                    this.ctrl = new ListBox();
                    this.Size = new System.Drawing.Size(120, 104);
                    this.TabIndex = form.cnt_ListBox;
                    form.cnt_ListBox++;
                    return true;
                case "TabControl":
                    this.ctrl = new TabControl();
                    this.Size = new System.Drawing.Size(250, 125);
                    this.TabIndex = form.cnt_TabControl;
                    form.cnt_TabControl++;
                    return true;
                case "TabPage":
                    this.ctrl = new TabPage();
                    this.Size = new System.Drawing.Size(250, 125);
                    this.TabIndex = form.cnt_TabPage;
                    form.cnt_TabPage++;
                    return true;
                case "CheckBox":
                    this.ctrl = new CheckBox();
                    this.Size = new System.Drawing.Size(120, 32);
                    this.TabIndex = form.cnt_CheckBox;
                    form.cnt_CheckBox++;
                    return true;
                case "ComboBox":
                    this.ctrl = new ComboBox();
                    this.Size = new System.Drawing.Size(120, 32);
                    this.TabIndex = form.cnt_ComboBox;
                    form.cnt_ComboBox++;
                    return true;

                default:
                    return false;
            }
        }
        // ****************************************************************************************

    }
}
