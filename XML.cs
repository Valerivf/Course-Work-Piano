using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Piano
{
    public static class XML
    {
        public static Stream Serialize<T>(T entity)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            Stream stream = new MemoryStream();

            serializer.Serialize(stream, entity);

            return stream;
        }

        public static T Deserialize<T>(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            T entity = (T)serializer.Deserialize(stream);

            return entity;
        }
    }
}
