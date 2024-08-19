using GoGraph.Graph.Nodes;
using GoGraph.ViewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GoGraph.Tools
{
    public static class MathTool
    {
        const double radius = 26;

        public static int GetQuarterNum(Point from, Point to)
            => from.X < to.X
            ? from.Y < to.Y ? 3 : 2
            : from.Y >= to.Y ? 1 : 4;

        public static Point CalcPointWithOffset(NodeView from, NodeView to)
        {            
            double x1 = from.Node.Margin.Left + from.Node.Width / 2;
            double x2 = to.Node.Margin.Left + to.Node.Width / 2;
            double y1 = from.Node.Margin.Top + from.Node.Height / 2;
            double y2 = to.Node.Margin.Top + to.Node.Height / 2;

            Point pFrom = new Point(x1, y1);
            Point pTo = new Point(x2, y2);

            double hypotenuse = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            double cosAlpha = (x1 - x2) / hypotenuse;

            int quarterNum = GetQuarterNum(pFrom, pTo);

            int sign = quarterNum > 2 ? -1 : 1;

            (double sinBeta, double cosBeta) = Math.SinCos(Math.Acos(cosAlpha) * sign + Math.PI);

            return new Point(x1 + radius * cosBeta, y1 + radius * sinBeta);
        }
    }
}
