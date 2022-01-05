﻿
namespace SWD4CS
{
    public partial class cls_label : Label
    {
        internal cls_control_base ctrlBase;
        private cls_form form;
        internal Control parentCtrl;

        public cls_label(cls_form form, Control parent, Control backPanel, ListBox toolList, DataGridView propertyList, int index, int X = 0, int Y = 0)
        {
            InitializeComponent(index, X, Y);

            this.form = form;
            this.parentCtrl = parent;
            this.form.CtrlItems!.Add(this);
            parent.Controls.Add(this.form.CtrlItems[this.form.CtrlItems.Count - 1] as cls_label);
            ctrlBase = new cls_control_base(form, this, parent, backPanel, propertyList);
        }

        public void Deleate(Control parent, cls_label ctrl)
        {
            parent.Controls.Remove(ctrl);
            ctrlBase.SetSelect(false);
            this.Dispose();
        }
    }
}
