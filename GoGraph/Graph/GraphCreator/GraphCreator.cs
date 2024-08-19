using GoGraph.Graph.Graphs;

namespace GoGraph.Graph.GraphCreator
{
    public abstract class GraphCreator
    {
        public abstract GraphBase Create(GraphTypes type);

        protected GraphCreator Susccessor { get; set; }
    }
}
