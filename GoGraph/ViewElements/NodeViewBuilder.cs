using System.Windows;

namespace GoGraph.ViewElements
{
    public class NodeViewBuilder
    {
        private int _nodeOffset = ViewConstants.NodeDiameter / 2;
        private int _nameOffset = ViewConstants.NameBlockSide / 2;
        private Point _nodePosition;
        private string _nodeName;

        public NodeViewBuilder SetPosition(Point nodePosition)
        {
            _nodePosition = nodePosition;
            return this;
        }

        public NodeViewBuilder SetName(string nodeName)
        {
            _nodeName = nodeName;
            return this;
        }

        public NodeView Build()
            => new NodeView
            {
                Name = ViewElementsCreator.CreateNodeNameBlock(_nodeName, _nodePosition.X - _nameOffset, _nodePosition.Y - _nameOffset),
                Node = ViewElementsCreator.CreateNodeEllipse(_nodePosition.X - _nodeOffset, _nodePosition.Y - _nodeOffset)
            };    
    }
}