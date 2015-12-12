using Newtonsoft.Json;
using System;
using System.Net;
using Toggl.DataObjects;

namespace TC.easyJet.Reporting
{
    internal class DetailedReportService
    {
        private string apiKey;
        private int workspaceId;

        public DetailedReportService(string apiKey, int workspaceId)
        {
            this.apiKey = apiKey;
            this.workspaceId = workspaceId;
        }

        public DetailedReport Download(int clientId, DateTime since)
        {
            DetailedReport detailedReport;

            var url = String.Format("https://toggl.com/reports/api/v2/details?user_agent=andy_tcuk.com&workspace_id={0}&since=2015-01-01&client_ids={1}", workspaceId, clientId);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", Base64Encode(String.Concat(apiKey, ":api_token")));
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                Console.Write(json);

                detailedReport = JsonConvert.DeserializeObject<DetailedReport>(json);
            }
            return detailedReport;

        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}