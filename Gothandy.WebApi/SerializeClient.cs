using Newtonsoft.Json;
using System.Net;

namespace Gothandy.WebApi
{
    public abstract class SerializeClient : JsonClient
    {
        public SerializeClient(string baseUrl) : base(baseUrl) { }
        public SerializeClient(string baseUrl, WebHeaderCollection headers) : base(baseUrl, headers) { }

        public string Post(string url, object obj)
        {
            string json = SerializeObject(obj);

            return UploadString(url, "POST", json);
        }

        public string Put(string url, object obj)
        {
            string json = SerializeObject(obj);

            return UploadString(url, "PUT", json);
        }

        protected static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(
                obj, new JsonSerializerSettings
                { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
