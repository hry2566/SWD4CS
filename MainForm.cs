using System.Reflection;

namespace SWD4CS
{
    public partial class MainForm : Form
    {
        private List<string> source_base = new();
        private List<string> source_custom = new();
        private string sourceFileName = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cls_design_form1.Init(tabPage5, listBox1, dataGridView1);

            cls_file file = new();
            List<string>[] ret = file.NewFile();

            source_base = ret[0];
            source_custom = ret[1];

            EnableDoubleBuffering(dataGridView1);
        }

        private static void EnableDoubleBuffering(Control control)
        {
            control.GetType().InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               control,
               new object[] { true });
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cls_design_form1.RemoveSelectedItem();
            }
        }

        private void ReadrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cls_file file = new();
            List<string>[] ret = file.OpenFile();

            if (ret[2] != null)
            {
                source_base = ret[0];
                source_custom = ret[1];
                sourceFileName = ret[2][0];

                // 全コントロールクリア
                cls_design_form1.CtrlAllClear();

                cls_design_form1.CreateControl(source_custom);
            }
        }

        private void SaveSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cls_file file = new();

            tabControl3.SelectedIndex = 1;
            tabControl3.SelectedIndex = 0;

            if (sourceFileName != "")
            {
                // 上書き保存
                file.SaveAs(sourceFileName, textBox1.Text);
            }
            else
            {
                // 新規保存
                file.Save(textBox1.Text);
            }
        }

        private void closeCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl3.SelectedIndex == 1)
            {
                InitSourceCode();

                textBox1.Text = CreateSourcecCode();
            }
        }

        private void InitSourceCode()
        {
            for (int i = 2; i < source_custom.Count; i++)
            {
                source_custom.RemoveAt(i);
                if (source_custom[i] == "    }")
                {
                    break;
                }
                i--;
            }

            bool flag = false;

            for (int i = 0; i < source_custom.Count; i++)
            {
                if (source_custom[i] == "}" && flag)
                {
                    flag = false;
                }

                if (flag)
                {
                    source_custom.RemoveAt(i);
                    i--;
                }

                if (source_custom[i] == "    #endregion")
                {
                    flag = true;
                }
            }
        }

        private string CreateSourcecCode()
        {
            int w;
            int h;
            string text;
            string source = "";

            int insertPos = 2;
            int insertPos2 = 0;
            int itemCount = cls_design_form1.CtrlItems!.Count;

            w = cls_design_form1.Width;
            h = cls_design_form1.Height;
            text = cls_design_form1.Text;

            source_custom.Insert(insertPos, "        //");
            insertPos++;
            source_custom.Insert(insertPos, "        // Form1");
            insertPos++;
            source_custom.Insert(insertPos, "        //");
            insertPos++;
            source_custom.Insert(insertPos, "        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);");
            insertPos++;
            source_custom.Insert(insertPos, "        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;");
            insertPos++;
            source_custom.Insert(insertPos, "        this.ClientSize = new System.Drawing.Size(" + w + "," + h + ");");
            insertPos++;
            source_custom.Insert(insertPos, "        this.Text = \"" + text + "\";");
            insertPos++;
            source_custom.Insert(insertPos, "");
            insertPos++;

            for (int i = 0; i < itemCount; i++)
            {
                cls_control ctrl = cls_design_form1.CtrlItems[i];
                string ctrlClass = ctrl.className;
                string parentName = "." + ctrl.parent!.Name;

                if (parentName == ".cls_design_form1")
                {
                    parentName = "";
                }

                source_custom.Insert(insertPos, "        //");
                insertPos++;
                source_custom.Insert(insertPos, "        // " + ctrl!.ctrl!.Name);
                insertPos++;
                source_custom.Insert(insertPos, "        //");
                insertPos++;
                source_custom.Insert(insertPos, "        this." + ctrl.ctrl.Name + " = new System.Windows.Forms." + ctrlClass + "();");
                insertPos++;


                //property
                foreach (PropertyInfo item in ctrl.ctrl!.GetType().GetProperties())
                {
                    if (ctrl.HideProperty(item.Name))
                    {
                        Control? baseCtrl = new();

                        // ****************************************************************************************
                        // コントロール追加時に下記を編集すること
                        // ****************************************************************************************
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

                        }
                        // ****************************************************************************************

                        try
                        {
                            if (item.GetValue(ctrl.ctrl)!.ToString() != item.GetValue(baseCtrl)!.ToString())
                            {
                                type = item.GetValue(ctrl.ctrl)!.GetType();

                                string[] split;
                                string str1 = "        this." + ctrl!.ctrl!.Name + "." + item.Name;
                                string str2 = item.GetValue(ctrl.ctrl)!.ToString()!;

                                switch (type)
                                {
                                    case Type t when t == typeof(System.Drawing.Point):
                                        Point point = (Point)item.GetValue(ctrl.ctrl)!;
                                        source_custom.Insert(insertPos, str1 + " = new " + type.ToString() + "(" + point.X + "," + point.Y + ");");
                                        insertPos++;
                                        break;

                                    case Type t when t == typeof(System.Drawing.Size):
                                        Size size = (Size)item.GetValue(ctrl.ctrl)!;
                                        source_custom.Insert(insertPos, str1 + " = new " + type.ToString() + "(" + size.Width + "," + size.Height + ");");
                                        insertPos++;
                                        break;

                                    case Type t when t == typeof(System.String):
                                        source_custom.Insert(insertPos, str1 + " =  " + "\"" + str2 + "\";");
                                        insertPos++;
                                        break;

                                    case Type t when t == typeof(System.Boolean):
                                        source_custom.Insert(insertPos, str1 + " =  " + str2.ToLower() + ";");
                                        insertPos++;
                                        break;

                                    case Type t when t == typeof(System.Windows.Forms.DockStyle):
                                        source_custom.Insert(insertPos, str1 + " =  " + type.ToString() + "." + str2 + ";");
                                        insertPos++;
                                        break;

                                    case Type t when t == typeof(System.Windows.Forms.AnchorStyles):
                                        split = item.GetValue(ctrl.ctrl)!.ToString()!.Split(',');

                                        if (split.Length == 1)
                                        {
                                            source_custom.Insert(insertPos, str1 + " = " + type.ToString() + "." + str2 + ";");
                                            insertPos++;
                                            break;
                                        }
                                        else
                                        {
                                            string ancho = "";

                                            for (int j = 0; j < split.Length; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    ancho = type.ToString() + "." + split[j].Trim();
                                                }
                                                else
                                                {
                                                    ancho = "(" + ancho + " | " + type.ToString() + "." + split[j].Trim() + ")";
                                                }
                                            }

                                            ancho = "(" + type.ToString() + ")" + ancho + ";";
                                            source_custom.Insert(insertPos, str1 + " = " + ancho);
                                            insertPos++;
                                            break;
                                        }

                                    case Type t when t == typeof(System.Int32):
                                        source_custom.Insert(insertPos, str1 + " = " + int.Parse(str2) + ";");
                                        insertPos++;
                                        break;

                                    case Type t when t == typeof(System.Drawing.ContentAlignment):
                                        source_custom.Insert(insertPos, str1 + " = " + type.ToString() + "." + str2 + ";");
                                        insertPos++;
                                        break;

                                    case Type t when t == typeof(System.Windows.Forms.ScrollBars):
                                        source_custom.Insert(insertPos, str1 + " = " + type.ToString() + "." + str2 + ";");
                                        insertPos++;
                                        break;
                                }
                            }
                        }
                        catch { }
                    }
                }

                source_custom.Insert(insertPos, "        this" + parentName + ".Controls.Add(this." + ctrl!.ctrl!.Name + ");\r\n");
                insertPos++;

                source_custom.Insert(3 + insertPos + insertPos2, "    private " + ctrlClass + " " + ctrl!.ctrl!.Name + ";");
                insertPos2++;
            }

            for (int i = 0; i < source_base.Count; i++)
            {
                source += source_base[i] + "\r\n";
            }
            for (int i = 0; i < source_custom.Count; i++)
            {
                source += source_custom[i] + "\r\n"; ;
            }

            return source;
        }
    }
}