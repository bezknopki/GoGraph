using GraphEngine.Graph.Nodes;

namespace GraphEngine.Algorithms.UninformedSearch
{
    public class BreadthFirstSearch : AlgorithmBase
    {
        private Queue<Node> _nodes = new Queue<Node>();
        private HashSet<Node> _visited = new HashSet<Node>();

        public async Task<LinkedList<Node>> Start(Node startNode)
        {
            LinkedList<Node> result = new LinkedList<Node>();
            _nodes.Enqueue(startNode);

            while (_nodes.Count > 0)
            {
                Node cur = _nodes.Dequeue();
                _visited.Add(cur);

                HighlightNode(cur);
                result.AddLast(cur);

                foreach (var nextNode in cur.Next.Keys)
                    if (!_visited.Contains(nextNode))
                    {
                        HighlightEdge(cur.Next[nextNode]);
                        _nodes.Enqueue(nextNode);
                        await Task.Delay(_delay);
                    }
            }

            return result;
        }
    }
}
