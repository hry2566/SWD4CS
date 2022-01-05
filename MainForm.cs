using System.Collections;

namespace SWD4CS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cls_design_form1.Init(tabPage5, listBox1, dataGridView1);

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cls_design_form1.RemoveSelectedItem();
            }
        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code1 = "\r\n";
            string code2 = "\r\n";
            string code3 = "\r\n";
            string code4 = "\r\n";
            string code5 = "\r\n";
            string code6 = "\r\n";
            string name = "";
            int x;
            int y;
            int w;
            int h;
            int tabindex;
            string text = "";

            if (tabControl3.SelectedIndex == 1)
            {

                // create source code

                w = cls_design_form1.Width;
                h = cls_design_form1.Height;
                text = cls_design_form1.Text;
                code1 += "        //\r\n";
                code1 += "        // Form1\r\n";
                code1 += "        //\r\n";
                code1 += "        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);\r\n";
                code1 += "        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;\r\n";
                code1 += "        this.ClientSize = new System.Drawing.Size(" + w + "," + h + ");\r\n";
                code1 += "        this.Text = \"" + text + "\";\r\n";
                code1 += "\r\n";

                code2 += "    //\r\n";
                code2 += "    private void InitializeComponent()\r\n";
                code2 += "    {\r\n";

                code4 += "    }\r\n";
                code4 += "\r\n";
                code4 += "    #endregion\r\n";
                code4 += "\r\n";

                int itemCount = cls_design_form1.CtrlItems!.Count;
                for (int i = 0; i < itemCount; i++)
                {
                    Control? ctrl = cls_design_form1.CtrlItems[i] as Control;
                    string ctrlClass = "";
                    string parentName = ".";

                    // ****************************************************************************************
                    // コントロール追加時に下記を編集すること
                    // ****************************************************************************************
                    if (cls_design_form1.CtrlItems[i] is cls_button)
                    {
                        cls_button? parent = cls_design_form1.CtrlItems[i] as cls_button;
                        ctrlClass = "Button";
                        parentName += parent!.parentCtrl.Name;
                    }
                    else if (cls_design_form1.CtrlItems[i] is cls_label)
                    {
                        cls_label? parent = cls_design_form1.CtrlItems[i] as cls_label;
                        ctrlClass = "Label";
                        parentName += parent!.parentCtrl.Name;
                    }
                    else if (cls_design_form1.CtrlItems[i] is cls_textbox)
                    {
                        cls_textbox? parent = cls_design_form1.CtrlItems[i] as cls_textbox;
                        ctrlClass = "TextBox";
                        parentName += parent!.parentCtrl.Name;
                    }
                    else if (cls_design_form1.CtrlItems[i] is cls_listbox)
                    {
                        cls_listbox? parent = cls_design_form1.CtrlItems[i] as cls_listbox;
                        ctrlClass = "ListBox";
                        parentName += parent!.parentCtrl.Name;
                    }
                    else if (cls_design_form1.CtrlItems[i] is cls_groupbox)
                    {
                        cls_groupbox? parent = cls_design_form1.CtrlItems[i] as cls_groupbox;
                        ctrlClass = "GroupBox";
                        parentName += parent!.parentCtrl.Name;
                    }
                    else if (cls_design_form1.CtrlItems[i] is cls_tabcontrol)
                    {
                        cls_tabcontrol? parent = cls_design_form1.CtrlItems[i] as cls_tabcontrol;
                        ctrlClass = "TabControl";
                        parentName += parent!.parentCtrl.Name;
                    }
                    else if (cls_design_form1.CtrlItems[i] is cls_tabpage)
                    {
                        cls_tabpage? parent = cls_design_form1.CtrlItems[i] as cls_tabpage;
                        ctrlClass = "TabPage";
                        parentName += parent!.parentCtrl.Name;
                    }

                    // ****************************************************************************************

                    if (parentName == ".cls_design_form1")
                    {
                        parentName = "";
                    }

                    name = ctrl!.Name;
                    x = ctrl.Left;
                    y = ctrl.Top;
                    w = ctrl.Width;
                    h = ctrl.Height;
                    tabindex = ctrl.TabIndex;
                    text = ctrl.Text;

                    code3 += "        //\r\n";
                    code3 += "        // " + name + "\r\n";
                    code3 += "        //\r\n";
                    code3 += "        this." + name + ".Location = new System.Drawing.Point(" + x + "," + y + ");\r\n";
                    code3 += "        this." + name + ".Name = \"" + name + "\";\r\n";
                    code3 += "        this." + name + ".Size = new System.Drawing.Size(" + w + "," + h + ");\r\n";
                    code3 += "        this." + name + ".TabIndex = " + tabindex + ";\r\n";
                    code3 += "        this." + name + ".Text = \"" + text + "\";\r\n";
                    code3 += "        this" + parentName + ".Controls.Add(this." + name + ");\r\n";

                    code5 += "    private " + ctrlClass + " " + name + ";\r\n";

                    code6 += "        this." + ctrl.Name + " = new System.Windows.Forms." + ctrlClass + "();\r\n";

                }

                textBox1.Text = code2 + code6 + code3 + code1 + code4 + code5;

            }
        }
    }
}