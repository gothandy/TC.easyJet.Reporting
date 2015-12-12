using System;
using Toggl.DataObjects;
using Toggl;

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

            for(var page=1; page<3; page++)
            {
                var detailedReport = detailedReportService.Download(clientId, since, page);
            
                foreach (ReportTimeEntry timeEntry in detailedReport.Data)
                {
                    //Console.WriteLine("{0},{1},{2}", timeEntry.ProjectName, timeEntry.UserName, timeEntry.TaskName);
                }

                Console.WriteLine("{0} {1} {2}", page, detailedReport.TotalCount, detailedReport.PerPage);
            }

        }
    }
}
