using GoGraph.Graph.Graphs;

namespace GoGraph.Graph.GraphCreator
{
    public class DirectedWeightenedGraphCreator : GraphCreator
    {
        public override GraphBase Create(GraphTypes type)
            => type == GraphTypes.DirectedWeightened
            ? new SimpleGraph()
            : throw new ArgumentException();
    }
}
