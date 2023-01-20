
using System.Globalization;
using System.Reflection;

namespace SWD4CS
{
    internal class cls_controls
    {
        private cls_userform? form;
        internal Control? ctrl;
        internal string? className;
        //internal Object? obj;
        private cls_selectbox? selectBox;
        private bool selectFlag = false;
        private bool changeFlag;
        private Point memPos;
        private int grid = 4;
        internal List<string> decHandler = new();
        internal List<string> decFunc = new();
        public cls_controls(cls_userform form, string className, Control parent, int X, int Y)
        {
            if ((className == "TabPage" && parent == form) || (parent is StatusStrip)) { return; }

            this.form = form;
            if (Init(className))
            {
                this.className = className;
                ctrl!.Location = new System.Drawing.Point(X, Y);
                this.form.CtrlItems!.Add(this);
                parent.Controls.Add(this.form.CtrlItems[this.form.CtrlItems.Count - 1].ctrl);

                if (ctrl is TabControl)
                {
                    _ = new cls_controls(form, "TabPage", this.ctrl!, X, Y);
                    _ = new cls_controls(form, "TabPage", this.ctrl!, X, Y);
                }

                if (this.ctrl is TabPage)
                {
                    selectBox = new cls_selectbox(this, this.ctrl);
                    Selected = false;
                }
                else if (parent is FlowLayoutPanel)
                {
                    selectBox = new cls_selectbox(this, this.ctrl);
                    Selected = true;
                }
                else if (parent is TableLayoutPanel)
                {
                    selectBox = new cls_selectbox(this, this.ctrl);
                    Selected = true;
                }
                else
                {
                    selectBox = new cls_selectbox(this, parent);
                    Selected = true;
                }
                ctrl!.Click += new System.EventHandler(Ctrl_Click);
                ctrl.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
                ctrl.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
            }
        }
        public cls_controls(cls_userform form, Form memForm, CONTROL_INFO ctrlInfo)
        {
            string log = "";
            this.form = form;

            if (Init(ctrlInfo.ctrlClassName!))
            {
                log += Set_Ini_Controls(ctrlInfo);
            }
            else if (ctrlInfo.ctrlName == "this")
            {
                log += Set_Ini_Form(ctrlInfo, memForm);
            }
            else
            {
                // obj
                //if(ctrlInfo.ctrlClassName == "DataGridViewTextBoxColumn")
                //{
                //    obj = new DataGridViewTextBoxColumn();
                //    form.CtrlItems.Add(this);
                //    for (int i = 0; i < ctrlInfo.propertyName.Count; i++)
                //    {
                //        log = SetCtrlProperty(obj, ctrlInfo.propertyName[i], ctrlInfo.strProperty[i]);
                //    }

                //}
                //else
                //{
                //未対応
                form.mainForm!.Add_Log("Unimplemented Control : " + ctrlInfo.ctrlClassName);
                //Console.WriteLine("Unimplemented Control : " + ctrlInfo.ctrlClassName);
                //}

            }
            if (log != "")
            {
                form.mainForm!.Add_Log(log);
            }
        }

        private string Set_Ini_Form(CONTROL_INFO ctrlInfo, Form memForm)
        {
            string log = "";
            // form_property
            for (int i = 0; i < ctrlInfo.propertyName.Count; i++)
            {
                log += SetCtrlProperty(memForm, ctrlInfo.propertyName[i], ctrlInfo.strProperty[i]);

                if (ctrlInfo.propertyName[i] != "Size")
                {
                    log += SetCtrlProperty(form, ctrlInfo.propertyName[i], ctrlInfo.strProperty[i]);
                }
                else
                {
                    form!.Size = memForm.ClientSize;
                }
            }

            // events
            for (int i = 0; i < ctrlInfo.decHandler.Count; i++)
            {
                string[] split = ctrlInfo.decHandler[i].Split("+=")[0].Split(".");
                string eventName = split[split.Length - 1].Trim();
                split = ctrlInfo.decHandler[i].Split("+=")[1].Split("(");
                string funcName = split[split.Length - 1].Replace(");", "");

                Type? delegateType = memForm.GetType().GetEvent(eventName!)!.EventHandlerType;
                MethodInfo? invoke = delegateType!.GetMethod("Invoke");
                ParameterInfo[] pars = invoke!.GetParameters();
                split = delegateType.AssemblyQualifiedName!.Split(",");
                string newHandler = "new " + split[0];
                string funcParam = "";

                foreach (ParameterInfo p in pars)
                {
                    string param = p.ParameterType.ToString();

                    if (param == "System.Object")
                    {
                        param += "? sender";
                    }
                    else
                    {
                        param += " e";
                    }

                    if (funcParam == "")
                    {
                        funcParam = param;
                    }
                    else
                    {
                        funcParam += ", " + param;
                    }
                }
                string decFunc = "private void " + funcName + "(" + funcParam + ")";
                this.form!.decHandler.Add(ctrlInfo.decHandler[i]);
                this.form.decFunc.Add(decFunc);

                //Console.WriteLine("{0}", decFunc);
            }
            return log;
        }

        private string Set_Ini_Controls(CONTROL_INFO ctrlInfo)
        {
            string log = "";
            this.className = ctrlInfo.ctrlClassName!;
            this.ctrl!.Name = ctrlInfo.ctrlName;
            this.ctrl!.Click += new System.EventHandler(Ctrl_Click);
            this.ctrl.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
            this.ctrl.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);

            form!.CtrlItems.Add(this);

            // Property設定
            for (int i = 0; i < ctrlInfo.propertyName.Count; i++)
            {
                log += SetCtrlProperty(this.ctrl, ctrlInfo.propertyName[i], ctrlInfo.strProperty[i]);
            }

