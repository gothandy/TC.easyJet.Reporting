using Gothandy.WebApi;
using System.Net;

namespace Gothandy.Toggl
{
    public class Workspace
    {
        public WebApiClient Client { get; set; }
        public string ApiKey { get; set; }
        public int WorkspaceId { get; set; }

        public Workspace(string apiKey, int workspaceId)
        {
            this.ApiKey = apiKey;
            this.WorkspaceId = workspaceId;
            
            WebHeaderCollection headers = new WebHeaderCollection();

            headers.Add("Authorization", GetAuthorization(apiKey, true));
            headers.Add("Content-Type", "application/json");

            this.Client = new WebApiClient("https://www.toggl.com/api/v8", headers);
        }

        private string GetAuthorization(string apiKey, bool basic)
        {
            var authorization = string.Concat(apiKey, ":api_token");
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