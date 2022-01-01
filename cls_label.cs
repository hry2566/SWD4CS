
namespace SWD4CS
{
    public partial class cls_label : Label
    {
        public cls_control_base ctrlBase;

        public cls_label(Control parent, Control backPanel, int index, int X = 0, int Y = 0)
        {
            InitializeComponent(index, X, Y);

            cls_form ctrl = (cls_form)parent;
            ctrl.CtrlItems.Add(this);
            ctrl.Controls.Add((cls_label)ctrl.CtrlItems[ctrl.CtrlItems.Count - 1]);
            ctrlBase = new cls_control_base(parent, this, backPanel);
        }

        public void Deleate(Control parent, cls_label ctrl)
        {
            parent.Controls.Remove(ctrl);
            ctrlBase.SetSelect(false);
            this.Dispose();
        }

    }
}
