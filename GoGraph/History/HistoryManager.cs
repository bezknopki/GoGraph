using GoGraph.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GoGraph.History
{
    public static class HistoryManager
    {
        private static Stack<HistoryElement> _history = new Stack<HistoryElement>();

        public static GraphModel Model { get; set; }

        public static void Push(HistoryElement historyElement)
        {
            _history.Push(historyElement);
        }

        public static List<UIElement> Undo()
        {
            HistoryElement historyElement = _history.Pop();
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

            return historyElement.Elements;
        }
    }
}
