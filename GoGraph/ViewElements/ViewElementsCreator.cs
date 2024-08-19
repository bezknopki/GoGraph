using GoGraph.Graph.Edges;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GoGraph.ViewElements
{
    public static class ViewElementsCreator
    {
        public static Polyline CreateArrowEmtyPolyline() 
            => new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 3
            };

        public static Ellipse CreateNodeEllipse(double mLeft, double mTop)
            => new Ellipse
            {
                Width = ViewConstants.NodeDiameter,
                Height = ViewConstants.NodeDiameter,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 3,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(mLeft, mTop, 0, 0)
            };

        public static TextBlock CreateNodeNameBlock(string name, double mLeft, double mTop)
            => new TextBlock
            {
                Text = name,
                Height = ViewConstants.NameBlockSide,
                Width = ViewConstants.NameBlockSide,
                Background = Brushes.Transparent,
                FontSize = 20,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(mLeft, mTop, 0, 0)
            };

        public static Line CreateEdgeLine(Point p1, Point p2)
            => new Line
            {
                X1 = p1.X,
                Y1 = p1.Y,
                X2 = p2.X,
                Y2 = p2.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 3
            };

        public static TextBlock CreateWeightTextBlock(double mLeft, double mTop, string weight)
            => new TextBlock
            {
                Text = weight,
                Height = ViewConstants.WeightBlockSide,
                Width = ViewConstants.WeightBlockSide,
                FontSize = 20,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(mLeft, mTop, 0, 0)
            };


        public static TextBlock CreateWeightTextBlock(Point p1, Point p2, double weight)
        {
            Point center = new Point
            {
                X = Math.Abs((p1.X - p2.X) / 2) + Math.Min(p1.X, p2.X),
                Y = Math.Abs((p1.Y - p2.Y) / 2) + Math.Min(p1.Y, p2.Y)
            };

            TextBlock weightTextBlock = new TextBlock
            {
                Text = weight.ToString(),
                Height = ViewConstants.WeightBlockSide,
                Width = ViewConstants.WeightBlockSide,
                FontSize = 20,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(center.X, center.Y, 0, 0)
            };

            return weightTextBlock;
        }

        public static List<Polyline>? CreateArrows(Direction direction, Point p1, Point p2)
        {
            List<Polyline> arrows = new List<Polyline>();
            Arrow arrow = new Arrow();

            switch (direction)
            {
                case Direction.FirstToSecond:
                    {
                        arrow.SetPoints(p1, p2);
                        arrows.Add(arrow.GetArrowView());
                        break;
                    }
                case Direction.SecondToFirst:
                    {
                        arrow.SetPoints(p2, p1);
                        arrows.Add(arrow.GetArrowView());
                        break;
                    }
                case Direction.Both:
                    {
                        arrow.SetPoints(p1, p2);
                        arrows.Add(arrow.GetArrowView());
                        arrow.SetPoints(p2, p1);
                        arrows.Add(arrow.GetArrowView());
                        break;
                    }
                default: return null;
            }

            return arrows;
        }
    }
}