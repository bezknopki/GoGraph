using GoGraph.Graph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGraph.Graph.Edges
{
    public abstract class Edge
    {
        public Node? First { get; set; }
        public Node? Second { get; set; }
        public bool IsWeightened { get; set; }
        public double Weight { get; set; }
    }
}
