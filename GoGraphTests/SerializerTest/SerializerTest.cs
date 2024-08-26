using GoGraph.Model;
using GoGraph.Serializer;
using GoGraph.ViewElements;
using GraphEngine.Graph.Edges;
using GraphEngine.Graph.Graphs;
using GraphEngine.Graph.Graphs.GraphCreator;
using GraphEngine.Graph.Nodes;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace GoGraphTests.SerializerTest
{
    public class SerializerTest
    {
        [StaFact]
        public void SerializeTest()
        {
            string path = Path.Combine("..\\..\\..\\", "TestProjects", "serTest.xml");
            GraphModel model = TestHelper.MakeTestModel();
            ProjectSerializer.SerializeXML(new SerializebleGraphModel(model), path);

            GraphModel deserializedModel = ProjectSerializer.DeserializeXML(path);

            Assert.NotNull(deserializedModel);

            for (int i = 0; i < model.Graph.Nodes.Count; i++)
            {
                Node orig = model.Graph.Nodes[i];
                Node deser = deserializedModel.Graph.Nodes[i];

                Assert.NotNull(deser);
                Assert.Equal(orig.Name, deser.Name);

                foreach (var next in deser.Next.Keys)
                    Assert.Contains(orig.Next.Keys, x => x.Name == next.Name);
            }

            for (int i = 0; i < model.Graph.Edges.Count; i++)
            {
                Edge orig = model.Graph.Edges[i];
                Edge deser = deserializedModel.Graph.Edges[i];

                Assert.NotNull(deser);
                Assert.Equal(orig.Weight, deser.Weight);
                Assert.Equal(orig.IsWeightened, deser.IsWeightened);
                Assert.Equal(orig.First.Name, deser.First.Name);
                Assert.Equal(orig.Second.Name, deser.Second.Name);
                Assert.Equal(orig.Direction, deser.Direction);
                Assert.Equal(orig.IsDirected, deser.IsDirected);
            }

            for (int i = 0; i < model.EdgeViews.Count; i++)
            {
                EdgeView orig = model.EdgeViews[i];
                EdgeView deser = deserializedModel.EdgeViews[i];

                Assert.NotNull(deser);
                Assert.Equal(orig.Edge.X1, deser.Edge.X1);
                Assert.Equal(orig.Edge.X2, deser.Edge.X2);
                Assert.Equal(orig.Edge.Y1, deser.Edge.Y1);
                Assert.Equal(orig.Edge.Y2, deser.Edge.Y2);

                for (int j = 0; j < orig.Arrows.Count; j++)
                    for (int v = 0; v < orig.Arrows[j].Points.Count; v++)
                        Assert.Equal(orig.Arrows[j].Points[v], deser.Arrows[j].Points[v]);

                Assert.Equal(((TextBlock)orig.Weight.Child).Text, ((TextBlock)deser.Weight.Child).Text);
                Assert.Equal(orig.Weight.Margin, deser.Weight.Margin);
            }

            for (int i = 0; i < model.NodeViews.Count; i++)
            {
                NodeView orig = model.NodeViews[i];
                NodeView deser = deserializedModel.NodeViews[i];

                Assert.NotNull(deser);
                Assert.Equal(orig.Name.Text, deser.Name.Text);
                Assert.Equal(orig.Name.Margin, deser.Name.Margin);
                Assert.Equal(orig.Node.Margin, deser.Node.Margin);
                Assert.Equal(orig.IsSelected, deser.IsSelected);
            }
        }
    }
}