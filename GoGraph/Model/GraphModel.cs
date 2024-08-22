﻿using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Graphs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GoGraph.ViewElements;

namespace GoGraph.Model
{
    [Serializable]
    public class GraphModel : INotifyPropertyChanged
    {
        public GraphBase? Graph { get; set; }

        public List<EdgeView> EdgeViews { get; set; } = new List<EdgeView>();
        public Dictionary<Edge, EdgeView> EdgesToViews { get; set; } = new Dictionary<Edge, EdgeView>();
        public List<NodeView> NodeViews { get; set; } = new List<NodeView>();  

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
