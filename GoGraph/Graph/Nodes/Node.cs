using GoGraph.Graph.Edges;
using GoGraph.ViewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoGraph.Graph.Nodes
{
    [Serializable]
    public class Node
    {
        public string Name { get; set; }
        public Dictionary<Node, Edge> Next { get; set; } = new Dictionary<Node, Edge>();
    }
}
