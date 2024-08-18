using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using GoGraph.Commands;

namespace GoGraph.ViewElements
{
    public class NodeViewBuilder
    {
        const int nodeDiameter = 50;
        const int nameBlockSide = 30;

        private int _nodeOffset = nodeDiameter / 2;
        private int _nameOffset = nameBlockSide / 2;
        private Point _nodePosition;
        private string _nodeName;

        public NodeViewBuilder SetPosition(Point nodePosition)
        {
            _nodePosition = nodePosition;
            return this;
        }

        public NodeViewBuilder SetName(string nodeName)
        {
            _nodeName = nodeName;
            return this;
        }

        public NodeView Build()
            => new NodeView
            {
                Name = CreateNodeNameBlock(),
                Node = CreateNodeEllipse()
            };


        private Ellipse CreateNodeEllipse()
        {
            Ellipse node = new Ellipse
            {
                Width = nodeDiameter,
                Height = nodeDiameter,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 3,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(_nodePosition.X - _nodeOffset, _nodePosition.Y - _nodeOffset, 0, 0)
            };

            return node;
        }        

        private void Node_MouseEnter(object sender, MouseEventArgs e)
        {
            var node = sender as Ellipse;

            node.Fill = Brushes.AliceBlue;
            node.Stroke = Brushes.Blue;
        }

        private void Node_MouseLeave(object sender, MouseEventArgs e)
        {
            var node = sender as Ellipse;

            node.Fill = Brushes.White;
            node.Stroke = Brushes.Black;
        }

        private TextBlock CreateNodeNameBlock()
            => new TextBlock
            {
                Text = _nodeName,
                Height = nameBlockSide,
                Width = nameBlockSide,
                Background = Brushes.Transparent,
                FontSize = 20,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(_nodePosition.X - _nameOffset, _nodePosition.Y - _nameOffset, 0, 0)
            };
    }
}
