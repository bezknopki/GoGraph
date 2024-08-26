using GraphEngine.Graph.Graphs;
using GraphEngine.Graph.Graphs.GraphCreator;

namespace GoGraphTests.GraphTests
{
    public class GraphCreateTests
    {
        SimpleGraphCreator creator = new SimpleGraphCreator();

        [Fact]
        public void CreateSimpleGraphTest()
        {
            GraphBase graph = creator.Create(GraphTypes.Simple);

            Assert.NotNull(graph);
            Assert.False(graph.IsWeightened);
            Assert.False(graph.IsDirected);
        }

        [Fact]
        public void CreateDirectedGraphTest() 
        {
            GraphBase graph = creator.Create(GraphTypes.Directed);

            Assert.NotNull(graph);
            Assert.False(graph.IsWeightened);
            Assert.True(graph.IsDirected);
        }

        [Fact]
        public void CreateDirectedWeightenedGraphTest()
        {
            GraphBase graph = creator.Create(GraphTypes.DirectedWeightened);

            Assert.NotNull(graph);
            Assert.True(graph.IsWeightened);
            Assert.True(graph.IsDirected);
        }

        [Fact]
        public void CreateWeightenedGraphTest()
        {
            GraphBase graph = creator.Create(GraphTypes.Weightened);

            Assert.NotNull(graph);
            Assert.True(graph.IsWeightened);
            Assert.False(graph.IsDirected);
        }

        [Fact]
        public void CreateWebGraphTest()
        {
            GraphBase graph = creator.Create(GraphTypes.Web);

            Assert.NotNull(graph);
            Assert.True(graph.IsWeightened);
            Assert.False(graph.IsDirected);
        }
    }
}
