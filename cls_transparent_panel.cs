namespace SWD4CS
{
    public class cls_transparent_panel : Panel
    {
        public cls_transparent_panel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var result = base.CreateParams;
                result.ExStyle |= 0x20;
                return result;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (var brush = new SolidBrush(BackColor)) { e.Graphics.FillRectangle(brush, e.ClipRectangle); }
        }
    }
}