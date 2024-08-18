using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GoGraph.ViewElements
{
    public class EdgeView
    {
        public Line Edge {  get; set; }
        public List<Polyline>? Arrows { get; set; }
        public TextBlock? Weight { get; set; }
    }
}
