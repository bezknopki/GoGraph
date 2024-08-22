using GraphEngine.Algorithms.ShortestWay;
using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Nodes;
using GoGraph.Model;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GoGraph.ViewElements
{
    public static class AnimationHelper
    {
        private static SolidColorBrush _highlightedBrush = Brushes.DeepPink;
        private static SolidColorBrush _defaultBrush = Brushes.Black;

        private static Dictionary<Node, Border> _markViews = new Dictionary<Node, Border>();

        public static Grid Grid { get; set; }

        public static GraphModel Model { get; set; }

        public static void HighlightEdge(Edge edge)
        {
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

        public static void ShowMark(Node node, double mark)
        {
            NodeView view = GetNodeViewByNode(node);
            Border tb = _markViews.ContainsKey(node)
                ? _markViews[node]
                : ViewElementsCreator.CreateMark(view.Node.Margin.Left, view.Node.Margin.Top - 30, mark.ToString());

            if (_markViews.ContainsKey(node))
                ((TextBlock)tb.Child).Text = mark.ToString();
            else
            {
                Grid.Children.Add(tb);
                _markViews[node] = tb;
            }
        }

        public static async Task WalkThrough(Way way)
        {
            Reset();
            Node from = way.From;
            Ellipse walker = new Ellipse
            {
                Stroke = _highlightedBrush,
                Fill = _highlightedBrush,
                Height = 10,
                Width = 10,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Top
            };

            Grid.Children.Add(walker);

            foreach (var edge in way.Edges)
            {
                HighlightEdge(edge);
            }

            foreach (var edge in way.Edges)
            {
                await WalkByLine(from, edge, walker);
                from = edge.First == from ? edge.Second : edge.First;
            }

            Grid.Children.Remove(walker);
            Reset();
        }

        private static Func<double, double, bool> GetFinishCondition(double xFrom, double xTo)
        {
            if (xFrom > xTo)
                return (xf, xt) => xf > xt;
            else
                return (xf, xt) => xf < xt;
        }

        private static async Task WalkByLine(Node from, Edge edge, Ellipse walker)
        {
            Line line = Model.EdgesToViews[edge].Edge;

            double xFrom = edge.First == from ? line.X1 : line.X2;
            double xTo = edge.First == from ? line.X2 : line.X1;
            double yFrom = edge.First == from ? line.Y1 : line.Y2;
            double yTo = edge.First == from ? line.Y2 : line.Y1;

            double mLeft = xFrom;
            double mTop = yFrom;

            double k = (yTo - yFrom) / (xTo - xFrom);

            double walkX(double x) => xFrom > xTo ? --x : ++x;
            double offset = walker.Width / 2;

            Func<double, double, bool> isFinished = GetFinishCondition(mLeft, xTo);

            while (isFinished(mLeft, xTo))
            {
                walker.Margin = new System.Windows.Thickness(mLeft - offset, mTop - offset, 0, 0);

                mLeft = walkX(mLeft);
                mTop = yFrom + (mLeft - xFrom) * k;

                await Task.Delay(10);
            }
            await Task.Delay(50);
        }

        public static void Reset()
        {
            foreach (var tb in _markViews.Values)
                Grid.Children.Remove(tb);

            foreach (var ev in Model.EdgeViews)
            {
                ev.Edge.Stroke = _defaultBrush;
                foreach (var ar in ev.Arrows)
                    ar.Stroke = _defaultBrush;
            }

            foreach (var node in Model.NodeViews)
                node.Node.Stroke = _defaultBrush;

            _markViews.Clear();
        }

        private static NodeView GetNodeViewByNode(Node node) => Model.NodeViews.First(x => x.Name.Text == node.Name);
    }
}
