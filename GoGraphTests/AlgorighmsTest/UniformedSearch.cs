using GoGraph.Model;
using GoGraph.Serializer;
using GraphEngine.GraphMath.UninformedSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGraphTests.AlgorighmsTest
{
    public class UniformedSearch
    {
        [StaFact]
        public void BFSTest()
        {
            string path = Path.Combine(TestHelper.ProjectsDirectory, "bfsWikiTest.xml");
            GraphModel model = ProjectSerializer.DeserializeXML(path);
            BreadthFirstSearch bfs = new BreadthFirstSearch();
            var result = bfs.Start(model.Graph.Nodes.First(x => x.Id == 1)).Result;
            Assert.Equal("12345678910", string.Join(string.Empty, result.Select(x => x.Id)));
        }

        [StaFact]
        public void DFSTest()
        {
            string path = Path.Combine(TestHelper.ProjectsDirectory, "dfsWikiTest.xml");
            GraphModel model = ProjectSerializer.DeserializeXML(path);
            DepthFirstSearch dfs = new DepthFirstSearch();
            var result = dfs.Start(model.Graph.Nodes.First(x => x.Id == 1)).Result;
            Assert.Equal("123456789101112", string.Join(string.Empty, result.Select(x => x.Id)));
        }
    }
}
