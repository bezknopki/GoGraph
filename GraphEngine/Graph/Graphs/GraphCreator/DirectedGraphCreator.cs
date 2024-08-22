namespace GraphEngine.Graph.Graphs.GraphCreator
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
