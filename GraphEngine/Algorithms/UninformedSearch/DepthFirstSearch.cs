using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Nodes;

namespace GraphEngine.Algorithms.UninformedSearch
{
    public class DepthFirstSearch : AlgorithmBase
    {
        private HashSet<Node> _visited = new HashSet<Node>();
        private Stack<Node> _nodes = new Stack<Node>();
        private LinkedList<Node> _result = new LinkedList<Node>();

        public async Task<LinkedList<Node>> Start(Node startNode)
        {
            HighlightNode(startNode);
            _visited.Add(startNode);

            _result.AddFirst(startNode);
            foreach (var next in startNode.Next)
                await Step(next.Key, next.Value);

            return _result;
        }

        private async Task Step(Node node, Edge edge)
        {
            HighlightNode(node);
            HighlightEdge(edge);
            _visited.Add(node);
            _result.AddLast(node);

            await Task.Delay(_delay);

            foreach (var next in node.Next)
                if (!_visited.Contains(next.Key))
                    await Step(next.Key, next.Value);
        }
    }
}
