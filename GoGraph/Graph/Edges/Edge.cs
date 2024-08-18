using GoGraph.Graph.Nodes;
using GoGraph.ViewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace GoGraph.Graph.Edges
{
    public abstract class Edge
    {
        public EdgeView View { get; set; }
        public Node? First { get; set; }
        public Node? Second { get; set; }
        public bool IsWeightened { get; set; }
        public double Weight { get; set; }
    }
}
