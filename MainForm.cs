namespace SWD4CS
{
    public partial class MainForm : Form
    {
        List<string> source_base = new List<string>();
        List<string> source_custom = new List<string>();
        string sourceFileName = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cls_design_form1.Init(tabPage5, listBox1, dataGridView1);

            cls_file file = new cls_file();
            List<string>[] ret = file.NewFile();
            source_base = ret[0];
            source_custom = ret[1];
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
            string name;
            int x;
            int y;
            int w;
            int h;
            int tabindex;
            string text;

            if (tabControl3.SelectedIndex == 1)
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

                int insertPos = 2;
                int insertPos2 = 0;
                int itemCount = cls_design_form1.CtrlItems!.Count;
                for (int i = 0; i < itemCount; i++)
                {
                    cls_control ctrl = cls_design_form1.CtrlItems[i];
                    string ctrlClass = "";
                    string parentName = ".";

                    ctrlClass = ctrl.className;
                    parentName += ctrl.parent!.Name;

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

                    source_custom.Insert(insertPos, "        //");
                    insertPos++;
                    source_custom.Insert(insertPos, "        // " + name);
                    insertPos++;
                    source_custom.Insert(insertPos, "        //");
                    insertPos++;
                    source_custom.Insert(insertPos, "        this." + ctrl.Name + " = new System.Windows.Forms." + ctrlClass + "();");
                    insertPos++;
                    source_custom.Insert(insertPos, "        this." + name + ".Location = new System.Drawing.Point(" + x + "," + y + ");");
                    insertPos++;
                    source_custom.Insert(insertPos, "        this." + name + ".Name = \"" + name + "\";");
                    insertPos++;
                    source_custom.Insert(insertPos, "        this." + name + ".Size = new System.Drawing.Size(" + w + "," + h + ");");
                    insertPos++;
                    source_custom.Insert(insertPos, "        this." + name + ".TabIndex = " + tabindex + ";");
                    insertPos++;
                    source_custom.Insert(insertPos, "        this." + name + ".Text = \"" + text + "\";");
                    insertPos++;
                    source_custom.Insert(insertPos, "        this" + parentName + ".Controls.Add(this." + name + ");\r\n");
                    insertPos++;

                    source_custom.Insert(3 + insertPos + insertPos2, "    private " + ctrlClass + " " + name + ";");
                    insertPos2++;
                }

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

                string source = "";
                for (int i = 0; i < source_base.Count; i++)
                {
                    source += source_base[i] + "\r\n";
                }
                for (int i = 0; i < source_custom.Count; i++)
                {
                    source += source_custom[i] + "\r\n"; ;
                }

                textBox1.Text = source;
            }
        }

        private void readrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cls_file file = new cls_file();
            //List<string>[] ret = file.OpenFile();

            //if (ret[2] != null)
            //{
            //    source_base = ret[0];
            //    source_custom = ret[1];
            //    sourceFileName = ret[2][0];

            //    // コントロール全クリア
            //    cls_design_form1.CtrlAllClear();






            //    //string source = "";
            //    //for (int i = 0; i < source_base.Count; i++)
            //    //{
            //    //    source += source_base[i] + "\r\n";
            //    //}
            //    //for (int i = 0; i < source_custom.Count; i++)
            //    //{
            //    //    source += source_custom[i] + "\r\n"; ;
            //    //}

            //    //textBox2.Text = source;
            //}

        }

        private void saveSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cls_file file = new cls_file();

            tabControl3.SelectedIndex = 1;

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
    }
}