using GoGraph.Model;
using GoGraph.Serializer;
using GraphEngine.GraphMath.MinimalSpannedTree;
using GraphEngine.Graph.Edges;

namespace GoGraphTests.AlgorighmsTest
{
    public class MinimalSpannedTreeTest
    {
        [StaFact]
        public void PrimTest()
        {
            string path = Path.Combine(TestHelper.ProjectsDirectory, "primWikiTest.xml");
            GraphModel model = ProjectSerializer.DeserializeXML(path);

            Prim prim = new Prim();
            List<Edge> result = prim.Start(model.Graph.Nodes.First(), model.Graph.Nodes.Count);

            Assert.Equal(model.Graph.Nodes.Count - 1, result.Count);
            Assert.Contains(result, x => x.IsBetweenNodes(1, 2));
            Assert.Contains(result, x => x.IsBetweenNodes(1, 4));
            Assert.Contains(result, x => x.IsBetweenNodes(2, 5));
            Assert.Contains(result, x => x.IsBetweenNodes(4, 6));
            Assert.Contains(result, x => x.IsBetweenNodes(5, 7));
            Assert.Contains(result, x => x.IsBetweenNodes(3, 5));
        }
    }
}
