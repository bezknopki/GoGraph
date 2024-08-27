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

        Grid _grid = new Grid();
        GraphViewModel _gvm = new GraphViewModel();

        [StaFact]
        public void HistoryUndoAddTest()
        {           
            var model = TestHelper.MakeTestModel();
            int originalCount = model.Graph.Nodes.Count;

            _gvm.SetModel(_grid, model);
            _gvm.AddNodeOrEdgeCommand.Execute(_grid);

            Assert.Contains(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.Contains(model.NodeViews, x => x.Name.Text == newNodeName);

            MethodInfo? selectNode = typeof(GraphViewModel).GetMethod("SelectNode", BindingFlags.NonPublic | BindingFlags.Instance);
            NodeView toSelect = model.NodeViews.First(x => x.Name.Text == existingNodeName);

            selectNode?.Invoke(_gvm, [toSelect]);

            _gvm.AddNodeOrEdgeCommand.Execute(_grid);

            Assert.Contains(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.Contains(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));

            Edge e = model.Graph.Edges.First(x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            EdgeView ev = model.EdgesToViews[e];

            Assert.Contains(model.EdgeViews, x => x == ev);

            _gvm.UndoLastActionCommand.Execute(_grid);

            Assert.Contains(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.Contains(model.NodeViews, x => x.Name.Text == newNodeName);
            Assert.DoesNotContain(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.DoesNotContain(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.DoesNotContain(model.EdgeViews, x => x == ev);

            _gvm.UndoLastActionCommand.Execute(_grid);

            Assert.DoesNotContain(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.DoesNotContain(model.NodeViews, x => x.Name.Text == newNodeName);
        }

        [StaFact]
        public void HistoryUndoRemoveTest()
        {
            var model = TestHelper.MakeTestModel();
            int originalCount = model.Graph.Nodes.Count;

            _gvm.SetModel(_grid, model);
            _gvm.AddNodeOrEdgeCommand.Execute(_grid);

            MethodInfo? selectNode = typeof(GraphViewModel).GetMethod("SelectNode", BindingFlags.NonPublic | BindingFlags.Instance);
            NodeView toSelect = model.NodeViews.First(x => x.Name.Text == existingNodeName);
            selectNode?.Invoke(_gvm, [toSelect]);

            _gvm.AddNodeOrEdgeCommand.Execute(_grid);

            MethodInfo? removeNode = typeof(GraphViewModel).GetMethod("RemoveNode", BindingFlags.NonPublic | BindingFlags.Instance);
            removeNode.Invoke(_gvm, [_grid, model.NodeViews.First(x => x.Name.Text == newNodeName)]);

            Assert.DoesNotContain(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.DoesNotContain(model.NodeViews, x => x.Name.Text == newNodeName);
            Assert.DoesNotContain(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.DoesNotContain(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));

            _gvm.UndoLastActionCommand.Execute(_grid);

            Assert.Contains(model.Graph.Nodes, x => x.Name == newNodeName);
            Assert.Contains(model.NodeViews, x => x.Name.Text == newNodeName);
            Assert.Contains(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
            Assert.Contains(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeName, newNodeName));
        }

        private bool IsEdgeBetweenNodes(Edge x, string n1, string n2) => (x.First.Name == n1 && x.Second.Name == n2) || (x.First.Name == n2 && x.Second.Name == n1);
    }
}
