using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Vincente.Toggl.Tables
{
    public class BaseTable
    {
        protected Workspace workspace;

        public BaseTable(Workspace workspace)
        {
            this.workspace = workspace;
        }

        protected T Get<T>(string url, bool basic)
        {
            T response = default(T);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", GetAuthorization(basic));
                webClient.Headers.Add("Content-Type", "application/json");

                var json = "";

                try
                {
                    json = webClient.DownloadString(url);
                }
                catch (WebException webException)
                {
                    if (webException.Response == null) throw webException;
                    var message = new StreamReader(webException.Response.GetResponseStream()).ReadToEnd();
                    throw new Exception(message, webException);
                }

                if (json != "null")
                {
                    response = JsonConvert.DeserializeObject<T>(json);
                }
            }

            return response;
        }

        protected T Post<T>(string url, object obj)
        {
            string json = JsonConvert.SerializeObject(
                obj, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            T response;

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", GetAuthorization(true));
                webClient.Headers.Add("Content-Type", "application/json");
                var responseJson = webClient.UploadString(url, json);

                response = JsonConvert.DeserializeObject<T>(responseJson);
            }

            return response;
        }

        private string GetAuthorization(bool basic)
        {
            var authorization = String.Concat(workspace.ApiKey, ":api_token");
            authorization = Base64Encode(authorization);
            if (basic) authorization = string.Concat("Basic ", authorization);
            return authorization;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
