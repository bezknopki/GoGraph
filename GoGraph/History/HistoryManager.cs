using GoGraph.Model;
using GoGraph.ViewElements;
using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Nodes;
using System.Windows;
using System.Windows.Shapes;

namespace GoGraph.History
{
    public static class HistoryManager
    {
        private static Stack<HistoryElement> _history = new Stack<HistoryElement>();

        public static GraphModel Model { get; set; }

        public static void Merge(ActionType type, int count)
        {
            HistoryElement he = new HistoryElement();
            he.ActionType = type;

            for (int i = 0; i < count; i++)
                he.MergeWith(_history.Pop());

            _history.Push(he);
        }

        public static void Push(ActionType type, params object[] objects)
        {
            HistoryElement he = new HistoryElement();
            he.ActionType = type;

            foreach (object obj in objects)
            {
                if (obj is UIElement elem)
                    he.Elements.Add(elem);

                if (obj is Edge edge)
                    he.Edges.Add(edge);

                if (obj is EdgeView ev)
                    he.EdgeViews.Add(ev);

                if (obj is Node node)
                    he.Nodes.Add(node);

                if (obj is NodeView nv)
                    he.NodeViews.Add(nv);

                if (obj is List<Polyline> arrows)
                    foreach (var arr in arrows)
                        he.Elements.Add(arr);
            }

            _history.Push(he);
        }

        public static (ActionType, List<UIElement>) Undo()
        {
            HistoryElement historyElement = _history.Pop();

            if (historyElement.ActionType == ActionType.Add)
                UndoAdd(historyElement);
            else
                UndoRemove(historyElement);

            return (historyElement.ActionType, historyElement.Elements);
        }

        private static void UndoRemove(HistoryElement historyElement)
        {
            foreach (var e in historyElement.Edges)
            {
                Model.Graph.Edges.Add(e);
                Model.EdgesToViews.Add(e, historyElement.EdgeViews[historyElement.Edges.IndexOf(e)]);
                switch (e.Direction)
                {
                    case Direction.FirstToSecond:
                        {
                            e.First.Next.Add(e.Second, e);
                            break;
                        }
                    case Direction.SecondToFirst:
                        {
                            e.Second.Next.Add(e.First, e);
                            break;
                        }
                    default:
                        {
                            e.First.Next.Add(e.Second, e);
                            e.Second.Next.Add(e.First, e);
                            break;
                        }
                }
            }

            foreach (var el in historyElement.EdgeViews)
                Model.EdgeViews.Add(el);

            foreach (var el in historyElement.Nodes)        
                Model.Graph.Nodes.Add(el);         

            foreach (var el in historyElement.NodeViews)
                Model.NodeViews.Add(el);
        }

        private static void UndoAdd(HistoryElement historyElement)
        {
            foreach (var el in historyElement.Edges)
            {
                Model.Graph.Edges.Remove(el);
                Model.EdgesToViews.Remove(el);
            }

            foreach (var el in historyElement.EdgeViews)
                Model.EdgeViews.Remove(el);

            foreach (var el in historyElement.Nodes)
            {
                Model.Graph.Nodes.Remove(el);
                foreach (var node in Model.Graph.Nodes)
                    if (node.Next.ContainsKey(el))
                        node.Next.Remove(node);
            }

            foreach (var el in historyElement.NodeViews)
                Model.NodeViews.Remove(el);
        }
    }
}
