using GoGraph.ViewElements;
using System.Windows.Shapes;

namespace GoGraph.Serializer
{
    [Serializable]
    public class SEdgeView
    {
        public SPoint P1 { get; set; }
        public SPoint P2 { get; set; }

        public List<List<SPoint>> Arrows { get; set; } = new List<List<SPoint>>();

        public string WeightText { get; set; }
        public double WeightMarginLeft { get; set; }
        public double WeightMarginTop { get; set; }
        public bool HasWeight { get; set; }

        public SEdgeView() { }

        public SEdgeView(EdgeView edgeView)
        {
            P1 = new SPoint(edgeView.Edge.X1, edgeView.Edge.Y1);
            P2 = new SPoint(edgeView.Edge.X2, edgeView.Edge.Y2);

            HasWeight = edgeView.Weight != null;

            if (HasWeight)
            {
                WeightText = edgeView.Weight.Text;
                WeightMarginTop = edgeView.Weight.Margin.Top;
                WeightMarginLeft = edgeView.Weight.Margin.Left;
            }

            if (edgeView.Arrows != null)
                foreach (var arrow in edgeView.Arrows)
                {
                    List<SPoint> sArrow = new List<SPoint>();
                    sArrow.AddRange(arrow.Points.Select(x => new SPoint(x)));
                    Arrows.Add(sArrow);
                }
        }

        public EdgeView ToEdgeView()
        {
            EdgeView edgeView = new EdgeView();

            edgeView.Edge = ViewElementsCreator.CreateEdgeLine(P1.ToPoint(), P2.ToPoint());

            foreach (var arrow in Arrows)
            {
                Polyline pl = ViewElementsCreator.CreateArrowEmtyPolyline();

                foreach (var point in arrow)
                    pl.Points.Add(point.ToPoint());

                edgeView.Arrows.Add(pl);
            }

            if (HasWeight)
                edgeView.Weight = ViewElementsCreator.CreateWeightTextBlock(WeightMarginLeft, WeightMarginTop, WeightText);

            return edgeView;
        }
    }
}
