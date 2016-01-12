using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Gothandy.Toggl.Tables
{
    public interface IHttpMethod
    {
        string Execute(string url, string request);
    }

    public abstract class BaseHttpMethod
    {
        
        protected WebClient webClient;

        public BaseHttpMethod(WebClient webClient)
        {
            this.webClient = webClient;
        }
    }

    public class HttpGet : BaseHttpMethod, IHttpMethod
    {
        public HttpGet(WebClient webClient) : base(webClient) { }

        public string Execute(string url, string request)
        {
            return webClient.DownloadString(url);
        }
    }

    public class HttpPut : BaseHttpMethod, IHttpMethod
    {
        public HttpPut(WebClient webClient) : base(webClient) { }

        public string Execute(string url, string request)
        {
            return webClient.UploadString(url, "PUT", request);
        }
    }

    public class HttpPost : BaseHttpMethod, IHttpMethod
    {
        public HttpPost(WebClient webClient) : base(webClient) { }

        public string Execute(string url, string request)
        {
            return webClient.UploadString(url, "POST", request);
        }
    }

    public class HttpDelete : BaseHttpMethod, IHttpMethod
    {
        public HttpDelete(WebClient webClient) : base(webClient) { }

        public string Execute(string url, string request)
        {
            return webClient.UploadString(url, "DELETE", string.Empty);
        }
    }

    public class BaseTable
    {
        protected Workspace workspace;

        public BaseTable(Workspace workspace)
        {
            this.workspace = workspace;
        }

        protected T Get<T>(string url, bool basic)
        {
            T response;

            using (var webClient = new WebClient())
            {
                var method = new HttpGet(webClient);

                SetHeaders(webClient, GetAuthorization(workspace.ApiKey, basic));

                response = HttpExecute<T>(method, url, null);
            }

            return response;
        }

        protected T Post<T>(string url, object obj)
        {
            string json = SerializeObjectIgnoreNulls(obj);

            T response;

            using (var webClient = new WebClient())
            {
                var method = new HttpPost(webClient);

                SetHeaders(webClient, GetAuthorization(workspace.ApiKey, true));

                response = HttpExecute<T>(method, url, json);
            }

            return response;
        }



        protected T Put<T>(string url, object obj)
        {
            string json = SerializeObjectIgnoreNulls(obj);

            T response;

            using (var webClient = new WebClient())
            {
                var method = new HttpPut(webClient);

                SetHeaders(webClient, GetAuthorization(workspace.ApiKey, true));

                response = HttpExecute<T>(method, url, json);
            }

            return response;
        }

        protected T Delete<T>(string url)
        {
            T response;

            using (var webClient = new WebClient())
            {
                var method = new HttpDelete(webClient);

                SetHeaders(webClient, GetAuthorization(workspace.ApiKey, true));

                response = HttpExecute<T>(method, url, null);
            }

            return response;
        }

        private T HttpExecute<T>(IHttpMethod method, string url, string request)
        {
            T response = default(T);

            string json;

            try
            {
                json = method.Execute(url, request);
            }
            catch (WebException webException)
            {
                if (webException.Response == null) throw webException;
                var message = new StreamReader(webException.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(message, webException);
            }

            if (json != "null") response = JsonConvert.DeserializeObject<T>(json);

            return response;
        }

        private void SetHeaders(WebClient webClient, string authorization)
        {
            webClient.Headers.Add("Authorization", authorization);
            webClient.Headers.Add("Content-Type", "application/json");
        }

        private string GetAuthorization(string apiKey, bool basic)
        {
            var authorization = String.Concat(apiKey, ":api_token");
            authorization = Base64Encode(authorization);
            if (basic) authorization = string.Concat("Basic ", authorization);
            return authorization;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static string SerializeObjectIgnoreNulls(object obj)
        {
            return JsonConvert.SerializeObject(
                obj, new JsonSerializerSettings
                { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
