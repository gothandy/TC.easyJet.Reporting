using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using Vincente.Toggl.DataObjects;

namespace Vincente.Toggl
{
    public class Workspace
    {
        public string ApiKey { get; set; }
        public int WorkspaceId { get; set; }

        public Workspace(string apiKey, int workspaceId)
        {
            this.ApiKey = apiKey;
            this.WorkspaceId = workspaceId;
        }

        public Project GetProject(int toggleProjectTemplateId)
        {
            var url = String.Format(
                "https://www.toggl.com/api/v8/projects/{0}",
                toggleProjectTemplateId);

            return Get<ProjectWrapper>(url, true).Data;
        }

        public List<Project> GetProjects(int clientId)
        {
            var url = String.Format(
                "https://www.toggl.com/api/v8/clients/{0}/projects?active=both",
                clientId);

            return Get<List<Project>>(url, true);
        }

        public List<ReportTimeEntry> GetReportTimeEntries(int clientId, DateTime since, DateTime until)
        {
            List<ReportTimeEntry> list = new List<ReportTimeEntry>();

            var page = 1;
            while (true)
            {
                var detailedReport = this.DetailedReport(clientId, since, until, page);

                list.AddRange(detailedReport.Data);

                if (detailedReport.LastPage) break;

                page++;
            }

            return list;
        }

        private DetailedReport DetailedReport(int clientId, DateTime since, DateTime until, int page)
        {
            var url = String.Format(
                "{0}?user_agent={1}&workspace_id={2}&client_ids={3}&since={4:yyyy-MM-dd}&until={5:yyyy-MM-dd}&page={6}&order_field=date&rounding=off",
                "https://toggl.com/reports/api/v2/details",
                "andy@tcuk.com",
                this.WorkspaceId, clientId, since, until, page);

            var detailedReport = Get<DetailedReport>(url, false);

            detailedReport.LastPage = (detailedReport.PerPage * page) > detailedReport.TotalCount;

            return detailedReport;
        }

        private T Get<T>(string url, bool basic)
        {
            T response;

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", GetAuthorization(basic));
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                response = JsonConvert.DeserializeObject<T>(json);
            }

            return response;
        }

        private string GetAuthorization(bool basic)
        {
            var authorization = String.Concat(this.ApiKey, ":api_token");
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