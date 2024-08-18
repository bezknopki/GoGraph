using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoGraph.Graph.Edges;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace GoGraph.ViewElements
{
    public class EdgeViewBuilder
    {
        const int weightBlockSide = 30;
        const int radius = 30;

        private Point _p1;
        private Point _p2;
        private bool _isWeightened;
        private double _weight;
        private Direction _direction;

        public EdgeViewBuilder SetPoints(Point first, Point second)
        {
            _p1 = first;
            _p2 = second;
            return this;
        }

        public EdgeViewBuilder SetWeight(double weight)
        {
            _isWeightened = true;
            _weight = weight;
            return this;
        }

        public EdgeViewBuilder SetDirection(Direction direction)
        {
            _direction = direction;
            return this;
        }

        public EdgeView Build()
            => new EdgeView
            {
                Edge = CreateEdgeLine(),
                Weight = CreateWeightTextBlock(),
                Arrows = CreateArrows()
            };

        private Line CreateEdgeLine()
            => new Line
            {
                X1 = _p1.X,
                Y1 = _p1.Y,
                X2 = _p2.X,
                Y2 = _p2.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 3
            };

        private TextBlock CreateWeightTextBlock()
        {
            if (!_isWeightened) return null;

            Point center = new Point
            {
                X = Math.Abs((_p1.X - _p2.X) / 2) + Math.Min(_p1.X, _p2.X),
                Y = Math.Abs((_p1.Y - _p2.Y) / 2) + Math.Min(_p1.Y, _p2.Y)
            };

            TextBlock weightTextBlock = new TextBlock
            {
                Text = _weight.ToString(),
                Height = weightBlockSide,
                Width = weightBlockSide,
                FontSize = 20,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(center.X, center.Y, 0, 0)
            };

            return weightTextBlock;
        }

        private List<Polyline>? CreateArrows()
        {
            List<Polyline> arrows = new List<Polyline>();
            ArrowBuilder arrowBuilder = new ArrowBuilder();

            switch (_direction)
            {
                case Direction.FirstToSecond:
                    {                       
                        arrows.Add(arrowBuilder.SetPoints(_p1, _p2).Build());
                        break;
                    }
                case Direction.SecondToFirst:
                    {
                        arrows.Add(arrowBuilder.SetPoints(_p2, _p1).Build());
                        break;
                    }
                case Direction.Both:
                    {
                        arrows.Add(arrowBuilder.SetPoints(_p1, _p2).Build());
                        arrows.Add(arrowBuilder.SetPoints(_p2, _p1).Build());
                        break;
                    }
                default: return null;
            }

            return arrows;
        }       
    }
}
