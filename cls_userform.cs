using System.Reflection;

namespace SWD4CS
{
    internal class cls_userform : Panel
    {
        internal MainForm? mainForm;
        internal List<cls_controls> CtrlItems = new();
        private cls_selectbox? selectBox;
        private bool selectFlag = false;
        internal Form memForm = new();
        internal TabPage? backPanel;
        private int grid = 8;
        internal List<string> decHandler = new();
        internal List<string> decFunc = new();

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
        internal int cnt_MaskedTextBox;
        internal int cnt_PropertyGrid;
        internal int cnt_DateTimePicker;
        internal int cnt_DomainUpDown;
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
            cnt_MaskedTextBox = 0;
            cnt_PropertyGrid = 0;
            cnt_DateTimePicker = 0;
            cnt_DomainUpDown = 0;
        }
        // ****************************************************************************************

        internal void Init(MainForm mainForm, TabPage backPanel)
        {
            this.mainForm = mainForm;
            this.backPanel = backPanel;
            this.Click += new System.EventHandler(Form_Click);
            this.Resize += new System.EventHandler(formResize);
            backPanel.Click += new System.EventHandler(Backpanel_Click);

            mainForm.propertyGrid!.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(PropertyValueChanged);
            memForm.Location = this.Location;
            memForm.ClientSize = this.Size;
            memForm.Name = "Form1";

            selectBox = new cls_selectbox(this, backPanel);
            SetSelect(true);
        }
        private void Backpanel_Click(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                SelectAllClear();
            }
        }
        private void PropertyValueChanged(object? s, PropertyValueChangedEventArgs e)
        {
            Control? ctrl = mainForm!.propertyGrid!.SelectedObject as Control;

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

        private void formResize(object? sender, EventArgs e)
        {
            memForm.ClientSize = this.Size;
        }
        private void Form_Click(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (mainForm!.toolLstBox!.Text == "")
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
                _ = new cls_controls(this, mainForm!.toolLstBox!.Text, this, X, Y);
                mainForm!.toolLstBox!.SelectedIndex = -1;
            }
        }
        private void Delete(cls_controls ctrl)
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
        internal void CtrlAllClear()
        {
            CountInit();

            for (int i = 0; i < CtrlItems!.Count; i++)
            {
                CtrlItems[i].Selected = true;
            }
            RemoveSelectedItem();
        }
        internal void Add_Controls(List<CONTROL_INFO> ctrlInfo)
        {
            // コントロール全削除
            CtrlAllClear();

            // Control作成
            for (int i = 0; i < ctrlInfo.Count; i++)
            {
                _ = new cls_controls(this, memForm, ctrlInfo[i]);
            }
            this.Location = new Point(16, 16);

            // その他設定

            for (int i = 0; i < ctrlInfo.Count; i++)
            {
                for (int j = 0; j < CtrlItems.Count; j++)
                {
                    if (CtrlItems[j].ctrl != null)
                    {
                        if (CtrlItems[j].ctrl!.Name == ctrlInfo[i].ctrlName || ctrlInfo[i].ctrlName == "this")
                        {
                            CtrlItems[j].SetControls(ctrlInfo[i]);
                        }
                    }
                }
            }
            // selectbox設定
            for (int j = 0; j < CtrlItems.Count; j++)
            {
                CtrlItems[j].InitSelectBox();
            }

            SelectAllClear();
        }
        internal void SetSelect(bool flag)
        {
            selectFlag = flag;
            selectBox!.SetSelectBoxPos(selectFlag);
            ShowProperty(flag);
            if (flag)
            {
                mainForm!.ctrlTree!.SelectedNode = mainForm.ctrlTree.TopNode;
            }
            mainForm!.eventView!.ShowEventList(flag, this);
        }
        internal void SelectAllClear()
        {
            SetSelect(false);

            for (int i = 0; i < CtrlItems!.Count; i++)
            {
                CtrlItems[i].Selected = false;
            }
        }
        private void ShowProperty(bool flag)
        {
            if (flag)
            {
                mainForm!.propertyGrid!.SelectedObject = this.memForm;
                mainForm.propertyCtrlName!.Text = this.memForm.Name;
            }
            else
            {
                mainForm!.propertyGrid!.SelectedObject = null;
                mainForm.propertyCtrlName!.Text = "";
            }
        }
    }
}
