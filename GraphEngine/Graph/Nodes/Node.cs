using GraphEngine.Graph.Edges;

namespace GraphEngine.Graph.Nodes
{
    [Serializable]
    public class Node
    {
        public string Name { get; set; }
        public Dictionary<Node, Edge> Next { get; set; } = new Dictionary<Node, Edge>();
    }
}
