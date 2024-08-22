namespace GraphEngine.Graph.Graphs.GraphCreator
{
    public class DirectedWeightenedGraphCreator : GraphCreator
    {
        public override GraphBase Create(GraphTypes type)
            => type == GraphTypes.DirectedWeightened
            ? new DirectedWeightenedGraph()
            : throw new ArgumentException();
    }
}
