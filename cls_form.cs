using System.Data;
using System.Reflection;

namespace SWD4CS
{
    public partial class cls_form : Panel
    {
        internal List<cls_control> CtrlItems = new();
        private cls_selectbox? selectBox;
        private Control? backPanel;
        private ListBox? toolList;
        private DataGridView? propertyList;
        private bool selectFlag = false;
        private int grid = 8;

        // ****************************************************************************************
        // コントロール追加時に下記を編集すること
        // ****************************************************************************************
        public int cnt_Control = -1;
        public int cnt_Button;
        public int cnt_Label;
        public int cnt_TextBox;
        public int cnt_ListBox;
        public int cnt_GroupBox;
        public int cnt_TabControl;
        public int cnt_TabPage;
        public int cnt_CheckBox;
        public int cnt_ComboBox;

        // ****************************************************************************************

        public cls_form()
        {
            InitializeComponent();
        }

        public void Init(Control backPanel, ListBox toolList, DataGridView dataGridView1)
        {
            this.backPanel = backPanel;
            this.toolList = toolList;
            this.propertyList = dataGridView1;
            this.Click += new System.EventHandler(Form_Click);
            this.propertyList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);

            backPanel.Click += new System.EventHandler(Backpanel_Click);

            selectBox = new cls_selectbox(this, backPanel);
        }

