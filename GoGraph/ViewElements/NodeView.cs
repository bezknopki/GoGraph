using System.Windows.Controls;
using System.Windows.Shapes;

namespace GoGraph.ViewElements
{
    [Serializable]
    public class NodeView
    {
        public Ellipse Node { get; set; }
        public TextBlock Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
