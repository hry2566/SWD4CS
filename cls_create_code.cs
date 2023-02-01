using System.ComponentModel;
using System.Reflection;

namespace SWD4CS
{
    public class cls_create_code
    {
        private static FILE_INFO fileInfo;
        private static cls_userform? userForm;

        // ********************************************************************************************
        // internal Function 
        // ********************************************************************************************
        internal static string Create_SourceCode(FILE_INFO fInfo, cls_userform form)
        {
            fileInfo = fInfo;
            userForm = form;
            string source = "";
            List<string> lstSuspend = new();
            List<string> lstResume = new();
            string space = "";

            if (fileInfo.source_base[0].IndexOf(";") == -1) { space = space.PadLeft(8); }
            else { space = space.PadLeft(4); }

            source = Create_Code_Instance(source, space);
            source = Create_Code_Suspend_Resume(source, lstSuspend, lstResume, space);

            // suspend
            for (int i = 0; i < lstSuspend.Count; i++) { source += lstSuspend[i]; }

            source = Create_Code_Property(source, space);

            source = Create_Code_FormProperty(source, space);
            source = Create_Code_FormAddControl(source, space);

            // resume
            for (int i = 0; i < lstResume.Count; i++) { source += lstResume[i]; }

            source = Create_Code_EventDeclaration(source, space);

            if (fileInfo.source_base[0].IndexOf(";") == -1) { source += "    }" + Environment.NewLine; }

            source += "}" + Environment.NewLine;
            source += Environment.NewLine;

            // events function
            source = Create_Code_FuncDeclaration(source);
            return source;
        }

        // ********************************************************************************************
        // private Function 
        // ********************************************************************************************
        private static string Create_Code_Instance(string source, string space)
        {
            // Instance
            for (int i = 0; i < fileInfo.source_base.Count; i++) { source += fileInfo.source_base[i] + Environment.NewLine; }
            source += space + "{" + Environment.NewLine;
            return source;
        }

        private static string Create_Code_Suspend_Resume(string source, List<string> lstSuspend, List<string> lstResume, string space)
        {
            // Suspend & resume
            for (int i = 0; i < userForm!.CtrlItems.Count; i++)
            {
                source += space + "    this." + userForm.CtrlItems[i].ctrl!.Name + " = new System.Windows.Forms." + userForm.CtrlItems[i].className + "();" + Environment.NewLine;

                List<string> className_group1 = new()
                {
                    "DataGridView",
                    "PictureBox",
                    "SplitContainer",
                    "TrackBar"
                };
                for (int j = 0; j < className_group1.Count; j++)
                {
                    if (userForm.CtrlItems[i].className == className_group1[j])
                    {
                        lstSuspend.Add(space + "    ((System.ComponentModel.ISupportInitialize)(this." + userForm.CtrlItems[i].ctrl!.Name + ")).BeginInit();" + Environment.NewLine);
                        lstResume.Add(space + "    ((System.ComponentModel.ISupportInitialize)(this." + userForm.CtrlItems[i].ctrl!.Name + ")).EndInit();" + Environment.NewLine);
                    }
                }

                List<string> className_group2 = new()
                {
                    "GroupBox",
                    "Panel",
                    "StatusStrip",
                    "TabControl",
                    "TabPage",
                    "FlowLayoutPanel",
                    "TableLayoutPanel"
                };
                for (int j = 0; j < className_group2.Count; j++)
                {
                    if (userForm.CtrlItems[i].className == className_group2[j])
                    {
                        lstSuspend.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".SuspendLayout();" + Environment.NewLine);
                        lstResume.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".ResumeLayout(false);" + Environment.NewLine);
                    }
                }

