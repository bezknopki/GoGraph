using GoGraph.Graph.Nodes;

namespace GoGraph.Serializer
{
    [Serializable]
    public class SNode
    {
        public string Name { get; set; }
        public HashSet<string> Next { get; set; } = new HashSet<string>();

        public SNode() { }

        public SNode(Node node)
        {
            Name = node.Name;
            foreach (var next in node.Next)
                Next.Add(next.Name);
        }       
    }
}
