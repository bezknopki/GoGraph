using GoGraph.Graph.Graphs;

namespace GoGraph.Graph.GraphCreator
{
    public class DirectedGraphCreator : GraphCreator
    {
        public DirectedGraphCreator()
        {
            Susccessor = new WeightenedGraphCreator();
        }

        public override GraphBase Create(GraphTypes type)
            => type == GraphTypes.Directed
            ? new DirectedGraph()
            : Susccessor.Create(type);
    }
}
