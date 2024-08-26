using GoGraph.Serializer;
using GoGraph.Model;
using GraphEngine.Algorithms.ShortestWay;
using GraphEngine.Graph.Nodes;

namespace GoGraphTests.AlgorighmsTest
{
    public class ShoretestWayTest
    {
        [StaFact]
        public void DijkstraTest()
        {
            string path = Path.Combine(TestHelper.ProjectsDirectory, "dijkstraWikiTest.xml");
            GraphModel model = ProjectSerializer.DeserializeXML(path);
            Dijkstra dijkstra = new Dijkstra();

            Node from = model.Graph.Nodes.First(x => x.Name == "1");
            Node to = model.Graph.Nodes.First(x => x.Name == "5");

            Way way = dijkstra.FindShortestWay(model.Graph, from, to);

            Assert.NotNull(way);
            Assert.Equal("1 -> 3 -> 6 -> 5 Length: 20", way.ToString());
        }
    }
}
