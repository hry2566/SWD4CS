namespace SWD4CS
{
    public struct CONTROL_INFO
    {
        public string? ctrlName = "";
        public string? ctrlClassName = "";
        public List<string> propertyName = new();
        public List<string> strProperty = new();
        public string? parent = "";
        public List<string> eventName = new();
        public List<string> eventFunc = new();
        public int panelNum = 0;
        public CONTROL_INFO() { }
    }
    public struct FILE_INFO
    {
        public string source_FileName = "";
        public List<string> source_base = new();
        public string filePath = "";
        public List<CONTROL_INFO> ctrlInfo = new();
        public string formName = "";

        public FILE_INFO() { }
    }

    internal class cls_file
    {
        private const int PROPERTY = 0;
        private const int PARENT = 1;
        private const int CHILD = 2;
        private const int INSTANCE = 3;
        private const int ADDCTRL = 4;
        private const int EVENTS = 5;


        // *****************************************************************************
        // private Function
        // *****************************************************************************
        private static FILE_INFO ReadCode(string filePath)
        {
            FILE_INFO fileInfo = new();
            fileInfo.source_FileName = filePath;
            string designCode = System.IO.File.ReadAllText(filePath);
            string[] line_code = designCode.Split(Environment.NewLine);
            int mode = 0;

            for (int i = 0; i < line_code.Length; i++)
            {
                if (line_code[i].Contains("partial class "))
                {
                    string[] spl = line_code[i].Split(" ");
                    fileInfo.formName = spl[spl.Length - 1];
                }

                if (mode == 0)
                {
                    fileInfo.source_base.Add(line_code[i]);
                    if (line_code[i].Contains("private void InitializeComponent()")) { mode = 1; }
                }

                if (mode == 1)
                {
                    Read_Instance(line_code[i], fileInfo);
                    if (line_code[i].Contains("//")) { mode = 2; }
                }

                if (mode == 2)
                {
                    Add_FormInfo(fileInfo);
                    mode = 3;
                }

                if (mode == 3)
                {
                    Read_Ctrl(line_code[i], fileInfo);
                    if (line_code[i + 1].Contains("this.ResumeLayout(false)")) { mode = 4; }
                }
            }
            return fileInfo;
        }

        private static void Add_FormInfo(FILE_INFO fileInfo)
        {
            CONTROL_INFO form = new();
            form.ctrlName = "this";
            form.ctrlClassName = "Form";
            fileInfo.ctrlInfo.Add(form);
        }

        private static void Read_Ctrl(string line_code, FILE_INFO fileInfo)
        {
            if (!line_code.Contains("this.")) { return; }
            if (line_code.Contains("+=")) { Read_Events(line_code, fileInfo); }
            else if (line_code.Contains("Controls.Add")) { Read_Parent(line_code, fileInfo, PARENT); }
            else if (line_code.Contains("="))
            {
                string[] spl1 = line_code.Split("=");
                if (spl1[0].Split(".").Length == 2) { Read_Property(line_code, fileInfo, PROPERTY); }
                else { Read_Property(line_code, fileInfo, PROPERTY); }
            }
        }

        private static void Read_Events(string line_code, FILE_INFO fileInfo)
        {
            // Console.WriteLine(line_code.Trim());
            string ctrlName = GetCtrlName(line_code, EVENTS);
            int index = Find_Control_Index(ctrlName, fileInfo);
            if (index == -1) { return; }

            CONTROL_INFO ctrl = fileInfo.ctrlInfo[index];
            ctrl.eventName.Add(GetEvents(line_code));
            ctrl.eventFunc.Add(GetFunc(line_code));
            fileInfo.ctrlInfo[index] = ctrl;
            // Console.WriteLine("{0}, {1}, {2}, {3}", ctrlName, GetEvents(line_code), GetFunc(line_code), line_code);
        }

        private static void Read_Parent(string line_code, FILE_INFO fileInfo, int mode)
        {
            string ctrlName = GetCtrlName(line_code, mode);
            int panelNum = 0;
            int index = Find_Control_Index(ctrlName, fileInfo);
            if (index == -1) { return; }

            CONTROL_INFO ctrl = fileInfo.ctrlInfo[index];
            ctrl.parent = GetCtrlName(line_code, 4);

            if (ctrl.parent.Contains(".Panel"))
            {
                string[] spl = ctrl.parent.Split(".");
                ctrl.parent = spl[0];
                panelNum = spl[1] == "Panel1" ? 1 : 2;
            }
            ctrl.panelNum = panelNum;
            fileInfo.ctrlInfo[index] = ctrl;
            // Console.WriteLine("parent; {0}, {1}, {2}, {3}", ctrlName, ctrl.parent, panelNum, line_code);
        }

        private static void Read_Property(string line_code, FILE_INFO fileInfo, int mode)
        {
            string ctrlName = GetCtrlName(line_code, mode);

            for (int i = 0; i < fileInfo.ctrlInfo.Count; i++)
            {
                if (fileInfo.ctrlInfo[i].ctrlName == ctrlName)
                {
                    fileInfo.ctrlInfo[i].propertyName.Add(GetPropertyName(line_code));
                    fileInfo.ctrlInfo[i].strProperty.Add(GetProperty(line_code));
                    // Console.WriteLine("property: {0}, {1}, {2}", ctrlName, GetPropertyName(line_code), GetProperty(line_code));
                    break;
                }
            }
        }

        private static void Read_Instance(string line_code, FILE_INFO fileInfo)
        {
            if (!TryParseInstance(line_code, out CONTROL_INFO ctrl)) { return; }
            fileInfo.ctrlInfo.Add(ctrl);
        }

        private static bool TryParseInstance(string line_code, out CONTROL_INFO ctrl)
        {
            ctrl = new();
            if (!line_code.Contains("new")) { return false; }
            ctrl.ctrlName = GetCtrlName(line_code, INSTANCE);
            ctrl.ctrlClassName = GetClassName(line_code);
            return true;
        }

        private static int Find_Control_Index(string ctrlName, FILE_INFO fileInfo)
        {
            for (int i = 0; i < fileInfo.ctrlInfo.Count; i++)
            {
                if (fileInfo.ctrlInfo[i].ctrlName == ctrlName) { return i; }
            }
            return -1;
        }

        private static string GetFunc(string line_code)
        {
            string[] split1 = line_code.Split("+=");
            string[] split2 = split1[1].Split("(");
            return split2[1].Replace(");", "");
        }

        private static string GetEvents(string line_code)
        {
            string[] split1 = line_code.Split("+=");
            string[] split2 = split1[0].Split(".");
            string eventName = split2[split2.Length - 1];
            return eventName.Trim();
        }

        private static string GetProperty(string line)
        {
            string[] split1 = line.Split("=");
            return split1[1].Replace(";", "").Replace("\"", "").Trim();
        }

        private static string GetPropertyName(string line)
        {
            string[] split1 = line.Split(" = ");
            string[] split2 = split1[0].Split(".");
            return split2[split2.Length - 1].Trim();
        }

        private static string GetClassName(string line)
        {
            string[] spl = line.Split(".");
            return spl[spl.Length - 1].Replace("();", "").Trim();
        }


        private static string GetCtrlName(string line, int mode)
        {
            // mode = 0; property(ctrl/this)
            // mode = 1; parent
            // mode = 2; child
            // mode = 3; instance
            // mode = 4; add control
            // mode = 5; events
            string ctrlName = "";
            string[] split1;
            string[] split2;
            switch (mode)
            {
                case 0:
                    split1 = line.Split("=");
                    split2 = split1[0].Split(".");
                    if (split2.Length == 3) { ctrlName = split2[1].Trim(); }
                    else { ctrlName = split2[0].Trim(); }
                    break;
                case 1:
                    split1 = line.Split("(");
                    split2 = split1[1].Split(".");
                    ctrlName = split2[1].Replace(");", "").Trim();
                    break;
                case 2:
                    split1 = line.Split("=");
                    split2 = split1[0].Split(".");
                    ctrlName = split2[2].Trim();
                    break;
                case 3:
                    split1 = line.Split("=");
                    split2 = split1[0].Split(".");
                    ctrlName = split2[1].Trim();
                    break;
                case 4:
                    split1 = line.Split("(");
                    split2 = split1[0].Split(".");
                    if (split2.Length == 5) { ctrlName = split2[1] + "." + split2[2].Replace(");", "").Trim(); }
                    else if (split2.Length == 4) { ctrlName = split2[1].Replace(");", "").Trim(); }
                    else { ctrlName = split2[0].Replace(");", "").Trim(); }
                    break;
                case 5:
                    split1 = line.Split("+=");
                    split2 = split1[0].Split(".");
                    if (split2.Length == 3) { ctrlName = split2[1].Trim(); }
                    else { ctrlName = split2[0].Trim(); }
                    break;
            }
            return ctrlName;
        }

        // *****************************************************************************
        // internal Function
        // *****************************************************************************
        internal static FILE_INFO CommandLine(string fName)
        {
            if (fName.IndexOf(".Designer.cs") == -1) { return new FILE_INFO(); }
            return ReadCode(fName);
        }
        internal static List<string> NewFile()
        {
            return new List<string>
            {
                "namespace WinFormsApp",
                "{",
                "    partial class Form1",
                "    {",
                "        /// <summary>",
                "        ///  Required designer variable.",
                "        /// </summary>",
                "        private System.ComponentModel.IContainer components = null;",
                "",
                "        /// <summary>",
                "        ///  Clean up any resources being used.",
                "        /// </summary>",
                "        /// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>",
                "        protected override void Dispose(bool disposing)",
                "        {",
                "            if (disposing && (components != null))",
                "            {",
                "                components.Dispose();",
                "            }",
                "            base.Dispose(disposing);",
                "        }",
                "",
                "        #region Windows Form Designer generated code",
                "",
                "        /// <summary>",
                "        ///  Required method for Designer support - do not modify",
                "        ///  the contents of this method with the code editor.",
                "        /// </summary>",
                "        private void InitializeComponent()"
            };
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
            if (dlg.ShowDialog() == DialogResult.OK) { File.WriteAllText(dlg.FileName, SourceCode); }
        }

        internal static FILE_INFO OpenFile()
        {
            OpenFileDialog dlg = new()
            {
                InitialDirectory = @"C:\",
                Filter = "Designer.csファイル(*.Designer.cs;*.Designer.cs)|*.Designer.cs;*.Designer.cs",
                FilterIndex = 1,
                Title = "開くファイルを選択してください",
                RestoreDirectory = true
            };
            if (dlg.ShowDialog() != DialogResult.OK) { return new FILE_INFO(); }
            return ReadCode(dlg.FileName);
        }
    }
}
