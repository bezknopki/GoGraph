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
    [Serializable]
    public class Edge
    {
        public Node? First { get; set; }
        public Node? Second { get; set; }
        public bool IsWeightened { get; set; }
        public bool IsDirected { get; set; }
        public Direction Direction { get; set; }
        public double Weight { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Edge edge)
                return First == edge.First
                    && Second == edge.Second
                    && IsWeightened == edge.IsWeightened
                    && Direction == edge.Direction
                    && Weight == edge.Weight;

            return false;
        }
    }
}
