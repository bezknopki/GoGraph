namespace GraphEngine.Graph.Graphs.GraphCreator
{
    public class DirectedWeightenedGraphCreator : GraphCreator
    {
        public DirectedWeightenedGraphCreator()
        {
            Susccessor = new WebGraphCreator();
        }
        public override GraphBase Create(GraphTypes type)
            => type == GraphTypes.DirectedWeightened
            ? new DirectedWeightenedGraph()
            : Susccessor.Create(type);
    }
}
