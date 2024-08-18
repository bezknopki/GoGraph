using GoGraph.ViewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGraph.Graph.Nodes
{
    public class Node
    {
        public NodeView View { get; set; }
        public string Name { get; set; }
        public List<Node> Next { get; set; } = new List<Node>();
        public bool IsSelected { get; set; }
    }
}