            // events
            for (int i = 0; i < ctrlInfo.decHandler.Count; i++)
            {
                string[] split = ctrlInfo.decHandler[i].Split("+=")[0].Split(".");
                string eventName = split[split.Length - 1].Trim();
                split = ctrlInfo.decHandler[i].Split("+=")[1].Split("(");
                string funcName = split[split.Length - 1].Replace(");", "");

                Type? delegateType = this.ctrl.GetType().GetEvent(eventName!)!.EventHandlerType;
                MethodInfo? invoke = delegateType!.GetMethod("Invoke");
                ParameterInfo[] pars = invoke!.GetParameters();
                split = delegateType.AssemblyQualifiedName!.Split(",");
                string newHandler = "new " + split[0];
                string funcParam = "";

                foreach (ParameterInfo p in pars)
                {
                    string param = p.ParameterType.ToString();

                    if (param == "System.Object")
                    {
                        param += "? sender";
                    }
                    else
                    {
                        param += " e";
                    }

                    if (funcParam == "")
                    {
                        funcParam = param;
                    }
                    else
                    {
                        funcParam += ", " + param;
                    }
                }
                string decFunc = "private void " + funcName + "(" + funcParam + ")";
                this!.decHandler.Add(ctrlInfo.decHandler[i]);
                this.decFunc.Add(decFunc);

                //Console.WriteLine("{0}", decFunc);
            }
            return log;
        }

        internal static bool HideProperty(string itemName)
        {
            List<string> propertyName = new()
            {
                "AccessibilityObject",
                "BindingContext",
                "Parent",
                "TopLevelControl",
                "DataSource",
                "FirstDisplayedCell",
                "Item",
                "TopItem",
                "Rtf",
                "ParentForm",
                "SelectedTab",
                "Top",
                "Left",
                "Right",
                "Bottom",
                "Width",
                "Height",
                "CanSelect",
                "Created",
                "IsHandleCreated",
                "PreferredSize",
                "Visible",
                "Enable",
                "ClientSize",
                "UseVisualStyleBackColor",
                "PreferredHeight",
                // "ColumnCount",
                "FirstDisplayedScrollingColumnIndex",
                "FirstDisplayedScrollingRowIndex",
                "NewRowIndex",
                // "RowCount",
                "HasChildren",
                "PreferredWidth",
                "SingleMonthSize",
                "TextLength",
                "SelectedIndex",
                "TabCount",
                "VisibleCount",
                "DesktopLocation",
                "AutoScale",
                "CanFocus",
                "IsMirrored",
                "SelectionStart",
                "ContextMenuDefaultLocation",
                "CanUndo",
                "",
                "",
            };

            for (int i = 0; i < propertyName.Count; i++)
            {
                if (propertyName[i] == itemName)
                {
                    return false;
                }
            }
            return true;
        }
        internal static string Property2String(Control ctrl, PropertyInfo item)
        {
            string strProperty = "";
            Type type = item.GetValue(ctrl)!.GetType();
            string str2 = item.GetValue(ctrl)!.ToString()!;

            // Console.WriteLine(item.Name);
            // Console.WriteLine(type);

            switch (type)
            {
                case Type t when t == typeof(System.Drawing.Point):
                    Point point = (Point)item.GetValue(ctrl)!;
                    strProperty = " = new " + type.ToString() + "(" + point.X + "," + point.Y + ");";
                    break;
                case Type t when t == typeof(System.Drawing.Size):
                    Size size = (Size)item.GetValue(ctrl)!;
                    strProperty = " = new " + type.ToString() + "(" + size.Width + "," + size.Height + ");";
                    break;
                case Type t when t == typeof(System.String):
                    strProperty = " =  " + "\"" + str2 + "\";";
                    break;
                case Type t when t == typeof(System.Boolean):
                    strProperty = " =  " + str2.ToLower() + ";";
                    break;
                case Type t when t == typeof(System.Windows.Forms.AnchorStyles):
                    strProperty = AnchorStyles2String(item.GetValue(ctrl));
                    break;
                case Type t when t == typeof(System.Int32):
                    strProperty = " = " + int.Parse(str2) + ";";
                    break;
                case Type t when t == typeof(System.Windows.Forms.DockStyle) ||
                                 t == typeof(System.Drawing.ContentAlignment) ||
                                 t == typeof(System.Windows.Forms.ScrollBars) ||
                                 t == typeof(System.Windows.Forms.HorizontalAlignment) ||
                                 t == typeof(System.Windows.Forms.FormWindowState) ||
                                 t == typeof(System.Windows.Forms.FixedPanel) ||
                                 t == typeof(System.Windows.Forms.PictureBoxSizeMode) ||
                                 t == typeof(System.Windows.Forms.View) ||
                                 t == typeof(System.Windows.Forms.Orientation) ||
                                 t == typeof(System.Windows.Forms.FormBorderStyle) ||
                                 t == typeof(System.Windows.Forms.AutoScaleMode) ||
                                 t == typeof(System.Windows.Forms.TableLayoutPanelCellBorderStyle) ||
                                 t == typeof(System.Windows.Forms.FormStartPosition):

                    strProperty = " = " + type.ToString() + "." + str2 + ";";
                    break;
                case Type t when t == typeof(System.Drawing.Color):
                    strProperty = " = " + Property2Color(str2) + ";";
                    break;
                case Type t when t == typeof(System.Drawing.Font):
                    strProperty = " = " + Property2Font(ctrl.Font) + ";";
                    break;
            }

            return strProperty;
        }

        private static string Property2Font(Font font)
        {
            string[] split = font.ToString().Split(",");
            string strSize = split[1].Replace("Size=", "").Trim() + "F, ";

            string strStyle = "";
            split = font.Style.ToString().Split(",");

            if (split.Length == 1)
            {
                strStyle = string.Format("System.Drawing.FontStyle.{0}, System.Drawing.GraphicsUnit.Point)", split[0]);
            }
            else if (split.Length == 2)
            {
                strStyle = string.Format("((System.Drawing.FontStyle)((System.Drawing.FontStyle.{0} | System.Drawing.FontStyle.{1}))), System.Drawing.GraphicsUnit.Point)", split[0].Trim(), split[1].Trim());
            }
            else if (split.Length == 3)
            {
                strStyle = string.Format("((System.Drawing.FontStyle)(((System.Drawing.FontStyle.{0} | System.Drawing.FontStyle.{1}) | System.Drawing.FontStyle.{2}))), System.Drawing.GraphicsUnit.Point)", split[0].Trim(), split[1].Trim(), split[2].Trim());
            }
            else if (split.Length == 4)
            {
                strStyle = string.Format("((System.Drawing.FontStyle)((((System.Drawing.FontStyle.{0} | System.Drawing.FontStyle.{1})  | System.Drawing.FontStyle.{2}) | System.Drawing.FontStyle.{3}))), System.Drawing.GraphicsUnit.Point)", split[0].Trim(), split[1].Trim(), split[2].Trim(), split[3].Trim());
            }

            string strProperty = "new System.Drawing.Font(\"" + font.Name + "\", " + strSize + strStyle;
            return strProperty;
        }

