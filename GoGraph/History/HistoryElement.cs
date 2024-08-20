using GoGraph.Graph.Edges;
using GoGraph.Graph.Nodes;
using GoGraph.ViewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoGraph.History
{
    public class HistoryElement
    {
        public ActionType ActionType { get; set; }
        public List<UIElement> Elements { get; set; } = new List<UIElement>();

        public List<Edge> Edges { get; set; } = new List<Edge> { };

        public List<Node> Nodes { get; set; } = new List<Node> { };
        public List<EdgeView> EdgeViews { get; set; } = new List<EdgeView> { };
        public List<NodeView> NodeViews { get; set; } = new List<NodeView> { };
    }
}
