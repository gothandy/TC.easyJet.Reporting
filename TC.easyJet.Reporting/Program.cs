using System;
using Toggl.DataObjects;
using Toggl;
using System.Collections.Generic;

namespace TC.easyJet.Reporting
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = "242d8528ee1e461cdda80cb6eb175967";
            var workspaceId = 605632;
            var clientId = 15242883;
            var since = new DateTime(2015, 12, 1);
            var until = new DateTime(2015, 12, 31);
            
            var workspace = new Workspace(apiKey, workspaceId);

            List<ReportTimeEntry> reportTimeEntries = workspace.GetReportTimeEntries(clientId, since, until);

            var csv = new System.IO.StreamWriter(@"C:\Users\Andrew Davies\Desktop\test.csv");

            
            foreach (ReportTimeEntry timeEntry in reportTimeEntries)
            {
                csv.WriteLine("{0},{1},{2},{3},{4}",
                    timeEntry.Start,
                    timeEntry.UserName,
                    timeEntry.TaskName,
                    timeEntry.Duration,
                    timeEntry.Billable
                    );
            }

            csv.Close();
        }
    }
}
