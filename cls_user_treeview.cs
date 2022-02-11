namespace SWD4CS
{
    public partial class cls_user_treeview : TreeView
    {

        public cls_user_treeview()
        {
            InitializeComponent();
        }

        internal void ControlViewShow(cls_user_form form)
        {
            this.Nodes.Clear();
            TreeNode NodeRoot = new("Form");
            cls_treenode[] itemNode = Array.Empty<cls_treenode>();

            for (int i = 0; i < form.CtrlItems.Count; i++)
            {
                if (form.CtrlItems[i].ctrl!.Parent == form)
                {
                    Array.Resize(ref itemNode, itemNode.Count() + 1);
                    if (form.CtrlItems[i].className == "SplitContainer")
                    {
                        itemNode[itemNode.Count() - 1] = new cls_treenode(form.CtrlItems[i].ctrl!.Name + ".Panel1");
                        Array.Resize(ref itemNode, itemNode.Count() + 1);
                        itemNode[itemNode.Count() - 1] = new cls_treenode(form.CtrlItems[i].ctrl!.Name + ".Panel2");
                    }
                    else
                    {
                        itemNode[itemNode.Count() - 1] = new cls_treenode(form.CtrlItems[i].ctrl!.Name);
                    }
                }
                else
                {
                    for (int j = 0; j < itemNode.Count(); j++)
                    {
                        cls_treenode? retNode = itemNode[j].Search(form.CtrlItems[i].ctrl!.Parent.Name);
                        if (retNode != null)
                        {
                            retNode.Add(form.CtrlItems[i].ctrl!.Name, form.CtrlItems[i].className);
                            break;
                        }
                    }
                }
            }

            if (itemNode.Count() > 0)
            {
                NodeRoot.Nodes.AddRange(itemNode);
            }

            this.Nodes.Add(NodeRoot);
            this.TopNode.Expand();
        }
    }
}
