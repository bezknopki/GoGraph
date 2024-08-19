using GoGraph.Graph.Edges;
using GoGraph.Graph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGraph.Graph.Graphs
{
    [Serializable]
    public abstract class GraphBase
    {
        public readonly bool IsWeightened;
        public readonly bool IsDirected;

        protected GraphBase(bool isWeightened, bool isDirected) 
        {
            IsWeightened = isWeightened;
            IsDirected = isDirected;
        }

        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
    }
}
