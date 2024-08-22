using System;
namespace GraphEngine.Graph.Graphs.GraphCreator
{
    internal class WebGraphCreator : GraphCreator
    {
        public override GraphBase Create(GraphTypes type)
            => type == GraphTypes.Web
            ? new WebGraph()
            : throw new ArgumentException();
    }
}
