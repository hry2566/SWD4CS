using System.Reflection;

namespace SWD4CS
{
    public partial class cls_form : Panel
    {
        internal List<cls_control> CtrlItems = new();
        private cls_selectbox? selectBox;
        private Control? backPanel;
        private ListBox? toolList;
        private PropertyGrid? propertyGrid;
        private TextBox? propertyCtrlName;
        private bool selectFlag = false;
        private int grid = 8;
        internal Form memForm = new();

        // ****************************************************************************************
        // コントロール追加時に下記を編集すること
        // ****************************************************************************************
        internal int cnt_Control = -1;
        internal int cnt_Button;
        internal int cnt_Label;
        internal int cnt_TextBox;
        internal int cnt_ListBox;
        internal int cnt_GroupBox;
        internal int cnt_TabControl;
        internal int cnt_TabPage;
        internal int cnt_CheckBox;
        internal int cnt_ComboBox;
        internal int cnt_SplitContainer;
        internal int cnt_DataGridView;
        internal int cnt_Panel;
        internal int cnt_CheckedListBox;
        internal int cnt_LinkLabel;
        internal int cnt_PictureBox;
        internal int cnt_ProgressBar;
        internal int cnt_RadioButton;
        internal int cnt_RichTextBox;
        internal int cnt_StatusStrip;
        internal int cnt_ListView;
        internal int cnt_TreeView;
        internal int cnt_MonthCalendar;
        internal int cnt_HScrollBar;
        internal int cnt_VScrollBar;

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
                case Type t when t == typeof(System.Windows.Forms.DataGridView):
                    baseCtrl = new DataGridView();
                    break;
                case Type t when t == typeof(System.Windows.Forms.Panel):
                    baseCtrl = new Panel();
                    break;
                case Type t when t == typeof(System.Windows.Forms.CheckedListBox):
                    baseCtrl = new CheckedListBox();
                    break;
                case Type t when t == typeof(System.Windows.Forms.LinkLabel):
                    baseCtrl = new LinkLabel();
                    break;
                case Type t when t == typeof(System.Windows.Forms.PictureBox):
                    baseCtrl = new PictureBox();
                    break;
                case Type t when t == typeof(System.Windows.Forms.ProgressBar):
                    baseCtrl = new ProgressBar();
                    break;
                case Type t when t == typeof(System.Windows.Forms.RadioButton):
                    baseCtrl = new RadioButton();
                    break;
                case Type t when t == typeof(System.Windows.Forms.RichTextBox):
                    baseCtrl = new RichTextBox();
                    break;
                case Type t when t == typeof(System.Windows.Forms.StatusStrip):
                    baseCtrl = new StatusStrip();
                    break;
                case Type t when t == typeof(System.Windows.Forms.ListView):
                    baseCtrl = new ListView();
                    break;
                case Type t when t == typeof(System.Windows.Forms.TreeView):
                    baseCtrl = new TreeView();
                    break;
                case Type t when t == typeof(System.Windows.Forms.MonthCalendar):
                    baseCtrl = new MonthCalendar();
                    break;
                case Type t when t == typeof(System.Windows.Forms.HScrollBar):
                    baseCtrl = new HScrollBar();
                    break;
                case Type t when t == typeof(System.Windows.Forms.VScrollBar):
                    baseCtrl = new VScrollBar();
                    break;
            }
            return baseCtrl;
        }
        private void CountInit()
        {
            cnt_Control = -1;
            cnt_Button = 0;
            cnt_Label = 0;
            cnt_TextBox = 0;
            cnt_ListBox = 0;
            cnt_GroupBox = 0;
            cnt_TabControl = 0;
            cnt_TabPage = 0;
            cnt_CheckBox = 0;
            cnt_ComboBox = 0;
            cnt_SplitContainer = 0;
            cnt_DataGridView = 0;
            cnt_Panel = 0;
            cnt_CheckedListBox = 0;
            cnt_LinkLabel = 0;
            cnt_PictureBox = 0;
            cnt_ProgressBar = 0;
            cnt_RadioButton = 0;
            cnt_RichTextBox = 0;
            cnt_StatusStrip = 0;
            cnt_ListView = 0;
            cnt_TreeView = 0;
            cnt_MonthCalendar = 0;
            cnt_HScrollBar = 0;
            cnt_VScrollBar = 0;
        }
        // ****************************************************************************************

        public cls_form()
        {
            InitializeComponent();
        }

        internal void Init(Control backPanel, ListBox toolList, PropertyGrid propertyGrid, TextBox propertyCtrlName)
        {
            this.backPanel = backPanel;
            this.toolList = toolList;
            this.propertyGrid = propertyGrid;
            this.propertyCtrlName = propertyCtrlName;
            this.Click += new System.EventHandler(Form_Click);

            backPanel.Click += new System.EventHandler(Backpanel_Click);

            propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(PropertyValueChanged);
            propertyCtrlName.TextChanged += new System.EventHandler(NameTextChanged);
            this.Resize += new System.EventHandler(formResize);

            selectBox = new cls_selectbox(this, backPanel);

            memForm.Location = this.Location;
            memForm.ClientSize = this.Size;
            memForm.Name = "Form1";
            SetSelect(true);
        }

        private void formResize(object? sender, EventArgs e)
        {
            memForm.ClientSize = this.Size;
        }

        private void NameTextChanged(object? sender, EventArgs e)
        {
            if (propertyGrid!.SelectedObject != null)
            {
                Control? ctrl = propertyGrid.SelectedObject as Control;
                ctrl!.Name = propertyCtrlName!.Text;
            }
        }

        private void PropertyValueChanged(object? s, PropertyValueChangedEventArgs e)
        {
            Control? ctrl = propertyGrid!.SelectedObject as Control;

            if (ctrl!.Name == memForm.Name)
            {
                string[] split = e.ChangedItem!.ToString()!.Split(" ");
                PropertyInfo? item;

                string? propertyName = split[1];
                if (propertyName == "Size")
                {
                    item = memForm.GetType().GetProperty("ClientSize");
                }
                else
                {
                    item = memForm.GetType().GetProperty(propertyName!);
                }

                PropertyInfo? formItem = this.GetType().GetProperty(propertyName!);

                if (formItem != null)
                {
                    formItem!.SetValue(this, item!.GetValue(memForm));
                    SetSelect(true);
                }
            }
            else
            {
                for (int i = 0; i < CtrlItems.Count; i++)
                {
                    if (CtrlItems[i].ctrl!.Name == ctrl!.Name)
                    {
                        CtrlItems[i].Selected = true;
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
            selectFlag = flag;
            selectBox!.SetSelectBoxPos(selectFlag);
            ShowProperty(flag);
        }

        private void ShowProperty(bool flag)
        {
            if (flag)
            {
                propertyGrid!.SelectedObject = this.memForm;
                propertyCtrlName!.Text = this.memForm.Name;
            }
            else
            {
                propertyGrid!.SelectedObject = null;
            }
        }

        private void Form_Click(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (toolList!.Text == "")
            {
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
                SelectAllClear();

                int X = (int)(me.X / grid) * grid;
                int Y = (int)(me.Y / grid) * grid;
                _ = new cls_control(this, toolList!.Text, this, backPanel!, toolList, propertyGrid!, propertyCtrlName!, X, Y);
                toolList.SelectedIndex = -1;
            }
        }



        internal void CtrlAllClear()
        {
            CountInit();

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
                        TabControl? tabctrl = CtrlItems[i].ctrl!.Parent as TabControl;

                        if (tabctrl!.TabPages.Count > 1)
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
            string ctrlClass = "";
            int cnt = Code2Form(source_custom);

            for (int i = cnt; i < source_custom.Count; i++)
            {
                if (source_custom[i].IndexOf("new System.Windows.Forms.") > -1)
                {
                    string[] split;
                    string dummy;

                    split = source_custom[i].Split('.');
                    dummy = split[split.Count() - 1];
                    ctrlClass = dummy.Substring(0, dummy.IndexOf('('));
                    flag = true;
                }

                if (flag)
                {
                    List<string> strLine = new();

                    while (true)
                    {
                        i++;
                        strLine.Add(source_custom[i]);

                        if (source_custom[i].IndexOf("Controls.Add") > -1)
                        {
                            break;
                        }
                    }
                    Code2Control(ctrlClass, strLine);
                    flag = false;
                }
            }
            SelectAllClear();
        }

        private void Code2Control(string ctrlClass, List<string> strLine)
        {
            string[] split;
            string[] split2;
            string strParent;

            split = strLine[strLine.Count - 1].Split("(");
            split = split[0].Split(".Controls.Add");
            split = split[0].Split("this");
            strParent = split[1];

            if (strParent == "")
            {
                _ = new cls_control(this, ctrlClass, this, backPanel!, toolList, propertyGrid!, propertyCtrlName!, 0, 0);
            }
            else
            {
                if (strParent.Substring(0, 1) == ".")
                {
                    strParent = strParent.Substring(1, strParent.Length - 1);
                }

                split2 = strParent.Split(".");
                if (split2.Count() == 2)
                {
                    CreateOnSplitContainer(ctrlClass, split2);
                }
                else
                {
                    CreateOnParent(ctrlClass, strParent);
                }
            }

            if (ctrlClass == "TabControl")
            {
                Delete(CtrlItems[CtrlItems.Count - 1]);
                Delete(CtrlItems[CtrlItems.Count - 1]);
                cnt_Control -= 2;
            }
            Code2Property(strLine, false);
        }

        private void CreateOnParent(string ctrlClass, string strParent)
        {
            for (int k = 0; k < CtrlItems.Count; k++)
            {
                if (CtrlItems[k].ctrl!.Name == strParent)
                {
                    _ = new cls_control(this, ctrlClass, CtrlItems[k].ctrl!, backPanel!, toolList, propertyGrid!, propertyCtrlName!, 0, 0);
                    break;
                }
            }
        }

        private void CreateOnSplitContainer(string ctrlClass, string[] split2)
        {
            string strParent = split2[0];

            for (int k = 0; k < CtrlItems.Count; k++)
            {
                if (CtrlItems[k].ctrl!.Name == strParent)
                {
                    SplitContainer? splcontainer = CtrlItems[k].ctrl as SplitContainer;
                    SplitterPanel splpanel;
                    if (split2[1] == "Panel1")
                    {
                        splpanel = splcontainer!.Panel1;
                        splpanel.Name = splcontainer.Name + ".Panel1";
                        _ = new cls_control(this, ctrlClass, splcontainer!.Panel1, backPanel!, toolList, propertyGrid!, propertyCtrlName!, 0, 0);
                    }
                    else
                    {
                        splpanel = splcontainer!.Panel2;
                        splpanel.Name = splcontainer.Name + ".Panel2";
                        _ = new cls_control(this, ctrlClass, splcontainer!.Panel2, backPanel!, toolList, propertyGrid!, propertyCtrlName!, 0, 0);
                    }
                    break;
                }
            }
        }

        private int Code2Form(List<string> source_custom)
        {
            int cnt = 0;
            List<string> formProperty = new();

            for (int i = 0; i < source_custom.Count; i++)
            {
                if (source_custom[i].IndexOf("this.") > -1)
                {
                    formProperty.Add(source_custom[i]);

                    if (source_custom[i + 1].IndexOf("this.") == -1)
                    {
                        Code2Property(formProperty, true);
                        this.Location = new Point(12, 12);
                        cnt = i + 1;
                        break;
                    }
                }
            }
            return cnt;
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

                if (ctrl.ctrl is SplitContainer)
                {
                    SplitContainer? splcontainer = ctrl.ctrl as SplitContainer;

                    for (int j = 0; j < CtrlItems.Count; j++)
                    {
                        if (splcontainer!.Panel1 == CtrlItems[j].ctrl!.Parent || splcontainer!.Panel2 == CtrlItems[j].ctrl!.Parent)
                        {
                            Delete(CtrlItems[j]);
                            i--;
                        }
                    }
                }
            }
            ctrl.Delete();
            CtrlItems.Remove(ctrl);
        }

        private void Code2Property(List<string> strLine, bool formFlag)
        {
            string[] split;
            string propertyName;
            string propertyValue;
            Control ctrl;
            Control form = new();

            if (formFlag)
            {
                ctrl = this;
                form = this.memForm;
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

                if (formFlag)
                {
                    SetCtrlProperty(form, propertyName, propertyValue);
                }
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
            string[] split = new string[1];
            string dummy;

            split = propertyValue.Split("(");
            dummy = split[1];
            split = dummy.Split(")");
            dummy = split[0];
            split = dummy.Split(",");
            Size size = new(int.Parse(split[0]), int.Parse(split[1]));
            return size;
        }

        private static Point String2Point(string propertyValue)
        {
            string[] split = new string[1];
            string dummy;

            split = propertyValue.Split("(");
            dummy = split[1];
            split = dummy.Split(")");
            dummy = split[0];
            split = dummy.Split(",");
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

        private static Color? String2Color(string? propertyValue)
        {
            Color color = new();
            string[] split;

            if (propertyValue!.IndexOf("FromArgb") > -1)
            {
                split = propertyValue!.Split("(");
                string strRGB = split[1].Trim().Replace(")", "");
                split = strRGB.Split(",");

                color = Color.FromArgb(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
            }
            else
            {
                split = propertyValue!.Split(".");
                color = Color.FromName(split[3]);
            }
            return color;
        }

        private static int? String2FormStartPosition(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.FormStartPosition") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Count() - 1];
            }

            switch (propertyValue)
            {
                case "CenterParent":
                    style = 4;
                    break;
                case "CenterScreen":
                    style = 1;
                    break;
                case "Manual":
                    style = 0;
                    break;
                case "WindowsDefaultBounds":
                    style = 3;
                    break;
                case "WindowsDefaultLocation":
                    style = 2;
                    break;
            }
            return style;
        }

        private static int? String2FormWindowState(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.FormWindowState") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Count() - 1];
            }

            switch (propertyValue)
            {
                case "Maximized":
                    style = 2;
                    break;
                case "Minimized":
                    style = 1;
                    break;
                case "Normal":
                    style = 0;
                    break;
            }
            return style;
        }

        private static int? String2FixedPanel(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.FixedPanel") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Count() - 1];
            }

            switch (propertyValue)
            {
                case "None":
                    style = 0;
                    break;
                case "Panel1":
                    style = 1;
                    break;
                case "Panel2":
                    style = 2;
                    break;
            }
            return style;
        }

        private static int? String2PictureBoxSizeMode(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.PictureBoxSizeMode") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Count() - 1];
            }

            switch (propertyValue)
            {
                case "AutoSize":
                    style = 2;
                    break;
                case "CenterImage":
                    style = 3;
                    break;
                case "Normal":
                    style = 0;
                    break;
                case "StretchImage":
                    style = 1;
                    break;
                case "Zoom":
                    style = 4;
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

                //Console.WriteLine(type);

                switch (type)
                {
                    case Type t when t == typeof(System.String):
                        property.SetValue(ctrl, propertyValue);
                        break;
                    case Type t when t == typeof(System.Boolean):
                        bool b = System.Convert.ToBoolean(propertyValue);
                        property.SetValue(ctrl, b);
                        break;
                    case Type t when t == typeof(System.Windows.Forms.DockStyle):
                        property.SetValue(ctrl, String2DockStyle(propertyValue));
                        break;
                    case Type t when t == typeof(System.Windows.Forms.AnchorStyles):
                        property.SetValue(ctrl, String2AnchorStyles(propertyValue!));
                        break;
                    case Type t when t == typeof(System.Drawing.Point):
                        property.SetValue(ctrl, String2Point(propertyValue!));
                        break;
                    case Type t when t == typeof(System.Drawing.Size):
                        property.SetValue(ctrl, String2Size(propertyValue!));
                        break;
                    case Type t when t == typeof(System.Int32):
                        property.SetValue(ctrl, int.Parse(propertyValue!));
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
                    case Type t when t == typeof(System.Drawing.Color):
                        property.SetValue(ctrl, String2Color(propertyValue));
                        break;
                    case Type t when t == typeof(System.Windows.Forms.FormStartPosition):
                        property.SetValue(ctrl, String2FormStartPosition(propertyValue));
                        break;
                    case Type t when t == typeof(System.Windows.Forms.FormWindowState):
                        property.SetValue(ctrl, String2FormWindowState(propertyValue));
                        break;
                    case Type t when t == typeof(System.Windows.Forms.FixedPanel):
                        property.SetValue(ctrl, String2FixedPanel(propertyValue));
                        break;
                    case Type t when t == typeof(System.Windows.Forms.PictureBoxSizeMode):
                        property.SetValue(ctrl, String2PictureBoxSizeMode(propertyValue));
                        break;
                }
            }
        }
    }
}
