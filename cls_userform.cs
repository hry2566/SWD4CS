using System.ComponentModel;
using System.Reflection;

namespace SWD4CS
{
    internal class cls_userform : Form
    {
        private cls_selectbox? selectBox; FlowLayoutPanel? otherCtrlPanel;
        private bool selectFlag = false; int grid = 4;
        internal MainForm? mainForm;
        internal SplitterPanel? backPanel;
        internal List<cls_controls> CtrlItems = new();
        internal int cnt_Control = 0;
        internal List<string> decHandler = new();
        internal List<string> decFunc = new();
        internal string viewName = "Form1";

        public cls_userform()
        {
            this.FormClosing += new FormClosingEventHandler(userForm_FormClosing);
            this.Resize += new EventHandler(userForm_Resize);
            this.Click += new EventHandler(Form_Click);

            this.TopLevel = false;
            this.Text = "Form1";
            this.Show();
        }

        // ********************************************************************************************
        // Event Function 
        // ********************************************************************************************
        private void Form_Click(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (mainForm!.toolLstBox!.Text == "" && me.Button == MouseButtons.Left)
            {
                if (!selectFlag) { SelectAllClear(); }
                SetSelect(!selectFlag);
            }
            else if (mainForm!.toolLstBox!.Text != "")
            {
                SelectAllClear();
                int X = (int)(me.X / grid) * grid;
                int Y = (int)(me.Y / grid) * grid;
                _ = new cls_controls(this, otherCtrlPanel, mainForm!.toolLstBox!.Text, this, X, Y);
                mainForm!.toolLstBox!.SelectedIndex = 0;
            }
        }
        private void userForm_Resize(object? sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized || this.WindowState == FormWindowState.Minimized)
            { this.WindowState = FormWindowState.Normal; }
        }
        private void userForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        protected override void WndProc(ref Message m)
        {
            const long WM_NCLBUTTONDOWN = 0x00A1;
            if (m.Msg == WM_NCLBUTTONDOWN)
            {
                if (!selectFlag) { SelectAllClear(); }
                SetSelect(!selectFlag);
                m.Result = IntPtr.Zero; return;
            }
            base.WndProc(ref m);
        }
        private void BackPanel_Click(object? sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left) { SelectAllClear(); }
        }

        // ********************************************************************************************
        // internal Function
        // ********************************************************************************************
        internal void Init(MainForm mainForm, SplitterPanel backPanel, FlowLayoutPanel? otherCtrlPanel)
        {
            this.mainForm = mainForm;
            this.backPanel = backPanel;
            this.backPanel.Click += new EventHandler(BackPanel_Click);
            this.otherCtrlPanel = otherCtrlPanel;
            selectBox = new cls_selectbox(this, backPanel);
            SetSelect(true);
        }
        internal void SetSelect(bool flag)
        {
            selectFlag = flag;
            selectBox!.SetSelectBoxPos(selectFlag);
            ShowProperty(flag);
            if (flag) { mainForm!.ctrlTree!.SelectedNode = mainForm.ctrlTree.TopNode; }
            mainForm!.eventView!.ShowEventList(flag, this);
        }
        internal void SelectAllClear()
        {
            SetSelect(false);
            for (int i = 0; i < CtrlItems!.Count; i++) { CtrlItems[i].Selected = false; }
        }

        internal void RemoveSelectedItem()
        {
            for (int i = 0; i < CtrlItems!.Count; i++)
            {
                if (CtrlItems[i].Selected)
                {
                    if (CtrlItems[i].ctrl is TabPage)
                    {
                        TabControl? tabctrl = CtrlItems[i].ctrl!.Parent as TabControl;
                        if (tabctrl!.TabPages.Count > 1) { Delete(CtrlItems[i]); i--; }
                    }
                    else { Delete(CtrlItems[i]); i--; }
                }
            }
        }

        internal void CtrlAllClear()
        {
            this.cnt_Control = 0;

            for (int i = 0; i < CtrlItems!.Count; i++)
            {
                CtrlItems[i].Selected = true;
            }
            RemoveSelectedItem();
        }

        internal void Add_Controls(List<CONTROL_INFO> ctrlInfo)
        {
            CtrlAllClear();

            this.SuspendLayout();
            for (int i = 0; i < ctrlInfo.Count; i++)
            {
                if (CreateControls(ctrlInfo[i]))
                {
                    SetProperty(ctrlInfo[i]);
                    SetEvents(ctrlInfo[i]);
                }
            }
            this.ResumeLayout(false);

            SelectAllClear();
        }

        // ********************************************************************************************
        // Private Function
        // ********************************************************************************************
        private void SetEvents(CONTROL_INFO ctrlInfo)
        {
            Component? ctrl;
            int index = Find_Control_Index(ctrlInfo.ctrlName);

            if (index != -1)
            {
                if (CtrlItems[index].nonCtrl!.GetType() == typeof(Component)) { ctrl = CtrlItems[index].ctrl; }
                else { ctrl = CtrlItems[index].nonCtrl; }
            }
            else { ctrl = this; }

            for (int j = 0; j < ctrlInfo.eventName.Count; j++)
            {
                string funcParam = "";
                string param = "";
                Type? delegateType = ctrl!.GetType().GetEvent(ctrlInfo.eventName[j])!.EventHandlerType;
                MethodInfo? invoke = delegateType!.GetMethod("Invoke");
                ParameterInfo[] pars = invoke!.GetParameters();
                string[] split = delegateType.AssemblyQualifiedName!.Split(",");
                string newHandler = "new " + split[0];
                SetArguments(ref funcParam, ref param, pars);
                string decHandler;
                string decFunc = "private void " + ctrlInfo.eventFunc[j] + "(" + funcParam + ")";
                if (ctrl != this)
                {
                    decHandler = GetDecHandler(ctrlInfo.eventName[j], newHandler, ctrlInfo.eventFunc[j], CtrlItems[index].ctrl!.Name);
                    CtrlItems[index].decHandler.Add(decHandler);
                    CtrlItems[index].decFunc.Add(decFunc);
                }
                else
                {
                    decHandler = GetDecHandler(ctrlInfo.eventName[j], newHandler, ctrlInfo.eventFunc[j], "");
                    this.decHandler.Add(decHandler);
                    this.decFunc.Add(decFunc);
                }
            }
        }

        private string GetDecHandler(string? eventName, string newHandler, string funcName, string ctrlName)
        {
            string decHandler;
            if (ctrlName != "") { decHandler = "this." + ctrlName + "." + eventName + " += " + newHandler + "(" + funcName + ");"; }
            else { decHandler = "this." + eventName + " += " + newHandler + "(" + funcName + ");"; }
            return decHandler;
        }

        private void SetArguments(ref string funcParam, ref string param, ParameterInfo[] pars)
        {
            foreach (ParameterInfo p in pars)
            {
                param = p.ParameterType.ToString();
                if (param == "System.Object") { param += "? sender"; }
                else { param += " e"; }
                if (funcParam == "") { funcParam = param; }
                else { funcParam += ", " + param; }
            }
        }

        private void SetProperty(CONTROL_INFO ctrlInfo)
        {
            int index = Find_Control_Index(ctrlInfo.ctrlName);
            if (index != -1)
            {
                for (int j = 0; j < ctrlInfo.propertyName.Count; j++)
                {
                    cls_controls.SetCtrlProperty(CtrlItems[index], ctrlInfo.propertyName[j], ctrlInfo.strProperty[j]);
                }
            }
            else
            {
                for (int j = 0; j < ctrlInfo.propertyName.Count; j++)
                {
                    cls_controls.SetFormProperty(this, ctrlInfo.propertyName[j], ctrlInfo.strProperty[j]);
                }
            }
        }

        private bool CreateControls(CONTROL_INFO ctrlInfo)
        {
            Control parent = new();
            if (ctrlInfo.parent != "")
            {
                int index = Find_Control_Index(ctrlInfo.parent);
                if (index != -1)
                {
                    if (ctrlInfo.panelNum == 0) { parent = CtrlItems[index].ctrl!; }
                    else
                    {
                        SplitContainer? splConta = CtrlItems[index].ctrl as SplitContainer;
                        if (ctrlInfo.panelNum == 1) { parent = splConta!.Panel1; }
                        else { parent = splConta!.Panel2; }
                    }
                }
                else { parent = this; }
            }

            if (ctrlInfo.ctrlClassName != "Form")
            {
                int mem_cnt = cnt_Control;
                _ = new cls_controls(this, otherCtrlPanel, ctrlInfo.ctrlClassName!, parent, 0, 0, true);
                if (mem_cnt != cnt_Control)
                {
                    CtrlItems[cnt_Control - 1].ctrl!.Name = ctrlInfo.ctrlName;
                    CtrlItems[cnt_Control - 1].ctrl!.Text = ctrlInfo.ctrlName;
                    return true;
                }
            }
            else if (ctrlInfo.ctrlClassName == "Form") { return true; }
            return false;
        }

        private int Find_Control_Index(string? parent)
        {
            for (int i = 0; i < CtrlItems.Count; i++)
            {
                if (parent == CtrlItems[i].ctrl!.Name) { return i; }
            }
            return -1;
        }

        private void ShowProperty(bool flag)
        {
            if (flag)
            {
                mainForm!.propertyGrid!.SelectedObject = this;
                if (this.GetType() == typeof(cls_userform)) { mainForm.propertyCtrlName!.Text = this.viewName; }
                else { mainForm.propertyCtrlName!.Text = this.Name; }
            }
            else
            {
                mainForm!.propertyGrid!.SelectedObject = null;
                mainForm.propertyCtrlName!.Text = "";
            }
        }

        private void Delete(cls_controls ctrl)
        {
            for (int i = 0; i < CtrlItems.Count; i++)
            {
                if (ctrl.ctrl == CtrlItems[i].ctrl!.Parent) { Delete(CtrlItems[i]); i--; }
                if (ctrl.ctrl is SplitContainer)
                {
                    SplitContainer? splcontainer = ctrl.ctrl as SplitContainer;
                    for (int j = 0; j < CtrlItems.Count; j++)
                    {
                        if (splcontainer!.Panel1 == CtrlItems[j].ctrl!.Parent || splcontainer!.Panel2 == CtrlItems[j].ctrl!.Parent)
                        {
                            Delete(CtrlItems[j]);
                            i--;
                        }
                    }
                }
            }
            ctrl.Delete();
            CtrlItems.Remove(ctrl);
        }
    }
}
