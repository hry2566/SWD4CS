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

            List<string>[] ret = cls_file.NewFile();

            source_base = ret[0];
            source_custom = ret[1];
            cls_design_form1.Init(tabPage5, listBox1, dataGridView1);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.Delete && tabControl3.SelectedIndex == 0)
            {
                cls_design_form1.RemoveSelectedItem();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeyEventArgs keyevents = new(Keys.Alt | Keys.Delete);
            MainForm_KeyDown(sender, keyevents);
        }

        private void ReadrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string>[] ret = cls_file.OpenFile();

            if (ret[2] != null)
            {
                source_base = ret[0];
                source_custom = ret[1];
                sourceFileName = ret[2][0];
                cls_design_form1.CtrlAllClear();
                cls_design_form1.CreateControl(source_custom);
            }
        }

        private void SaveSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cls_file file = new();

            tabControl3.SelectedIndex = 1;
            tabControl3.SelectedIndex = 0;

            if (sourceFileName != "")
            {
                cls_file.SaveAs(sourceFileName, textBox1.Text);
            }
            else
            {
                cls_file.Save(textBox1.Text);
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
            bool flag = false;

            for (int i = 2; i < source_custom.Count; i++)
            {
                source_custom.RemoveAt(i);
                if (source_custom[i] == "    }")
                {
                    break;
                }
                i--;
            }

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

                foreach (PropertyInfo item in ctrl.ctrl!.GetType().GetProperties())
                {
                    if (cls_control.HideProperty(item.Name))
                    {
                        Control? baseCtrl = cls_form.GetBaseCtrl(ctrl);

                        if (item.GetValue(ctrl.ctrl) != null && item.GetValue(ctrl.ctrl)!.ToString() != item.GetValue(baseCtrl)!.ToString())
                        {
                            string str1 = "        this." + ctrl!.ctrl!.Name + "." + item.Name;
                            string strProperty = Property2String(ctrl, item);

                            if (strProperty != "")
                            {
                                source_custom.Insert(insertPos, str1 + Property2String(ctrl, item));
                                insertPos++;
                            }
                        }
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

        private string AnchorStyles2String(object? propertyinfo)
        {
            string strProperty;
            string[] split = propertyinfo!.ToString()!.Split(',');
            Type type = propertyinfo.GetType();
            string str2 = propertyinfo.ToString()!;

            if (split.Length == 1)
            {
                strProperty = " = " + type.ToString() + "." + str2 + ";";
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
                strProperty = " = " + ancho;
            }
            return strProperty;
        }

        private string Property2String(cls_control ctrl, PropertyInfo item)
        {
            string strProperty = "";
            Type type = item.GetValue(ctrl.ctrl)!.GetType();
            string str2 = item.GetValue(ctrl.ctrl)!.ToString()!;

            switch (type)
            {
                case Type t when t == typeof(System.Drawing.Point):
                    Point point = (Point)item.GetValue(ctrl.ctrl)!;
                    strProperty = " = new " + type.ToString() + "(" + point.X + "," + point.Y + ");";
                    break;
                case Type t when t == typeof(System.Drawing.Size):
                    Size size = (Size)item.GetValue(ctrl.ctrl)!;
                    strProperty = " = new " + type.ToString() + "(" + size.Width + "," + size.Height + ");";
                    break;
                case Type t when t == typeof(System.String):
                    strProperty = " =  " + "\"" + str2 + "\";";
                    break;
                case Type t when t == typeof(System.Boolean):
                    strProperty = " =  " + str2.ToLower() + ";";
                    break;
                case Type t when t == typeof(System.Windows.Forms.AnchorStyles):
                    strProperty = AnchorStyles2String(item.GetValue(ctrl.ctrl));
                    break;
                case Type t when t == typeof(System.Int32):
                    strProperty = " = " + int.Parse(str2) + ";";
                    break;
                case Type t when t == typeof(System.Windows.Forms.DockStyle) |
                                 t == typeof(System.Drawing.ContentAlignment) |
                                 t == typeof(System.Windows.Forms.ScrollBars) |
                                 t == typeof(System.Windows.Forms.HorizontalAlignment):

                    strProperty = " = " + type.ToString() + "." + str2 + ";";
                    break;
            }
            return strProperty;
        }
    }
}