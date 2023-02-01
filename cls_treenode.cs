
namespace SWD4CS
{
    internal class cls_treenode : TreeNode
    {
        private cls_treenode[] itemNode = Array.Empty<cls_treenode>();

        public cls_treenode(string nodeName)
        {
            this.Text = nodeName;
        }

        internal cls_treenode? Search(string name)
        {
            if (Text == name) { return this; }

            for (int i = 0; i < itemNode.Count(); i++)
            {
                if (itemNode[i].Search(name) != null) { return itemNode[i].Search(name); }
            }
            return null;
        }

        internal void Add(string name, string className)
        {
            Array.Resize(ref itemNode, itemNode.Count() + 1);
            if (className == "SplitContainer")
            {
                itemNode[itemNode.Count() - 1] = new cls_treenode(name + ".Panel1");
                Array.Resize(ref itemNode, itemNode.Count() + 1);
                itemNode[itemNode.Count() - 1] = new cls_treenode(name + ".Panel2");
            }
            else { itemNode[itemNode.Count() - 1] = new cls_treenode(name); }

            this.Nodes.Clear();
            this.Nodes.AddRange(itemNode);
            this.Expand();
        }
    }
}
