using GraphEngine.Graph.Nodes;

namespace GraphEngine.Algorithms.ShortestWay
{
    public abstract class ShortestWay : AlgorithmBase
    {
        public event Action<Node, double>? ShowMarkEvent;

        protected void ShowMark(Node node, double mark) => ShowMarkEvent?.Invoke(node, mark);
    }
}
