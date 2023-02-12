namespace SWD4CS
{
    internal class cls_selectbox
    {
        private cls_userform? form; cls_controls? ctrl; Control parent; Point memPos;
        private Panel[] selectbox = new Panel[8];
        private int grid = 4;

        public cls_selectbox(cls_userform ctrl, Control parent)
        {
            this.form = ctrl;
            this.parent = parent;
            Init();
        }

        public cls_selectbox(cls_controls ctrl, Control parent)
        {
            this.ctrl = ctrl;
            this.parent = parent;
            Init();
        }

        // ********************************************************************************************
        // internal Function 
        // ********************************************************************************************
        private void Init()
        {
            for (int i = 0; i < 8; i++)
            {
                this.selectbox[i] = new Panel();

                if (i == 2 || i == 3) { this.selectbox[i].Cursor = Cursors.SizeNESW; }
                else if (i == 0 || i == 5) { this.selectbox[i].Cursor = Cursors.SizeNWSE; }
                else if (i == 1 || i == 4) { this.selectbox[i].Cursor = Cursors.SizeNS; }
                else if (i == 6 || i == 7) { this.selectbox[i].Cursor = Cursors.SizeWE; }

                this.selectbox[i].BorderStyle = BorderStyle.FixedSingle;
                this.selectbox[i].BackColor = System.Drawing.Color.White;
                this.selectbox[i].Size = new Size(8, 8);
                this.selectbox[i].Visible = false;
                this.selectbox[i].TabIndex = i;

                this.selectbox[i].MouseDown += new MouseEventHandler(SelectboxMouseDown!);
                this.selectbox[i].MouseMove += new MouseEventHandler(SelectboxMouseMove!);
            }
            parent.Controls.AddRange(this.selectbox);
        }

        private void SelectboxMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { memPos = e.Location; }
        }

        private void SelectboxMouseMove(object? sender, MouseEventArgs e)
        {
            var move_selectbox = (Panel)sender!;

            if (e.Button == MouseButtons.Left)
            {
                int x = e.X - memPos.X + move_selectbox.Location.X;
                int y = e.Y - memPos.Y + move_selectbox.Location.Y;
                var newPos = new Point(x, y);
                move_selectbox.Location = newPos;
                if (ctrl == null) { SetFormSize(newPos, move_selectbox.TabIndex); }
                else { SetControlSize(newPos, move_selectbox.TabIndex); }
            }
        }

        private void SetFormSize(Point pos, int index)
        {
            int newX = (int)(pos.X / grid) * grid;
            int newY = (int)(pos.Y / grid) * grid;
            int width = newX - form!.Left;
            int height = newY - form.Top;

            if (index == 3 || index == 4) { form.Height = height; }
            if (index == 5) { form.Width = width; form.Height = height; }
            if (index == 2 || index == 7) { form.Width = width; }

            form.Width = Math.Max(form.Width, 160);
            form.Height = Math.Max(form.Height, 40);
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
            if (ctrl.ctrl!.Width < 16) { size.Width = 16; point.X = memleft; }
            if (ctrl.ctrl!.Height < 16) { size.Height = 16; point.Y = memtop; }
            ctrl.ctrl.Location = point;
            ctrl.ctrl.Size = size;
            ctrl.Selected = true;
        }

        internal void SetSelectBoxPos(bool flag)
        {
            if (flag)
            {
                int x1, x2, x3, y1, y2, y3;
                Control? control = ctrl?.ctrl ?? form;

                x1 = control!.Left - 8;
                x2 = control.Width / 2 + control.Left - 4;
                x3 = control.Width + control.Left;
                y1 = control.Top - 8;
                y2 = control.Height / 2 + control.Top - 4;
                y3 = control.Height + control.Top;

                if (ctrl != null)
                {
                    int a1 = 0, a2 = 0, a3 = 0, b1 = 0, b2 = 0, b3 = 0;
                    if (control is TabPage) { a2 = 8; a3 = 16; b1 = 27; b2 = 35; b3 = 40; }
                    else { a1 = 9; a2 = 4; b1 = 8; b2 = 4; }

                    x1 = control.Left - a1;
                    x2 = control.Width / 2 + control.Left - a2;
                    x3 = control.Width + control.Left - a3;
                    y1 = control.Top - b1;
                    y2 = control.Height / 2 + control.Top - b2;
                    y3 = control.Height + control.Top - b3;
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
            for (int i = 0; i < 8; i++) { this.selectbox[i].Visible = flag; }
        }
    }
}
