using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Nodes;

namespace GoGraph.Serializer
{
    [Serializable]
    public class SEdge
    {
        public SNode First { get; set; }
        public SNode Second { get; set; }
        public bool IsWeightened { get; set; }
        public bool IsDirected { get; set; }
        public Direction Direction { get; set; }
        public double Weight { get; set; }

        public SEdge() { }

        public SEdge(Edge edge)
        {
            Direction = edge.Direction;
            First = new SNode(edge.First);
            Second = new SNode(edge.Second);
            Weight = edge.Weight;
            IsDirected = edge.IsDirected;
            IsWeightened = edge.IsWeightened;
        }

        public Edge ToEdge(Node first, Node second)
            => new Edge
            {
                First = first,
                Second = second,
                Direction = Direction,
                Weight = Weight,
                IsDirected = IsDirected,
                IsWeightened = IsWeightened
            };
    }
}
