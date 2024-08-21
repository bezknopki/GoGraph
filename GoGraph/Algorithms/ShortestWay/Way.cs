using GoGraph.Graph.Edges;
using GoGraph.Graph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGraph.Algorithms.ShortestWay
{
    public class Way
    {
        public Way(Node from, Node to)
        {
            From = from;
            To = to;
        }
        public Node From { get; set; }
        public Node To { get; set; }
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public override string ToString()
        {
            Node f = From;

            StringBuilder sb = new StringBuilder();
            foreach (Edge e in Edges)
            {
                sb.Append($"{f.Name} -> ");
                f = f == e.First ? e.Second : e.First;               
            }
            sb.Append(To.Name);
            sb.Append($" Length: {Edges.Sum(x => x.Weight)}");
            return sb.ToString();
        }
    }
}
