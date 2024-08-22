using GraphEngine.Graph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEngine.Algorithms.InformedSearch
{
    public class AStar
    {
        private Queue<WebNode> _open = new Queue<WebNode>();
        private HashSet<WebNode> _closed = new HashSet<WebNode>();
        private Dictionary<WebNode, double> _dstFromStart = new Dictionary<WebNode, double>();
        private Dictionary<WebNode, double> _estimatedDstToGoal = new Dictionary<WebNode, double>();
        private Dictionary<WebNode, WebNode> _parents = new Dictionary<WebNode, WebNode>();

        public List<WebNode> Start(WebNode start, WebNode goal)
        {
            List<WebNode> path = new List<WebNode>();
            _open.Enqueue(start);
            _dstFromStart.Add(start, 0);

            while (_open.Count > 0)
            {
                WebNode cur = _open.Dequeue();

                if (cur == goal)
                {
                    while (_parents.ContainsKey(cur))
                    {
                        path.Add(cur);
                        cur = _parents[cur];
                    }
                    path.Add(cur);
                    return path;
                }

                _closed.Add(cur);

                foreach (WebNode node in cur.Next.Keys)
                {
                    if (_closed.Contains(node)) continue;

                    double newDst = _dstFromStart[cur] + 1;

                    if (_open.Contains(node))
                    {
                        if (newDst < _dstFromStart[node])
                        {
                            _dstFromStart[node] = newDst;
                            _estimatedDstToGoal[node] = Math.Sqrt(Math.Pow(goal.X - node.X, 2) + Math.Pow(goal.Y - node.Y, 2));
                            _parents[node] = cur;
                        }
                    }
                    else
                    {
                        _dstFromStart.Add(node, newDst);
                        _estimatedDstToGoal.Add(node, Math.Sqrt(Math.Pow(goal.X - node.X, 2) + Math.Pow(goal.Y - node.Y, 2)));
                        _parents.Add(node, cur);
                        _open.Enqueue(node);
                    }
                }
            }

            return path;
        }
    }
}
