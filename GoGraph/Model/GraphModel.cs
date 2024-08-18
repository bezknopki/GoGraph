using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoGraph.Graph.Nodes;
using GoGraph.Graph.Edges;

namespace GoGraph.Model
{
    public class GraphModel
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
    }
}
