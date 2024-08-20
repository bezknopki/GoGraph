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
        public List<SNodeView> NodeViews { get; set; } = new List<SNodeView>();
        public List<SEdgeView> EdgeViews { get; set; } = new List<SEdgeView>();
        public List<SItem<SEdge, SEdgeView>> EdgesToViews { get; set; } = new List<SItem<SEdge, SEdgeView>>();
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
            EdgeViews = model.EdgeViews.Select(x => new SEdgeView(x)).ToList();
            NodeViews = model.NodeViews.Select(x => new SNodeView(x)).ToList();

            IsDirected = model.Graph.IsDirected;
            IsWeightened = model.Graph.IsWeightened;

            Edges = model.Graph.Edges.Select(x => new SEdge(x)).ToList();
            Nodes = model.Graph.Nodes.Select(x => new SNode(x)).ToList();

            EdgesToViews = model.EdgesToViews.Select(x => new SItem<SEdge, SEdgeView>(new SEdge(x.Key), new SEdgeView(x.Value))).ToList();

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

            List<NodeView> nodeViews = NodeViews.Select(x => x.ToNodeView()).ToList();

            List<EdgeView> edgeViews = EdgeViews.Select(x => x.ToEdgeView()).ToList();            

            GraphBase graph = new SimpleGraphCreator().Create(GraphType);
            graph.Nodes = nodes;
            graph.Edges = Edges.Select(x => x.ToEdge(
                    nodes.First(v => v.Name == x.First.Name),
                    nodes.First(v => v.Name == x.Second.Name)))
                .ToList();

            Dictionary<Edge, EdgeView> edgesToViews = new Dictionary<Edge, EdgeView>();

            foreach (var etv in EdgesToViews)
            {
                Edge edge;
                EdgeView edgeView;

                edge = graph.Edges.First(x => x.First.Name == etv.Item1.First.Name && x.Second.Name == etv.Item1.Second.Name);
                edgeView = edgeViews.First(x => x.Edge.X1 == etv.Item2.P1.X 
                && x.Edge.Y1 == etv.Item2.P1.Y
                && x.Edge.X2 == etv.Item2.P2.X
                && x.Edge.Y2 == etv.Item2.P2.Y);

                edgesToViews.Add(edge, edgeView);
            }

            return new GraphModel
            {
                EdgeViews = edgeViews,
                NodeViews = nodeViews,
                Graph = graph,
                EdgesToViews = edgesToViews
            };
        }
    }
}
