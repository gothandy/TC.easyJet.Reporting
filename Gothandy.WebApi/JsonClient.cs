using Newtonsoft.Json;
using System.Net;

namespace Gothandy.WebApi
{
    public abstract class JsonClient
    {
        protected string baseUrl;
        protected WebHeaderCollection headers;

        public JsonClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
            this.headers = null;
        }

        public JsonClient(string baseUrl, WebHeaderCollection headers)
        {
            this.baseUrl = baseUrl;
            this.headers = headers;
        }

        public string Get(string url)
        {
            return DownloadString(url);
        }

        public string Post(string url, string json)
        {
            return UploadString(url, "POST", json);
        }

        public string Put(string url, string json)
        {
            return UploadString(url, "PUT", json);
        }

        public string Delete(string url)
        {
            return UploadString(url, "DELETE", string.Empty);
        }

        protected string DownloadString(string url)
        {
            string response;

            using (var webClient = CreateWebClient())
            {
                response = webClient.DownloadString(BuildUrl(url));
            }

            return response;
        }

        protected string UploadString(string url, string method, string json)
        {
            string response;

            using (var webClient = CreateWebClient())
            {
                response = webClient.UploadString(BuildUrl(url), method, json);
            }

            return response;
        }

        private WebClient CreateWebClient()
        {
            var client = new WebClient();

            AddHeaders(client);

            return client;
        }

        private void AddHeaders(WebClient client)
        {
            if (headers == null) return;

            foreach (string key in headers.AllKeys)
            {
                client.Headers.Add(key, headers.Get(key));
            }
        }

        private string BuildUrl(string url)
        {
            return string.Concat(baseUrl, url);
        }
    }
}
