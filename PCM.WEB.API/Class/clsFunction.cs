using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PCM.WEB.API.Class
{
    public class clsFunction
    {

        public string ConverteObjectParaJSon<T>(T obj)
        {
            try
            {
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream memoryStream = new MemoryStream();
                dataContractJsonSerializer.WriteObject(memoryStream, obj);
                string jsonString = Encoding.UTF8.GetString(memoryStream.ToArray());
                memoryStream.Close();
                return jsonString;
            }
            catch
            {
                throw;
            }
        }

        public T ConverteJSonParaObject<T>(string jsonString)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T obj = (T)serializer.ReadObject(ms);
                return obj;
            }
            catch
            {
                throw;
            }
        }

    }
}