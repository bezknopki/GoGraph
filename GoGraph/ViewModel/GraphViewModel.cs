using GoGraph.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GoGraph.Model;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using GoGraph.ViewElements;
using GoGraph.Tools;
using GraphEngine.Graph.Nodes;
using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Graphs.GraphCreator;
using GoGraph.Serializer;
using GoGraph.History;
using GraphEngine.Algorithms.ShortestWay;
using System.Collections.ObjectModel;
using GraphEngine.Algorithms.MinimalSpannedTree;

namespace GoGraph.ViewModel
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        private GraphModel _model = new GraphModel();

        private GraphCreator _creator = new SimpleGraphCreator();

        private RelayCommand _addNodeCommand;
        private RelayCommand _createGraphCommand;
        private RelayCommand _openCommand;
        private RelayCommand _saveCommand;
        private RelayCommand _saveAsCommand;
        private RelayCommand _beginSearchCommand;
        private RelayCommand _undoLastActionCommand;
        private RelayCommand _removeCommand;

        private List<Direction> _directions = new List<Direction> { Direction.FirstToSecond, Direction.SecondToFirst, Direction.Both };

        private Direction _selectedDirection = Direction.FirstToSecond;

        private string _weight = "0";
        private string _savePath = string.Empty;

        private NodeView? _firstSelected;
        private NodeView? _secondSelected;

        public GraphViewModel()
        {
            HistoryManager.Model = _model;
        }

        public List<Direction> Directions => _directions;

        public bool IsDirected => _model.Graph != null && _model.Graph.IsDirected;
        public bool IsWeightened => _model.Graph != null && _model.Graph.IsWeightened;
        public bool IsSaveAvailable => !string.IsNullOrEmpty(_savePath);
        public bool IsSaveAsAvailable => _model.Graph != null;
        public ObservableCollection<string> Results { get; set; } = new ObservableCollection<string>();

        public string Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public Direction SelectedDirection
        {
            get => _selectedDirection;
            set
            {
                _selectedDirection = value;
                OnPropertyChanged(nameof(SelectedDirection));
            }
        }

        #region COMMANDS
        public RelayCommand RemoveCommand
        {
            get => _removeCommand ??= new RelayCommand(obj => Remove(obj));
            set { _removeCommand = value; }
        }

        public RelayCommand UndoLastActionCommand
        {
            get => _undoLastActionCommand ??= new RelayCommand(obj => UndoLastAction(obj));
            set { _undoLastActionCommand = value; }
        }

        public RelayCommand BeginSearchCommand
        {
            get => _beginSearchCommand ??= new RelayCommand(obj => BeginSearch(obj), obj => obj is Grid);
            set { _beginSearchCommand = value; }
        }

        public RelayCommand SaveCommand
        {
            get => _saveCommand ??= new RelayCommand(_ => SaveProjectCommand(), _ => IsSaveAvailable);
            set { _saveCommand = value; }
        }

        public RelayCommand SaveAsCommand
        {
            get => _saveAsCommand ??= new RelayCommand(_ => SaveAsProjectCommand(), _ => IsSaveAsAvailable);
            set { _saveAsCommand = value; }
        }

        public RelayCommand OpenCommand
        {
            get => _openCommand ??= new RelayCommand(obj => OpenProjectCommand(obj), obj => obj is Grid);
            set { _openCommand = value; }
        }

        public RelayCommand CreateGraphCommand
        {
            get => _createGraphCommand ??= new RelayCommand(obj => CreateGraph(obj), obj => obj is GraphTypes);
            set { _createGraphCommand = value; }
        }

        public RelayCommand AddNodeCommand
        {
            get => _addNodeCommand ??= new RelayCommand(obj => AddNodeOrEdge(obj), obj => obj is Grid && _model.Graph != null);
            set { _addNodeCommand = value; }
        }
        #endregion

        private void Remove(object obj)
        {
            //TODO: add history remove
            if (obj is Grid grid)
            {
                (bool isMouseOverEdge, EdgeView? edgeView) = IsMouseOverEdge(grid);
                if (isMouseOverEdge) RemoveEdge(grid, edgeView);

                (bool isMouseOverNode, NodeView? nodeView) = IsMouseOverNode(grid);
                if (isMouseOverNode) RemoveNode(grid, nodeView);
            }
        }

        private void RemoveNode(Grid grid, NodeView nodeView)
        {
            grid.Children.Remove(nodeView.Node);
            grid.Children.Remove(nodeView.Name);
            Node associatedNode = _model.Graph.Nodes.First(x => x.Name == nodeView.Name.Text);
            foreach (var node in _model.Graph.Nodes)
                if (node.Next.ContainsKey(associatedNode))
                    node.Next.Remove(associatedNode);

            List<Edge> toRemove = new List<Edge>();
            foreach (var edge in _model.Graph.Edges)
                if (edge.First == associatedNode || edge.Second == associatedNode)
                    toRemove.Add(edge);

            foreach (var e in toRemove)
                RemoveEdge(grid, _model.EdgesToViews[e]);

            UnselectNode(nodeView);
        }

        private void RemoveEdge(Grid grid, EdgeView edgeView)
        {
            grid.Children.Remove(edgeView.Edge);
            if (edgeView.Weight != null)
                grid.Children.Remove(edgeView.Weight);

            if (edgeView.Arrows != null)
                foreach (var arr in edgeView.Arrows)
                    grid.Children.Remove(arr);

            Edge associatedEdge = _model.EdgesToViews.First(x => x.Value == edgeView).Key;
            _model.Graph.Edges.Remove(associatedEdge);

            if (associatedEdge.First.Next.ContainsKey(associatedEdge.Second))
                associatedEdge.First.Next.Remove(associatedEdge.Second);

            if (associatedEdge.Second.Next.ContainsKey(associatedEdge.First))
                associatedEdge.Second.Next.Remove(associatedEdge.First);

            _model.EdgesToViews.Remove(associatedEdge);
        }

        private async void BeginSearch(object obj)
        {
            AnimationHelper.Model = _model;
            AnimationHelper.Grid = (Grid)obj;
            Prim bfs = new();

            //bfs.HighlightNodeEvent += AnimationHelper.HighlightNode;
            //bfs.HighlightEdgeEvent += AnimationHelper.HighlightEdge;

            //string res = await bfs.Start(_model.Graph.Nodes.First());
            //MessageBox.Show(res);

            var res = bfs.Start(_model.Graph.Nodes.First(), _model.Graph.Nodes.Count);
            foreach(var e in _model.Graph.Edges)
            {
                if (!res.Contains(e))
                {
                    _model.EdgesToViews[e].Edge.Opacity = 0.3;
                    _model.EdgesToViews[e].Weight.Opacity = 0.3;
                }
            }
            //await AnimationHelper.WalkThrough(w);
            //AnimationHelper.Reset();
            //Results.Add(w.ToString());
        }

        private void SaveAsProjectCommand()
        {
            _savePath = ProjectSerializer.SerializeXML(new SerializebleGraphModel(_model));
        }

        private void SaveProjectCommand()
        {
            ProjectSerializer.SerializeXML(new SerializebleGraphModel(_model), _savePath);
        }

        private void OpenProjectCommand(object obj)
        {
            if (obj is Grid grid)
            {
                (string savePath, GraphModel model) = ProjectSerializer.DeserializeXML();
                SetModel(grid, model);
                _savePath = savePath;

                OnPropertyChanged(nameof(IsDirected));
                OnPropertyChanged(nameof(IsWeightened));
                OnPropertyChanged(nameof(IsSaveAvailable));
                OnPropertyChanged(nameof(IsSaveAsAvailable));
            }
        }

        private void SetModel(Grid grid, GraphModel model)
        {
            if (model == null) return;

            _model = model;
            grid.Children.Clear();

            foreach (var view in model.EdgeViews)
            {
                grid.Children.Add(view.Edge);

                if (view.Arrows != null)
                    foreach (var arrow in view.Arrows)
                        grid.Children.Add(arrow);

                if (view.Weight != null)
                    grid.Children.Add(view.Weight);
            }

            foreach (var view in model.NodeViews)
            {
                grid.Children.Add(view.Node);
                grid.Children.Add(view.Name);
            }

            NodeNameSequence.SetStart(model.Graph.Nodes.Max(x => int.Parse(x.Name)));
            HistoryManager.Model = model;
        }

        private void UndoLastAction(object obj)
        {
            if (obj is Grid grid)
            {
                var elements = HistoryManager.Undo();
                foreach (var element in elements)
                    grid.Children.Remove(element);
            }
        }

        private void CreateGraph(object obj)
        {
            var type = (GraphTypes)obj;
            _model.Graph = _creator.Create(type);
            OnPropertyChanged(nameof(IsDirected));
            OnPropertyChanged(nameof(IsWeightened));
            OnPropertyChanged(nameof(IsSaveAsAvailable));
        }

        private void AddNodeOrEdge(object obj)
        {
            Grid grid = (Grid)obj;

            (bool isMouseOverNode, NodeView? node) = IsMouseOverNode(grid);

            if (isMouseOverNode)
            {
                if (!node.IsSelected)
                    SelectNode(node);
                else
                    UnselectNode(node);

                if (_firstSelected != null && _secondSelected != null)
                {
                    AddEdge(grid);

                    UnselectNode(_firstSelected);
                    UnselectNode(_secondSelected);
                }

                return;
            }

            AddNode(grid);
        }

        private (bool, NodeView?) IsMouseOverNode(Grid grid)
        {
            Point curPos = Mouse.GetPosition(grid);

            foreach (var node in _model.NodeViews)
                if (curPos.X > node.Node.Margin.Left && curPos.X < node.Node.Margin.Left + node.Node.Width)
                    if (curPos.Y > node.Node.Margin.Top && curPos.Y < node.Node.Margin.Top + node.Node.Height)
                        return (true, node);

            return (false, null);
        }

        private (bool, EdgeView?) IsMouseOverEdge(Grid grid)
        {
            Point curPos = Mouse.GetPosition(grid);

            foreach (var edge in _model.EdgeViews)
                if (edge.Edge.IsMouseDirectlyOver)
                    return (true, edge);

            return (false, null);
        }

        private void AddEdge(Grid grid)
        {
            if (_firstSelected == null || _secondSelected == null) return;

            Point first = MathTool.CalcPointWithOffset(_firstSelected, _secondSelected);
            Point second = MathTool.CalcPointWithOffset(_secondSelected, _firstSelected);

            EdgeViewBuilder edgeViewBuilder = new EdgeViewBuilder();
            edgeViewBuilder.SetPoints(first, second)
                .SetDirection(_model.Graph.IsDirected ? _selectedDirection : Direction.None);

            EdgeBuilder edgeBuilder = new EdgeBuilder();
            edgeBuilder.SetNodes(
                GetNodeByView(_firstSelected),
                GetNodeByView(_secondSelected));

            SetWeight(edgeBuilder, edgeViewBuilder);

            Edge edge = edgeBuilder.Build();
            if (_model.Graph.Edges.Contains(edge))
            {
                MessageBox.Show("edge is already exist");
                return;
            }
            EdgeView edgeView = edgeViewBuilder.Build();

            DrawEdge(grid, edgeView);

            Node fNode = GetNodeByView(_firstSelected);
            Node sNode = GetNodeByView(_secondSelected);

            if (IsDirected)
            {
                edgeViewBuilder.SetDirection(SelectedDirection);
                edgeBuilder.SetDirection(SelectedDirection);
                switch (SelectedDirection)
                {
                    case Direction.FirstToSecond: fNode.Next.Add(sNode, edge); break;
                    case Direction.SecondToFirst: sNode.Next.Add(fNode, edge); break;
                    case Direction.Both:
                        {
                            fNode.Next.Add(sNode, edge);
                            sNode.Next.Add(fNode, edge);
                            break;
                        }
                }
            }
            else
            {
                fNode.Next.Add(sNode, edge);
                sNode.Next.Add(fNode, edge);
            }

            HistoryElement he = new HistoryElement();
            he.ActionType = ActionType.Add;
            he.Edges.Add(edge);
            he.EdgeViews.Add(edgeView);
            he.Elements.Add(edgeView.Edge);
            if (edgeView.Weight != null)
                he.Elements.Add(edgeView.Weight);
            if (edgeView.Arrows != null)
                he.Elements.AddRange(edgeView.Arrows);
            HistoryManager.Push(he);

            _model.EdgeViews.Add(edgeView);
            _model.EdgesToViews.Add(edge, edgeView);
            _model.Graph.Edges.Add(edge);
        }

        private void DrawEdge(Grid grid, EdgeView edgeView)
        {
            grid.Children.Add(edgeView.Edge);

            if (IsWeightened)
                grid.Children.Add(edgeView.Weight);

            if (IsDirected)
                foreach (var line in edgeView.Arrows)
                    grid.Children.Add(line);
        }

        private void AddNode(Grid grid)
        {
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
                Name = name
            };

            HistoryElement he = new HistoryElement();
            he.ActionType = ActionType.Add;
            he.Nodes.Add(newNode);
            he.NodeViews.Add(view);
            he.Elements.Add(view.Name);
            he.Elements.Add(view.Node);
            HistoryManager.Push(he);

            _model.Graph.Nodes.Add(newNode);
            _model.NodeViews.Add(view);
        }

        private void SetWeight(EdgeBuilder edgeBuilder, EdgeViewBuilder edgeViewBuilder)
        {
            if (_firstSelected == null || _secondSelected == null) return;

            if (IsWeightened)
            {
                if (int.TryParse(Weight, out int w))
                {
                    edgeViewBuilder.SetWeight(w);
                    edgeBuilder.SetWeight(w);
                }
                else throw new ArgumentException();
            }
        }

        private void SelectNode(NodeView node)
        {
            node.Node.Stroke = Brushes.Yellow;
            node.Node.Fill = Brushes.LightYellow;
            node.IsSelected = true;
            if (_firstSelected == null)
                _firstSelected = node;
            else
                _secondSelected = node;
        }

        private void UnselectNode(NodeView node)
        {
            node.Node.Stroke = Brushes.Black;
            node.Node.Fill = Brushes.White;
            node.IsSelected = false;
            if (_firstSelected == node)
                _firstSelected = null;
            else _secondSelected = null;
        }

        private Node GetNodeByView(NodeView view) => _model.Graph?.Nodes.First(x => x.Name == view.Name.Text);

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
