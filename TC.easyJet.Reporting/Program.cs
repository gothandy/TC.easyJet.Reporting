using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
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
            var since = new DateTime(2015, 1, 1);

            var detailedReportService = new DetailedReportService(apiKey, workspaceId);

            var detailedReport = detailedReportService.Download(clientId, since);

            foreach (ReportTimeEntry timeEntry in detailedReport.Data)
            {
                Console.WriteLine("{0},{1},{2}", timeEntry.ProjectName, timeEntry.UserName, timeEntry.TaskName);
            }
        }
    }
}
