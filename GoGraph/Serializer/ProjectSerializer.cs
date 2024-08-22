using GoGraph.Model;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;

namespace GoGraph.Serializer
{
    public static class ProjectSerializer
    {
        private static XmlSerializer _serializer = new XmlSerializer(typeof(SerializebleGraphModel));
        private static string _filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";

        public static string SerializeXML(SerializebleGraphModel model)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = _filter;
            sfd.DefaultExt = "xml";
            string path = string.Empty;

            if (sfd.ShowDialog() == true)
            {
                path = sfd.FileName;
                SerializeXML(model, path);
            }

            return path;
        }

        public static void SerializeXML(SerializebleGraphModel model, string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            using FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            _serializer.Serialize(fs, model);
        }

        public static (string, GraphModel?) DeserializeXML()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = _filter;
            SerializebleGraphModel? model = null;
            string path = string.Empty;

            if (ofd.ShowDialog() == true)
            {
                path = ofd.FileName;
                using (Stream reader = new FileStream(path, FileMode.Open))
                    try
                    {
                        model = (SerializebleGraphModel)_serializer.Deserialize(reader);
                    }
                    catch { }
            }

            return (path, model?.ToGraphModel());
        }
    }
}
