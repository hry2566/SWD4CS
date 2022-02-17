namespace SWD4CS
{
    internal class cls_selectbox
    {
        private cls_user_form? form;
        private cls_control? ctrl;
        private Control parent;
        private Panel[] selectbox = new Panel[8];
        private Point memPos;
        private int grid = 8;

        public cls_selectbox(cls_user_form ctrl, Control parent)
        {
            this.form = ctrl;
            this.parent = parent;

            Init();
        }
        public cls_selectbox(cls_control ctrl, Control parent)
        {
            this.ctrl = ctrl;
            this.parent = parent;

            Init();
        }

        private void Init()
        {
            for (int i = 0; i < 8; i++)
            {
                this.selectbox[i] = new Panel();

                if (i == 2 || i == 3)
                {
                    this.selectbox[i].Cursor = Cursors.SizeNESW;
                }
                else if (i == 0 || i == 5)
                {
                    this.selectbox[i].Cursor = Cursors.SizeNWSE;
                }
                else if (i == 1 || i == 4)
                {
                    this.selectbox[i].Cursor = Cursors.SizeNS;
                }
                else if (i == 6 || i == 7)
                {
                    this.selectbox[i].Cursor = Cursors.SizeWE;
                }

                this.selectbox[i].BorderStyle = BorderStyle.FixedSingle;
                this.selectbox[i].BackColor = System.Drawing.Color.White;
                this.selectbox[i].Size = new Size(8, 8);
                this.selectbox[i].Visible = false;
                this.selectbox[i].TabIndex = i;

                this.selectbox[i].MouseDown += new MouseEventHandler(SelectboxMouseDown!);
                this.selectbox[i].MouseMove += new MouseEventHandler(SelectboxMouseMove!);

                //cls_form.EnableDoubleBuffering(this.selectbox[i]);
            }
            parent.Controls.AddRange(this.selectbox);
        }

        internal void SetSelectBoxPos(bool flag)
        {
            if (flag)
            {
                int x1;
                int x2;
                int x3;
                int y1;
                int y2;
                int y3;

                if (ctrl != null)
                {
                    if (ctrl.ctrl is TabPage)
                    {
                        x1 = ctrl.ctrl.Left;
                        x2 = ctrl.ctrl.Width / 2 + ctrl.ctrl.Left - 8;
                        x3 = ctrl.ctrl.Width + ctrl.ctrl.Left - 16;
                        y1 = ctrl.ctrl.Top - 27;
                        y2 = ctrl.ctrl.Height / 2 + ctrl.ctrl.Top - 35;
                        y3 = ctrl.ctrl.Height + ctrl.ctrl.Top - 40;
                    }
                    else
                    {
                        x1 = ctrl.ctrl!.Left - 8;
                        x2 = ctrl.ctrl!.Width / 2 + ctrl.ctrl!.Left - 4;
                        x3 = ctrl.ctrl!.Width + ctrl.ctrl!.Left;
                        y1 = ctrl.ctrl!.Top - 8;
                        y2 = ctrl.ctrl!.Height / 2 + ctrl.ctrl!.Top - 4;
                        y3 = ctrl.ctrl!.Height + ctrl.ctrl!.Top;
                    }
                }
                else
                {
                    x1 = form!.Left - 8;
                    x2 = form.Width / 2 + form.Left - 4;
                    x3 = form.Width + form.Left;
                    y1 = form.Top - 8;
                    y2 = form.Height / 2 + form.Top - 4;
                    y3 = form.Height + form.Top;
                }

                this.selectbox[0].Location = new Point(x1, y1);
                this.selectbox[1].Location = new Point(x2, y1);
                this.selectbox[2].Location = new Point(x3, y1);
                this.selectbox[3].Location = new Point(x1, y3);
                this.selectbox[4].Location = new Point(x2, y3);
                this.selectbox[5].Location = new Point(x3, y3);
                this.selectbox[6].Location = new Point(x1, y2);
                this.selectbox[7].Location = new Point(x3, y2);
            }

            for (int i = 0; i < 8; i++)
            {
                this.selectbox[i].Visible = flag;
            }
        }

        private void SelectboxMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                memPos = e.Location;
            }
        }

        private void SelectboxMouseMove(object sender, MouseEventArgs e)
        {
            var move_selectbox = (Panel)sender;

            if (e.Button == MouseButtons.Left)
            {
                var newPos = new Point(e.X - memPos.X + move_selectbox.Location.X, e.Y - memPos.Y + move_selectbox.Location.Y);
                move_selectbox.Location = newPos;

                if (ctrl == null)
                {
                    SetFormSize(newPos, move_selectbox.TabIndex);
                }
                else
                {
                    SetControlSize(newPos, move_selectbox.TabIndex);
                }
            }
        }

        private void SetFormSize(Point pos, int index)
        {
            Point newPos = new((int)(pos.X / grid) * grid, (int)(pos.Y / grid) * grid);
            int width = newPos.X - form!.Left;
            int height = newPos.Y - form.Top;

            switch (index)
            {
                case 3:
                case 4:
                    form.Height = height;
                    break;
                case 5:
                    form.Width = width;
                    form.Height = height;
                    break;
                case 2:
                case 7:
                    form.Width = width;
                    break;
            }

            if (form.Width < 160)
            {
                form.Width = 160;
            }

            if (form.Height < 40)
            {
                form.Height = 40;
            }
            form.SetSelect(true);
        }

        private void SetControlSize(Point pos, int index)
        {
            Point point = new(ctrl!.ctrl!.Left, ctrl.ctrl!.Top);
            Size size = new(ctrl.ctrl!.Width, ctrl.ctrl!.Height);
            Point newPos = new((int)(pos.X / grid) * grid, (int)(pos.Y / grid) * grid);
            int width = newPos.X - ctrl!.ctrl!.Left;
            int height = newPos.Y - ctrl.ctrl!.Top;
            int width2 = ctrl.ctrl!.Width - (newPos.X + 8 - ctrl.ctrl!.Left);
            int height2 = ctrl.ctrl!.Height - (newPos.Y + 8 - ctrl.ctrl!.Top);
            int memleft = ctrl.ctrl!.Left;
            int memtop = ctrl.ctrl!.Top;

            switch (index)
            {
                case 0:
                    point.X = newPos.X + 8;
                    point.Y = newPos.Y + 8;
                    size.Width = width2;
                    size.Height = height2;
                    break;
                case 1:
                    point.Y = newPos.Y + 8;
                    size.Height = height2;
                    break;
                case 2:
                    point.Y = newPos.Y + 8;
                    size.Height = height2;
                    size.Width = width;
                    break;
                case 3:
                    point.X = newPos.X + 8;
                    size.Width = width2;
                    size.Height = height;
                    break;
                case 4:
                    size.Height = height;
                    break;
                case 5:
                    size.Width = width;
                    size.Height = height;
                    break;
                case 6:
                    point.X = newPos.X + 8;
                    size.Width = width2;
                    break;
                case 7:
                    size.Width = width;
                    break;
            }

            if (ctrl.ctrl!.Width < 24)
            {
                size.Width = 24;
                point.X = memleft;
            }

            if (ctrl.ctrl!.Height < 24)
            {
                size.Height = 24;
                point.Y = memtop;
            }

            ctrl.ctrl.Location = point;
            ctrl.ctrl.Size = size;

            ctrl.Selected = true;
        }
    }
}
