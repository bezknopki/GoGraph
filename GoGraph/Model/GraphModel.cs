using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Graphs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GoGraph.ViewElements;
using System.Windows.Controls;
using GraphEngine.Graph.Nodes;

namespace GoGraph.Model
{
    [Serializable]
    public class GraphModel : INotifyPropertyChanged
    {
        public GraphBase? Graph { get; set; }

        public List<EdgeView> EdgeViews { get; set; } = new List<EdgeView>();
        public Dictionary<Edge, EdgeView> EdgesToViews { get; set; } = new Dictionary<Edge, EdgeView>();
        public List<NodeView> NodeViews { get; set; } = new List<NodeView>();
        public Dictionary<Node, Border> NodesToViews { get; set; } = new Dictionary<Node, Border>();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
