using GoGraph.ViewElements;
using GoGraph.ViewModel;
using GraphEngine.Graph.Edges;
using System.Reflection;
using System.Windows.Controls;

namespace GoGraphTests.ViewTests
{
    public class ViewModelTests
    {
        const string newNodeName = "20";
        const string existingNodeName = "10";

        [StaFact]
        public void HistoryUndoAddTest()
        {
            Grid grid = new Grid();
            GraphViewModel gvm = new GraphViewModel();
            var model = TestHelper.MakeTestModel();
            int originalCount = model.Graph.Nodes.Count;

            gvm.SetModel(grid, model);
            gvm.AddNodeOrEdgeCommand.Execute(grid);

            Assert.Contains(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.Contains(model.NodeViews, x => x.Name.Text == newNodeName);

            MethodInfo? selectNode = typeof(GraphViewModel).GetMethod("SelectNode", BindingFlags.NonPublic | BindingFlags.Instance);
            NodeView toSelect = model.NodeViews.First(x => x.Name.Text == existingNodeName);

            selectNode?.Invoke(gvm, [toSelect]);

            gvm.AddNodeOrEdgeCommand.Execute(grid);

            Assert.Contains(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.Contains(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));

            Edge e = model.Graph.Edges.First(x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            EdgeView ev = model.EdgesToViews[e];

            Assert.Contains(model.EdgeViews, x => x == ev);

            gvm.UndoLastActionCommand.Execute(grid);

            Assert.Contains(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.Contains(model.NodeViews, x => x.Name.Text == newNodeName);
            Assert.DoesNotContain(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.DoesNotContain(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.DoesNotContain(model.EdgeViews, x => x == ev);

            gvm.UndoLastActionCommand.Execute(grid);

            Assert.DoesNotContain(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.DoesNotContain(model.NodeViews, x => x.Name.Text == newNodeName);
        }

        [StaFact]
        public void HistoryUndoRemoveTest()
        {
            Grid grid = new Grid();
            GraphViewModel gvm = new GraphViewModel();
            var model = TestHelper.MakeTestModel();
            int originalCount = model.Graph.Nodes.Count;

            gvm.SetModel(grid, model);
            gvm.AddNodeOrEdgeCommand.Execute(grid);

            MethodInfo? selectNode = typeof(GraphViewModel).GetMethod("SelectNode", BindingFlags.NonPublic | BindingFlags.Instance);
            NodeView toSelect = model.NodeViews.First(x => x.Name.Text == existingNodeName);
            selectNode?.Invoke(gvm, [toSelect]);

            gvm.AddNodeOrEdgeCommand.Execute(grid);

            MethodInfo? removeNode = typeof(GraphViewModel).GetMethod("RemoveNode", BindingFlags.NonPublic | BindingFlags.Instance);
            removeNode.Invoke(gvm, [grid, model.NodeViews.First(x => x.Name.Text == newNodeName)]);

            Assert.DoesNotContain(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.DoesNotContain(model.NodeViews, x => x.Name.Text == newNodeName);
            Assert.DoesNotContain(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.DoesNotContain(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));

            gvm.UndoLastActionCommand.Execute(grid);

            Assert.Contains(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.Contains(model.NodeViews, x => x.Name.Text == newNodeName);
            Assert.Contains(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.Contains(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
        }

        private bool IsEdgeBetweenNodes(Edge x, string n1, string n2) => (x.First.Name == n1 && x.Second.Name == n2) || (x.First.Name == n2 && x.Second.Name == n1);
    }
}
