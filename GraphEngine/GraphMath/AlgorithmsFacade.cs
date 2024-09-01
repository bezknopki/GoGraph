using GraphEngine.GraphMath.InformedSearch;
using GraphEngine.GraphMath.MinimalSpannedTree;
using GraphEngine.GraphMath.ShortestWay;
using GraphEngine.GraphMath.UninformedSearch;
using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Graphs;
using GraphEngine.Graph.Nodes;

namespace GraphEngine.GraphMath
{
    public class AlgorithmsFacade
    {
        public Way FindShortestWayDijkstra(GraphBase graph, Node root, Node goal)
        {
            Dijkstra dijkstra = new Dijkstra();
            return dijkstra.FindShortestWay(graph, root, goal);
        }

        public List<Edge> MSTPrim(Node root)
        {
            Prim prim = new Prim();
            BreadthFirstSearch bfs = new BreadthFirstSearch();
            int count = bfs.Start(root).Result.Count;
            return prim.Start(root, count);
        }

        public async Task<LinkedList<Node>> BreadthFirstSearch(Node root)
        {
            BreadthFirstSearch bfs = new BreadthFirstSearch();
            return await bfs.Start(root);
        }

        public async Task<LinkedList<Node>> DepthFirstSearch(Node root)
        {
            DepthFirstSearch dfs = new DepthFirstSearch();
            return await dfs.Start(root);
        }

        public LinkedList<WebNode> AStar(WebNode root, WebNode goal)
        {
            AStar a = new AStar();
            return a.Start(root, goal);
        }
    }
}
