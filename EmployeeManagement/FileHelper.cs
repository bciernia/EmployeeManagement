using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmployeeManagement
{
    public class FileHelper<T> where T : new()
    {
        private string _filePath;
    
        public FileHelper(string filePath)
        {
            _filePath = filePath;
        }

        public void SerializeToFile(T employees)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var streamWriter = new StreamWriter(_filePath))
            {
                serializer.Serialize(streamWriter, employees);
                streamWriter.Close();
            }
        }

        public T DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
                return new T();

            var serializer = new XmlSerializer(typeof(T));

            using (var streamReader = new StreamReader(_filePath))
            {
                var employees = (T)serializer.Deserialize(streamReader);
                streamReader.Close();
                return employees;
            }
        }
  /*    SERIALIZACJA I DESERIALIZACJA W FORMACIE JSON -----------------------------TODO
        public void SerializeToFile(T employees)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            if (File.Exists(_filePath))
                File.Delete(_filePath);

            StreamWriter sw = new StreamWriter(_filePath);
            JsonWriter jsonWriter = new JsonTextWriter(sw);

            jsonSerializer.Serialize(jsonWriter, employees);

            jsonWriter.Close();
            sw.Close();
        }

        public List<T> DeserializeFromFile()
        {
            JObject obj = null;
            JsonSerializer jsonSerializer = new JsonSerializer();

            if (File.Exists(_filePath))
            {
                StreamReader sr = new StreamReader(_filePath);
                JsonReader jsonReader = new JsonTextReader(sr);
                obj = jsonSerializer.Deserialize(jsonReader) as JObject;
                jsonReader.Close();
                sr.Close();
            }
        }
    */
    }
}
