
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

        internal static void WriteIni(MainForm form, string fileName, SplitContainer splitContainer1, SplitContainer splitContainer2)
        {
            string[] split = Application.ExecutablePath.Split("/");
            if (split.Length == 1)
            {
                split = Application.ExecutablePath.Split("\\");
            }
            string filePath = Application.ExecutablePath.Replace(split[split.Length - 1], "") + fileName;
            string line = "";
            line += "WndPos.X:" + form.Left.ToString() + "\n";
            line += "WndPos.Y:" + form.Top.ToString() + "\n";
            line += "WndSize.Width:" + form.Width.ToString() + "\n";
            line += "WndSize.Height:" + form.Height.ToString() + "\n";
            line += "SplitContainer1.SplitDistance:" + splitContainer1.SplitterDistance.ToString() + "\n";
            line += "SplitContainer2.SplitDistance:" + splitContainer2.SplitterDistance.ToString() + "\n";

            File.WriteAllText(filePath, line);
        }

        internal static void ReadIni(MainForm form, string fileName, SplitContainer splitContainer1, SplitContainer splitContainer2)
        {
            string[] split = Application.ExecutablePath.Split("/");
            if (split.Length == 1)
            {
                split = Application.ExecutablePath.Split("\\");
            }

            string filePath = Application.ExecutablePath.Replace(split[split.Length - 1], "") + fileName;
            if (!System.IO.File.Exists(filePath)) { return; }
            form.StartPosition = FormStartPosition.Manual;

            foreach (string line in System.IO.File.ReadLines(filePath))
            {
                split = line.Split(":");
                switch (split[0])
                {
                    case "WndPos.X":
                        form.Left = int.Parse(split[1]);
                        break;
                    case "WndPos.Y":
                        form.Top = int.Parse(split[1]);
                        break;
                    case "WndSize.Width":
                        form.Width = int.Parse(split[1]);
                        break;
                    case "WndSize.Height":
                        form.Height = int.Parse(split[1]);
                        break;
                    case "SplitContainer1.SplitDistance":
                        splitContainer1.SplitterDistance = int.Parse(split[1]);
                        break;
                    case "SplitContainer2.SplitDistance":
                        splitContainer2.SplitterDistance = int.Parse(split[1]);
                        break;
                }
            }
        }
        internal static List<string>[] CommandLine(string fName)
        {
            if (fName.IndexOf(".Designer.cs") == -1) { return new List<string>[3]; }
            return ReadCode(fName);
        }
        internal static List<string>[] OpenFile()
        {
            OpenFileDialog dlg = new()
            {
                InitialDirectory = @"C:\",
                Filter = "Designer.csファイル(*.Designer.cs;*.Designer.cs)|*.Designer.cs;*.Designer.cs",
                FilterIndex = 1,
                Title = "開くファイルを選択してください",
                RestoreDirectory = true
            };
            if (dlg.ShowDialog() != DialogResult.OK) { return new List<string>[3]; }
            return ReadCode(dlg.FileName);
        }

        private static List<string>[] ReadCode(string filePath)
        {
            List<string> source_base = new List<string>();
            List<string> source_custom = new List<string>();
            List<string> fileName = new List<string>();
            List<string>[] ret = new List<string>[3];

            bool flag = true;

            foreach (string line in System.IO.File.ReadLines(filePath))
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

            fileName.Add(filePath);

            ret[0] = source_base;
            ret[1] = source_custom;
            ret[2] = fileName;
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
