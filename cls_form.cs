
using System.Collections;

namespace SWD4CS
{
    public partial class cls_form : Panel
    {
        public ArrayList CtrlItems = new ArrayList();
        private cls_selectbox selectBox;
        private Control backPanel;
        private ListBox toolList;
        private bool selectFlag = false;
        private int grid = 8;
        private int cnt_Button;
        private int cnt_Label;
        private int cnt_TextBox;
        

        public cls_form()
        {
            InitializeComponent();
        }

        public void Init(Control backPanel, ListBox toolList)
        {
            this.backPanel = backPanel;
            this.toolList = toolList;

            backPanel.Click += new System.EventHandler(Backpanel_Click);
            this.Click += new System.EventHandler(Form_Click);

            selectBox = new cls_selectbox(this, backPanel);
        }

        private void Backpanel_Click(object sender, EventArgs e)
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
            selectBox.SetSelectBoxPos(selectFlag);
        }

        private void Form_Click(object sender, EventArgs e)
        {
            if (toolList.Text == "")
            {
                MouseEventArgs me = (MouseEventArgs)e;

                if (me.Button == MouseButtons.Left)
                {
                    if (selectFlag)
                    {
                        SelectAllClear();
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

                // Add Button
                int X = (int)(me.X / grid) * grid;
                int Y = (int)(me.Y / grid) * grid;

                if (toolList.Text == "Button")
                {
                    cls_button ctrl = new cls_button(this, backPanel, cnt_Button, X, Y);
                    cnt_Button++;
                }
                else if (toolList.Text == "Label")
                {
                    cls_label ctrl = new cls_label(this, backPanel, cnt_Label, X, Y);
                    cnt_Label++;
                }
                else if (toolList.Text == "TextBox")
                {
                    cls_textbox ctrl = new cls_textbox(this, backPanel, cnt_TextBox, X, Y);
                    cnt_TextBox++;
                }

                // unselect
                SelectAllClear(true);

                toolList.SelectedIndex = -1;

                SetSelect(false);
            }

        }

        private void SelectAllClear(bool flag = false)
        {
            int cnt = 0;

            if (flag)
            {
                cnt = 1;
            }

            for (int i = 0; i < CtrlItems.Count - cnt; i++)
            {
                if (CtrlItems[i].GetType().ToString() == "SWD4CS.cls_button")
                {
                    cls_button ctrl = (cls_button)CtrlItems[i];
                    ctrl.ctrlBase.SetSelect(false);
                }
                else if (CtrlItems[i].GetType().ToString() == "SWD4CS.cls_label")
                {
                    cls_label ctrl = (cls_label)CtrlItems[i];
                    ctrl.ctrlBase.SetSelect(false);
                }
                else if (CtrlItems[i].GetType().ToString() == "SWD4CS.cls_textbox")
                {
                    cls_textbox ctrl = (cls_textbox)CtrlItems[i];
                    ctrl.ctrlBase.SetSelect(false);
                }
            }
        }

        public void RemoveSelectedItem()
        {
            for (int i = 0; i < CtrlItems.Count; i++)
            {
                if (CtrlItems[i].GetType().ToString() == "SWD4CS.cls_button")
                {
                    cls_button ctrl = (cls_button)CtrlItems[i];
                    if (ctrl.ctrlBase.GetSelected())
                    {
                        ctrl.Deleate(this, ctrl);
                        CtrlItems.Remove(ctrl);
                        i = -1;
                    }
                }
                else if (CtrlItems[i].GetType().ToString() == "SWD4CS.cls_label")
                {
                    cls_label ctrl = (cls_label)CtrlItems[i];
                    if (ctrl.ctrlBase.GetSelected())
                    {
                        ctrl.Deleate(this, ctrl);
                        CtrlItems.Remove(ctrl);
                        i = -1;
                    }
                }
                else if (CtrlItems[i].GetType().ToString() == "SWD4CS.cls_textbox")
                {
                    cls_textbox ctrl = (cls_textbox)CtrlItems[i];
                    if (ctrl.ctrlBase.GetSelected())
                    {
                        ctrl.Deleate(this, ctrl);
                        CtrlItems.Remove(ctrl);
                        i = -1;
                    }
                }
            }
        }
    }
}
