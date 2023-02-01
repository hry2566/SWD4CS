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
                if (line_code[i].IndexOf("partial class ") > -1)
                {
                    string[] spl = line_code[i].Split(" ");
                    fileInfo.formName = spl[spl.Length - 1];
                }

                if (mode == 0)
                {
                    fileInfo.source_base.Add(line_code[i]);
                    if (line_code[i].IndexOf("private void InitializeComponent()") > -1) { mode = 1; }
                }

                if (mode == 1)
                {
                    Read_Instance(line_code[i], fileInfo);
                    if (line_code[i].IndexOf("//") > -1) { mode = 2; }
                }

                if (mode == 2)
                {
                    Add_FormInfo(fileInfo);
                    mode = 3;
                }

                if (mode == 3)
                {
                    Read_Ctrl(line_code[i], fileInfo);
                    if (line_code[i + 1].IndexOf("this.ResumeLayout(false)") >= 1) { mode = 4; }
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
            if (line_code.IndexOf("this.") == -1) { return; }

            // イベント
            if (line_code.IndexOf("+=") > -1) { Read_Events(line_code, fileInfo); }
            // parent
            else if (line_code.IndexOf("Controls.Add") > -1) { Read_Parent(line_code, fileInfo, PARENT); }
            else
            {
                string[] spl1 = line_code.Split("=");
                // form
                if (spl1[0].Split(".").Length == 2) { Read_Property(line_code, fileInfo, PROPERTY); }
                else
                {
                    // property
                    if (line_code.IndexOf("=") > -1) { Read_Property(line_code, fileInfo, PROPERTY); }
                }
            }
        }

        private static void Read_Events(string line_code, FILE_INFO fileInfo)
        {
            // Console.WriteLine(line_code.Trim());
            string ctrlName = GetCtrlName(line_code, EVENTS);
            int index = Find_Control_Index(ctrlName, fileInfo);
            if (index != -1)
            {
                CONTROL_INFO ctrl = fileInfo.ctrlInfo[index];
                ctrl.eventName.Add(GetEvents(line_code));
                ctrl.eventFunc.Add(GetFunc(line_code));
                fileInfo.ctrlInfo[index] = ctrl;
                // Console.WriteLine("{0}, {1}, {2}, {3}", ctrlName, GetEvents(line_code), GetFunc(line_code), line_code);
            }
        }

        private static void Read_Parent(string line_code, FILE_INFO fileInfo, int mode)
        {
            string ctrlName = GetCtrlName(line_code, mode);
            int panelNum = 0;
            int index = Find_Control_Index(ctrlName, fileInfo);
            if (index != -1)
            {
                CONTROL_INFO ctrl = fileInfo.ctrlInfo[index];
                ctrl.parent = GetCtrlName(line_code, 4);

                if (ctrl.parent.IndexOf(".Panel") > -1)
                {
                    string[] spl = ctrl.parent.Split(".");
                    ctrl.parent = spl[0];
                    if (spl[1] == "Panel1") { panelNum = 1; }
                    else { panelNum = 2; }
                }
                ctrl.panelNum = panelNum;
                fileInfo.ctrlInfo[index] = ctrl;
                // Console.WriteLine("parent; {0}, {1}, {2}, {3}", ctrlName, ctrl.parent, panelNum, line_code);
            }
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
            CONTROL_INFO ctrl = new();

            if (line_code.IndexOf("this.SuspendLayout();") > -1) { return; }
            if (line_code.IndexOf("new") == -1) { return; }

            ctrl.ctrlName = GetCtrlName(line_code, INSTANCE);
            ctrl.ctrlClassName = GetClassName(line_code);
            fileInfo.ctrlInfo.Add(ctrl);
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
            string[] split1 = line.Split("=");
            string[] split2 = split1[0].Split(".");
            return split2[split2.Length - 1].Trim();
        }

        private static string GetClassName(string line)
        {
            string[] split1 = line.Split("new");
            string[] split2 = split1[1].Split(".");
            return split2[split2.Length - 1].Replace("();", "").Trim();
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
                    // if (split2.Length == 5) { ctrlName = split2[1].Replace(");", "").Trim(); }
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
            List<string> source_base = new();

            source_base.Add("namespace WinFormsApp");
            source_base.Add("{");
            source_base.Add("    partial class Form1");
            source_base.Add("    {");
            source_base.Add("        /// <summary>");
            source_base.Add("        ///  Required designer variable.");
            source_base.Add("        /// </summary>");
            source_base.Add("        private System.ComponentModel.IContainer components = null;");
            source_base.Add("");
            source_base.Add("        /// <summary>");
            source_base.Add("        ///  Clean up any resources being used.");
            source_base.Add("        /// </summary>");
            source_base.Add("        /// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>");
            source_base.Add("        protected override void Dispose(bool disposing)");
            source_base.Add("        {");
            source_base.Add("            if (disposing && (components != null))");
            source_base.Add("            {");
            source_base.Add("                components.Dispose();");
            source_base.Add("            }");
            source_base.Add("            base.Dispose(disposing);");
            source_base.Add("        }");
            source_base.Add("");
            source_base.Add("        #region Windows Form Designer generated code");
            source_base.Add("");
            source_base.Add("        /// <summary>");
            source_base.Add("        ///  Required method for Designer support - do not modify");
            source_base.Add("        ///  the contents of this method with the code editor.");
            source_base.Add("        /// </summary>");
            source_base.Add("        private void InitializeComponent()");
            return source_base;
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
