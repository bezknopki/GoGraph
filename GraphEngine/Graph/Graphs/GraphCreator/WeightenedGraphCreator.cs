﻿namespace GraphEngine.Graph.Graphs.GraphCreator
{
    public class WeightenedGraphCreator : GraphCreator
    {
        public WeightenedGraphCreator()
        {
            Susccessor = new DirectedWeightenedGraphCreator();
        }

        public override GraphBase Create(GraphTypes type)
            => type == GraphTypes.Weightened
            ? new WeightenedGraph()
            : Susccessor.Create(type);
    }
}
