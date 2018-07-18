using System.IO;
using System.Xml.Serialization;

namespace GridEditor.Serialization
{
    public class XMLSerializer 
    {
		public static void Serialize(object item, string path)
		{
			XmlSerializer serializer = new XmlSerializer(item.GetType());
			StreamWriter writer = new StreamWriter("Assets/" + path);
		
            serializer.Serialize(writer.BaseStream, item);
			writer.Close();
		}

		public static T Deserialize<T>(string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			StreamReader reader = new StreamReader("Assets/" + path);
			T deserialized = (T)serializer.Deserialize(reader.BaseStream);
			reader.Close();
			return deserialized;
		}
    }
}
