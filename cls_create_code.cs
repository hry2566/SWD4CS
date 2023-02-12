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
            space = !fileInfo.source_base[0].Contains(";") ? space.PadLeft(8) : space.PadLeft(4);

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

            if (!fileInfo.source_base[0].Contains(";")) { source += "    }" + Environment.NewLine; }

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
            string[] className_group1 = new string[] { "DataGridView", "PictureBox", "SplitContainer", "TrackBar" };
            string[] className_group2 = new string[] { "GroupBox", "Panel", "StatusStrip", "TabControl", "TabPage", "FlowLayoutPanel", "TableLayoutPanel" };

            // Suspend & resume
            foreach (var item in userForm!.CtrlItems)
            {
                source += $"{space}    this.{item.ctrl!.Name} = new System.Windows.Forms.{item.className}();{Environment.NewLine}";

                if (className_group1.Contains(item.className))
                {
                    lstSuspend.Add($"{space}    ((System.ComponentModel.ISupportInitialize)(this.{item.ctrl!.Name})).BeginInit();{Environment.NewLine}");
                    lstResume.Add($"{space}    ((System.ComponentModel.ISupportInitialize)(this.{item.ctrl!.Name})).EndInit();{Environment.NewLine}");
                }
                if (className_group2.Contains(item.className))
                {
                    lstSuspend.Add($"{space}    this.{item.ctrl!.Name}.SuspendLayout();{Environment.NewLine}");
                    lstResume.Add($"{space}    this.{item.ctrl!.Name}.ResumeLayout(false);{Environment.NewLine}");
                }
                if (item.className == "SplitContainer")
                {
                    lstSuspend.Add($"{space}    this.{item.ctrl!.Name}.Panel1.SuspendLayout();{Environment.NewLine}");
                    lstSuspend.Add($"{space}    this.{item.ctrl!.Name}.Panel2.SuspendLayout();{Environment.NewLine}");
                    lstSuspend.Add($"{space}    this.{item.ctrl!.Name}.SuspendLayout();{Environment.NewLine}");
                    lstResume.Add($"{space}    this.{item.ctrl!.Name}.Panel1.ResumeLayout(false);{Environment.NewLine}");
                    lstResume.Add($"{space}    this.{item.ctrl!.Name}.Panel2.ResumeLayout(false);{Environment.NewLine}");
                    lstResume.Add($"{space}    this.{item.ctrl!.Name}.ResumeLayout(false);{Environment.NewLine}");
                }
            }
            lstSuspend.Add($"{space}    this.SuspendLayout();{Environment.NewLine}");
            lstResume.Add($"{space}    this.ResumeLayout(false);{Environment.NewLine}");
            return source;
        }

        private static string Create_Code_Property(string source, string space)
        {
            for (int i = 0; i < userForm!.CtrlItems.Count; i++)
            {
                string memCode = "";
                Control ctrl = userForm.CtrlItems[i].ctrl!;
                Component comp = userForm.CtrlItems[i].nonCtrl!;
                Type ctlType = comp.GetType() == typeof(Component) ? ctrl.GetType() : comp.GetType();

                source += $"{space}    //{Environment.NewLine}" +
                          $"{space}    // {ctrl.Name}{Environment.NewLine}" +
                          $"{space}    //{Environment.NewLine}";
                source = Create_Code_AddControl(source, space, i);

                foreach (PropertyInfo item in ctlType.GetProperties())
                {
                    if (!IsReadOnlyProperty(item) && HideProperty(item.Name))
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
            source += $"{space} //{Environment.NewLine}" +
                      $"{space} // form{Environment.NewLine}" +
                      $"{space} //{Environment.NewLine}";

            Type formType = typeof(Form);
            var formProperties = userForm!.GetType().GetProperties().Where(prop => !IsReadOnlyProperty(prop) && HideProperty(prop.Name));

            foreach (PropertyInfo item in formProperties)
            {
                object? userFormValue = item.GetValue(userForm);
                object? baseFormValue = item.GetValue(new Form());

                if (userFormValue != null && baseFormValue != null && userFormValue.ToString() != baseFormValue.ToString())
                {
                    string str1 = $"{space}    this.{item.Name}";
                    string strProperty = Property2String(userForm, item);

                    if (!string.IsNullOrEmpty(strProperty) && item.Name != "Name" && item.Name != "Location")
                    {
                        source += str1 + strProperty + Environment.NewLine;
                    }
                }
            }
            source = Create_Code_FormEventsDec(source, space, userForm);
            return source;
        }

        private static bool IsReadOnlyProperty(PropertyInfo item)
        {
            var attributes = item.GetCustomAttributes(typeof(ReadOnlyAttribute), false);
            return attributes.Length > 0;
        }

        private static string Create_Code_FormAddControl(string source, string space)
        {
            // AddControl
            foreach (var ctrlItem in userForm!.CtrlItems.Where(i => i.ctrl!.Parent == userForm))
            {
                source += $"{space}    this.Controls.Add(this.{ctrlItem.ctrl!.Name});{Environment.NewLine}";
            }
            return source;
        }

        private static string Create_Code_EventDeclaration(string source, string space)
        {
            source += $"{space}}} {Environment.NewLine}" +
                      $"{Environment.NewLine}" +
                      $"{space}#endregion {Environment.NewLine}" +
                      $"{Environment.NewLine}";

            // declaration
            foreach (var ctrlItem in userForm!.CtrlItems)
            {
                Type type = ctrlItem.nonCtrl!.GetType();
                string typeName1 = ctrlItem.ctrl!.GetType().ToString();
                string typeName2 = ctrlItem.nonCtrl!.GetType().ToString();
                string dec = type == typeof(Component) ? typeName1 : typeName2;
                source += $"{space}private {dec} {ctrlItem.ctrl!.Name};{Environment.NewLine}";
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
                    source += $"// {userForm.CtrlItems[i].decFunc[j]}{Environment.NewLine}" +
                              $"// {{{Environment.NewLine}" +
                              $"// {Environment.NewLine}" +
                              $"// }}{Environment.NewLine}" +
                              Environment.NewLine;
                }
            }

            // form
            foreach (string decFunc in userForm.decFunc)
            {
                source += $"// {decFunc}{Environment.NewLine}" +
                          "// {" + Environment.NewLine +
                          "//" + Environment.NewLine +
                          "// }" + Environment.NewLine +
                          Environment.NewLine;
            }
            return source;
        }

        private static string Create_Code_AddControl(string source, string space, int i)
        {
            // AddControl
            Control ctrl1 = userForm!.CtrlItems[i].ctrl!;

            for (int j = 0; j < userForm.CtrlItems.Count; j++)
            {
                Control ctrl2 = userForm.CtrlItems[j].ctrl!;

                if (ctrl1!.Name == ctrl2!.Parent!.Name)
                {
                    source += $"{space}    this.{ctrl1.Name}.Controls.Add(this.{ctrl2.Name});{Environment.NewLine}";
                }
                else if (ctrl1.Name == ctrl2.Parent!.Parent!.Name)
                {
                    if (ctrl2.Parent!.Name.Contains("Panel1"))
                    {
                        source += $"{space}    this.{ctrl1.Name}.Panel1.Controls.Add(this.{ctrl2.Name});{Environment.NewLine}";
                    }
                    else if (ctrl2.Parent!.Name.Contains("Panel2"))
                    {
                        source += $"{space}    this.{ctrl1!.Name}.Panel2.Controls.Add(this.{ctrl2.Name});{Environment.NewLine}";
                    }
                }
            }
            return source;
        }

        private static void Get_Code_Property(ref string source, ref string memCode, PropertyInfo item, cls_controls ctrlItems, string space)
        {
            Component? comp = (ctrlItems.nonCtrl!.GetType() == typeof(Component)) ? ctrlItems.ctrl : ctrlItems.nonCtrl;
            Component? baseCtrl = GetBaseCtrl(ctrlItems);

            if (item.GetValue(comp) == null || item.GetValue(baseCtrl) == null) { return; }
            if (item.GetValue(comp)!.ToString() == item.GetValue(baseCtrl)!.ToString()) { return; }

            string str1 = space + "    this." + ctrlItems.ctrl!.Name + "." + item.Name;
            string strProperty = Property2String(comp!, item);

            if (strProperty == "") { return; }

            bool flag = item.Name != "SplitterDistance" && item.Name != "Anchor";
            if (flag) { source += str1 + strProperty + Environment.NewLine; }
            else { memCode += str1 + strProperty + Environment.NewLine; }
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
            object value = item.GetValue(ctrl)!;
            if (value == null) { return ""; }

            Type type = value.GetType();
            switch (type.Name)
            {
                case nameof(System.Drawing.Point):
                    Point point = (Point)value;
                    return $" = new {type}({point.X},{point.Y});";
                case nameof(System.Drawing.Size):
                    Size size = (Size)value;
                    return $" = new {type}({size.Width},{size.Height});";
                case nameof(System.String):
                    return $" =  \"{value}\";";
                case nameof(System.Boolean):
                    return $" =  {value.ToString()!.ToLower()};";
                case nameof(System.Windows.Forms.AnchorStyles):
                    return AnchorStyles2String(value);
                case nameof(System.Int32):
                    return $" = {(int)value};";
                case nameof(System.Drawing.Color):
                    return $" = {Property2Color(value.ToString()!)};";
                case nameof(System.Drawing.Font):
                    Control ctl = (Control)ctrl;
                    return $" = {Property2Font(ctl.Font)};";
                case nameof(System.Windows.Forms.DockStyle):
                case nameof(System.Drawing.ContentAlignment):
                case nameof(System.Windows.Forms.ScrollBars):
                case nameof(System.Windows.Forms.HorizontalAlignment):
                case nameof(System.Windows.Forms.FormWindowState):
                case nameof(System.Windows.Forms.FixedPanel):
                case nameof(System.Windows.Forms.PictureBoxSizeMode):
                case nameof(System.Windows.Forms.View):
                case nameof(System.Windows.Forms.Orientation):
                case nameof(System.Windows.Forms.FormBorderStyle):
                case nameof(System.Windows.Forms.AutoScaleMode):
                case nameof(System.Windows.Forms.TableLayoutPanelCellBorderStyle):
                case nameof(System.Windows.Forms.FormStartPosition):
                    return $" = {type}.{value};";
            }
            return "";
        }

        private static Component? GetBaseCtrl(cls_controls ctrl)
        {
            Type type = ctrl.nonCtrl!.GetType() == typeof(Component) ? ctrl.ctrl!.GetType() : ctrl.nonCtrl.GetType();
            return (Component)Activator.CreateInstance(type)!;
        }

        private static bool HideProperty(string itemName)
        {
            List<string> propertyName = new()
            {
                // "AccessibilityObject",
                // "BindingContext",
                // "Parent",
                // "TopLevelControl",
                // "DataSource",
                // "FirstDisplayedCell",
                // "Item",
                // "TopItem",
                // "Rtf",
                // "ParentForm",
                // "SelectedTab",
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
                // "Enable",
                "ClientSize",
                "UseVisualStyleBackColor",
                "PreferredHeight",
                // "ColumnCount",
                // "FirstDisplayedScrollingColumnIndex",
                // "FirstDisplayedScrollingRowIndex",
                // "NewRowIndex",
                "RowCount",
                "HasChildren",
                "PreferredWidth",
                // "SingleMonthSize",
                "TextLength",
                // "SelectedIndex",
                "TabCount",
                "VisibleCount",
                "DesktopLocation",
                "AutoScale",
                // "CanFocus",
                // "IsMirrored",
                // "SelectionStart",
                "ContextMenuDefaultLocation",
                // "CanUndo",
                "CompanyName",
                "ProductName",
                "ProductVersion",
                "TopLevel",
                // "Tag",
                // "Site",
                // "Container",
                "Name",
                ""
            };
            return !propertyName.Contains(itemName);
        }

        private static readonly string[] SystemColorName = new string[]
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

        private static readonly string[] ColorName = new string[]
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

        private static string Property2Color(string color)
        {
            color = color.Replace("Color [", "").Replace("]", "");
            if (color == "Transparent") { return "Color.Transparent"; }

            int index = Array.IndexOf(SystemColorName, color);
            if (index >= 0) { return $"System.Drawing.SystemColors.{color}"; }

            index = Array.IndexOf(ColorName, color);
            if (index >= 0) { return $"System.Drawing.Color.{color}"; }

            string[] split = color.Split(new char[] { 'A', 'R', 'G', 'B', '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return $"System.Drawing.Color.FromArgb({split[2]}, {split[4]}, {split[6]})";
        }

        private static string Property2Font(Font font)
        {
            string strSize = font.Size + "F, ";
            string strStyle = "System.Drawing.FontStyle." + font.Style.ToString() + ", System.Drawing.GraphicsUnit.Point)";
            string strProperty = "new System.Drawing.Font(\"" + font.Name + "\", " + strSize + strStyle;
            return strProperty;
        }
    }
}