        private static string AnchorStyles2String(object? propertyinfo)
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
                        ancho = "(" + type.ToString() + "." + split[j].Trim();
                    }
                    else
                    {
                        ancho = "(" + ancho + " | " + type.ToString() + "." + split[j].Trim() + ")";
                    }
                }
                ancho = "((" + type.ToString() + ")" + ancho + "));";
                strProperty = " = " + ancho;
            }
            return strProperty;
        }
        private static string Property2Color(string color)
        {
            List<string> systemColorName = new()
            {
                "ActiveBorder",
                "ActiveCaption",
                "ActiveCaptionText",
                "AppWorkspace",
                "ButtonFace",
                "ButtonHighlight",
                "ButtonShadow",
                "Control",
                "ControlDark",
                "ControlDarkDark",
                "ControlLight",
                "ControlLightLight",
                "ControlText",
                "Desktop",
                "GradientActiveCaption",
                "GradientInactiveCaption",
                "GrayText",
                "Highlight",
                "HighlightText",
                "HotTrack",
                "InactiveBorder",
                "InactiveCaption",
                "InactiveCaptionText",
                "Info",
                "InfoText",
                "Menu",
                "MenuBar",
                "MenuHighlight",
                "MenuText",
                "ScrollBar",
                "Window",
                "WindowFrame",
                "WindowText",
            };

            List<string> colorName = new()
            {
                "Black",
                "DimGray",
                "Gray",
                "DarkGray",
                "Silver",
                "LightGray",
                "Gainsboro",
                "WhiteSmoke",
                "White",
                "RosyBrown",
                "IndianRed",
                "Brown",
                "Firebrick",
                "LightCoral",
                "Maroon",
                "DarkRed",
                "Red",
                "Snow",
                "MistyRose",
                "Salmon",
                "Tomato",
                "DarkSalmon",
                "Coral",
                "OrangeRed",
                "LightSalmon",
                "Sienna",
                "SeaShell",
                "Chocolate",
                "SaddleBrown",
                "SandyBrown",
                "PeachPuff",
                "Peru",
                "Linen",
                "Bisque",
                "DarkOrange",
                "BurlyWood",
                "Tan",
                "AntiqueWhite",
                "NavajoWhite",
                "BlanchedAlmond",
                "PapayaWhip",
                "Moccasin",
                "Orange",
                "Wheat",
                "OldLace",
                "FloralWhite",
                "DarkGoldenrod",
                "Goldenrod",
                "Cornsilk",
                "Gold",
                "Khaki",
                "LemonChiffon",
                "PaleGoldenrod",
                "DarkKhaki",
                "Beige",
                "LightGoldenrodYellow",
                "Olive",
                "Yellow",
                "LightYellow",
                "Ivory",
                "OliveDrab",
                "YellowGreen",
                "DarkOliveGreen",
                "GreenYellow",
                "Chartreuse",
                "LawnGreen",
                "DarkSeaGreen",
                "ForestGreen",
                "LimeGreen",
                "LightGreen",
                "PaleGreen",
                "DarkGreen",
                "Green",
                "Lime",
                "Honeydew",
                "SeaGreen",
                "MediumSeaGreen",
                "SpringGreen",
                "MintCream",
                "MediumSpringGreen",
                "MediumAquamarine",
                "Aquamarine",
                "Turquoise",
                "LightSeaGreen",
                "MediumTurquoise",
                "DarkSlateGray",
                "PaleTurquoise",
                "Teal",
                "DarkCyan",
                "Cyan",
                "Aqua",
                "LightCyan",
                "Azure",
                "DarkTurquoise",
                "CadetBlue",
                "PowderBlue",
                "LightBlue",
                "DeepSkyBlue",
                "SkyBlue",
                "LightSkyBlue",
                "SteelBlue",
                "AliceBlue",
                "DodgerBlue",
                "SlateGray",
                "LightSlateGray",
                "LightSteelBlue",
                "CornflowerBlue",
                "RoyalBlue",
                "MidnightBlue",
                "Lavender",
                "Navy",
                "DarkBlue",
                "MediumBlue",
                "Blue",
                "GhostWhite",
                "SlateBlue",
                "DarkSlateBlue",
                "MediumSlateBlue",
                "MediumPurple",
                "BlueViolet",
                "Indigo",
                "DarkOrchid",
                "DarkViolet",
                "MediumOrchid",
                "Thistle",
                "Plum",
                "Violet",
                "Purple",
                "DarkMagenta",
                "Fuchsia",
                "Magenta",
                "Orchid",
                "MediumVioletRed",
                "DeepPink",
                "HotPink",
                "LavenderBlush",
                "PaleVioletRed",
                "Crimson",
            };

            color = color.Replace("Color [", "").Replace("]", "");
            string? strSystemColor = "System.Drawing.SystemColors.";
            string? strColor = "System.Drawing.Color.";
            string? strRGB = "System.Drawing.Color.FromArgb(";

            if (color == "Transparent")
            {
                return "Color.Transparent";
            }

            for (int i = 0; i < systemColorName.Count; i++)
            {
                if (systemColorName[i] == color)
                {
                    return strSystemColor + color;
                }
            }

            for (int i = 0; i < colorName.Count; i++)
            {
                if (colorName[i] == color)
                {
                    return strColor + color;
                }
            }

            color = color.Replace("A", "").Replace("R", "").Replace("G", "").Replace("B", "").Replace("=", "");
            string[] split = color.Split(",");
            strRGB += split[1] + "," + split[2] + "," + split[3] + ")";
            return strRGB;

        }
        internal Control? GetBaseCtrl()
        {
            Type type = this.ctrl!.GetType();
            Control? baseCtrl = (Control)Activator.CreateInstance(type)!;
            return baseCtrl;
        }
        internal void Delete()
        {
            Selected = false;
            ctrl!.Parent.Controls.Remove(ctrl);
        }

        private void ControlMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Selected)
            {
                memPos.X = (int)(e.X / grid) * grid;
                memPos.Y = (int)(e.Y / grid) * grid;
            }
        }
        private void ControlMouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Selected)
            {
                Point pos = new((int)(e.X / grid) * grid, (int)(e.Y / grid) * grid);
                Point newPos = new(pos.X - memPos.X + ctrl!.Location.X, pos.Y - memPos.Y + ctrl.Location.Y);

                ctrl.Location = newPos;
                Selected = true;
                changeFlag = false;
            }
            else
            {
                changeFlag = true;
            }
        }
        private void SetSelected(MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left)
            {
                if (Selected && changeFlag)
                {
                    Selected = false;
                }
                else
                {
                    if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                    {
                        form!.SelectAllClear();
                    }
                    Selected = true;
                }
            }
        }
        private void Ctrl_Click(object? sender, EventArgs e)
        {
            if (e.ToString() != "System.EventArgs")
            {
                MouseEventArgs me = (MouseEventArgs)e;

                if (form!.mainForm!.toolLstBox!.Text == "")
                {
                    SetSelected(me);
                    foreach (TreeNode n in form.mainForm.ctrlTree!.Nodes)
                    {
                        TreeNode ret = FindNode(n, this.ctrl!.Name);
                        if (ret != null)
                        {
                            form!.mainForm.ctrlTree.SelectedNode = ret;
                            break;
                        }
                    }
                }
                else
                {
                    AddControls(me);
                }
            }
        }
        private TreeNode FindNode(TreeNode treeNode, string ctrlName)
        {
            if (ctrlName == treeNode.Text)
            {
                return treeNode;
            }

            TreeNode ret;
            foreach (TreeNode tn in treeNode.Nodes)
            {
                ret = FindNode(tn, ctrlName);
                if (ret != null)
                {
                    return ret;
                }
            }
            return null!;
        }
        private void AddControls(MouseEventArgs me, SplitterPanel? splitpanel = null)
        {
            int X = (int)(me.X / grid) * grid;
            int Y = (int)(me.Y / grid) * grid;

            form!.SelectAllClear();

            if ((this.ctrl is TabControl && form!.mainForm!.toolLstBox!.Text == "TabPage") || (this.ctrl is TabControl == false && form!.mainForm!.toolLstBox!.Text != "TabPage"))
            {
                if (splitpanel == null)
                {
                    _ = new cls_controls(form, form!.mainForm!.toolLstBox!.Text, this.ctrl!, X, Y);
                }
                else
                {
                    _ = new cls_controls(form, form!.mainForm!.toolLstBox!.Text, splitpanel!, X, Y);
                }
            }
            form!.mainForm!.toolLstBox!.SelectedIndex = -1;
        }

        private void ShowProperty(bool flag)
        {
            if (flag)
            {
                form!.mainForm!.propertyGrid!.SelectedObject = this.ctrl;
                form.mainForm.propertyCtrlName!.Text = this.ctrl!.Name;
            }
            else
            {
                form!.mainForm!.propertyGrid!.SelectedObject = null;
                form.mainForm!.propertyCtrlName!.Text = "";
            }
        }
        internal bool Selected
        {
            set
            {
                if (selectBox != null)
                {
                    selectFlag = value;
                    selectBox!.SetSelectBoxPos(value);
                }
                ShowProperty(value);
                form!.mainForm!.eventView!.ShowEventList(value, this);
            }
            get
            {
                return selectFlag;
            }
        }
        internal void InitSelectBox()
        {
            if (this.ctrl is TabPage)
            {
                selectBox = new cls_selectbox(this, this.ctrl);
            }
            else if (this.ctrl != null)
            {
                selectBox = new cls_selectbox(this, this.ctrl!.Parent);
            }
        }

        internal void SetControls(CONTROL_INFO ctrlInfo)
        {
            //add設定
            for (int i = 0; i < ctrlInfo.addCtrlName.Count; i++)
            {
                Control? parent = this.ctrl;
                Control? child = new();

                if (ctrlInfo.ctrlName == "this")
                {
                    parent = form;
                }

                for (int j = 0; j < form!.CtrlItems.Count; j++)
                {
                    if (form.CtrlItems[j].ctrl != null)
                    {
                        if (form.CtrlItems[j].ctrl!.Name == ctrlInfo.addCtrlName[i])
                        {
                            child = form.CtrlItems[j].ctrl;
                            break;
                        }
                    }

                }
                parent!.Controls.Add(child);
            }

            // subadd設定(splitcontainer)
            for (int i = 0; i < ctrlInfo.subAdd_CtrlName.Count; i++)
            {
                SplitContainer? parent = this.ctrl as SplitContainer;
                Control? child = new();

                for (int j = 0; j < form!.CtrlItems.Count; j++)
                {
                    if (form.CtrlItems[j].ctrl != null)
                    {
                        if (form.CtrlItems[j].ctrl!.Name == ctrlInfo.subAdd_childCtrlName[i])
                        {
                            child = form.CtrlItems[j].ctrl;
                            break;
                        }
                    }

                }
                if (ctrlInfo.subAdd_CtrlName[i] == "Panel1" && parent != null)
                {
                    parent!.Panel1.Controls.Add(child);
                }
                else if (ctrlInfo.subAdd_CtrlName[i] == "Panel2" && parent != null)
                {
                    parent!.Panel2.Controls.Add(child);
                }
            }

            // obj
            //for (int i = 0; i < ctrlInfo.subAddRange_childCtrlName.Count; i++)
            //{
            //    Control? parent = this.ctrl;
            //    for (int j = 0; j < form!.CtrlItems.Count; j++)
            //    {
            //        if (form.CtrlItems[j].obj is DataGridViewTextBoxColumn)
            //        {
            //            DataGridViewTextBoxColumn? textBoxColumn = form.CtrlItems[j].obj as DataGridViewTextBoxColumn;
            //            if (textBoxColumn!.Name == ctrlInfo.subAddRange_childCtrlName[i])
            //            {
            //                DataGridView? dgView = parent as DataGridView;
            //                dgView!.Columns.Add(textBoxColumn);
            //                break;
            //            }
            //        }
            //    }
            //}

        }

        private static Size String2Size(string propertyValue)
        {
            string[] split;
            string dummy;

            split = propertyValue.Split("(");
            dummy = split[1];
            split = dummy.Split(")");
            dummy = split[0];
            split = dummy.Split(",");
            Size size = new(int.Parse(split[0]), int.Parse(split[1]));
            return size;
        }
        private static Point String2Point(string propertyValue)
        {
            string[] split;
            string dummy;

            split = propertyValue.Split("(");
            dummy = split[1];
            split = dummy.Split(")");
            dummy = split[0];
            split = dummy.Split(",");
            Point point = new(int.Parse(split[0]), int.Parse(split[1]));


            return point;
        }
        private static Color? String2Color(string? propertyValue)
        {
            Color color;
            string[] split;

            if (propertyValue == "Color.Transparent")
            {
                color = Color.Transparent;
            }
            else if (propertyValue!.IndexOf("FromArgb") > -1)
            {
                split = propertyValue!.Split("(");
                string strRGB = split[1].Trim().Replace(")", "");
                split = strRGB.Split(",");

                color = Color.FromArgb(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
            }
            else
            {
                split = propertyValue!.Split(".");
                color = Color.FromName(split[3]);
            }
            return color;
        }
        private static int String2AnchorStyles(string propertyValue)
        {
            int style = 0;

            if (propertyValue.IndexOf("System.Windows.Forms.AnchorStyles") > -1)
            {
                string dummy = propertyValue.Replace("System.Windows.Forms.AnchorStyles", "").Replace("(", "").Replace(")", "").Replace(";", "");
                string[] split2 = dummy.Split("|");

                propertyValue = "";
                for (int i = 0; i < split2.Length; i++)
                {
                    string[] split3 = split2[i].Trim().Split(".");
                    if (propertyValue == "")
                    {
                        propertyValue += split3[split3.Length - 1];
                    }
                    else
                    {
                        propertyValue += "," + split3[split3.Length - 1];
                    }
                }
            }

            string[] split = propertyValue!.Split(',');

            for (int j = 0; j < split.Length; j++)
            {
                switch (split[j].Trim().ToLower())
                {
                    case "bottom":
                        style += 2;
                        break;
                    case "left":
                        style += 4;
                        break;
                    case "right":
                        style += 8;
                        break;
                    case "top":
                        style += 1;
                        break;
                }
            }
            return style;
        }
        private static int String2DockStyle(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.DockStyle") > -1)
            {
                string[] split = propertyValue.Split(".");
                split = split[split.Length - 1].Split(";");
                propertyValue = split[0];
            }

            switch (propertyValue!.ToLower())
            {
                case "fill":
                    style = 5;
                    break;
                case "bottom":
                    style = 2;
                    break;
                case "left":
                    style = 3;
                    break;
                case "right":
                    style = 4;
                    break;
                case "top":
                    style = 1;
                    break;
            }
            return style;
        }
        private static int? String2FixedPanel(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.FixedPanel") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue)
            {
                case "None":
                    style = 0;
                    break;
                case "Panel1":
                    style = 1;
                    break;
                case "Panel2":
                    style = 2;
                    break;
            }
            return style;
        }
        private static int? String2View(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.View") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue)
            {
                case "Details":
                    style = 1;
                    break;
                case "LargeIcon":
                    style = 0;
                    break;
                case "List":
                    style = 3;
                    break;
                case "SmallIcon":
                    style = 2;
                    break;
                case "Tile":
                    style = 4;
                    break;
            }
            return style;
        }
        private static int? String2PictureBoxSizeMode(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.PictureBoxSizeMode") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue)
            {
                case "AutoSize":
                    style = 2;
                    break;
                case "CenterImage":
                    style = 3;
                    break;
                case "Normal":
                    style = 0;
                    break;
                case "StretchImage":
                    style = 1;
                    break;
                case "Zoom":
                    style = 4;
                    break;
            }
            return style;
        }
        private static int String2HorizontalAlignment(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.HorizontalAlignment") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue!.ToLower())
            {
                case "left":
                    style = 0;
                    break;
                case "right":
                    style = 1;
                    break;
                case "center":
                    style = 2;
                    break;
            }
            return style;
        }
        private static int String2ContentAlignment(string? propertyValue)
        {
            int style = 32;

            if (propertyValue!.IndexOf("System.Drawing.ContentAlignment") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1].Replace(";", "");
            }

            switch (propertyValue!.ToLower())
            {
                case "bottomcenter":
                    style = 512;
                    break;
                case "bottomleft":
                    style = 256;
                    break;
                case "bottomright":
                    style = 1024;
                    break;
                case "middleleft":
                    style = 16;
                    break;
                case "middleright":
                    style = 64;
                    break;
                case "topcenter":
                    style = 2;
                    break;
                case "topleft":
                    style = 1;
                    break;
                case "topright":
                    style = 4;
                    break;
            }
            return style;
        }
        private static int String2ScrollBars(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.ScrollBars") > -1)
            {
                string[] split = propertyValue.Split(".");
                split = split[split.Length - 1].Split(";");
                propertyValue = split[0];
            }

            switch (propertyValue!.ToLower())
            {
                case "both":
                    style = 3;
                    break;
                case "horizontal":
                    style = 1;
                    break;
                case "vertical":
                    style = 2;
                    break;
            }
            return style;
        }
        private static int? String2FormStartPosition(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.FormStartPosition") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue)
            {
                case "CenterParent":
                    style = 4;
                    break;
                case "CenterScreen":
                    style = 1;
                    break;
                case "Manual":
                    style = 0;
                    break;
                case "WindowsDefaultBounds":
                    style = 3;
                    break;
                case "WindowsDefaultLocation":
                    style = 2;
                    break;
            }
            return style;
        }
        private static int? String2FormWindowState(string? propertyValue)
        {
            int style = 0;

            if (propertyValue!.IndexOf("System.Windows.Forms.FormWindowState") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue)
            {
                case "Maximized":
                    style = 2;
                    break;
                case "Minimized":
                    style = 1;
                    break;
                case "Normal":
                    style = 0;
                    break;
            }
            return style;
        }
        private static int? String2Orientation(string? propertyValue)
        {
            int style = 1;

            if (propertyValue!.IndexOf("System.Windows.Forms.Orientation") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue)
            {
                case "Horizontal":
                    style = 0;
                    break;
                case "Vertical":
                    style = 1;
                    break;
            }
            return style;
        }
        private static int? String2FormBorderStyle(string? propertyValue)
        {
            int style = 4;

            if (propertyValue!.IndexOf("System.Windows.Forms.FormBorderStyle") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue)
            {
                case "Fixed3D":
                    style = 2;
                    break;
                case "FixedDialog":
                    style = 3;
                    break;
                case "FixedSingle":
                    style = 1;
                    break;
                case "FixedToolWindow":
                    style = 5;
                    break;
                case "None":
                    style = 0;
                    break;
                case "Sizable":
                    style = 4;
                    break;
                case "SizableToolWindow":
                    style = 6;
                    break;
            }
            return style;
        }
        private static object? String2AutoScaleMode(string? propertyValue)
        {
            int style = 1;

            if (propertyValue!.IndexOf("System.Windows.Forms.AutoScaleMode") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue)
            {
                case "Dpi":
                    style = 2;
                    break;
                case "Font":
                    style = 1;
                    break;
                case "Inherit":
                    style = 2;
                    break;
                case "None":
                    style = 0;
                    break;
            }
            return style;
        }
        private static object? String2Font(string? propertyValue)
        {
            Font? font = null;

            if (propertyValue!.IndexOf("System.Drawing.Font") > -1)
            {
                string[] split = propertyValue.Split(",");
                string strName = split[0].Replace("new System.Drawing.Font(", "").Trim();
                string strSize = split[1].Replace("F", "").Trim();
                float fSize = float.Parse(strSize, CultureInfo.InvariantCulture.NumberFormat);

                string strStyle = split[2].Replace("System.Drawing.FontStyle", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "");
                split = strStyle.Split("|");

                int iStyle = 0;
                for (int i = 0; i < split.Length; i++)
                {
                    switch (split[i])
                    {
                        case "Bold":
                            iStyle += 1;
                            break;
                        case "Italic":
                            iStyle += 2;
                            break;
                        case "Regular":
                            iStyle += 0;
                            break;
                        case "Strikeout":
                            iStyle += 8;
                            break;
                        case "Underline":
                            iStyle += 4;
                            break;
                    }
                }

                font = new System.Drawing.Font(strName, fSize, (FontStyle)iStyle, System.Drawing.GraphicsUnit.Point);

                //Console.WriteLine(propertyValue);
                //Console.WriteLine(strName);
                //Console.WriteLine(strSize);
                //Console.WriteLine(fSize);
            }

            return font;
        }

        private static object? String2CellBorderStyle(string? propertyValue)
        {
            int style = 1;

            if (propertyValue!.IndexOf("System.Windows.Forms.TableLayoutPanelCellBorderStyle") > -1)
            {
                string[] split = propertyValue.Split(".");
                propertyValue = split[split.Length - 1];
            }

            switch (propertyValue)
            {
                case "Inset":
                    style = 2;
                    break;
                case "InsetDouble":
                    style = 3;
                    break;
                case "None":
                    style = 0;
                    break;
                case "Outset":
                    style = 0;
                    break;
                case "OutsetDouble":
                    style = 5;
                    break;
                case "OutsetPartial":
                    style = 6;
                    break;
                case "Single":
                    style = 1;
                    break;
            }
            return style;
        }


        private static string SetCtrlProperty(Control? ctrl, string? propertyName, string? propertyValue)
        {
            Type type;

            PropertyInfo? property = ctrl!.GetType().GetProperty(propertyName!);

            if (property != null && property!.GetValue(ctrl) != null)
            {
                type = property!.GetValue(ctrl)!.GetType();

                switch (type)
                {
                    case Type t when t == typeof(System.String):
                        property.SetValue(ctrl, propertyValue);
                        return "";
                    case Type t when t == typeof(System.Boolean):
                        bool b = System.Convert.ToBoolean(propertyValue);
                        property.SetValue(ctrl, b);
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.DockStyle):
                        property.SetValue(ctrl, String2DockStyle(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.AnchorStyles):
                        property.SetValue(ctrl, String2AnchorStyles(propertyValue!));
                        return "";
                    case Type t when t == typeof(System.Drawing.Point):
                        property.SetValue(ctrl, String2Point(propertyValue!));
                        return "";
                    case Type t when t == typeof(System.Drawing.Size):
                        property.SetValue(ctrl, String2Size(propertyValue!));
                        return "";
                    case Type t when t == typeof(System.Int32):
                        property.SetValue(ctrl, int.Parse(propertyValue!));
                        return "";
                    case Type t when t == typeof(System.Drawing.ContentAlignment):
                        property.SetValue(ctrl, String2ContentAlignment(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.ScrollBars):
                        property.SetValue(ctrl, String2ScrollBars(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.HorizontalAlignment):
                        property.SetValue(ctrl, String2HorizontalAlignment(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Drawing.Color):
                        property.SetValue(ctrl, String2Color(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.FormStartPosition):
                        property.SetValue(ctrl, String2FormStartPosition(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.FormWindowState):
                        property.SetValue(ctrl, String2FormWindowState(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.FixedPanel):
                        property.SetValue(ctrl, String2FixedPanel(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.PictureBoxSizeMode):
                        property.SetValue(ctrl, String2PictureBoxSizeMode(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.View):
                        property.SetValue(ctrl, String2View(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.Orientation):
                        property.SetValue(ctrl, String2Orientation(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.FormBorderStyle):
                        property.SetValue(ctrl, String2FormBorderStyle(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.AutoScaleMode):
                        property.SetValue(ctrl, String2AutoScaleMode(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Drawing.Font):
                        property.SetValue(ctrl, String2Font(propertyValue));
                        return "";
                    case Type t when t == typeof(System.Windows.Forms.TableLayoutPanelCellBorderStyle):
                        property.SetValue(ctrl, String2CellBorderStyle(propertyValue));
                        return "";
                }
                return "Unimplemented PropertyType : " + type;
                //Console.WriteLine("Unimplemented PropertyType : " + type);
            }
            return "";
        }



        //private static string SetCtrlProperty(Object? obj, string? propertyName, string? propertyValue)
        //{
        //    Type type;

        //    PropertyInfo? property = obj!.GetType().GetProperty(propertyName!);

        //    if (property != null && property!.GetValue(obj) != null)
        //    {
        //        type = property!.GetValue(obj)!.GetType();

        //        switch (type)
        //        {
        //            case Type t when t == typeof(System.String):
        //                property.SetValue(obj, propertyValue);
        //                return "";
        //            case Type t when t == typeof(System.Int32):
        //                property.SetValue(obj, int.Parse(propertyValue!));
        //                return "";

        //        }
        //        return "Unimplemented PropertyType : " + type;
        //        //Console.WriteLine("Unimplemented PropertyType : " + type);
        //    }
        //    return "";
        //}

        private void CreateTrancePanel(Control ctrl)
        {
            cls_transparent_panel trancepanel = new();
            trancepanel.Dock = DockStyle.Fill;
            trancepanel.BackColor = Color.FromArgb(0, 0, 0, 0);

            trancepanel.Click += new System.EventHandler(Ctrl_Click);
            trancepanel.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
            trancepanel.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
            ctrl.Controls.Add(trancepanel);

            trancepanel.BringToFront();
            trancepanel.Invalidate();
        }

        private void CreatePickBox(Control ctrl)
        {
            Button pickbox = new();
            pickbox.Size = new System.Drawing.Size(24, 24);
            pickbox.Text = "▼";
            pickbox.Click += new System.EventHandler(Ctrl_Click);
            pickbox.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
            pickbox.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
            ctrl.Controls.Add(pickbox);
        }
        private void SplitContainerPanelClick(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            SplitterPanel? panel = sender as SplitterPanel;

            if (e.ToString() == "System.EventArgs")
            {
                return;
            }

            if (form!.mainForm!.toolLstBox!.Text == "")
            {
                SetSelected(me);
            }
            else
            {
                AddControls(me, panel);
            }
        }

        // ****************************************************************************************
        // コントロール追加時に下記を編集すること
        // ****************************************************************************************
        private bool Init(string className)
        {
            switch (className)
            {
                case "Button":
                    this.ctrl = new Button();
                    this.ctrl.Size = new System.Drawing.Size(96, 32);
                    this.ctrl!.Name = className + form!.cnt_Button;
                    form.cnt_Button++;
                    break;
                case "Label":
                    this.ctrl = new Label();
                    this.ctrl!.Name = className + form!.cnt_Label;
                    this.ctrl!.AutoSize = true;
                    form.cnt_Label++;
                    break;
                case "GroupBox":
                    this.ctrl = new GroupBox();
                    this.ctrl.Size = new System.Drawing.Size(250, 125);
                    this.ctrl!.Name = className + form!.cnt_GroupBox;
                    form.cnt_GroupBox++;
                    break;
                case "TextBox":
                    this.ctrl = new TextBox();
                    this.ctrl!.Name = className + form!.cnt_TextBox;
                    form.cnt_TextBox++;
                    break;
                case "ListBox":
                    this.ctrl = new ListBox();
                    this.ctrl.Size = new System.Drawing.Size(120, 104);
                    this.ctrl!.Name = className + form!.cnt_ListBox;
                    ListBox? listbox = this.ctrl as ListBox;
                    listbox!.Items.Add("ListBox");
                    form.cnt_ListBox++;
                    break;
                case "TabControl":
                    this.ctrl = new TabControl();
                    this.ctrl.Size = new System.Drawing.Size(250, 125);
                    this.ctrl!.Name = className + form!.cnt_TabControl;
                    form.cnt_TabControl++;
                    break;
                case "TabPage":
                    this.ctrl = new TabPage();
                    this.ctrl.Size = new System.Drawing.Size(250, 125);
                    this.ctrl!.Name = className + form!.cnt_TabPage;
                    form.cnt_TabPage++;
                    break;
                case "CheckBox":
                    this.ctrl = new CheckBox();
                    this.ctrl!.Name = className + form!.cnt_CheckBox;
                    this.ctrl!.AutoSize = true;
                    form.cnt_CheckBox++;
                    break;
                case "ComboBox":
                    this.ctrl = new ComboBox();
                    this.ctrl!.Name = className + form!.cnt_ComboBox;
                    form.cnt_ComboBox++;
                    break;
                case "SplitContainer":
                    this.ctrl = new SplitContainer();
                    this.ctrl.Size = new System.Drawing.Size(120, 32);
                    this.ctrl!.Name = className + form!.cnt_SplitContainer;
                    this.ctrl.Size = new System.Drawing.Size(250, 125);
                    SplitContainer? splitcontainer = this.ctrl as SplitContainer;
                    splitcontainer!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    splitcontainer.Panel1.Name = this.ctrl.Name + ".Panel1";
                    splitcontainer.Panel1.Click += new System.EventHandler(this.SplitContainerPanelClick);
                    splitcontainer.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
                    splitcontainer.Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
                    splitcontainer.Panel2.Name = this.ctrl.Name + ".Panel2";
                    splitcontainer.Panel2.Click += new System.EventHandler(this.SplitContainerPanelClick);
                    splitcontainer.Panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(ControlMouseMove);
                    splitcontainer.Panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(ControlMouseDown);
                    form.cnt_SplitContainer++;
                    break;
                case "DataGridView":
                    this.ctrl = new DataGridView();
                    this.ctrl.Size = new System.Drawing.Size(304, 192);
                    this.ctrl!.Name = className + form!.cnt_DataGridView;
                    form.cnt_DataGridView++;
                    break;
                case "Panel":
                    this.ctrl = new Panel();
                    this.ctrl.Size = new System.Drawing.Size(304, 192);
                    this.ctrl!.Name = className + form!.cnt_Panel;
                    Panel? panel = this.ctrl as Panel;
                    panel!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    form.cnt_Panel++;
                    break;
                case "CheckedListBox":
                    this.ctrl = new CheckedListBox();
                    this.ctrl.Size = new System.Drawing.Size(152, 112);
                    this.ctrl!.Name = className + form!.cnt_CheckedListBox;
                    CheckedListBox? checkedlistbox = this.ctrl as CheckedListBox;
                    checkedlistbox!.Items.Add("CheckedListBox");
                    form.cnt_CheckedListBox++;
                    break;
                case "LinkLabel":
                    this.ctrl = new LinkLabel();
                    this.ctrl.Size = new System.Drawing.Size(120, 32);
                    this.ctrl!.Name = className + form!.cnt_LinkLabel;
                    this.ctrl!.AutoSize = true;
                    form.cnt_LinkLabel++;
                    break;
                case "PictureBox":
                    this.ctrl = new PictureBox();
                    this.ctrl.Size = new System.Drawing.Size(125, 62);
                    this.ctrl!.Name = className + form!.cnt_PictureBox;
                    PictureBox? picbox = this.ctrl as PictureBox;
                    picbox!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    form.cnt_PictureBox++;
                    break;
                case "ProgressBar":
                    this.ctrl = new ProgressBar();
                    this.ctrl.Size = new System.Drawing.Size(125, 29);
                    this.ctrl!.Name = className + form!.cnt_ProgressBar;
                    ProgressBar? prgressbar = this.ctrl as ProgressBar;
                    prgressbar!.Value = 50;
                    form.cnt_ProgressBar++;
                    break;
                case "RadioButton":
                    this.ctrl = new RadioButton();
                    this.ctrl.Size = new System.Drawing.Size(125, 29);
                    this.ctrl!.Name = className + form!.cnt_RadioButton;
                    this.ctrl!.AutoSize = true;
                    form.cnt_RadioButton++;
                    break;
                case "RichTextBox":
                    this.ctrl = new RichTextBox();
                    this.ctrl.Size = new System.Drawing.Size(125, 120);
                    this.ctrl!.Name = className + form!.cnt_RichTextBox;
                    form.cnt_RichTextBox++;
                    break;
                case "StatusStrip":
                    this.ctrl = new StatusStrip();
                    this.ctrl.Size = new System.Drawing.Size(125, 120);
                    this.ctrl!.Name = className + form!.cnt_StatusStrip;
                    form.cnt_StatusStrip++;
                    break;
                case "HScrollBar":
                    this.ctrl = new HScrollBar();
                    this.ctrl.Size = new System.Drawing.Size(120, 32);
                    this.ctrl!.Name = className + form!.cnt_HScrollBar;
                    CreateTrancePanel(this.ctrl);
                    form.cnt_HScrollBar++;
                    break;
                case "VScrollBar":
                    this.ctrl = new VScrollBar();
                    this.ctrl.Size = new System.Drawing.Size(32, 120);
                    this.ctrl!.Name = className + form!.cnt_VScrollBar;
                    CreateTrancePanel(this.ctrl);
                    form.cnt_VScrollBar++;
                    break;
                case "MonthCalendar":
                    this.ctrl = new MonthCalendar();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_MonthCalendar;
                    CreateTrancePanel(this.ctrl);
                    form.cnt_MonthCalendar++;
                    break;
                case "ListView":
                    this.ctrl = new ListView();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_ListView;
                    CreateTrancePanel(this.ctrl);
                    form.cnt_ListView++;
                    break;
                case "TreeView":
                    this.ctrl = new TreeView();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_TreeView;
                    CreateTrancePanel(this.ctrl);
                    form.cnt_TreeView++;
                    break;
                case "MaskedTextBox":
                    this.ctrl = new MaskedTextBox();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_MaskedTextBox;
                    form.cnt_MaskedTextBox++;
                    break;
                case "PropertyGrid":
                    this.ctrl = new PropertyGrid();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_PropertyGrid;
                    CreateTrancePanel(this.ctrl);
                    form.cnt_PropertyGrid++;
                    break;
                case "DateTimePicker":
                    this.ctrl = new DateTimePicker();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_DateTimePicker;
                    CreatePickBox(this.ctrl);
                    form.cnt_DateTimePicker++;
                    break;
                case "DomainUpDown":
                    this.ctrl = new DomainUpDown();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_DomainUpDown;
                    form.cnt_DomainUpDown++;
                    break;
                case "FlowLayoutPanel":
                    this.ctrl = new FlowLayoutPanel();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_FlowLayoutPanel;
                    FlowLayoutPanel? flwlayoutpnl = this.ctrl as FlowLayoutPanel;
                    flwlayoutpnl!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    form.cnt_FlowLayoutPanel++;
                    break;
                case "Splitter":
                    this.ctrl = new Splitter();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_Splitter;
                    Splitter? splitter = this.ctrl as Splitter;
                    splitter!.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    form.cnt_Splitter++;
                    break;
                case "TableLayoutPanel":
                    this.ctrl = new TableLayoutPanel();
                    this.ctrl.Size = new System.Drawing.Size(151, 121);
                    this.ctrl!.Name = className + form!.cnt_TblLayPnl;
                    TableLayoutPanel? tbllaypnl = this.ctrl as TableLayoutPanel;
                    tbllaypnl!.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
                    form.cnt_TblLayPnl++;
                    break;
                default:
                    return false;
            }

            if (className != "DateTimePicker" && className != "WebBrowser")
            {
                this.ctrl!.Text = this.ctrl!.Name;
            }
            form!.cnt_Control++;
            this.ctrl!.TabIndex = form.cnt_Control;
            return true;
        }
        internal static void AddToolList(ListBox ctrlLstBox)
        {
            ctrlLstBox.Items.Add("");
            ctrlLstBox.Items.Add("Button");
            ctrlLstBox.Items.Add("Label");
            ctrlLstBox.Items.Add("GroupBox");
            ctrlLstBox.Items.Add("TextBox");
            ctrlLstBox.Items.Add("ListBox");
            ctrlLstBox.Items.Add("TabControl");
            ctrlLstBox.Items.Add("TabPage");
            ctrlLstBox.Items.Add("CheckBox");
            ctrlLstBox.Items.Add("ComboBox");
            ctrlLstBox.Items.Add("SplitContainer");
            ctrlLstBox.Items.Add("DataGridView");
            ctrlLstBox.Items.Add("Panel");
            ctrlLstBox.Items.Add("CheckedListBox");
            ctrlLstBox.Items.Add("LinkLabel");
            ctrlLstBox.Items.Add("PictureBox");
            ctrlLstBox.Items.Add("ProgressBar");
            ctrlLstBox.Items.Add("RadioButton");
            ctrlLstBox.Items.Add("RichTextBox");
            ctrlLstBox.Items.Add("StatusStrip");
            ctrlLstBox.Items.Add("HScrollBar");
            ctrlLstBox.Items.Add("VScrollBar");
            ctrlLstBox.Items.Add("MonthCalendar");
            ctrlLstBox.Items.Add("ListView");
            ctrlLstBox.Items.Add("TreeView");
            ctrlLstBox.Items.Add("MaskedTextBox");
            ctrlLstBox.Items.Add("PropertyGrid");
            ctrlLstBox.Items.Add("DateTimePicker");
            ctrlLstBox.Items.Add("DomainUpDown");
            ctrlLstBox.Items.Add("FlowLayoutPanel");
            ctrlLstBox.Items.Add("Splitter");
            ctrlLstBox.Items.Add("TableLayoutPanel");
            // ctrlLstBox.Items.Add("ToolStlipContainer");
            // ctrlLstBox.Items.Add("TrackBar");
        }
        // ****************************************************************************************
    }


}
