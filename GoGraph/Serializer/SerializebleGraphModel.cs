using GoGraph.Graph.Edges;
using GoGraph.Graph.Graphs;
using GoGraph.Graph.Graphs.GraphCreator;
using GoGraph.Graph.Nodes;
using GoGraph.Model;
using GoGraph.ViewElements;

namespace GoGraph.Serializer
{
    [Serializable]
    public class SerializebleGraphModel
    {
        public List<SItem<SNodeView, SNode>> NodeViews { get; set; } = new List<SItem<SNodeView, SNode>>();
        public List<SItem<SEdge, SEdgeView>> EdgeViews { get; set; } = new List<SItem<SEdge, SEdgeView>>();
        public bool IsWeightened { get; set; }
        public bool IsDirected { get; set; }
        public List<SNode> Nodes { get; set; } = new List<SNode>();
        public List<SEdge> Edges { get; set; } = new List<SEdge>();
        public GraphTypes GraphType { get; set; }

        public SerializebleGraphModel()
        {
            
        }

        public SerializebleGraphModel(GraphModel model)
        {
            foreach (var kvp in model.EdgeViews)         
                EdgeViews.Add(new SItem<SEdge, SEdgeView>(new SEdge(kvp.Key), new SEdgeView(kvp.Value)));           

            foreach (var kvp in model.NodeViews)
                NodeViews.Add(new SItem<SNodeView, SNode>(new SNodeView(kvp.Key), new SNode(kvp.Value)));

            IsDirected = model.Graph.IsDirected;
            IsWeightened = model.Graph.IsWeightened;

            Edges = model.Graph.Edges.Select(x => new SEdge(x)).ToList();
            Nodes = model.Graph.Nodes.Select(x => new SNode(x)).ToList();

            if (IsDirected && IsWeightened) GraphType = GraphTypes.DirectedWeightened;
            else if (!IsDirected && IsWeightened) GraphType = GraphTypes.Weightened;
            else if (IsDirected && !IsWeightened) GraphType = GraphTypes.Directed;
            else GraphType = GraphTypes.Simple;
        }

        public GraphModel ToGraphModel()
        {           
            List<Node> nodes = new List<Node>();

            foreach (var sNode in Nodes)
            {
                Node node = new Node();
                node.Name = sNode.Name;
                nodes.Add(node);
            }

            foreach (var node in nodes)
            {
                SNode sNode = Nodes.First(x => x.Name == node.Name);
                node.Next = nodes.Where(x => sNode.Next.Contains(x.Name)).ToList();
            }

            Dictionary<NodeView, Node> nodeViews = new Dictionary<NodeView, Node>();

            foreach (var si in NodeViews)
                nodeViews.Add(si.Item1.ToNodeView(), nodes.First(x => x.Name == si.Item2.Name));

            Dictionary<Edge, EdgeView> edgeViews = new Dictionary<Edge, EdgeView>();

            foreach (var si in EdgeViews)
                edgeViews.Add(si.Item1.ToEdge(
                    nodes.First(x => x.Name == si.Item1.First.Name),
                    nodes.First(x => x.Name == si.Item1.Second.Name)),
                    si.Item2.ToEdgeView());

            GraphBase graph = new SimpleGraphCreator().Create(GraphType);
            graph.Nodes = nodes;
            graph.Edges = Edges.Select(x => x.ToEdge(
                    nodes.First(v => v.Name == x.First.Name),
                    nodes.First(v => v.Name == x.Second.Name)))
                .ToList();

            return new GraphModel
            {
                EdgeViews = edgeViews,
                NodeViews = nodeViews,
                Graph = graph
            };
        }
    }
}
