using GraphEngine.Graph.Graphs;
using GraphEngine.Graph.Nodes;
using System.Text;

namespace GraphEngine.Algorithms.ShortestWay
{
    public class Dijkstra
    {
        private Dictionary<Node, double> _marks = new Dictionary<Node, double>();
        private HashSet<Node> _visited = new HashSet<Node>();
        private List<Way> _ways = new List<Way>();

        public async Task<Way> FindShortestWay(GraphBase graph, Node from, Node to)
        {
            if (!graph.IsWeightened) throw new ArgumentException();
            Init(graph, from);

            StringBuilder res = new StringBuilder();
            while (_visited.Count < graph.Nodes.Count)
                await Step(graph);

            return GetWayTo(to);
        }

        private async Task Step(GraphBase graph)
        {
            Node cur = GetLightestNode();
            Way curWay = GetWayTo(cur);
            //AnimationHelper.HighlightNode(cur);
            
            double curMark = _marks[cur];

            foreach (var node in cur.Next)
            {
                if (_visited.Contains(node.Key)) continue;
                Way nextWay = GetWayTo(node.Key);

                double nMark = node.Value.Weight + curMark;
                //AnimationHelper.HighlightEdge(node.Value);
                await Task.Delay(50);
                if (nMark < _marks[node.Key])
                {
                    _marks[node.Key] = nMark;
                    nextWay.Edges.Clear();
                    nextWay.Edges.AddRange(curWay.Edges);
                    nextWay.Edges.Add(node.Value);
                    //AnimationHelper.ShowMark(node.Key, nMark);
                    await Task.Delay(250);
                }
            }

            _visited.Add(cur);
        }

        private void Init(GraphBase graph, Node from)
        {
            foreach (var node in graph.Nodes)
                _marks.Add(node, double.PositiveInfinity);

            _marks[from] = 0;

            foreach (var node in graph.Nodes)
                _ways.Add(new Way(from, node));

            //AnimationHelper.ShowMark(from, 0);
        }

        private Way GetWayTo(Node node) => _ways.First(x => x.To == node);

        private Node GetLightestNode()
        {
            double minMark = double.PositiveInfinity;
            Node node = null;

            foreach (var kvp in _marks)
            {
                if (_visited.Contains(kvp.Key)) continue;

                if (kvp.Value < minMark)
                {
                    minMark = kvp.Value;
                    node = kvp.Key;
                }
            }

            return node;
        }
    }
}
