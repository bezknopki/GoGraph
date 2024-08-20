using GoGraph.Graph.Nodes;
using GoGraph.ViewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGraph.Algorithms.UninformedSearch
{
    public class DepthFirstSearch
    {
        const int _delay = 250;

        private HashSet<Node> _visited = new HashSet<Node>();
        private Stack<Node> _nodes = new Stack<Node>();

        public async Task<string> Start(Node startNode)
        {
            StringBuilder result = new StringBuilder();
            _nodes.Push(startNode);

            while (_nodes.Count > 0)
            {
                Node cur = _nodes.Pop();
                AnimationHelper.HighlightNode(cur);

                result.Append($"{cur.Name}, ");

                foreach (var nextNode in cur.Next)
                    if (!_visited.Contains(nextNode))
                    {
                        AnimationHelper.HighlightEdgeBetweenNodes(cur, nextNode);
                        _nodes.Push(nextNode);
                        await Task.Delay(_delay);
                    }

                _visited.Add(cur);
            }

            return result.ToString();
        }
    }
}
