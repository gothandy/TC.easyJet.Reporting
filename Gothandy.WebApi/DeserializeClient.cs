using Newtonsoft.Json;
using System.Net;

namespace Gothandy.WebApi
{
    public abstract class DeserializeClient : SerializeClient
    {
        public DeserializeClient(string baseUrl) : base(baseUrl) { }
        public DeserializeClient(string baseUrl, WebHeaderCollection headers) : base(baseUrl, headers) { }

        public T Get<T>(string url)
        {
            var json = DownloadString(url);

            return DeserializeObject<T>(json);
        }

        public T Post<T>(string url, object obj)
        {
            var json = SerializeObject(obj);
            var response = UploadString(url, "POST", json);

            return DeserializeObject<T>(response);
        }

        public T Put<T>(string url, object obj)
        {
            var json = SerializeObject(obj);
            var response = UploadString(url, "PUT", json);

            return DeserializeObject<T>(response);
        }

        public T Delete<T>(string url)
        {
            var response = UploadString(url, "DELETE", string.Empty);

            return DeserializeObject<T>(response);
        }

        private static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
