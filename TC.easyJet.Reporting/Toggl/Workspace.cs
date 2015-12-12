using Newtonsoft.Json;
using System;
using System.Net;
using Toggl.DataObjects;

namespace Toggl
{
    internal class Workspace
    {
        private string apiKey;
        private int workspaceId;

        public Workspace(string apiKey, int workspaceId)
        {
            this.apiKey = apiKey;
            this.workspaceId = workspaceId;
        }

        public DetailedReport DetailedReport(int clientId, DateTime since, DateTime until, int page)
        {
            DetailedReport detailedReport;

            var url = String.Format(
                "{0}?user_agent={1}&workspace_id={2}&client_ids={3}&since={4:yyyy-MM-dd}&until={5:yyyy-MM-dd}&page={6}",
                "https://toggl.com/reports/api/v2/details",
                "andy@tcuk.com",
                workspaceId, clientId, since, until, page);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", Base64Encode(String.Concat(apiKey, ":api_token")));
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                detailedReport = JsonConvert.DeserializeObject<DetailedReport>(json);
            }
            detailedReport.LastPage = (detailedReport.PerPage * page) > detailedReport.TotalCount;
            return detailedReport;

        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}