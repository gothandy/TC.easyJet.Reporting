using Azure.Tables;
using System;
using System.Collections.Generic;
using Toggl;
using Toggl.DataObjects;

namespace TC.easyJet.Reporting
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureAccountKey = args[0];
            var togglApiKey = args[1];

            var togglWorkspaceId = 605632;
            var togglClientId = 15242883;

            var since = new DateTime(2015, 12, 1);
            var until = new DateTime(2015, 12, 31);

            ToggleToAzure(azureAccountKey, togglApiKey, togglWorkspaceId, togglClientId, since, until);
        }

        private static void ToggleToAzure(string accountKey, string apiKey, int workspaceId, int clientId, DateTime since, DateTime until)
        {
            TimeEntryTable table = new TimeEntryTable(accountKey);

            var workspace = new Workspace(apiKey, workspaceId);

            List<ReportTimeEntry> reportTimeEntries = workspace.GetReportTimeEntries(clientId, since, until);

            foreach (ReportTimeEntry timeEntry in reportTimeEntries)
            {
                TimeEntryEntity entity = new TimeEntryEntity(timeEntry.TaskId, timeEntry.Id, timeEntry.ProjectId, timeEntry.TaskName, timeEntry.Start, timeEntry.UserName, timeEntry.Billable);

                table.InsertOrReplace(entity);
            }
        }
    }
}
