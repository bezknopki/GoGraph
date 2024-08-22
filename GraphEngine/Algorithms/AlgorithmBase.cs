using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Nodes;

namespace GraphEngine.Algorithms
{
    public abstract class AlgorithmBase
    {
        protected const int _delay = 250;
        public event Action<Node> HighlightNodeEvent;
        public event Action<Edge> HighlightEdgeEvent;

        protected void HighlightNode(Node node) => HighlightNodeEvent?.Invoke(node);
        protected void HighlightEdge(Edge edge) => HighlightEdgeEvent?.Invoke(edge);
    }
}
