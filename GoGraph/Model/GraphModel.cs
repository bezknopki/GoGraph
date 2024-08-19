using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoGraph.Graph.Nodes;
using GoGraph.Graph.Edges;
using GoGraph.Graph.Graphs;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GoGraph.Model
{
    public class GraphModel : INotifyPropertyChanged
    {
        public GraphBase? Graph { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
