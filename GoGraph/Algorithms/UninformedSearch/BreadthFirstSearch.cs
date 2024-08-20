using GoGraph.Graph.Nodes;
using GoGraph.ViewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGraph.Algorithms.UninformedSearch
{
    public class BreadthFirstSearch : AlgorithmBase
    {       
        private Queue<Node> _nodes = new Queue<Node>();

        private HashSet<Node> _visited = new HashSet<Node>();

        public async Task<string> Start(Node startNode)
        {
            StringBuilder result = new StringBuilder();
            _nodes.Enqueue(startNode);

            while (_nodes.Count > 0)
            {
                Node cur = _nodes.Dequeue();
                _visited.Add(cur);

                HighlightNode(cur);
                result.Append($"{cur.Name} -> ");

                foreach (var nextNode in cur.Next)
                    if (!_visited.Contains(nextNode))
                    {
                        HighlightEdge(cur, nextNode);
                        _nodes.Enqueue(nextNode);
                        await Task.Delay(_delay);
                    }              
            }

            return result.ToString();
        }
    }
}
