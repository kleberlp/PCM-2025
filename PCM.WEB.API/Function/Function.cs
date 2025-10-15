using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PCM.WEBAPI.Class
{
    public class Function
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
    }
}