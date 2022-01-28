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
        public int cnt_SplitContainer;

        internal static Control? GetBaseCtrl(cls_control ctrl)
        {
            Control? baseCtrl = new();
            Type type = ctrl.ctrl!.GetType();

            switch (type)
            {
                case Type t when t == typeof(System.Windows.Forms.Button):
                    baseCtrl = new Button();
                    break;
                case Type t when t == typeof(System.Windows.Forms.Label):
                    baseCtrl = new Label();
                    break;
                case Type t when t == typeof(System.Windows.Forms.GroupBox):
                    baseCtrl = new GroupBox();
                    break;
                case Type t when t == typeof(System.Windows.Forms.TextBox):
                    baseCtrl = new TextBox();
                    break;
                case Type t when t == typeof(System.Windows.Forms.ListBox):
                    baseCtrl = new ListBox();
                    break;
                case Type t when t == typeof(System.Windows.Forms.TabControl):
                    baseCtrl = new TabControl();
                    break;
                case Type t when t == typeof(System.Windows.Forms.TabPage):
                    baseCtrl = new TabPage();
                    break;
                case Type t when t == typeof(System.Windows.Forms.CheckBox):
                    baseCtrl = new CheckBox();
                    break;
                case Type t when t == typeof(System.Windows.Forms.ComboBox):
                    baseCtrl = new ComboBox();
                    break;
                case Type t when t == typeof(System.Windows.Forms.SplitContainer):
                    baseCtrl = new SplitContainer();
                    break;
            }
            return baseCtrl;
        }
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
            this.propertyList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellValueChanged);

            backPanel.Click += new System.EventHandler(Backpanel_Click);

            selectBox = new cls_selectbox(this, backPanel);
            SetSelect(true);
        }

        private void SetProperty(int i, int index, bool formFlag)
        {
            string? propertyName;
            string? propertyValue;

            if (propertyList!.Rows[index].Cells[0].Value != null && propertyList.Rows[index].Cells[1].Value != null)
            {
                propertyName = propertyList.Rows[index].Cells[0].Value.ToString();
                propertyValue = propertyList.Rows[index].Cells[1].Value.ToString();

                if (formFlag)
                {
                    SetFormProperty(propertyName, propertyValue);
                }
                else
                {
                    SetCtrlProperty(CtrlItems[i].ctrl, propertyName, propertyValue);
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
            ShowProperty(flag);
        }

        private void ShowProperty(bool flag)
        {
            DataTable table = new DataTable();

            propertyList!.Columns.Clear();
            table.Columns.Add("Property");
            table.Columns.Add("Value");

            if (flag)
            {
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
            }
            propertyList.DataSource = table;
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

                SelectAllClear();

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
                            Delete(CtrlItems[i]);
                            i--;
                        }
                    }
                    else
                    {
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
            List<string> formProperty = new();

            for (int i = 0; i < source_custom.Count; i++)
            {
                if (source_custom[i].IndexOf("this.") > -1 && formflag == false)
                {
                    formProperty.Add(source_custom[i]);

                    if (source_custom[i + 1].IndexOf("this.") == -1)
                    {
                        Code2Property(formProperty, true);
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
                            split = strLine[strLine.Count - 1].Split("(");
                            split = split[0].Split(".Controls.Add");
                            split = split[0].Split("this");

                            strParent = split[1];

                            if (strParent == "")
                            {
                                _ = new cls_control(this, ctrlClass, this, backPanel!, toolList, propertyList!, 0, 0);
                            }
                            else
                            {
                                if (strParent.Substring(0, 1) == ".")
                                {
                                    strParent = strParent.Substring(1, strParent.Length - 1);
                                }

                                bool splitcontainer_flag = false;
                                string[] split2 = strParent.Split(".");
                                if (split2.Count() == 2)
                                {
                                    strParent = split2[0];
                                    splitcontainer_flag = true;
                                }

                                for (int k = 0; k < CtrlItems.Count; k++)
                                {
                                    if (CtrlItems[k].ctrl!.Name == strParent)
                                    {
                                        if (splitcontainer_flag)
                                        {
                                            SplitContainer? splcontainer = CtrlItems[k].ctrl as SplitContainer;
                                            SplitterPanel splpanel;
                                            if (split2[1] == "Panel1")
                                            {
                                                splpanel = splcontainer!.Panel1;
                                                splpanel.Name = splcontainer.Name + ".Panel1";
                                                _ = new cls_control(this, ctrlClass, splcontainer!.Panel1, backPanel!, toolList, propertyList!, 0, 0);
                                            }
                                            else
                                            {
                                                splpanel = splcontainer!.Panel2;
                                                splpanel.Name = splcontainer.Name + ".Panel2";
                                                _ = new cls_control(this, ctrlClass, splcontainer!.Panel2, backPanel!, toolList, propertyList!, 0, 0);
                                            }
                                        }
                                        else
                                        {
                                            _ = new cls_control(this, ctrlClass, CtrlItems[k].ctrl!, backPanel!, toolList, propertyList!, 0, 0);
                                        }
                                        break;
                                    }
                                }
                            }

                            if (ctrlClass == "TabControl")
                            {
                                Delete(CtrlItems[CtrlItems.Count - 1]);
                                Delete(CtrlItems[CtrlItems.Count - 1]);
                                cnt_Control -= 2;
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

        private void DataGridView1_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
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

        private void SetFormProperty(string? propertyName, string? propertyValue)
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
        }

        private void Code2Property(List<string> strLine, bool formFlag)
        {
            string[] split;
            string propertyName;
            string propertyValue;
            Control ctrl;

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
                propertyValue = split[1].Replace(";", "").Replace("\"", "").Trim();

                split = split[0].Split(".");
                propertyName = split[split.Length - 1];

                SetCtrlProperty(ctrl, propertyName, propertyValue);
            }
        }

        private static int String2ScrollBars(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.ScrollBars") > -1)
            {
                string[] split = propertyValue.Split(".");
                split = split[split.Count() - 1].Split(";");
                propertyValue = split[0];
            }

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
            return style;
        }

        private static int String2ContentAlignment(string? propertyValue)
        {
            int style = 32;

            if (propertyValue!.IndexOf("System.Drawing.ContentAlignment") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Count() - 1].Replace(";", "");
            }

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
            return style;
        }

        private static Size String2Size(string propertyValue)
        {
            string[] split;
            string dummy;

            if (propertyValue.IndexOf("{Width=") >= 1)
            {
                split = propertyValue!.Split(',');
                split[0] = split[0].Replace("{Width=", "");
                split[1] = split[1].Replace("Height=", "").Replace("}", "");
            }
            else
            {
                split = propertyValue.Split("(");
                dummy = split[1];
                split = dummy.Split(")");
                dummy = split[0];
                split = dummy.Split(",");
            }

            Size size = new(int.Parse(split[0]), int.Parse(split[1]));
            return size;
        }

        private static Point String2Point(string propertyValue)
        {
            string[] split;
            string dummy;

            if (propertyValue.IndexOf("{X=") > -1)
            {
                split = propertyValue!.Split(',');
                split[0] = split[0].Replace("{X=", "");
                split[1] = split[1].Replace("Y=", "").Replace("}", "");
            }
            else
            {
                split = propertyValue.Split("(");
                dummy = split[1];
                split = dummy.Split(")");
                dummy = split[0];
                split = dummy.Split(",");
            }
            Point point = new(int.Parse(split[0]), int.Parse(split[1]));
            return point;
        }

        private static int String2AnchorStyles(string propertyValue)
        {
            int style = 0;

            if (propertyValue.IndexOf("System.Windows.Forms.AnchorStyles") > -1)
            {
                string dummy = propertyValue.Replace("System.Windows.Forms.AnchorStyles", "").Replace("(", "").Replace(")", "").Replace(";", "");
                string[] split2 = dummy.Split(" | ");

                propertyValue = "";
                for (int i = 0; i < split2.Count(); i++)
                {
                    string[] split3 = split2[i].Split(".");
                    if (propertyValue == "")
                    {
                        propertyValue += split3[split3.Count() - 1];
                    }
                    else
                    {
                        propertyValue += "," + split3[split3.Count() - 1];
                    }
                }
            }

            string[] split = propertyValue!.Split(',');

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
            return style;
        }

        private static int String2DockStyle(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.DockStyle") > -1)
            {
                string[] split = propertyValue.Split(".");
                split = split[split.Count() - 1].Split(";");
                propertyValue = split[0];
            }

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
            return style;
        }

        private static int String2HorizontalAlignment(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.HorizontalAlignment") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Count() - 1];
            }

            switch (propertyValue!.ToLower())
            {
                case "left":
                    style = 0;
                    break;
                case "right":
                    style = 1;
                    break;
                case "center":
                    style = 2;
                    break;
            }
            return style;
        }

        private static void SetCtrlProperty(Control? ctrl, string? propertyName, string? propertyValue)
        {
            Type type;
            PropertyInfo? property = ctrl!.GetType().GetProperty(propertyName!);

            if (property != null && property!.GetValue(ctrl) != null)
            {
                type = property!.GetValue(ctrl)!.GetType();

                switch (type)
                {
                    case Type t when t == typeof(System.String):
                        property.SetValue(ctrl, propertyValue);
                        break;
                    case Type t when t == typeof(System.Boolean):
                        try
                        {
                            bool b = System.Convert.ToBoolean(propertyValue);
                            property.SetValue(ctrl, b);
                        }
                        catch { }
                        break;
                    case Type t when t == typeof(System.Windows.Forms.DockStyle):
                        property.SetValue(ctrl, String2DockStyle(propertyValue));
                        break;
                    case Type t when t == typeof(System.Windows.Forms.AnchorStyles):
                        property.SetValue(ctrl, String2AnchorStyles(propertyValue!));
                        break;
                    case Type t when t == typeof(System.Drawing.Point):
                        try
                        {
                            property.SetValue(ctrl, String2Point(propertyValue!));
                        }
                        catch { }
                        break;
                    case Type t when t == typeof(System.Drawing.Size):
                        try
                        {
                            property.SetValue(ctrl, String2Size(propertyValue!));
                        }
                        catch { }
                        break;
                    case Type t when t == typeof(System.Int32):
                        try
                        {
                            property.SetValue(ctrl, int.Parse(propertyValue!));
                        }
                        catch { }
                        break;
                    case Type t when t == typeof(System.Drawing.ContentAlignment):
                        property.SetValue(ctrl, String2ContentAlignment(propertyValue));
                        break;
                    case Type t when t == typeof(System.Windows.Forms.ScrollBars):
                        property.SetValue(ctrl, String2ScrollBars(propertyValue));
                        break;
                    case Type t when t == typeof(System.Windows.Forms.HorizontalAlignment):
                        property.SetValue(ctrl, String2HorizontalAlignment(propertyValue));
                        break;
                }
            }
        }
    }
}
