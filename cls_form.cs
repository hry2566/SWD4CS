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
            string? propertyName = "";
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
                    this.Width = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Size.Height")
                {
                    this.Height = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Text")
                {
                    this.Text = propertyValue;
                }
                return;
            }
            else
            {
                if (propertyName == "Name")
                {
                    CtrlItems[i].Name = propertyValue!;
                }
                else if (propertyName == "Location.X")
                {
                    CtrlItems[i].Left = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Location.Y")
                {
                    CtrlItems[i].Top = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Size.Width")
                {
                    CtrlItems[i].Width = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Size.Height")
                {
                    CtrlItems[i].Height = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Text")
                {
                    CtrlItems[i].Text = propertyValue!;
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
                propertyList!.Rows.Clear();
                propertyList.Rows.Add("Name", "From1");
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
            for (int i = 0; i < source_custom.Count; i++)
            {
                string[] split;
                string dummy;
                string ctrlClass;
                int locationX;
                int locationY;
                string ctrlName;
                int sizeX;
                int sizeY;
                int tabIndex;
                string ctrlText;
                string ctrlParent;

                if (source_custom[i].IndexOf("new System.Windows.Forms.") > -1)
                {
                    split = source_custom[i].Split('.');
                    dummy = split[split.Count() - 1];
                    ctrlClass = dummy.Substring(0, dummy.IndexOf('('));

                    split = source_custom[i + 1].Split('(');
                    dummy = split[split.Count() - 1];
                    split = dummy.Split(',');
                    locationX = int.Parse(split[0]);
                    dummy = split[1];
                    locationY = int.Parse(dummy.Substring(0, dummy.IndexOf(')')));

                    split = source_custom[i + 2].Split('"');
                    ctrlName = split[split.Count() - 2];

                    split = source_custom[i + 3].Split('(');
                    dummy = split[split.Count() - 1];
                    split = dummy.Split(',');
                    sizeX = int.Parse(split[0]);
                    dummy = split[1];
                    sizeY = int.Parse(dummy.Substring(0, dummy.IndexOf(')')));

                    split = source_custom[i + 4].Split('=');
                    dummy = split[split.Count() - 1];
                    split = dummy.Split(';');
                    dummy = split[0];
                    tabIndex = int.Parse(dummy);

                    split = source_custom[i + 5].Split('"');
                    ctrlText = split[split.Count() - 2];

                    split = source_custom[i + 6].Split('.');
                    ctrlParent = split[1];

                    if (ctrlParent == "Controls")
                    {
                        _ = new cls_control(this, ctrlClass, this, backPanel!, toolList, propertyList!, locationX, locationY);
                        if (ctrlClass == "TabControl")
                        {
                            Delete(CtrlItems[CtrlItems.Count - 1]);
                            Delete(CtrlItems[CtrlItems.Count - 1]);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < CtrlItems.Count; j++)
                        {
                            if (CtrlItems[j].Name == ctrlParent)
                            {
                                _ = new cls_control(this, ctrlClass, CtrlItems[j].ctrl!, backPanel!, toolList, propertyList!, locationX, locationY);
                                if (ctrlClass == "TabControl")
                                {
                                    Delete(CtrlItems[CtrlItems.Count - 1]);
                                    Delete(CtrlItems[CtrlItems.Count - 1]);
                                }
                                break;
                            }
                        }
                    }

                    CtrlItems[CtrlItems.Count - 1].Name = ctrlName;
                    CtrlItems[CtrlItems.Count - 1].Width = sizeX;
                    CtrlItems[CtrlItems.Count - 1].Height = sizeY;
                    CtrlItems[CtrlItems.Count - 1].TabIndex = tabIndex;
                    CtrlItems[CtrlItems.Count - 1].Text = ctrlText;
                }

                if (source_custom[i].IndexOf("this.ClientSize") > -1)
                {
                    split = source_custom[i].Split('(');
                    dummy = split[split.Count() - 1];
                    split = dummy.Split(',');
                    sizeX = int.Parse(split[0]);
                    dummy = split[1];
                    sizeY = int.Parse(dummy.Substring(0, dummy.IndexOf(')')));

                    split = source_custom[i + 1].Split('"');
                    ctrlText = split[split.Count() - 2];

                    this.Width = sizeX;
                    this.Height = sizeY;
                    this.Text = ctrlText;
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
