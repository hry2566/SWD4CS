using System.Data;
using System.Reflection;

namespace SWD4CS
{
    public partial class cls_user_datagridview : DataGridView
    {
        private cls_userform? form;
        private cls_controls? cls_ctrl;
        public cls_user_datagridview()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.AllowUserToAddRows = false;
            this.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(cls_user_datagridview1_CellMouseDoubleClick);
        }

        internal void ShowEventList(bool flag, cls_userform form)
        {
            this.form = form;
            this.cls_ctrl = null;

            List<string> evnt = new();
            List<string> fnc = new();
            string[] split;
            for (int i = 0; i < form.decHandler.Count; i++)
            {
                split = form.decHandler[i].Split("+=")[0].Split(".");
                evnt.Add(split[split.Length - 1].Trim());

                split = form.decFunc[i].Split("(")[0].Split(" ");
                fnc.Add(split[split.Length - 1].Trim());
            }

            SetEventsData(flag, form.memForm, evnt, fnc);
        }
        internal void ShowEventList(bool flag, cls_controls ctrl)
        {
            this.form = null;
            this.cls_ctrl = ctrl;

            List<string> evnt = new();
            List<string> fnc = new();
            string[] split;
            for (int i = 0; i < ctrl.decHandler.Count; i++)
            {
                split = ctrl.decHandler[i].Split("+=")[0].Split(".");
                evnt.Add(split[split.Length - 1].Trim());

                split = ctrl.decFunc[i].Split("(")[0].Split(" ");
                fnc.Add(split[split.Length - 1].Trim());
            }
            SetEventsData(flag, ctrl.ctrl, evnt, fnc);
        }

        private void SetEventsData(bool flag, Control? ctrl, List<string> evnt, List<string> fnc)
        {
            DataTable table = new();

            table.Columns.Add("Event");
            table.Columns.Add("Function");

            if (flag)
            {
                Type type = ctrl!.GetType();
                MemberInfo[] members = type.GetMembers();
                foreach (MemberInfo m in members)
                {
                    string funcName = "";
                    if (m.MemberType.ToString() == "Event")
                    {
                        for (int i = 0; i < evnt.Count; i++)
                        {
                            if (evnt[i] == m.Name)
                            {
                                funcName = fnc[i];
                                break;
                            }
                        }
                        table.Rows.Add(m.Name, funcName);
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
            Control? ctrl;

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
            string param = "";

            if (this.Rows[e.RowIndex].Cells[1].Value.ToString() == "")
            {
                this.Rows[e.RowIndex].Cells[1].Value = funcName;

                Type? delegateType = ctrl.GetType().GetEvent(eventName!)!.EventHandlerType;
                MethodInfo? invoke = delegateType!.GetMethod("Invoke");
                ParameterInfo[] pars = invoke!.GetParameters();
                string[] split = delegateType.AssemblyQualifiedName!.Split(",");
                newHandler = "new " + split[0];

                SetArguments(ref funcParam, ref param, pars);

                string decHandler = GetDecHandler(eventName, newHandler, funcName, ctrl.Name);
                string decFunc = "private void " + funcName + "(" + funcParam + ")";
                DeclarationAdd(decHandler, decFunc);
            }
            else
            {
                Delete_Event(e, funcName);
            }
        }

        private void Delete_Event(DataGridViewCellMouseEventArgs e, string funcName)
        {
            this.Rows[e.RowIndex].Cells[1].Value = "";

            if (this.form != null)
            {
                for (int i = 0; i < this.form!.decFunc.Count; i++)
                {
                    string[] split = this.form!.decFunc[i].Split("(")[0].Split(" ");

                    if (split[split.Length - 1] == funcName)
                    {
                        this.form!.decHandler.Remove(this.form!.decHandler[i]);
                        this.form!.decFunc.Remove(this.form!.decFunc[i]);
                        break;
                    }
                }
            }
            else if (this.cls_ctrl != null)
            {
                for (int i = 0; i < this.cls_ctrl!.decFunc.Count; i++)
                {
                    string[] split = this.cls_ctrl!.decFunc[i].Split("(")[0].Split(" ");

                    if (split[split.Length - 1] == funcName)
                    {
                        this.cls_ctrl!.decHandler.Remove(this.cls_ctrl!.decHandler[i]);
                        this.cls_ctrl!.decFunc.Remove(this.cls_ctrl!.decFunc[i]);
                        break;
                    }
                }
            }
        }

        private void DeclarationAdd(string decHandler, string decFunc)
        {
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
        }

        private string GetDecHandler(string? eventName, string newHandler, string funcName, string ctrlName)
        {
            string decHandler;
            if (this.form != null)
            {
                decHandler = "this." + eventName + " += " + newHandler + "(" + funcName + ");";
            }
            else
            {
                decHandler = "this." + ctrlName + "." + eventName + " += " + newHandler + "(" + funcName + ");";
            }
            return decHandler;
        }

        private void SetArguments(ref string funcParam, ref string param, ParameterInfo[] pars)
        {
            foreach (ParameterInfo p in pars)
            {
                param = p.ParameterType.ToString();

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
        }
    }
}