                if (userForm.CtrlItems[i].className == "SplitContainer")
                {
                    lstSuspend.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel1.SuspendLayout();" + Environment.NewLine);
                    lstSuspend.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel2.SuspendLayout();" + Environment.NewLine);
                    lstSuspend.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".SuspendLayout();" + Environment.NewLine);
                    lstResume.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel1.ResumeLayout(false);" + Environment.NewLine);
                    lstResume.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel2.ResumeLayout(false);" + Environment.NewLine);
                    lstResume.Add(space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".ResumeLayout(false);" + Environment.NewLine);
                }
            }
            lstSuspend.Add(space + "    this.SuspendLayout();" + Environment.NewLine);
            lstResume.Add(space + "    this.ResumeLayout(false);" + Environment.NewLine);
            return source;
        }

        private static string Create_Code_Property(string source, string space)
        {
            // Property
            for (int i = 0; i < userForm!.CtrlItems.Count; i++)
            {
                string memCode = "";
                source += space + "    //" + Environment.NewLine;
                source += space + "    // " + userForm.CtrlItems[i].ctrl!.Name + Environment.NewLine;
                source += space + "    //" + Environment.NewLine;
                source = Create_Code_AddControl(source, space, i);

                // Property
                Type? ctlType = userForm.CtrlItems[i].nonCtrl!.GetType();

                if (ctlType == typeof(Component)) { ctlType = userForm.CtrlItems[i].ctrl!.GetType(); }
                else { ctlType = userForm.CtrlItems[i].nonCtrl!.GetType(); }

                foreach (PropertyInfo item in ctlType.GetProperties())
                {
                    if (HideProperty(item.Name))
                    {
                        Get_Code_Property(ref source, ref memCode, item, userForm.CtrlItems[i], space);
                    }
                }
                if (memCode != "") { source += memCode; }
                source = Create_Code_EventsDec(source, space, userForm.CtrlItems[i]);
            }
            return source;
        }

        private static string Create_Code_FormProperty(string source, string space)
        {
            // form-property
            source += space + "    //" + Environment.NewLine;
            source += space + "    // form" + Environment.NewLine;
            source += space + "    //" + Environment.NewLine;

            foreach (PropertyInfo item in userForm!.GetType().GetProperties())
            {
                if (HideProperty(item.Name))
                {
                    Control? baseForm = new Form();

                    if (item.GetValue(userForm) != null && item.GetValue(baseForm) != null)
                    {
                        if (item.GetValue(userForm)!.ToString() != item.GetValue(baseForm)!.ToString())
                        {
                            string str1 = space + "    this." + item.Name;
                            string strProperty = Property2String(userForm, item);

                            if (strProperty != "" && item.Name != "Name" && item.Name != "Location")
                            {
                                source += str1 + strProperty + Environment.NewLine;
                            }
                        }
                    }
                }
            }
            source = Create_Code_FormEventsDec(source, space, userForm);
            return source;
        }

        private static string Create_Code_FormAddControl(string source, string space)
        {
            // AddControl
            for (int i = 0; i < userForm!.CtrlItems.Count; i++)
            {
                if (userForm.CtrlItems[i].ctrl!.Parent == userForm)
                {
                    source += space + "    this.Controls.Add(this." + userForm.CtrlItems[i].ctrl!.Name + ");" + Environment.NewLine;
                }
            }
            return source;
        }

        private static string Create_Code_EventDeclaration(string source, string space)
        {
            source += space + "}" + Environment.NewLine;
            source += Environment.NewLine;
            source += space + "#endregion" + Environment.NewLine;
            source += Environment.NewLine;

            // declaration
            for (int i = 0; i < userForm!.CtrlItems.Count; i++)
            {
                string dec = "";
                Type type = userForm.CtrlItems[i].nonCtrl!.GetType();
                if (type == typeof(Component)) { dec = userForm.CtrlItems[i].ctrl!.GetType().ToString(); }
                else { dec = userForm.CtrlItems[i].nonCtrl!.GetType().ToString(); }
                source += space + "private " + dec + " " + userForm.CtrlItems[i].ctrl!.Name + ";" + Environment.NewLine;
            }
            return source;
        }

        private static string Create_Code_FuncDeclaration(string source)
        {
            // control
            for (int i = 0; i < userForm!.CtrlItems.Count; i++)
            {
                for (int j = 0; j < userForm.CtrlItems[i].decFunc.Count; j++)
                {
                    source += "//" + userForm.CtrlItems[i].decFunc[j] + Environment.NewLine;
                    source += "//{" + Environment.NewLine;
                    source += "//" + Environment.NewLine;
                    source += "//}" + Environment.NewLine;
                    source += Environment.NewLine;
                }
            }

            // form
            for (int i = 0; i < userForm.decFunc.Count; i++)
            {
                source += "//" + userForm.decFunc[i] + Environment.NewLine;
                source += "//{" + Environment.NewLine;
                source += "//" + Environment.NewLine;
                source += "//}" + Environment.NewLine;
                source += Environment.NewLine;
            }
            return source;
        }

        private static string Create_Code_AddControl(string source, string space, int i)
        {
            // AddControl
            for (int j = 0; j < userForm!.CtrlItems.Count; j++)
            {
                if (userForm.CtrlItems[i].ctrl!.Name == userForm.CtrlItems[j].ctrl!.Parent!.Name)
                {
                    source += space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Controls.Add(this." + userForm.CtrlItems[j].ctrl!.Name + ");" + Environment.NewLine;
                }
                else if (userForm.CtrlItems[i].ctrl!.Name == userForm.CtrlItems[j].ctrl!.Parent!.Parent!.Name)
                {
                    if (userForm.CtrlItems[j].ctrl!.Parent!.Name.IndexOf("Panel1") > -1)
                    {
                        source += space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel1.Controls.Add(this." + userForm.CtrlItems[j].ctrl!.Name + ");" + Environment.NewLine;
                    }
                    else if (userForm.CtrlItems[j].ctrl!.Parent!.Name.IndexOf("Panel2") > -1)
                    {
                        source += space + "    this." + userForm.CtrlItems[i].ctrl!.Name + ".Panel2.Controls.Add(this." + userForm.CtrlItems[j].ctrl!.Name + ");" + Environment.NewLine;
                    }
                }
            }
            return source;
        }

        private static void Get_Code_Property(ref string source, ref string memCode, PropertyInfo item, cls_controls ctrlItems, string space)
        {
            Component? comp;
            Component? baseCtrl;
            if (ctrlItems.nonCtrl!.GetType() == typeof(Component)) { comp = ctrlItems.ctrl; }
            else { comp = ctrlItems.nonCtrl; }
            baseCtrl = GetBaseCtrl(ctrlItems);

            if (item.GetValue(comp) == null || item.GetValue(baseCtrl) == null) { return; }
            if (item.GetValue(comp)!.ToString() != item.GetValue(baseCtrl)!.ToString())
            {
                string str1 = space + "    this." + ctrlItems.ctrl!.Name + "." + item.Name;
                string strProperty = Property2String(comp!, item);
                if (strProperty != "")
                {
                    bool flag = item.Name != "SplitterDistance" && item.Name != "Anchor";
                    if (flag) { source += str1 + strProperty + Environment.NewLine; }
                    else { memCode += str1 + strProperty + Environment.NewLine; }
                }
            }
        }

        private static string Create_Code_EventsDec(string source, string space, cls_controls cls_ctrl)
        {
            int cnt = cls_ctrl.decHandler.Count;
            for (int i = 0; i < cnt; i++)
            { source += space + "    " + cls_ctrl.decHandler[i] + Environment.NewLine; }
            return source;
        }

        private static string Create_Code_FormEventsDec(string source, string space, cls_userform userForm)
        {
            int cnt = userForm.decHandler.Count;
            for (int i = 0; i < cnt; i++) { source += space + "    " + userForm.decHandler[i] + Environment.NewLine; }
            return source;
        }

        private static string AnchorStyles2String(object? propertyinfo)
        {
            string strProperty;
            string[] split = propertyinfo!.ToString()!.Split(',');
            Type type = propertyinfo.GetType();
            string str2 = propertyinfo.ToString()!;

            if (split.Length == 1) { strProperty = " = " + type.ToString() + "." + str2 + ";"; }
            else
            {
                string ancho = "";
                for (int j = 0; j < split.Length; j++)
                {
                    if (j == 0) { ancho = "(" + type.ToString() + "." + split[j].Trim(); }
                    else { ancho = "(" + ancho + " | " + type.ToString() + "." + split[j].Trim() + ")"; }
                }
                ancho = "((" + type.ToString() + ")" + ancho + "));";
                strProperty = " = " + ancho;
            }
            return strProperty;
        }
        private static string Property2String(Component ctrl, PropertyInfo item)
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
                    Control ctl = (Control)ctrl;
                    strProperty = " = " + Property2Font(ctl.Font) + ";";
                    break;
            }
            return strProperty;
        }

        private static Component? GetBaseCtrl(cls_controls ctrl)
        {
            Type type;
            if (ctrl.nonCtrl!.GetType() == typeof(Component)) { type = ctrl.ctrl!.GetType(); }
            else { type = ctrl.nonCtrl!.GetType(); }
            Component? baseCtrl = (Component)Activator.CreateInstance(type)!;
            return baseCtrl;
        }

        private static bool HideProperty(string itemName)
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
                "ColumnCount",
                "FirstDisplayedScrollingColumnIndex",
                "FirstDisplayedScrollingRowIndex",
                "NewRowIndex",
                "RowCount",
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
                "CompanyName",
                "ProductName",
                "ProductVersion",
                "TopLevel",
                "Tag",
                "Site",
                "Container",
                "",
                "",
            };

            for (int i = 0; i < propertyName.Count; i++)
            {
                if (propertyName[i] == itemName) { return false; }
            }
            return true;
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

            if (color == "Transparent") { return "Color.Transparent"; }

            for (int i = 0; i < systemColorName.Count; i++)
            {
                if (systemColorName[i] == color) { return strSystemColor + color; }
            }

            for (int i = 0; i < colorName.Count; i++)
            {
                if (colorName[i] == color) { return strColor + color; }
            }

            color = color.Replace("A", "").Replace("R", "").Replace("G", "").Replace("B", "").Replace("=", "");
            string[] split = color.Split(",");
            strRGB += split[1] + "," + split[2] + "," + split[3] + ")";
            return strRGB;
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
    }
}