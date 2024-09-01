using GoGraph.ViewElements;
using GoGraph.ViewModel;
using GraphEngine.Graph.Edges;
using System.Reflection;
using System.Windows.Controls;

namespace GoGraphTests.ViewTests
{
    public class ViewModelTests
    {
        const int newNodeId = 20;
        const int existingNodeId = 10;

        Grid _grid = new Grid();
        GraphViewModel _gvm = new GraphViewModel();

        [StaFact]
        public void HistoryUndoAddTest()
        {           
            var model = TestHelper.MakeTestModel();
            int originalCount = model.Graph.Nodes.Count;

            _gvm.SetModel(_grid, model);
            _gvm.AddNodeOrEdgeCommand.Execute(_grid);

            Assert.Contains(model.Graph.Nodes, x => x.Id == newNodeId);
            Assert.Contains(model.NodeViews, x => x.Name.Text == newNodeId.ToString());

            MethodInfo? selectNode = typeof(GraphViewModel).GetMethod("SelectNode", BindingFlags.NonPublic | BindingFlags.Instance);
            NodeView toSelect = model.NodeViews.First(x => x.Name.Text == existingNodeId.ToString());

            selectNode?.Invoke(_gvm, [toSelect]);

            _gvm.AddNodeOrEdgeCommand.Execute(_grid);

            Assert.Contains(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeId, newNodeId));
            Assert.Contains(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeId, newNodeId));

            Edge e = model.Graph.Edges.First(x => IsEdgeBetweenNodes(x, existingNodeId, newNodeId));
            EdgeView ev = model.EdgesToViews[e];

            Assert.Contains(model.EdgeViews, x => x == ev);

            _gvm.UndoLastActionCommand.Execute(_grid);

            Assert.Contains(model.Graph.Nodes, x => x.Id == newNodeId);
            Assert.Contains(model.NodeViews, x => x.Name.Text == newNodeId.ToString());
            Assert.DoesNotContain(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeId, newNodeId));
            Assert.DoesNotContain(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeId, newNodeId));
            Assert.DoesNotContain(model.EdgeViews, x => x == ev);

            _gvm.UndoLastActionCommand.Execute(_grid);

            Assert.DoesNotContain(model.Graph.Nodes, x => x.Id == newNodeId);
            Assert.DoesNotContain(model.NodeViews, x => x.Name.Text == newNodeId.ToString());
        }

        [StaFact]
        public void HistoryUndoRemoveTest()
        {
            var model = TestHelper.MakeTestModel();
            int originalCount = model.Graph.Nodes.Count;

            _gvm.SetModel(_grid, model);
            _gvm.AddNodeOrEdgeCommand.Execute(_grid);

            MethodInfo? selectNode = typeof(GraphViewModel).GetMethod("SelectNode", BindingFlags.NonPublic | BindingFlags.Instance);
            NodeView toSelect = model.NodeViews.First(x => x.Name.Text == existingNodeId.ToString());
            selectNode?.Invoke(_gvm, [toSelect]);

            _gvm.AddNodeOrEdgeCommand.Execute(_grid);

            MethodInfo? removeNode = typeof(GraphViewModel).GetMethod("RemoveNode", BindingFlags.NonPublic | BindingFlags.Instance);
            removeNode.Invoke(_gvm, [_grid, model.NodeViews.First(x => x.Name.Text == newNodeId.ToString())]);

            Assert.DoesNotContain(model.Graph.Nodes, x => x.Id == newNodeId);
            Assert.DoesNotContain(model.NodeViews, x => x.Name.Text == newNodeId.ToString());
            Assert.DoesNotContain(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeId, newNodeId));
            Assert.DoesNotContain(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeId, newNodeId));

            _gvm.UndoLastActionCommand.Execute(_grid);

            Assert.Contains(model.Graph.Nodes, x => x.Id == newNodeId);
            Assert.Contains(model.NodeViews, x => x.Name.Text == newNodeId.ToString());
            Assert.Contains(model.Graph.Edges, x => IsEdgeBetweenNodes(x, existingNodeId, newNodeId));
            Assert.Contains(model.EdgesToViews.Keys, x => IsEdgeBetweenNodes(x, existingNodeId, newNodeId));
        }

        private bool IsEdgeBetweenNodes(Edge x, int n1, int n2) => (x.First.Id == n1 && x.Second.Id == n2) || (x.First.Id == n2 && x.Second.Id == n1);
    }
}
