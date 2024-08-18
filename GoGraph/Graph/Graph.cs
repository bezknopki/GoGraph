using GoGraph.Graph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGraph.Graph
{
    public abstract class Graph
    {
        public List<Node> Nodes { get; set; } = new List<Node>();

    }
}
