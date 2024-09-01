using GraphEngine.Graph.Nodes;

namespace GoGraph.Serializer
{
    [Serializable]
    public class SNode
    {
        public int Id { get; set; }
        public HashSet<int> Next { get; set; } = new HashSet<int>();

        public SNode() { }

        public SNode(Node node)
        {
            Id = node.Id;
            foreach (var next in node.Next.Keys)
                Next.Add(next.Id);
        }       
    }
}
