using System.Data;
using System.Reflection;

namespace SWD4CS
{
    public partial class cls_user_datagridview : DataGridView
    {
        private cls_user_form? form;
        private cls_control? cls_ctrl;
        public cls_user_datagridview()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.AllowUserToAddRows = false;
            this.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(cls_user_datagridview1_CellMouseDoubleClick);
        }

        internal void ShowEventList(bool flag, cls_user_form form)
        {
            this.form = form;
            this.cls_ctrl = null;
            SetEventsData(flag, form.memForm);
        }
        internal void ShowEventList(bool flag, cls_control ctrl)
        {
            this.form = null;
            this.cls_ctrl = ctrl;
            SetEventsData(flag, ctrl.ctrl);
        }

        private void SetEventsData(bool flag, Control? ctrl)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Event");
            table.Columns.Add("Function");

            if (flag)
            {
                Type type = ctrl!.GetType();
                MemberInfo[] members = type.GetMembers();
                foreach (MemberInfo m in members)
                {
                    if (m.MemberType.ToString() == "Event")
                    {
                        table.Rows.Add(m.Name);
                    }
                }
            }

            this.DataSource = table;
            this.Sort(this.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            this.Columns[0].ReadOnly = true;
            this.Columns[1].ReadOnly = true;
            this.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void cls_user_datagridview1_CellMouseDoubleClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            Control? ctrl = new();

            if (this.form != null)
            {
                ctrl = this.form.memForm;
            }
            else
            {
                ctrl = this.cls_ctrl!.ctrl;
            }

            string? eventName = this.Rows[e.RowIndex].Cells[0].Value.ToString();
            string? newHandler;
            string? funcName = ctrl!.Name + "_" + eventName;
            string funcParam = "";

            if (this.Rows[e.RowIndex].Cells[1].Value.ToString() != "") { return; }
            this.Rows[e.RowIndex].Cells[1].Value = funcName;

            Type? delegateType = ctrl.GetType().GetEvent(eventName!)!.EventHandlerType;
            MethodInfo? invoke = delegateType!.GetMethod("Invoke");
            ParameterInfo[] pars = invoke!.GetParameters();
            string[] split = delegateType.AssemblyQualifiedName!.Split(",");
            newHandler = "new " + split[0];

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

            string decHandler = "";
            if (this.form != null)
            {
                decHandler = "this." + eventName + " += " + newHandler + "(" + funcName + ");";

            }
            else
            {
                decHandler = "this." + ctrl.Name + "." + eventName + " += " + newHandler + "(" + funcName + ");";

            }
            string decFunc = "private void " + funcName + "(" + funcParam + ")";

            if (this.form != null)
            {
                this.form!.decHandler.Add(decHandler);
                this.form.decFunc.Add(decFunc);
            }
            else
            {
                this.cls_ctrl!.decHandler.Add(decHandler);
                this.cls_ctrl.decFunc.Add(decFunc);
            }

            //Console.WriteLine(decHandler);
            //Console.WriteLine(decFunc);
        }
    }
}
