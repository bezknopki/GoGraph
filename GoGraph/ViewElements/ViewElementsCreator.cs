using GoGraph.Graph.Edges;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
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

        public static Border CreateMark(double mLeft, double mTop, string mark)
        {
            Border borderWithText = CreateBorderWithWeight(mark, Brushes.AliceBlue);

            borderWithText.Margin = new Thickness(mLeft, mTop, 0, 0);

            return borderWithText;
        }

        public static Border CreateWeightTextBlock(double mLeft, double mTop, string weight)
        {
            Border borderWithText = CreateBorderWithWeight(weight, Brushes.LightCoral);
            
            borderWithText.Margin = new Thickness(mLeft, mTop, 0, 0);

            return borderWithText;
        }

        public static Border CreateWeightTextBlock(Point p1, Point p2, double weight)
        {
            Point center = new Point
            {
                X = Math.Abs((p1.X - p2.X) / 2) + Math.Min(p1.X, p2.X),
                Y = Math.Abs((p1.Y - p2.Y) / 2) + Math.Min(p1.Y, p2.Y)
            };

            Border borderWithText = CreateBorderWithWeight(weight.ToString(), Brushes.LightCoral);
            TextBlock weightTextBlock = (TextBlock)borderWithText.Child;         

            borderWithText.Margin = new Thickness(center.X - weightTextBlock.ActualWidth / 2, center.Y - ViewConstants.WeightBlockSide / 2, 0, 0);

            return borderWithText;
        }

        public static List<Polyline> CreateArrows(Direction direction, Point p1, Point p2)
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
                default: return arrows;
            }

            return arrows;
        }

        private static Border CreateBorderWithWeight(string weight, SolidColorBrush bg)
        {
            TextBlock weightTextBlock = new TextBlock
            {
                Text = weight,
                FontSize = 20,
                Visibility = Visibility.Visible,
                Background = bg,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Border borderWithText = new Border
            {
                Child = weightTextBlock,
                BorderBrush = Brushes.Black,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = ViewConstants.WeightBlockSide,
                BorderThickness = new Thickness(1)
            };

            weightTextBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            weightTextBlock.Arrange(new Rect(weightTextBlock.DesiredSize));

            return borderWithText;
        }
    }
}