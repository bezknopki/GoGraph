using GoGraph.Model;
using GoGraph.Serializer;
using GraphEngine.Algorithms.MinimalSpannedTree;
using GraphEngine.Graph.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Assert.Contains(result, x => (x.First.Name == "1" && x.Second.Name == "2") || (x.First.Name == "2" && x.Second.Name == "1"));
            Assert.Contains(result, x => (x.First.Name == "1" && x.Second.Name == "4") || (x.First.Name == "4" && x.Second.Name == "1"));
            Assert.Contains(result, x => (x.First.Name == "2" && x.Second.Name == "5") || (x.First.Name == "5" && x.Second.Name == "2"));
            Assert.Contains(result, x => (x.First.Name == "4" && x.Second.Name == "6") || (x.First.Name == "6" && x.Second.Name == "4"));
            Assert.Contains(result, x => (x.First.Name == "5" && x.Second.Name == "7") || (x.First.Name == "7" && x.Second.Name == "5"));
            Assert.Contains(result, x => (x.First.Name == "3" && x.Second.Name == "5") || (x.First.Name == "5" && x.Second.Name == "3"));
        }
    }
}