        private void SetProperty(int i, int index, bool formFlag)
        {
            string? propertyName;
            string? propertyValue = "";

            if (propertyList!.Rows[index].Cells[0].Value == null)
            {
                return;
            }
            else
            {
                propertyName = propertyList.Rows[index].Cells[0].Value.ToString();
            }

            if (propertyList.Rows[index].Cells[1].Value != null)
            {
                propertyValue = propertyList.Rows[index].Cells[1].Value.ToString();
            }

            if (formFlag)
            {
                if (propertyName == "Size.Width")
                {
                    this.Width = int.Parse(propertyValue!);
                }
                else if (propertyName == "Size.Height")
                {
                    this.Height = int.Parse(propertyValue!);
                }
                else if (propertyName == "Text")
                {
                    this.Text = propertyValue;
                }
                return;
            }
            else
            {
                Type type;
                PropertyInfo? property = CtrlItems[i].ctrl!.GetType().GetProperty(propertyName!);

                try
                {
                    type = property!.GetValue(CtrlItems[i].ctrl)!.GetType();
                }
                catch
                {
                    return;
                }

                //Console.WriteLine(type);

                string[] split;
                int style;

                switch (type)
                {
                    case Type t when t == typeof(System.String):
                        property.SetValue(CtrlItems[i].ctrl, propertyValue);
                        break;

                    case Type t when t == typeof(System.Boolean):
                        try
                        {
                            bool b = System.Convert.ToBoolean(propertyValue);
                            property.SetValue(CtrlItems[i].ctrl, b);
                        }
                        catch { }
                        break;

                    case Type t when t == typeof(System.Windows.Forms.DockStyle):
                        style = 0;
                        switch (propertyValue!.ToLower())
                        {
                            case "fill":
                                style = 5;
                                break;
                            case "bottom":
                                style = 2;
                                break;
                            case "left":
                                style = 3;
                                break;
                            case "right":
                                style = 4;
                                break;
                            case "top":
                                style = 1;
                                break;
                        }
                        property.SetValue(CtrlItems[i].ctrl, style);
                        break;

                    case Type t when t == typeof(System.Windows.Forms.AnchorStyles):
                        style = 0;
                        split = propertyValue!.Split(',');

                        for (int j = 0; j < split.Length; j++)
                        {
                            switch (split[j].Trim().ToLower())
                            {
                                case "bottom":
                                    style += 2;
                                    break;
                                case "left":
                                    style += 4;
                                    break;
                                case "right":
                                    style += 8;
                                    break;
                                case "top":
                                    style += 1;
                                    break;
                            }
                        }
                        property.SetValue(CtrlItems[i].ctrl, style);
                        break;

                    case Type t when t == typeof(System.Drawing.Point):
                        try
                        {
                            split = propertyValue!.Split(',');
                            split[0] = split[0].Replace("{X=", "");
                            split[1] = split[1].Replace("Y=", "");
                            split[1] = split[1].Replace("}", "");

                            Point point = new(int.Parse(split[0]), int.Parse(split[1]));
                            property.SetValue(CtrlItems[i].ctrl, point);
                        }
                        catch { }
                        break;

                    case Type t when t == typeof(System.Drawing.Size):
                        try
                        {
                            split = propertyValue!.Split(',');
                            split[0] = split[0].Replace("{Width=", "");
                            split[1] = split[1].Replace("Height=", "");
                            split[1] = split[1].Replace("}", "");

                            Size size = new(int.Parse(split[0]), int.Parse(split[1]));
                            property.SetValue(CtrlItems[i].ctrl, size);
                        }
                        catch { }
                        break;

                    case Type t when t == typeof(System.Int32):
                        try
                        {
                            int value = int.Parse(propertyValue!);
                            property.SetValue(CtrlItems[i].ctrl, value);
                        }
                        catch { }
                        break;

                    case Type t when t == typeof(System.Drawing.ContentAlignment):
                        style = 32;
                        switch (propertyValue!.ToLower())
                        {
                            case "bottomcenter":
                                style = 512;
                                break;
                            case "bottomleft":
                                style = 256;
                                break;
                            case "bottomright":
                                style = 1024;
                                break;
                            case "middleleft":
                                style = 16;
                                break;
                            case "middleright":
                                style = 64;
                                break;
                            case "topcenter":
                                style = 2;
                                break;
                            case "topleft":
                                style = 1;
                                break;
                            case "topright":
                                style = 4;
                                break;
                        }
                        property.SetValue(CtrlItems[i].ctrl, style);
                        break;

                    case Type t when t == typeof(System.Windows.Forms.ScrollBars):
                        style = 0;
                        switch (propertyValue!.ToLower())
                        {
                            case "both":
                                style = 3;
                                break;
                            case "horizontal":
                                style = 1;
                                break;
                            case "vertical":
                                style = 2;
                                break;
                        }
                        property.SetValue(CtrlItems[i].ctrl, style);
                        break;
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

        public void SetSelect(bool flag)
        {
            selectFlag = flag;
            selectBox!.SetSelectBoxPos(selectFlag);

            // 選択されていたらプロパティ表示
            if (flag)
            {
                propertyList!.Columns.Clear();
                DataTable table = new DataTable();
                table.Columns.Add("Property");
                table.Columns.Add("Value");


                DataRow row = table.NewRow();
                row[0] = "Name";
                row[1] = "From1";
                table.Rows.Add(row);

                row = table.NewRow();
                row[0] = "Size.Width";
                row[1] = this.Size.Width;
                table.Rows.Add(row);

                row = table.NewRow();
                row[0] = "Size.Height";
                row[1] = this.Size.Height;
                table.Rows.Add(row);

                row = table.NewRow();
                row[0] = "Text";
                row[1] = this.Text;
                table.Rows.Add(row);

                propertyList.DataSource = table;
            }
            else
            {
                propertyList!.Columns.Clear();

                DataTable table = new DataTable();
                table.Columns.Add("Property");
                table.Columns.Add("Value");
                propertyList.DataSource = table;
            }
        }

        private void Form_Click(object? sender, EventArgs e)
        {
            if (toolList!.Text == "")
            {
                MouseEventArgs me = (MouseEventArgs)e;

                if (me.Button == MouseButtons.Left)
                {
                    if (selectFlag)
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
                _ = new cls_control(this, toolList!.Text, this, backPanel!, toolList, propertyList!, X, Y);

                toolList.SelectedIndex = -1;
            }

        }

        internal void CtrlAllClear()
        {
            for (int i = 0; i < CtrlItems!.Count; i++)
            {
                CtrlItems[i].Selected = true;
            }

            RemoveSelectedItem();
        }

        internal void SelectAllClear()
        {
            SetSelect(false);

            for (int i = 0; i < CtrlItems!.Count; i++)
            {
                CtrlItems[i].Selected = false;
            }
        }

        internal void RemoveSelectedItem()
        {
            for (int i = 0; i < CtrlItems!.Count; i++)
            {
                if (CtrlItems[i].Selected)
                {
                    if (CtrlItems[i].ctrl is TabPage)
                    {
                        int cnt = 0;
                        for (int j = 0; j < CtrlItems.Count; j++)
                        {
                            if (CtrlItems[j].ctrl is TabPage)
                            {
                                cnt++;
                            }
                        }
                        if (cnt > 1)
                        {
                            //コントロール削除（子含む）
                            Delete(CtrlItems[i]);
                            i--;
                        }
                    }
                    else
                    {
                        //コントロール削除（子含む）
                        Delete(CtrlItems[i]);
                        i--;
                    }
                }
            }
        }

        internal void CreateControl(List<string> source_custom)
        {
            bool flag = false;
            bool formflag = false;
            List<string> formaProperty = new();

            //Console.Clear();

            for (int i = 0; i < source_custom.Count; i++)
            {
                if (source_custom[i].IndexOf("this.") > -1 && formflag == false)
                {
                    formaProperty.Add(source_custom[i]);

                    if (source_custom[i + 1].IndexOf("this.") == -1)
                    {
                        Code2Property(formaProperty, true);
                        formflag = true;
                    }
                }

                if (source_custom[i].IndexOf("new System.Windows.Forms.") > -1)
                {
                    flag = true;
                }

                if (flag)
                {
                    string[] split;
                    string dummy;
                    string ctrlClass;
                    string strParent;
                    List<string> strLine = new();

                    split = source_custom[i].Split('.');
                    dummy = split[split.Count() - 1];
                    ctrlClass = dummy.Substring(0, dummy.IndexOf('('));

                    while (true)
                    {
                        i++;
                        strLine.Add(source_custom[i]);

                        if (source_custom[i].IndexOf("Controls.Add") > -1)
                        {
                            split = strLine[strLine.Count - 1].Split(".");
                            strParent = split[1];

                            if (strParent == "Controls")
                            {
                                _ = new cls_control(this, ctrlClass, this, backPanel!, toolList, propertyList!, 0, 0);
                            }
                            else
                            {
                                for (int k = 0; k < CtrlItems.Count; k++)
                                {
                                    if (CtrlItems[k].ctrl!.Name == strParent)
                                    {
                                        _ = new cls_control(this, ctrlClass, CtrlItems[k].ctrl!, backPanel!, toolList, propertyList!, 0, 0);
                                        break;
                                    }
                                }
                            }

                            if (ctrlClass == "TabControl")
                            {
                                Delete(CtrlItems[CtrlItems.Count - 1]);
                                Delete(CtrlItems[CtrlItems.Count - 1]);
                            }

                            Code2Property(strLine, false);

                            break;
                        }
                    }

                    flag = false;
                }
            }

            SelectAllClear();
        }

        private void Code2Property(List<string> strLine, bool formFlag)
        {
            string[] split;
            string dummy;
            string propertyName;
            Control ctrl = new();

            if (formFlag)
            {
                ctrl = this;
            }
            else
            {
                ctrl = CtrlItems[CtrlItems.Count - 1].ctrl!;
            }

            for (int j = 0; j < strLine.Count - 1; j++)
            {
                split = strLine[j].Split(" = ");
                dummy = split[0];
                split = dummy.Split(".");
                propertyName = split[split.Length - 1];

                PropertyInfo? propertyinfo = ctrl.GetType().GetProperty(propertyName);

                if (propertyinfo != null)
                {
                    split = strLine[j].Split(" = ");
                    dummy = split[1];

                    if (strLine[j].IndexOf("\";") > 0)    //string
                    {
                        split = dummy.Split("\"");
                        propertyinfo.SetValue(ctrl, split[1]);
                    }
                    else if (strLine[j].IndexOf("false") > -1 || strLine[j].IndexOf("true") > -1)    //boolean
                    {

                        split = dummy.Split(";");
                        propertyinfo.SetValue(ctrl, Boolean.Parse(split[0]));
                    }
                    else if (strLine[j].IndexOf("System.Drawing.Point") > 0)    //point
                    {
                        split = dummy.Split("(");
                        dummy = split[1];
                        split = dummy.Split(")");
                        dummy = split[0];
                        split = dummy.Split(",");
                        propertyinfo.SetValue(ctrl, new Point(int.Parse(split[0]), int.Parse(split[1])));
                    }
                    else if (strLine[j].IndexOf("System.Drawing.Size") > 0)    //size
                    {
                        split = dummy.Split("(");
                        dummy = split[1];
                        split = dummy.Split(")");
                        dummy = split[0];
                        split = dummy.Split(",");
                        propertyinfo.SetValue(ctrl, new Size(int.Parse(split[0]), int.Parse(split[1])));
                    }
                    else // int
                    {
                        split = dummy.Split(";");
                        propertyinfo.SetValue(ctrl, int.Parse(split[0]));
                    }
                }
            }
        }

        private void Delete(cls_control ctrl)
        {
            for (int i = 0; i < CtrlItems.Count; i++)
            {
                if (ctrl.ctrl == CtrlItems[i].ctrl!.Parent)
                {
                    Delete(CtrlItems[i]);
                    i--;
                }
            }
            ctrl.Delete();
            CtrlItems.Remove(ctrl);
        }

        private void dataGridView1_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;

            if (this.selectFlag)
            {
                SetProperty(0, index, true);
                SetSelect(true);
            }
            else
            {
                for (int i = 0; i < CtrlItems!.Count; i++)
                {
                    if (CtrlItems[i].Selected)
                    {
                        SetProperty(i, index, false);
                        CtrlItems[i].Selected = true;
                        break;
                    }
                }
            }
        }
    }
}
