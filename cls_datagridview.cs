

using System.Reflection;

namespace SWD4CS
{
    public partial class cls_datagridview : DataGridView
    {
        private cls_form? form;
        private readonly ComboBox pval_combobox = new();
        private readonly CheckedListBox pval_checkedlistbox = new();

        public cls_datagridview()
        {
            InitializeComponent();

            pval_combobox.Visible = false;
            pval_combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            pval_combobox.MouseLeave += new System.EventHandler(Ctrl_MouseLeave);
            pval_combobox.SelectedValueChanged += new System.EventHandler(Pval_combobox_ValueChanged);
            this.Controls.Add(pval_combobox);

            pval_checkedlistbox.Visible = false;
            pval_checkedlistbox.CheckOnClick = true;
            pval_checkedlistbox.MouseLeave += new System.EventHandler(Ctrl_MouseLeave);
            pval_checkedlistbox.SelectedValueChanged += new System.EventHandler(Pval_checkedlistbox_Click);
            this.Controls.Add(pval_checkedlistbox);

            this.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(PropertyCellClick);
        }

        private void Ctrl_MouseLeave(object? sender, EventArgs e)
        {
            Control? ctrl = sender as Control;
            ctrl!.Visible = false;
        }

        private void Pval_checkedlistbox_Click(object? sender, EventArgs e)
        {
            string? property = "";

            for (int i = 0; i < pval_checkedlistbox.Items.Count; i++)
            {
                if (pval_checkedlistbox.GetItemChecked(i))
                {
                    if (property == "")
                    {
                        property = pval_checkedlistbox.Items[i].ToString();
                    }
                    else
                    {
                        property += "," + pval_checkedlistbox.Items[i].ToString();
                    }
                }
            }

            this.CurrentRow.Cells[1].Value = property;
            pval_checkedlistbox.Visible = false;
        }

        private void Pval_combobox_ValueChanged(object? sender, EventArgs e)
        {
            if (pval_combobox.Visible)
            {
                this.CurrentRow.Cells[1].Value = pval_combobox.Text;
                pval_combobox.Visible = false;
            }
        }

        internal void Init(cls_form cls_design_form1)
        {
            this.form = cls_design_form1;
        }

        private void PropertyCellClick(object? sender, DataGridViewCellEventArgs e)
        {
            string? ctrlName;
            string? strValue;
            string? strProperty;
            int propertyMode = -1;
            string[] strItems = new string[1];
            Type type;

            if (e.ColumnIndex == 1)
            {
                ctrlName = GetCtrlName();
                strProperty = this.CurrentRow.Cells[0].Value.ToString();
                strValue = this.CurrentRow.Cells[1].Value.ToString();
                type = GetCtrlType(ctrlName, strProperty);

                if (type != null)
                {
                    pval_combobox.Items.Clear();
                    pval_checkedlistbox.Items.Clear();

                    switch (type)
                    {
                        case Type t when t == typeof(System.Boolean):
                            strItems = new string[2] { "True", "False" };
                            pval_combobox.Items.AddRange(strItems);
                            propertyMode = 0;
                            break;
                        case Type t when t == typeof(System.Drawing.ContentAlignment):
                            strItems = new string[9] { "BottomCenter", "BottomLeft", "BottomRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "TopCenter", "TopLeft", "TopRight" };
                            pval_combobox.Items.AddRange(strItems);
                            propertyMode = 0;
                            break;
                        case Type t when t == typeof(System.Windows.Forms.DockStyle):
                            strItems = new string[6] { "None", "Top", "Bottom", "Fill", "Left", "Right" };
                            pval_combobox.Items.AddRange(strItems);
                            propertyMode = 0;
                            break;
                        case Type t when t == typeof(System.Windows.Forms.AnchorStyles):
                            strItems = new string[4] { "Top", "Bottom", "Left", "Right" };
                            pval_checkedlistbox.Items.AddRange(strItems);

                            string[] split = strValue!.Split(",");
                            for (int i = 0; i < split.Count(); i++)
                            {
                                switch (split[i].Trim())
                                {
                                    case "Left":
                                        pval_checkedlistbox.SetItemChecked(2, true);
                                        break;
                                    case "Right":
                                        pval_checkedlistbox.SetItemChecked(3, true);
                                        break;
                                    case "Top":
                                        pval_checkedlistbox.SetItemChecked(0, true);
                                        break;
                                    case "Bottom":
                                        pval_checkedlistbox.SetItemChecked(1, true);
                                        break;
                                }
                            }
                            propertyMode = 1;
                            break;
                        case Type t when t == typeof(System.Windows.Forms.ScrollBars):
                            strItems = new string[4] { "None", "Both", "Vertical", "Horizontal" };
                            pval_combobox.Items.AddRange(strItems);
                            propertyMode = 0;
                            break;
                        case Type t when t == typeof(System.Windows.Forms.HorizontalAlignment):
                            strItems = new string[3] { "Center", "Left", "Right" };
                            pval_combobox.Items.AddRange(strItems);
                            propertyMode = 0;
                            break;
                    }

                    Rectangle rect = this.GetCellDisplayRectangle(this.CurrentCell.ColumnIndex, this.CurrentCell.RowIndex, false);
                    Control ctrl = new();
                    switch (propertyMode)
                    {
                        case 0:
                            ctrl = pval_combobox;
                            break;
                        case 1:
                            ctrl = pval_checkedlistbox;
                            rect.Size = new(rect.Size.Width, 100);
                            break;
                    }

                    ctrl.Location = rect.Location;
                    ctrl.Size = rect.Size;

                    if (propertyMode != -1)
                    {
                        ctrl.Visible = true;
                    }
                }
            }
        }

        private Type GetCtrlType(string ctrlName, string? strProperty)
        {
            Type? type = null;

            for (int i = 0; i < form!.CtrlItems.Count; i++)
            {
                if (form.CtrlItems[i].ctrl!.Name == ctrlName)
                {
                    PropertyInfo? property = form.CtrlItems[i].ctrl!.GetType().GetProperty(strProperty!);
                    type = property!.GetValue(form.CtrlItems[i].ctrl)!.GetType();
                    break;
                }
            }

            return type!;
        }

        private string GetCtrlName()
        {
            string strName = "";

            for (int i = 0; i < this.Rows.Count - 1; i++)
            {
                if (this.Rows[i].Cells[0].Value.ToString() == "Name")
                {
                    strName = this.Rows[i].Cells[1].Value.ToString()!;
                    break;
                }
            }
            return strName;
        }
    }
}
