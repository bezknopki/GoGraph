﻿namespace GoGraph.Tools
{
    public static class NodeNameSequence
    {
        private static int _current = 1;
        public static int Next { get { return _current++; } }

        public static void SetStart(int start) => _current = ++start;
    }
}
