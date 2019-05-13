using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piano
{
    public static class Loader
    {
        public static T Load<T>(string file)
        {
            if (File.Exists(file))
            {
                MemoryStream stream = new MemoryStream(File.ReadAllBytes(file));

                return XML.Deserialize<T>(stream);
            }
            else
            {
                // Создаём новый объект типа 'T'
                return Activator.CreateInstance<T>();
            }
        }

        public static void Save<T>(string file, T entity)
        {
            MemoryStream stream = (MemoryStream)XML.Serialize(entity);

            String text = Encoding.UTF8.GetString(stream.ToArray());

            File.WriteAllText(file, text, Encoding.UTF8);
        }
    }
}
