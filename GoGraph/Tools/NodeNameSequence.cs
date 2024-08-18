namespace GoGraph.Tools
{
    public static class NodeNameSequence
    {
        private static int _current;
        public static int Next { get { return _current++; } }
    }
}
