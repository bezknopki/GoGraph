using GoGraph.ViewElements;

namespace GoGraph.Serializer
{
    [Serializable]
    public class SNodeView
    {
        public bool IsSelected { get; set; }
        public double NodeMarginTop { get; set; }
        public double NodeMarginLeft { get; set; }
        public double NameMarginTop { get; set; }
        public double NameMarginLeft { get; set; }
        public string Name { get; set; }

        public SNodeView() { }

        public SNodeView(NodeView nodeView)
        {
            IsSelected = nodeView.IsSelected;
            NodeMarginLeft = nodeView.Node.Margin.Left;
            NodeMarginTop = nodeView.Node.Margin.Top;
            NameMarginLeft = nodeView.Name.Margin.Left;
            NameMarginTop = nodeView.Name.Margin.Top;
            Name = nodeView.Name.Text;
        }

        public NodeView ToNodeView()
        => new NodeView
        {
            IsSelected = IsSelected,
            Node = ViewElementsCreator.CreateNodeEllipse(NodeMarginLeft, NodeMarginTop),
            Name = ViewElementsCreator.CreateNodeNameBlock(Name, NameMarginLeft, NameMarginTop)
        };
    }
}
