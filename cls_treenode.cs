
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

            foreach (var node in itemNode)
            {
                var result = node.Search(name);
                if (result != null) { return result; }
            }
            return null;
        }

        internal void Add(string name, string className)
        {
            Array.Resize(ref itemNode, itemNode.Count() + 1);
            if (className == "SplitContainer")
            {
                itemNode[^1] = new cls_treenode(name + ".Panel1");
                Array.Resize(ref itemNode, itemNode.Count() + 1);
                itemNode[^1] = new cls_treenode(name + ".Panel2");
            }
            else { itemNode[^1] = new cls_treenode(name); }

            this.Nodes.Clear();
            this.Nodes.AddRange(itemNode);
            this.Expand();
        }
    }
}
