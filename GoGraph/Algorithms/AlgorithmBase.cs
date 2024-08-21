using GoGraph.Graph.Edges;
using GoGraph.Graph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGraph.Algorithms
{
    public abstract class AlgorithmBase
    {
        protected const int _delay = 250;
        public event Action<Node> HighlightNodeEvent;
        public event Action<Edge> HighlightEdgeEvent;

        protected void HighlightNode(Node node) => HighlightNodeEvent.Invoke(node);
        protected void HighlightEdge(Edge edge) => HighlightEdgeEvent.Invoke(edge);
    }
}
