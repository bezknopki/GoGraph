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
        public List<Node> Next { get; set; } = new List<Node>();
    }
}
