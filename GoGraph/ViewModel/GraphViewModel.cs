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
using System.Collections.ObjectModel;
using GraphEngine.GraphMath.InformedSearch;
using System.Windows.Shapes;
using DialogWindow;
using GraphEngine.GraphMath;
using GraphEngine.GraphMath.ShortestWay;

namespace GoGraph.ViewModel
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        private WpfUIWindowDialogService _uiDialogService = new WpfUIWindowDialogService();
        private GraphModel _model = new GraphModel();

        private GraphCreator _creator = new SimpleGraphCreator();

        private RelayCommand _addNodeCommand;
        private RelayCommand _createGraphCommand;
        private RelayCommand _openCommand;
        private RelayCommand _saveCommand;
        private RelayCommand _saveAsCommand;
        private RelayCommand _executeCommand;
        private RelayCommand _undoLastActionCommand;
        private RelayCommand _removeCommand;

        private RelayCommand _closeCommand;
        private RelayCommand _selectCommand;
        private RelayCommand _createWebGraphCommand;

        private List<Direction> _directions = new List<Direction> { Direction.FirstToSecond, Direction.SecondToFirst, Direction.Both };

        private Direction _selectedDirection = Direction.FirstToSecond;
        private Algorithms _selectedAlgorithm = Algorithms.Prim;

        private string _weight = "0";
        private string _savePath = string.Empty;

        private NodeView? _firstSelected;
        private NodeView? _secondSelected;

        public GraphViewModel()
        {
            HistoryManager.Model = _model;
            foreach (var a in Enum.GetValues(typeof(Algorithms)))
                AvailableAlgorithms.Add((Algorithms)a);
        }

        public List<Direction> Directions => _directions;

        public bool IsDirected => _model.Graph != null && _model.Graph.IsDirected;
        public bool IsWeightened => _model.Graph != null && _model.Graph.IsWeightened;
        public bool IsSaveAvailable => !string.IsNullOrEmpty(_savePath);
        public bool IsSaveAsAvailable => _model.Graph != null;
        public ObservableCollection<string> Results { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Algorithms> AvailableAlgorithms { get; set; } = new ObservableCollection<Algorithms>();

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

        public Algorithms SelectedAlgorithm
        {
            get => _selectedAlgorithm;
            set
            {
                _selectedAlgorithm = value;
                OnPropertyChanged(nameof(SelectedAlgorithm));
            }
        }

        #region COMMANDS
        public RelayCommand CreateWebGraphCommand
        {
            get => _createWebGraphCommand ??= new RelayCommand(obj => CreateWebGraph(obj));
            set { _createWebGraphCommand = value; }
        }

        public RelayCommand CloseCommand
        {
            get => _closeCommand ??= new RelayCommand(obj => CloseNode(obj));
            set { _closeCommand = value; }
        }

        public RelayCommand SelectCommand
        {
            get => _selectCommand ??= new RelayCommand(obj => SelectFromOrDst(obj));
            set { _selectCommand = value; }
        }

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

        public RelayCommand ExecuteCommand
        {
            get => _executeCommand ??= new RelayCommand(obj => ExecuteAlgorithm(obj), obj => obj is Grid);
            set { _executeCommand = value; }
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

        public RelayCommand AddNodeOrEdgeCommand
        {
            get => _addNodeCommand ??= new RelayCommand(obj => AddNodeOrEdge(obj), obj => obj is Grid && _model.Graph != null);
            set { _addNodeCommand = value; }
        }
        #endregion

        public void SetModel(Grid grid, GraphModel model)
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

            NodeNameSequence.SetStart(model.Graph.Nodes.Max(x => x.Id));
            HistoryManager.Model = model;
        }

        private void CreateWebGraph(object obj)
        {
            Grid grid = (Grid)obj;

            CreateGraph(GraphTypes.Web);

            WebGraphSettingsViewModel settingsVM = new WebGraphSettingsViewModel();

            if (!_uiDialogService.ShowDialog("Web Graph Settings", settingsVM).Value) return;

            int rows = settingsVM.Rows;
            int cols = settingsVM.Columns;          

            WebNode[][] nodeMatrix = new WebNode[cols][];
            for (int i = 0; i < cols; i++)
                nodeMatrix[i] = new WebNode[rows];

            SplitGrid(grid, nodeMatrix);
            AdaptGridForWebGraph(grid);
            FillWebGraphNeighbours(nodeMatrix);
        }

        private void SplitGrid(Grid grid, WebNode[][] nodeMatrix)
        {
            int cols = nodeMatrix.Length;
            int rows = nodeMatrix[0].Length;

            for (int i = 0; i < cols; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int j = 0; j < rows; j++)
                grid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                {
                    Border border = new Border()
                    {
                        BorderThickness = new Thickness(1),
                        Background = Brushes.FloralWhite,
                        BorderBrush = Brushes.Black
                    };

                    grid.Children.Add(border);

                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, j);

                    nodeMatrix[i][j] = new WebNode { Id = NodeNameSequence.Next, X = i, Y = j };
                    _model.Graph.Nodes.Add(nodeMatrix[i][j]);
                    _model.NodesToViews.Add(nodeMatrix[i][j], border);
                }
        }

        private void AdaptGridForWebGraph(Grid grid)
        {
            grid.InputBindings.Clear();

            MouseBinding lmb = new MouseBinding();
            lmb.MouseAction = MouseAction.LeftClick;
            lmb.Command = CloseCommand;
            lmb.CommandParameter = grid;
            grid.InputBindings.Add(lmb);

            MouseBinding rmb = new MouseBinding();
            rmb.MouseAction = MouseAction.RightClick;
            rmb.Command = SelectCommand;
            rmb.CommandParameter = grid;
            grid.InputBindings.Add(rmb);
        }

        private void FillWebGraphNeighbours(WebNode[][] nodeMatrix)
        {
            int cols = nodeMatrix.Length;
            int rows = nodeMatrix[0].Length;

            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                {
                    if (i > 0)
                    {
                        nodeMatrix[i][j].Next.Add(nodeMatrix[i - 1][j], new Edge { Weight = 1, IsWeightened = true });

                        if (j < rows - 1)
                            nodeMatrix[i][j].Next.Add(nodeMatrix[i - 1][j + 1], new Edge { Weight = Math.Sqrt(2), IsWeightened = true });

                        if (j > 0)
                            nodeMatrix[i][j].Next.Add(nodeMatrix[i - 1][j - 1], new Edge { Weight = Math.Sqrt(2), IsWeightened = true });
                    }

                    if (j > 0)
                        nodeMatrix[i][j].Next.Add(nodeMatrix[i][j - 1], new Edge { Weight = 1, IsWeightened = true });

                    if (j < rows - 1)
                        nodeMatrix[i][j].Next.Add(nodeMatrix[i][j + 1], new Edge { Weight = 1, IsWeightened = true });

                    if (i < cols - 1)
                    {
                        if (j > 0)
                            nodeMatrix[i][j].Next.Add(nodeMatrix[i + 1][j - 1], new Edge { Weight = Math.Sqrt(2), IsWeightened = true });

                        nodeMatrix[i][j].Next.Add(nodeMatrix[i + 1][j], new Edge { Weight = 1, IsWeightened = true });

                        if (j < rows - 1)
                            nodeMatrix[i][j].Next.Add(nodeMatrix[i + 1][j + 1], new Edge { Weight = Math.Sqrt(2), IsWeightened = true });
                    }
                }
        }

        private void UndoLastAction(object obj)
        {
            if (obj is Grid grid)
            {
                (ActionType type, List<UIElement> elements) = HistoryManager.Undo();

                if (type == ActionType.Add)
                    foreach (var element in elements)
                        grid.Children.Remove(element);
                else
                    foreach (var element in elements)
                        grid.Children.Add(element);
            }
        }

        private void CloseNode(object obj)
        {
            Grid grid = (Grid)obj;
            Border selected = null;
            foreach (var el in grid.Children)
                if (el is Border br && br.IsMouseDirectlyOver)
                {
                    selected = br;
                    break;
                }
            int i = Grid.GetColumn(selected) * 20;
            int j = Grid.GetRow(selected);

            Node node = _model.Graph.Nodes[i + j];
            foreach (var next in node.Next)
                next.Key.Next.Remove(node);
            node.Next.Clear();
            selected.Background = Brushes.Crimson;
        }

        WebNode _from = null;
        WebNode _dst = null;

        Ellipse wlkr = null;
        //TODO: ref
        private async void SelectFromOrDst(object obj)
        {
            Grid grid = (Grid)obj;
            Border selected = null;
            foreach (var el in grid.Children)
                if (el is Border br && br.IsMouseDirectlyOver)
                {
                    selected = br;
                    break;
                }
            int i = Grid.GetColumn(selected) * grid.RowDefinitions.Count;
            int j = Grid.GetRow(selected);

            WebNode node = (WebNode)_model.Graph.Nodes[i + j];
            if (_from == null)
            {
                _from = node;
                Ellipse walker = new Ellipse
                {
                    Stroke = Brushes.ForestGreen,
                    Fill = Brushes.ForestGreen,
                    Height = 10,
                    Width = 10,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
                walker.Margin = new Thickness(Grid.GetColumn(selected) * selected.ActualWidth + selected.ActualWidth / 2 - 5,
                    Grid.GetRow(selected) * selected.ActualHeight + selected.ActualHeight / 2 - 5, 0, 0);
                Grid.SetColumnSpan(walker, grid.ColumnDefinitions.Count);
                Grid.SetRowSpan(walker, grid.RowDefinitions.Count);
                grid.Children.Add(walker);

                wlkr = walker;
            }
            else
            {
                _dst = node;
                AStar astar = new AStar();

                var res = astar.Start(_from, _dst);
                Point start = new Point(wlkr.Margin.Left + 5, wlkr.Margin.Top + 5);

                foreach (var item in res)
                {
                    double width = _model.NodesToViews[item].ActualWidth;
                    double height = _model.NodesToViews[item].ActualHeight;
                    int col = Grid.GetColumn(_model.NodesToViews[item]);
                    int row = Grid.GetRow(_model.NodesToViews[item]);
                    Point end = new Point(col * width + width / 2, row * height + height / 2);
                    await AnimationHelper.Walk(start, end, wlkr);
                    start = end;
                }

                _from = _dst;
            }

            //selected.Background = Brushes.Blue;
        }

        private void Remove(object obj)
        {
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
            Node associatedNode = _model.Graph.Nodes.First(x => x.Id.ToString() == nodeView.Name.Text);
            foreach (var node in _model.Graph.Nodes)
                if (node.Next.ContainsKey(associatedNode))
                    node.Next.Remove(associatedNode);

            List<Edge> toRemove = new List<Edge>();
            foreach (var edge in _model.Graph.Edges)
                if ((edge.First == associatedNode || edge.Second == associatedNode) && !toRemove.Contains(edge))
                    toRemove.Add(edge);

            foreach (var e in toRemove)
                RemoveEdge(grid, _model.EdgesToViews[e]);

            _model.Graph.Nodes.Remove(associatedNode);
            _model.NodeViews.Remove(nodeView);

            UnselectNode(nodeView);

            HistoryManager.Push(ActionType.Remove, nodeView, nodeView.Node, nodeView.Name, associatedNode);
            HistoryManager.Merge(ActionType.Remove, toRemove.Count + 1);
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

            if (associatedEdge.First.Next.ContainsKey(associatedEdge.Second))
                associatedEdge.First.Next.Remove(associatedEdge.Second);

            if (associatedEdge.Second.Next.ContainsKey(associatedEdge.First))
                associatedEdge.Second.Next.Remove(associatedEdge.First);

            _model.Graph.Edges.Remove(associatedEdge);
            _model.EdgesToViews.Remove(associatedEdge);
            _model.EdgeViews.Remove(edgeView);

            HistoryManager.Push(ActionType.Remove, associatedEdge, edgeView.Edge, edgeView, edgeView.Arrows, edgeView.Weight);
        }

        private async void ExecuteAlgorithm(object obj)
        {
            AlgorithmsFacade algorithmsFacade = new AlgorithmsFacade();
            switch (SelectedAlgorithm)
            {
                case Algorithms.Dijkstra:
                    {
                        ExecuteSettingsViewModel esvm = new ExecuteSettingsViewModel(true, true);
                        if (_uiDialogService.ShowDialog("Dijkstra Settings", esvm).Value)
                        {
                            Node root = _model.Graph.Nodes.First(x => x.Id == esvm.RootNodeName);
                            Node goal = _model.Graph.Nodes.First(x => x.Id == esvm.GoalNodeName);
                            Way res = algorithmsFacade.FindShortestWayDijkstra(_model.Graph, root, goal);
                            Results.Add(res.ToString()); 
                            await AnimationHelper.WalkThrough(res);
                        }
                        break;
                    }
                case Algorithms.Prim:
                    {
                        ExecuteSettingsViewModel esvm = new ExecuteSettingsViewModel(true, false);

                        if (_uiDialogService.ShowDialog("Dijkstra Settings", esvm).Value)
                        {
                            Node root = _model.Graph.Nodes.First(x => x.Id == esvm.RootNodeName);
                            List<Edge> tree = algorithmsFacade.MSTPrim(root);
                            Results.Add(string.Join("\n", tree.Select(x => $"{x.First.Id} - {x.Second.Id} Weight: {x.Weight}").Union([$"Sum: {tree.Sum(x => x.Weight)}"])));
                        }
                        break;
                    }
                case Algorithms.BreadthFirstSearch:
                    {
                        ExecuteSettingsViewModel esvm = new ExecuteSettingsViewModel(true, false);

                        if (_uiDialogService.ShowDialog("Dijkstra Settings", esvm).Value)
                        {
                            Node root = _model.Graph.Nodes.First(x => x.Id == esvm.RootNodeName);
                            LinkedList<Node> nodes = await algorithmsFacade.BreadthFirstSearch(root);
                            Results.Add(string.Join(" -> ", nodes.Select(x => x.Id)));
                        }
                        break;
                    }
                case Algorithms.DepthFirstSearch:
                    {
                        ExecuteSettingsViewModel esvm = new ExecuteSettingsViewModel(true, false);

                        if (_uiDialogService.ShowDialog("Dijkstra Settings", esvm).Value)
                        {
                            Node root = _model.Graph.Nodes.First(x => x.Id == esvm.RootNodeName);
                            LinkedList<Node> nodes = await algorithmsFacade.DepthFirstSearch(root);
                            Results.Add(string.Join(" -> ", nodes.Select(x => x.Id)));
                        }
                        break;
                    }
                case Algorithms.ASTar:
                    {
                        break;
                    }
            }
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

            HistoryManager.Push(ActionType.Add, edge, edgeView, edgeView.Edge, edgeView.Weight, edgeView.Arrows);

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
            Point curPos = Mouse.GetPosition(grid);

            int id = NodeNameSequence.Next;
            NodeViewBuilder builder = new NodeViewBuilder();
            builder.SetPosition(curPos)
                   .SetName(id.ToString());

            var view = builder.Build();

            grid.Children.Add(view.Node);
            grid.Children.Add(view.Name);
            Node newNode = new Node
            {
                Id = id
            };

            HistoryManager.Push(ActionType.Add, newNode, view, view.Node, view.Name);

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

        private Node GetNodeByView(NodeView view) => _model.Graph?.Nodes.First(x => x.Id.ToString() == view.Name.Text);

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
