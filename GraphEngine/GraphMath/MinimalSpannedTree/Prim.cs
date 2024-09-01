using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Nodes;

namespace GraphEngine.GraphMath.MinimalSpannedTree
{
    public class Prim : AlgorithmBase
    {
        private List<Edge> _tree = new List<Edge>();
        private HashSet<Node> _nodesInTree = new HashSet<Node>();

        public List<Edge> Start(Node startNode, int nodesCount)
        {
            _nodesInTree.Add(startNode);

            while (_tree.Count < nodesCount - 1)
                Step();

            return _tree;
        }

        private void Step()
        {
            double minWeight = double.PositiveInfinity;
            Edge? minEdge = null;
            Node? nextNode = null;

            foreach (var node in _nodesInTree)
                foreach (var kvp in node.Next)
                    if (!_nodesInTree.Contains(kvp.Key) && kvp.Value.Weight < minWeight)
                    {
                        minWeight = kvp.Value.Weight;
                        minEdge = kvp.Value;
                        nextNode = kvp.Key;
                    }

            if (minEdge != null)
            {
                HighlightNode(nextNode);
                HighlightEdge(minEdge);
                _tree.Add(minEdge);
                _nodesInTree.Add(nextNode);
            }
        }
    }
}
