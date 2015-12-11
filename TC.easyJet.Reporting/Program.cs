using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Toggl.DataObjects;

namespace TC.easyJet.Reporting
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = "242d8528ee1e461cdda80cb6eb175967";
            var workspaceId = 605632;
            var clientId = 15242883;

            var url = String.Format("https://toggl.com/reports/api/v2/details?user_agent=andy_tcuk.com&workspace_id={0}&since=2015-01-01&client_ids={1}", workspaceId, clientId);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", Base64Encode(String.Concat(apiKey, ":api_token")));
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                Console.Write(json);

                var detailed = JsonConvert.DeserializeObject<DetailedReport>(json);

                foreach(ReportTimeEntry timeEntry in detailed.Data)
                {
                    Console.WriteLine("{0},{1},{2}", timeEntry.ProjectName, timeEntry.UserName, timeEntry.TaskName);
                }

            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
