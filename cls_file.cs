namespace SWD4CS
{
    internal class cls_file
    {
        //public cls_file()
        //{

        //}

        internal static List<string>[] NewFile()
        {
            List<string> source_base = new();
            List<string> source_custom = new();
            List<string>[] ret = new List<string>[2];

            source_base.Add("namespace WinFormsApp;");
            source_base.Add("");
            source_base.Add("partial class Form1");
            source_base.Add("{");
            source_base.Add("    /// <summary>");
            source_base.Add("    ///  Required designer variable.");
            source_base.Add("    /// </summary>");
            source_base.Add("    private System.ComponentModel.IContainer components = null;");
            source_base.Add("");
            source_base.Add("    /// <summary>");
            source_base.Add("    ///  Clean up any resources being used.");
            source_base.Add("    /// </summary>");
            source_base.Add("    /// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>");
            source_base.Add("    protected override void Dispose(bool disposing)");
            source_base.Add("    {");
            source_base.Add("        if (disposing && (components != null))");
            source_base.Add("        {");
            source_base.Add("            components.Dispose();");
            source_base.Add("        }");
            source_base.Add("        base.Dispose(disposing);");
            source_base.Add("    }");
            source_base.Add("");
            source_base.Add("    #region Windows Form Designer generated code");
            source_base.Add("");
            source_base.Add("    /// <summary>");
            source_base.Add("    ///  Required method for Designer support - do not modify");
            source_base.Add("    ///  the contents of this method with the code editor.");
            source_base.Add("    /// </summary>");
            source_custom.Add("    private void InitializeComponent()");
            source_custom.Add("    {");
            source_custom.Add("        this.components = new System.ComponentModel.Container();");
            source_custom.Add("        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;");
            source_custom.Add("        this.ClientSize = new System.Drawing.Size(160, 80);");
            source_custom.Add("        this.Text = \"Form1\";");
            source_custom.Add("    }");
            source_custom.Add("");
            source_custom.Add("    #endregion");
            source_custom.Add("}");

            ret[0] = source_base;
            ret[1] = source_custom;

            return ret;
        }
        internal static List<string>[] CommandLine(string fName)
        {
            List<string> source_base = new List<string>();
            List<string> source_custom = new List<string>();
            List<string> fileName = new List<string>();
            List<string>[] ret = new List<string>[3];
            bool flag = true;

            if (fName.IndexOf(".Designer.cs") == -1) { return ret; }

            foreach (string line in System.IO.File.ReadLines(fName))
            {
                if (line.IndexOf("private void InitializeComponent()") > 1)
                {
                    flag = false;
                }

                if (flag)
                {
                    source_base.Add(line);
                }
                else
                {
                    source_custom.Add(line);
                }
            }

            fileName.Add(fName);
            ret[0] = source_base;
            ret[1] = source_custom;
            ret[2] = fileName;
            return ret;
        }
        internal static List<string>[] OpenFile()
        {
            List<string> source_base = new List<string>();
            List<string> source_custom = new List<string>();
            List<string> fileName = new List<string>();
            List<string>[] ret = new List<string>[3];

            OpenFileDialog dlg = new()
            {
                InitialDirectory = @"C:\",
                Filter = "Designer.csファイル(*.Designer.cs;*.Designer.cs)|*.Designer.cs;*.Designer.cs",
                FilterIndex = 1,
                Title = "開くファイルを選択してください",
                RestoreDirectory = true
            };


            if (dlg.ShowDialog() == DialogResult.OK)
            {
                bool flag = true;

                foreach (string line in System.IO.File.ReadLines(dlg.FileName))
                {
                    if (line.IndexOf("private void InitializeComponent()") > 1)
                    {
                        flag = false;
                    }

                    if (flag)
                    {
                        source_base.Add(line);
                    }
                    else
                    {
                        source_custom.Add(line);
                    }
                }

                fileName.Add(dlg.FileName);

                ret[0] = source_base;
                ret[1] = source_custom;
                ret[2] = fileName;
            }

            return ret;
        }

        internal static void SaveAs(string FileName, string SourceCode)
        {
            File.WriteAllText(FileName, SourceCode);
        }

        internal static void Save(string SourceCode)
        {
            SaveFileDialog dlg = new()
            {
                FileName = "Form1.Designer.cs",
                InitialDirectory = @"C:\",
                Filter = "Designer.csファイル(*.Designer.cs;*.Designer.cs)|*.Designer.cs;*.Designer.cs",
                FilterIndex = 1,
                Title = "保存先のファイルを選択してください",
                RestoreDirectory = true
            };

            //ダイアログを表示する
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dlg.FileName, SourceCode);
            }
        }
    }
}
