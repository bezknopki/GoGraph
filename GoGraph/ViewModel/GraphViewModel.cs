﻿using GoGraph.Commands;
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
using GoGraph.Graph.Graphs.GraphCreator;
using System.Printing;
using GoGraph.Serializer;

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

        private List<Direction> _directions = new List<Direction> { Direction.FirstToSecond, Direction.SecondToFirst, Direction.Both };

        private Direction _selectedDirection = Direction.FirstToSecond;

        private string _weight = "0";
        private string _savePath = string.Empty;
        
        private NodeView? _firstSelected;
        private NodeView? _secondSelected;

        public List<Direction> Directions => _directions;

        public bool IsDirected => _model.Graph != null && _model.Graph.IsDirected;
        public bool IsWeightened => _model.Graph != null && _model.Graph.IsWeightened;
        public bool IsSaveAvailable => !string.IsNullOrEmpty(_savePath);
        public bool IsSaveAsAvailable => _model.Graph != null;

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

            foreach (var view in model.EdgeViews.Values)
            {
                grid.Children.Add(view.Edge);

                if (view.Arrows != null)
                    foreach (var arrow in view.Arrows)
                        grid.Children.Add(arrow);

                if (view.Weight != null)
                    grid.Children.Add(view.Weight);
            }

            foreach (var view in model.NodeViews.Keys)
            {
                grid.Children.Add(view.Node);
                grid.Children.Add(view.Name);
            }

            NodeNameSequence.SetStart(model.Graph.Nodes.Max(x => int.Parse(x.Name)));
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

            foreach (var node in _model.NodeViews.Keys)
                if (curPos.X > node.Node.Margin.Left && curPos.X < node.Node.Margin.Left + node.Node.Width)
                    if (curPos.Y > node.Node.Margin.Top && curPos.Y < node.Node.Margin.Top + node.Node.Height)
                        return (true, node);

            return (false, null);
        }

        private void AddEdge(Grid grid)
        {
            if (_firstSelected == null || _secondSelected == null) return;

            Point first = MathTool.CalcPointWithOffset(_firstSelected, _secondSelected);
            Point second = MathTool.CalcPointWithOffset(_secondSelected, _firstSelected);

            EdgeViewBuilder edgeViewBuilder = new EdgeViewBuilder();
            edgeViewBuilder.SetPoints(first, second);

            EdgeBuilder edgeBuilder = new EdgeBuilder();
            edgeBuilder.SetNodes(_model.NodeViews[_firstSelected], _model.NodeViews[_secondSelected]);

            SetWeight(edgeBuilder, edgeViewBuilder);

            SetDirection(edgeBuilder, edgeViewBuilder);

            Edge edge = edgeBuilder.Build();
            EdgeView edgeView = edgeViewBuilder.Build();

            DrawEdge(grid, edgeView);

            _model.EdgeViews.Add(edge, edgeView);
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
            _model.Graph.Nodes.Add(newNode);
            _model.NodeViews.Add(view, newNode);
        }

        private void SetDirection(EdgeBuilder edgeBuilder, EdgeViewBuilder edgeViewBuilder)
        {
            if (_firstSelected == null || _secondSelected == null) return;

            Node first = _model.NodeViews[_firstSelected];
            Node second = _model.NodeViews[_secondSelected];

            if (IsDirected)
            {
                edgeViewBuilder.SetDirection(SelectedDirection);
                edgeBuilder.SetDirection(SelectedDirection);
                switch (SelectedDirection)
                {
                    case Direction.FirstToSecond: first.Next.Add(second); break;
                    case Direction.SecondToFirst: second.Next.Add(first); break;
                    case Direction.Both:
                        {
                            first.Next.Add(second);
                            second.Next.Add(first);
                            break;
                        }
                }
            }
            else
            {
                first.Next.Add(second);
                second.Next.Add(first);
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
