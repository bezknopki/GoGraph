using GoGraph.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GoGraph.Model;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using GoGraph.ViewElements;
using GoGraph.Tools;
using GoGraph.Graph.Nodes;
using GoGraph.Graph.Edges;
using System.Xml.Linq;

namespace GoGraph.ViewModel
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        private RelayCommand _addNodeCommand;
        private GraphModel _model = new GraphModel();

        private Node? firstSelected;
        private Node? secondSelected;

        public RelayCommand AddNodeCommand
        {
            get
            {
                return _addNodeCommand ??= new RelayCommand(obj => AddOrSelectNode(obj), obj => obj is Grid);
            }
            set { _addNodeCommand = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void AddOrSelectNode(object obj)
        {
            Grid grid = (Grid)obj;

            (bool isMouseOverNode, Node? node) = IsMouseOverNode(grid);

            if (isMouseOverNode)
            {
                if (!node.IsSelected)
                    SelectNode(node);
                else
                    UnselectNode(node);

                if (firstSelected != null && secondSelected != null)
                {
                    Point first = MathTool.CalcPointWithOffset(firstSelected, secondSelected);
                    Point second = MathTool.CalcPointWithOffset(secondSelected, firstSelected);

                    EdgeViewBuilder edgeBuilder = new EdgeViewBuilder();
                    EdgeView edgeView = edgeBuilder
                        .SetDirection(Direction.FirstToSecond)
                        .SetPoints(first, second)
                        .SetWeight(10)
                        .Build();

                    grid.Children.Add(edgeView.Edge);
                    grid.Children.Add(edgeView.Weight);

                    foreach (var line in edgeView.Arrows)
                        grid.Children.Add(line);

                    UnselectNode(firstSelected);
                    UnselectNode(secondSelected);
                }

                return;
            }

            string name = NodeNameSequence.Next.ToString();

            Point curPos = Mouse.GetPosition(grid);
            NodeViewBuilder builder = new NodeViewBuilder();
            builder.SetPosition(curPos)
                   .SetName(name);

            var view = builder.Build();

            grid.Children.Add(view.Node);
            grid.Children.Add(view.Name);
            Node newNode = new Node
            {
                Name = name,
                View = view
            };
            _model.Nodes.Add(newNode);
        }

        private (bool, Node?) IsMouseOverNode(Grid grid)
        {
            Point curPos = Mouse.GetPosition(grid);

            foreach (var node in _model.Nodes)
                if (curPos.X > node.View.Node.Margin.Left && curPos.X < node.View.Node.Margin.Left + node.View.Node.Width)
                    if (curPos.Y > node.View.Node.Margin.Top && curPos.Y < node.View.Node.Margin.Top + node.View.Node.Height)
                        return (true, node);

            return (false, null);
        }

        private void AddEdge(object obj)
        {

        }

        private void SelectNode(Node node)
        {
            node.View.Node.Stroke = Brushes.Yellow;
            node.View.Node.Fill = Brushes.LightYellow;
            node.IsSelected = true;
            if (firstSelected == null)
                firstSelected = node;
            else
                secondSelected = node;
        }

        private void UnselectNode(Node node)
        {
            node.View.Node.Stroke = Brushes.Black;
            node.View.Node.Fill = Brushes.White;
            node.IsSelected = false;
            if (firstSelected == node)
                firstSelected = null;
            else secondSelected = null;
        }
    }
}
