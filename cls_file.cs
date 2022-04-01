namespace SWD4CS
{
    public struct CONTROL_INFO
    {
        public string? ctrlName = "";
        public string? ctrlClassName = "";
        public List<string> propertyName = new();
        public List<string> strProperty = new();
        public List<string> addCtrlName = new();
        public List<string> subAdd_CtrlName = new();
        public List<string> subAdd_childCtrlName = new();
        public List<string> subAddRange_CtrlName = new();
        public List<string> subAddRange_childCtrlName = new();
        public List<string> decHandler = new();

        public CONTROL_INFO() { }
    }
    public struct FILE_INFO
    {
        public string source_FileName = "";
        public List<string> source_base = new();
        public string? filePath = "";
        public List<CONTROL_INFO> ctrlInfo = new();

        public FILE_INFO() { }
    }

    internal class cls_file
    {
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
        internal static FILE_INFO CommandLine(string fName)
        {
            if (fName.IndexOf(".Designer.cs") == -1) { return new FILE_INFO(); }
            return ReadCode(fName);
        }

        private static FILE_INFO ReadCode(string filePath)
        {
            FILE_INFO fileInfo = new();

            bool flag = true;
            string memLine = "";
            fileInfo.source_FileName = filePath;

            foreach (string line in System.IO.File.ReadLines(filePath))
            {
                if (line.IndexOf("private void InitializeComponent()") > -1)
                {
                    fileInfo.source_base!.Add(line);
                    flag = false;
                }

                if (flag)
                {
                    fileInfo.source_base!.Add(line);
                }
                else
                {
                    if (line != "" && line.Trim() != "{" && line.IndexOf("//") == -1 &&
                        line.IndexOf("private void InitializeComponent()") == -1)
                    {
                        if (line.Substring(line.Length - 1, 1) == ";")
                        {
                            if (memLine == "")
                            {
                                //Console.WriteLine(line.Trim());
                                fileInfo.ctrlInfo = AnalysisCode(line.Trim(), fileInfo.ctrlInfo);
                            }
                            else
                            {
                                memLine += line.Trim();
                                //Console.WriteLine(memLine);
                                fileInfo.ctrlInfo = AnalysisCode(memLine, fileInfo.ctrlInfo);
                                memLine = "";
                            }
                        }
                        else
                        {
                            memLine += line.Trim();
                        }
                    }
                }
            }

            fileInfo.filePath = filePath;

            //for (int i = 0; i < fileInfo.ctrlInfo.Count; i++)
            //{
            //    Console.WriteLine("ctrlName:{0}", fileInfo.ctrlInfo[i].ctrlName);
            //    Console.WriteLine("ctrlClassName:{0}", fileInfo.ctrlInfo[i].ctrlClassName);

            //    for (int j = 0; j < fileInfo.ctrlInfo[i].propertyName.Count; j++)
            //    {
            //        Console.WriteLine("    PropertyName:{0}", fileInfo.ctrlInfo[i].propertyName[j]);
            //        Console.WriteLine("    strProperty:{0}", fileInfo.ctrlInfo[i].strProperty[j]);
            //    }
            //    for (int j = 0; j < fileInfo.ctrlInfo[i].addCtrlName.Count; j++)
            //    {
            //        Console.WriteLine("        Control.Add:{0}", fileInfo.ctrlInfo[i].addCtrlName[j]);
            //    }
            //    for (int j = 0; j < fileInfo.ctrlInfo[i].subAdd_CtrlName.Count; j++)
            //    {
            //        Console.WriteLine("        Control.AddRange:{0},{1}", fileInfo.ctrlInfo[i].subAdd_CtrlName[j], fileInfo.ctrlInfo[i].subAdd_childCtrlName[j]);
            //    }
            //}
            return fileInfo;
        }

        private static List<CONTROL_INFO> AnalysisCode(string line, List<CONTROL_INFO> lstCtrlInfo)
        {
            if (line.IndexOf("=") > -1 && line.IndexOf("new") > -1 &&
                line.IndexOf("System.Windows.Forms.") > -1 && line.IndexOf("+=") == -1)
            {
                lstCtrlInfo = AnalysisCode_Control_Declaration(line, lstCtrlInfo);
            }
            else if (line.IndexOf("=") > -1 && line.IndexOf("+=") == -1)
            {
                lstCtrlInfo = AnalysisCode_Control_Property(line, lstCtrlInfo);
            }
            else if (line.IndexOf("Add") > -1 && line.IndexOf("AddRange") == -1)
            {
                lstCtrlInfo = AnalysisCode_Control_Add(line, lstCtrlInfo);
            }
            else if (line.IndexOf("AddRange") > -1)
            {
                lstCtrlInfo = AnalysisCode_Control_SpecialProperty(line, lstCtrlInfo);
            }
            else if (line.IndexOf("+=") > -1)
            {
                lstCtrlInfo = AnalysisCode_Control_Events(line, lstCtrlInfo);
            }
            else
            {
                //Console.WriteLine(line);
            }
            return lstCtrlInfo;
        }

        private static List<CONTROL_INFO> AnalysisCode_Control_Events(string line, List<CONTROL_INFO> lstCtrlInfo)
        {
            // events
            string ctrlName, decHandler;
            decHandler = line;
            string[] split1 = line.Split("+=")[0].Split(".");
            if (split1.Length == 2)
            {
                ctrlName = split1[0];
            }
            else
            {
                ctrlName = split1[1];
            }
            lstCtrlInfo = AddInfo(lstCtrlInfo, ctrlName, null, null, null, null, null, null, null, null, decHandler);
            //Console.WriteLine(eventName);
            //Console.WriteLine(decHandler);
            return lstCtrlInfo;
        }

        private static List<CONTROL_INFO> AnalysisCode_Control_SpecialProperty(string line, List<CONTROL_INFO> lstCtrlInfo)
        {
            // 特殊Parent
            string ctrlName, subAddRange_CtrlName, subAddRange_childCtrlName;
            string[] lineArray = splitAddRange(line);

            for (int i = 0; i < lineArray.Length; i++)
            {
                string[] split1 = lineArray[i].Split(",");
                ctrlName = split1[0];
                subAddRange_CtrlName = split1[1];
                subAddRange_childCtrlName = split1[2];
                lstCtrlInfo = AddInfo(lstCtrlInfo, ctrlName, null, null, null, null, null, null, subAddRange_CtrlName, subAddRange_childCtrlName, null);
            }
            //Console.WriteLine(lineArray[0]);
            //Console.WriteLine(lineArray[1]);
            return lstCtrlInfo;
        }

        private static List<CONTROL_INFO> AnalysisCode_Control_Add(string line, List<CONTROL_INFO> lstCtrlInfo)
        {
            // Parent
            string[] split1 = line.Split("Controls.Add");
            string[] split2 = split1[0].Split(".");

            string ctrlName, addCtrlName, subAdd_CtrlName, subAdd_childCtrlName;

            if (split2.Length == 2)
            {
                ctrlName = "this";
                addCtrlName = GetCtrlName(line, 1);

                lstCtrlInfo = AddInfo(lstCtrlInfo, ctrlName, null, null, null, addCtrlName, null, null, null, null, null);
                //Console.WriteLine(line);
                //Console.WriteLine(addCtrlName);
            }
            else if (split2.Length == 3)
            {
                ctrlName = GetCtrlName(line, 0);
                addCtrlName = GetCtrlName(line, 1);
                lstCtrlInfo = AddInfo(lstCtrlInfo, ctrlName, null, null, null, addCtrlName, null, null, null, null, null);
                //Console.WriteLine(line);
                //Console.WriteLine("{0} : {1}", ctrlName, addCtrlName);
            }
            else if (split2.Length == 4)
            {
                ctrlName = GetCtrlName(line, 0);
                subAdd_CtrlName = GetCtrlName(line, 2);
                subAdd_childCtrlName = GetCtrlName(line, 1);
                lstCtrlInfo = AddInfo(lstCtrlInfo, ctrlName, null, null, null, null, subAdd_CtrlName, subAdd_childCtrlName, null, null, null);
                //Console.WriteLine(line);
                //Console.WriteLine("{0} : {1} : {2}", ctrlName, subAdd_CtrlName, subAdd_childCtrlName);
            }
            else
            {
                //Console.WriteLine(line);
            }
            return lstCtrlInfo;
        }

        private static List<CONTROL_INFO> AnalysisCode_Control_Property(string line, List<CONTROL_INFO> lstCtrlInfo)
        {
            string[] split1 = line.Split('=');
            string[] split2 = split1[0].Split('.');
            if (split2.Length == 3)
            {
                // Property ctrlName, propertyName, strProperty
                string ctrlName, propertyName, strProperty;
                ctrlName = GetCtrlName(line, 0);
                propertyName = GetPropertyName(line);
                strProperty = GetProperty(line);
                lstCtrlInfo = AddInfo(lstCtrlInfo, ctrlName, null, propertyName, strProperty, null, null, null, null, null, null);
                //Console.WriteLine(line);
                //Console.WriteLine("{0} : {1} : {2}", ctrlName, propertyName, strProperty);
            }
            else if (split2.Length == 4)
            {
                // 特殊Property
                //Console.WriteLine(line);
            }
            else
            {
                // this
                string ctrlName, ctrlClassName, propertyName, strProperty;
                ctrlName = "this";
                ctrlClassName = "Form";
                propertyName = GetPropertyName(line);
                strProperty = GetProperty(line);
                lstCtrlInfo = AddInfo(lstCtrlInfo, ctrlName, ctrlClassName, propertyName, strProperty, null, null, null, null, null, null);
                //Console.WriteLine(line);
                //Console.WriteLine("{0} : {1} : {2} : {3}", ctrlName, ctrlClassName, propertyName, strProperty);
            }
            return lstCtrlInfo;
        }

        private static List<CONTROL_INFO> AnalysisCode_Control_Declaration(string line, List<CONTROL_INFO> lstCtrlInfo)
        {
            string[] split1 = line.Split('=');
            string[] split2 = split1[0].Split('.');
            if (split2.Length == 2)
            {
                // Control宣言 ctrlName, ctrlClassName
                string ctrlName, ctrlClassName;
                ctrlName = GetCtrlName(line, 0);
                ctrlClassName = GetClassName(line);
                lstCtrlInfo = AddInfo(lstCtrlInfo, ctrlName, ctrlClassName, null, null, null, null, null, null, null, null);
                //Console.WriteLine(line);
                //Console.WriteLine("{0} : {1}", ctrlName, ctrlClassName);
            }
            else
            {
                // 特殊Property
                //Console.WriteLine(line);
            }
            return lstCtrlInfo;
        }

        private static string[] splitAddRange(string line)
        {
            string ctrlName = GetCtrlName(line, 0);
            string subAddRange_CtrlName = GetCtrlName(line, 2);
            string[] subAddRange_childCtrlName = GetRangeItem(line);
            string[] lineArray = new string[subAddRange_childCtrlName.Length];

            for (int i = 0; i < subAddRange_childCtrlName.Length; i++)
            {
                lineArray[i] = String.Format("{0},{1},{2}", ctrlName, subAddRange_CtrlName, subAddRange_childCtrlName[i]);
            }
            return lineArray;
        }

        private static string[] GetRangeItem(string line)
        {
            string[] split1 = line.Split("{");
            string[] split2 = split1[1].Replace("});", "").Replace("this.", "").Split(",");
            return split2;
        }

        private static List<CONTROL_INFO> AddInfo(List<CONTROL_INFO> lstCtrlInfo, string? ctrlName, string? ctrlClassName, string? propertyName, string? strProperty, string? addCtrlName, string? subAdd_CtrlName, string? subAdd_childCtrlName, string? subAddRange_CtrlName, string? subAddRange_childCtrlName, string? decHandler)
        {
            if (lstCtrlInfo.Count == 0)
            {
                CONTROL_INFO info = new();
                info = WriteInfo(info, ctrlName, ctrlClassName, propertyName, strProperty, addCtrlName, subAdd_CtrlName, subAdd_childCtrlName, subAddRange_CtrlName, subAddRange_childCtrlName, decHandler);
                lstCtrlInfo.Add(info);
            }
            else
            {
                bool flag = false;
                for (int i = 0; i < lstCtrlInfo.Count; i++)
                {
                    if (lstCtrlInfo[i].ctrlName == ctrlName)
                    {
                        CONTROL_INFO info = lstCtrlInfo[i];
                        info = WriteInfo(info, ctrlName, ctrlClassName, propertyName, strProperty, addCtrlName, subAdd_CtrlName, subAdd_childCtrlName, subAddRange_CtrlName, subAddRange_childCtrlName, decHandler);
                        lstCtrlInfo[i] = info;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    CONTROL_INFO info = new();
                    info = WriteInfo(info, ctrlName, ctrlClassName, propertyName, strProperty, addCtrlName, subAdd_CtrlName, subAdd_childCtrlName, subAddRange_CtrlName, subAddRange_childCtrlName, decHandler);
                    lstCtrlInfo.Add(info);
                }
            }
            return lstCtrlInfo;
        }

        private static CONTROL_INFO WriteInfo(CONTROL_INFO info, string? ctrlName, string? ctrlClassName, string? propertyName, string? strProperty, string? addCtrlName, string? subAdd_CtrlName, string? subAdd_childCtrlName, string? subAddRange_CtrlName, string? subAddRange_childCtrlName, string? decHandler)
        {
            info.ctrlName = ctrlName;
            if (ctrlClassName != null) { info.ctrlClassName = ctrlClassName; }
            if (propertyName != null) { info.propertyName.Add(propertyName); }
            if (strProperty != null) { info.strProperty.Add(strProperty); }
            if (addCtrlName != null) { info.addCtrlName.Add(addCtrlName); }
            if (subAdd_CtrlName != null) { info.subAdd_CtrlName.Add(subAdd_CtrlName); }
            if (subAdd_childCtrlName != null) { info.subAdd_childCtrlName.Add(subAdd_childCtrlName); }
            if (subAddRange_CtrlName != null) { info.subAddRange_CtrlName.Add(subAddRange_CtrlName); }
            if (subAddRange_childCtrlName != null) { info.subAddRange_childCtrlName.Add(subAddRange_childCtrlName); }
            if (decHandler != null) { info.decHandler.Add(decHandler); }
            return info;
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
            string ctrlName = "";
            string[] split1;
            string[] split2;
            switch (mode)
            {
                case 0:
                    split1 = line.Split("=");
                    split2 = split1[0].Split(".");
                    ctrlName = split2[1].Trim();
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
            }
            return ctrlName;
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

            //ダイアログを表示する
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dlg.FileName, SourceCode);
            }
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
    }
}
