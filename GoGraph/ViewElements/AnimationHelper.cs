using GraphEngine.GraphMath.ShortestWay;
using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Nodes;
using GoGraph.Model;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
//using System.Drawing;
using GoGraph.Serializer;
using System.Windows;

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
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
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

        public static async Task Walk(Point start, Point end, Ellipse walker)
        {
            double mLeft = start.X;
            double mTop = start.Y;

            double k = (end.Y - start.Y) / (end.X - start.X);

            double hypotenuse = Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));

            double offset = walker.Width / 2;

            Func<double, double, bool> isFinished = GetFinishCondition(mLeft, end.X);

            for (int i = 0; i < hypotenuse; i++)
            {
                walker.Margin = new Thickness(mLeft - offset, mTop - offset, 0, 0);

                if (Math.Round(end.X) != Math.Round(start.X))
                {
                    double xOffset = Math.Sqrt(Math.Pow(i, 2) / (Math.Pow(k, 2) + 1));
                    mLeft = start.X + (start.X > end.X ? -xOffset : xOffset);
                    double yOffset = k * xOffset;
                    mTop = start.Y + (start.X > end.X ? -yOffset : yOffset);
                }
                else if (Math.Round(end.Y) != Math.Round(start.Y))
                {
                    mTop += start.Y > end.Y ? -1 : 1;
                }

                await Task.Delay(10);
            }
        }

        private static async Task WalkByLine(Node from, Edge edge, Ellipse walker)
        {
            Line line = Model.EdgesToViews[edge].Edge;

            double xFrom = edge.First == from ? line.X1 : line.X2;
            double xTo = edge.First == from ? line.X2 : line.X1;
            double yFrom = edge.First == from ? line.Y1 : line.Y2;
            double yTo = edge.First == from ? line.Y2 : line.Y1;

            Point start = new Point(xFrom, yFrom);
            Point end = new Point(xTo, yTo);

            await Walk(start, end, walker);
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

        private static NodeView GetNodeViewByNode(Node node) => Model.NodeViews.First(x => x.Name.Text == node.Id.ToString());
    }
}
