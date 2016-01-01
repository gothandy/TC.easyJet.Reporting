using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using Vincente.Toggl.DataObjects;

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
                var json = webClient.DownloadString(url);

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
