using System.Windows;

namespace GoGraph.Serializer
{
    [Serializable]
    public class SPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public SPoint() { }

        public SPoint(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public SPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point ToPoint() => new Point(X, Y);
    }
}
