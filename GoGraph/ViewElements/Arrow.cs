using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection.Metadata;
using GoGraph.Tools;

namespace GoGraph.ViewElements
{
    public class Arrow
    {
        const double arrowAngle = Math.PI / 8;
        const double radius = 20;

        private Point arrowPoint;
        private Point startPoint;
        
        public void SetPoints(Point from, Point to)
        {
            arrowPoint = to;
            startPoint = from;
        }

        public Polyline GetArrowView()
        {
            (Point first, Point second) = CalcArrowPoints();

            Polyline arrow = ViewElementsCreator.CreateArrowEmtyPolyline();

            arrow.Points.Add(first);
            arrow.Points.Add(arrowPoint);
            arrow.Points.Add(second);

            return arrow;
        }

        public void Redraw(Polyline arrow)
        {
            (Point first, Point second) = CalcArrowPoints();

            arrow.Points.Clear();
            arrow.Points.Add(first);
            arrow.Points.Add(arrowPoint);
            arrow.Points.Add(second);
        }

        private (Point, Point) CalcArrowPoints()
        {
            int quarterNum = MathTool.GetQuarterNum(startPoint, arrowPoint);

            double hypotenuse = Math.Sqrt(Math.Pow(startPoint.X - arrowPoint.X, 2) + Math.Pow(startPoint.Y - arrowPoint.Y, 2));
            double cosAlpha = (startPoint.X - arrowPoint.X) / hypotenuse;

            int sign = quarterNum > 2 ? -1 : 1;

            (double sinBeta, double cosBeta) = Math.SinCos(Math.Acos(cosAlpha) * sign + arrowAngle);
            (double sinGamma, double cosGamma) = Math.SinCos(Math.Acos(cosAlpha) * sign - arrowAngle);

            Point first = new Point(arrowPoint.X + radius * cosBeta, arrowPoint.Y + radius * sinBeta);
            Point second = new Point(arrowPoint.X + radius * cosGamma, arrowPoint.Y + radius * sinGamma);

            return (first, second);
        }
    }
}
