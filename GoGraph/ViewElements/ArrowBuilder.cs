using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection.Metadata;

namespace GoGraph.ViewElements
{
    public class ArrowBuilder
    {
        const double arrowAngle = Math.PI / 8;
        const double radius = 30;

        private Point arrowPoint;
        private Point startPoint;
        
        public ArrowBuilder SetPoints(Point from, Point to)
        {
            arrowPoint = to;
            startPoint = from;
            return this;
        }

        public Polyline Build()
        {
            int quarterNum = GetQuarterNum(startPoint, arrowPoint);

            double lineLength = Math.Sqrt(Math.Pow(startPoint.X - arrowPoint.X, 2) + Math.Pow(startPoint.Y - arrowPoint.Y, 2));
            double cosAlpha = (startPoint.X - arrowPoint.X) / lineLength;

            int sign = quarterNum > 2 ? -1 : 1;

            (double sinBeta, double cosBeta) = Math.SinCos(Math.Acos(cosAlpha) * sign + arrowAngle);
            (double sinGamma, double cosGamma) = Math.SinCos(Math.Acos(cosAlpha) * sign - arrowAngle);

            Point first = new Point(arrowPoint.X + radius * cosBeta, arrowPoint.Y + radius * sinBeta);
            Point second = new Point(arrowPoint.X + radius * cosGamma, arrowPoint.Y + radius * sinGamma);

            Polyline arrow = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 3
            };

            arrow.Points.Add(first);
            arrow.Points.Add(arrowPoint);
            arrow.Points.Add(second);

            return arrow;
        }

        private int GetQuarterNum(Point from, Point to)
            => from.X < to.X
            ? from.Y < to.Y ? 3 : 2
            : from.Y >= to.Y ? 1 : 4;
    }
}
