using GraphEngine.Graph.Nodes;

namespace GraphEngine.Graph.Edges
{
    public class EdgeBuilder
    {
        private Node? _first;
        private Node? _second;
        private bool _isWeightened = false;
        private bool _isDirected = false;
        private Direction _direction;
        private double _weight;

        public EdgeBuilder SetNodes(Node first, Node second)
        {
            _first = first;
            _second = second;
            return this;
        }

        public EdgeBuilder SetWeight(double weight)
        {
            _weight = weight;
            _isWeightened = true;
            return this;
        }

        public EdgeBuilder SetDirection(Direction direction)
        {
            _direction = direction;
            _isDirected = true;
            return this;
        }

        public Edge Build()
            => new Edge
            {
                IsDirected = _isDirected,
                IsWeightened = _isWeightened,
                Weight = _weight,
                Direction = _direction,
                First = _first,
                Second = _second
            };
    }
}
