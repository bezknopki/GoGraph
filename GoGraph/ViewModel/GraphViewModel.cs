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

namespace GoGraph.ViewModel
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        private RelayCommand _addNodeCommand;
        private GraphModel _model = new GraphModel();

        Point first;
        Point second;

        private int count = 0;

        public RelayCommand AddNodeCommand
        {
            get
            {
                return _addNodeCommand ??= new RelayCommand(obj => AddNode(obj), obj => obj is Grid);
            }
            set { _addNodeCommand = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void AddNode(object obj)
        {
            Grid grid = (Grid)obj;

            Point curPos = Mouse.GetPosition(grid);
            NodeViewBuilder builder = new NodeViewBuilder();
            builder.SetPosition(curPos)
                   .SetName(NodeNameSequence.Next.ToString());

            var view = builder.Build();

            grid.Children.Add(view.Node);
            grid.Children.Add(view.Name);
        }

        private void AddEdge(object obj)
        {
            Grid grid = (Grid)obj;

            if (count % 2 == 0)
                first = Mouse.GetPosition(grid);
            else
            {
                second = Mouse.GetPosition(grid);
                EdgeViewBuilder builder = new EdgeViewBuilder();
                EdgeView view = builder
                    .SetDirection(Graph.Edges.Direction.SecondToFirst)
                    .SetPoints(first, second)
                    .SetWeight(10)
                    .Build();

                grid.Children.Add(view.Edge);
                grid.Children.Add(view.Weight);
                foreach (var line in view.Arrows)
                    grid.Children.Add(line);
            }

            count++;
        }
    }
}
