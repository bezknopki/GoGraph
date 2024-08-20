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
                Edge = ViewElementsCreator.CreateEdgeLine(_p1, _p2),
                Weight = CreateWeightTextBlock(),
                Arrows = ViewElementsCreator.CreateArrows(_direction, _p1, _p2)
            };

        private Border CreateWeightTextBlock()
        {
            if (!_isWeightened) return null;

            return ViewElementsCreator.CreateWeightTextBlock(_p1, _p2, _weight);
        }     
    }
}
