using GoGraph.Model;
using GoGraph.ViewElements;
using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Graphs.GraphCreator;
using GraphEngine.Graph.Nodes;
using System.Windows;

namespace GoGraphTests
{
    public static class TestHelper
    {
        public static string ProjectsDirectory = "..\\..\\..\\TestProjects";

        public static GraphModel MakeTestModel()
        {
            GraphModel model = new GraphModel();
            model.Graph = new SimpleGraphCreator().Create(GraphTypes.DirectedWeightened);

            Random r = new Random();

            for (int i = 0; i < 20; i++)
            {
                Node node = new Node
                {
                    Name = i.ToString()
                };

                NodeViewBuilder nvb = new NodeViewBuilder();
                NodeView nv = nvb
                    .SetPosition(new Point(r.Next(100, 200), r.Next(100, 200)))
                    .SetName(i.ToString())
                    .Build();

                model.NodeViews.Add(nv);
                model.Graph.Nodes.Add(node);
            }

            for (int i = 0; i < 20; i++)
            {
                Node second = model.Graph.Nodes.FirstOrDefault(x => x.Next.Count == 0) ?? model.Graph.Nodes[r.Next(0, i - 1)];
                int j = model.Graph.Nodes.IndexOf(second);
                Edge e = new Edge
                {
                    Direction = Direction.FirstToSecond,
                    First = model.Graph.Nodes[i],
                    Second = second,
                    IsDirected = true,
                    IsWeightened = true,
                    Weight = r.Next(1, 10)
                };

                EdgeViewBuilder evb = new EdgeViewBuilder();
                EdgeView ev = evb
                    .SetDirection(Direction.FirstToSecond)
                    .SetPoints(new Point(model.NodeViews[i].Node.Margin.Left, model.NodeViews[i].Node.Margin.Top),
                    new Point(model.NodeViews[j].Node.Margin.Left, model.NodeViews[j].Node.Margin.Top))
                    .SetWeight(e.Weight)
                    .Build();

                model.Graph.Nodes[i].Next.Add(second, e);

                model.EdgesToViews.Add(e,ev);
                model.EdgeViews.Add(ev);
                model.Graph.Edges.Add(e);
            }

           return model;
        }
    }
}
