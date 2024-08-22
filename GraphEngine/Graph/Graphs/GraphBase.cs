using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Nodes;

namespace GraphEngine.Graph.Graphs
{
    [Serializable]
    public abstract class GraphBase
    {
        public readonly bool IsWeightened;
        public readonly bool IsDirected;

        protected GraphBase(bool isWeightened, bool isDirected) 
        {
            IsWeightened = isWeightened;
            IsDirected = isDirected;
        }

        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
    }
}
