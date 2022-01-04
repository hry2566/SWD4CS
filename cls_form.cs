
using System.Collections;
using System.Data;

namespace SWD4CS
{
    public partial class cls_form : Panel
    {
        public ArrayList CtrlItems = new ArrayList();
        private cls_selectbox? selectBox;
        private Control? backPanel;
        private ListBox? toolList;
        private DataGridView? propertyList;
        private bool selectFlag = false;
        private int grid = 8;
        private int cnt_Button;
        private int cnt_Label;
        private int cnt_TextBox;
        private int cnt_ListBox;

        public cls_form()
        {
            InitializeComponent();
        }

        public void Init(Control backPanel, ListBox toolList, DataGridView dataGridView1)
        {
            this.backPanel = backPanel;
            this.toolList = toolList;
            this.propertyList = dataGridView1;

            backPanel.Click += new System.EventHandler(Backpanel_Click);
            this.Click += new System.EventHandler(Form_Click);
            this.propertyList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);


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
                Control? item = CtrlItems[i] as Control;

                if (propertyName == "Name")
                {
                    item!.Name = propertyValue;
                }
                else if (propertyName == "Location.X")
                {
                    item!.Left = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Location.Y")
                {
                    item!.Top = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Size.Width")
                {
                    item!.Width = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Size.Height")
                {
                    item!.Height = Int32.Parse(propertyValue!);
                }
                else if (propertyName == "Text")
                {
                    item!.Text = propertyValue;
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

                AddControl(X, Y);

                toolList.SelectedIndex = -1;
            }

        }

        internal void SelectAllClear()
        {
            SetSelect(false);

            for (int i = 0; i < CtrlItems.Count; i++)
            {
                ControlCom(i, 0, 0);
            }
        }

        internal void RemoveSelectedItem()
        {
            for (int i = 0; i < CtrlItems.Count; i++)
            {
                if (ControlCom(i, 1, 0))
                {
                    i--;
                }
            }
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
                for (int i = 0; i < CtrlItems.Count; i++)
                {
                    if (ControlCom(i, 2, index))
                    {
                        break;
                    }
                }
            }
        }

        // ****************************************************************************************
        // コントロール追加時に下記を編集すること
        // ****************************************************************************************
        private void AddControl(int X, int Y)
        {
            if (toolList!.Text == "Button")
            {
                cls_button ctrl = new cls_button(this, backPanel!, propertyList!, cnt_Button, X, Y);
                cnt_Button++;
            }
            else if (toolList.Text == "Label")
            {
                cls_label ctrl = new cls_label(this, backPanel!, propertyList!, cnt_Label, X, Y);
                cnt_Label++;
            }
            else if (toolList.Text == "TextBox")
            {
                cls_textbox ctrl = new cls_textbox(this, backPanel!, propertyList!, cnt_TextBox, X, Y);
                cnt_TextBox++;
            }
            else if (toolList.Text == "ListBox")
            {
                cls_listbox ctrl = new cls_listbox(this, backPanel!, propertyList!, cnt_TextBox, X, Y);
                cnt_ListBox++;
            }
        }
        
        private bool ControlCom(int i, int mode, int index)
        {
            if (CtrlItems[i] is cls_button)
            {
                cls_button? ctrl = CtrlItems[i] as cls_button;

                if (mode == 0)
                {
                    ctrl!.ctrlBase.SetSelect(false);
                }
                else
                {
                    if (ctrl!.ctrlBase.GetSelected())
                    {
                        if (mode == 1)
                        {
                            ctrl.Deleate(this, ctrl);
                            CtrlItems.Remove(ctrl);
                        }
                        else if (mode == 2)
                        {
                            SetProperty(i, index, false);
                            ctrl.ctrlBase.SetSelect(true);
                        }
                        return true;
                    }
                }
            }
            else if (CtrlItems[i] is cls_label)
            {
                cls_label? ctrl = CtrlItems[i] as cls_label;
                if (mode == 0)
                {
                    ctrl!.ctrlBase.SetSelect(false);
                }
                else
                {
                    if (ctrl!.ctrlBase.GetSelected())
                    {
                        if (mode == 1)
                        {
                            ctrl.Deleate(this, ctrl);
                            CtrlItems.Remove(ctrl);
                        }
                        else if (mode == 2)
                        {
                            SetProperty(i, index, false);
                            ctrl.ctrlBase.SetSelect(true);
                        }
                        return true;
                    }
                }
            }
            else if (CtrlItems[i] is cls_textbox)
            {
                cls_textbox? ctrl = CtrlItems[i] as cls_textbox;
                if (mode == 0)
                {
                    ctrl!.ctrlBase.SetSelect(false);
                }
                else
                {
                    if (ctrl!.ctrlBase.GetSelected())
                    {
                        if (mode == 1)
                        {
                            ctrl.Deleate(this, ctrl);
                            CtrlItems.Remove(ctrl);
                        }
                        else if (mode == 2)
                        {
                            SetProperty(i, index, false);
                            ctrl.ctrlBase.SetSelect(true);
                        }
                        return true;
                    }
                }
            }
            else if (CtrlItems[i] is cls_listbox)
            {
                cls_listbox? ctrl = CtrlItems[i] as cls_listbox;
                if (mode == 0)
                {
                    ctrl!.ctrlBase.SetSelect(false);
                }
                else
                {
                    if (ctrl!.ctrlBase.GetSelected())
                    {
                        if (mode == 1)
                        {
                            ctrl.Deleate(this, ctrl);
                            CtrlItems.Remove(ctrl);
                        }
                        else if (mode == 2)
                        {
                            SetProperty(i, index, false);
                            ctrl.ctrlBase.SetSelect(true);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        // ****************************************************************************************
    }
}
