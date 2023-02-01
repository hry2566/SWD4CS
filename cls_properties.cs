using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace SWD4CS
{
    public class cls_properties
    {
        // ********************************************************************************************
        // internal Function 
        // ********************************************************************************************
        internal static string SetProperty(Component ctrl, string propertyName, string propertyValue)
        {
            PropertyInfo? property = ctrl!.GetType().GetProperty(propertyName!);

            if (property != null && property!.GetValue(ctrl) != null)
            {
                Type type = property!.GetValue(ctrl)!.GetType();

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

        // ********************************************************************************************
        // private Function 
        // ********************************************************************************************
        private static Size String2Size(string propertyValue)
        {
            string[] split = propertyValue.Split("(");
            string dummy = split[1];
            split = dummy.Split(")");
            dummy = split[0];
            split = dummy.Split(",");
            Size size = new(int.Parse(split[0]), int.Parse(split[1]));
            return size;
        }
        private static Point String2Point(string propertyValue)
        {
            string[] split = propertyValue.Split("(");
            string dummy = split[1];
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

            if (propertyValue == "Color.Transparent") { color = Color.Transparent; }
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
                    if (propertyValue == "") { propertyValue += split3[split3.Length - 1]; }
                    else { propertyValue += "," + split3[split3.Length - 1]; }
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
    }
}