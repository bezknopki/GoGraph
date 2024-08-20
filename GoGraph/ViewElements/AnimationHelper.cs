using GoGraph.Graph.Edges;
using GoGraph.Graph.Nodes;
using GoGraph.Model;
using GoGraph.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GoGraph.ViewElements
{
    public static class AnimationHelper
    {
        private static SolidColorBrush _highlightedBrush = Brushes.DeepPink;

        public static GraphModel Model { get; set; }

        public static void HighlightEdgeBetweenNodes(Node from, Node to)
        {
            Edge edge = GetEdgeBetweenNodes(from, to);
            EdgeView edgeView = Model.EdgesToViews[edge];
            edgeView.Edge.Stroke = _highlightedBrush;
            foreach (var arrow in edgeView.Arrows)
                arrow.Stroke = _highlightedBrush;
        }

        public static void HighlightNode(Node node)
        {
            NodeView nodeView = GetNodeViewByNode(node);
            nodeView.Node.Stroke = _highlightedBrush;
        }

        private static NodeView GetNodeViewByNode(Node node) => Model.NodeViews.First(x => x.Name.Text == node.Name);
        private static Edge GetEdgeBetweenNodes(Node from, Node to) => Model.Graph.Edges.First(x => x.First == from && x.Second == to 
        || ((x.Direction == Direction.SecondToFirst || x.Direction == Direction.Both) && x.First == to && x.Second == from));
    }
}
