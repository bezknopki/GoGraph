using GoGraph.Graph.Graphs;

namespace GoGraph.Graph.Graphs.GraphCreator
{
    public class SimpleGraphCreator : GraphCreator
    {
        public SimpleGraphCreator()
        {
            Susccessor = new DirectedGraphCreator();
        }

        public override GraphBase Create(GraphTypes type)
            => type == GraphTypes.Simple
            ? new SimpleGraph()
            : Susccessor.Create(type);
    }
}
