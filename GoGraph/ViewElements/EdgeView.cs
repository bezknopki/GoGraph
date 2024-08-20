using System.Windows.Controls;
using System.Windows.Shapes;

namespace GoGraph.ViewElements
{
    [Serializable]
    public class EdgeView
    {
        public Line Edge {  get; set; }
        public List<Polyline> Arrows { get; set; } = new List<Polyline>();
        public Border? Weight { get; set; }
    }
}
