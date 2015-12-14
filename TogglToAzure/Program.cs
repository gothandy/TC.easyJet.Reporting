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
            var accountKey = args[0];
            var apiKey = args[1];

            var workspaceId = 605632;
            var clientId = 15242883;

            var since = new DateTime(2015, 12, 1);
            var until = new DateTime(2015, 12, 31);

            TimeEntryTable table = new TimeEntryTable(accountKey);

            var workspace = new Workspace(apiKey, workspaceId);

            List<ReportTimeEntry> reportTimeEntries = workspace.GetReportTimeEntries(clientId, since, until);

            foreach (ReportTimeEntry timeEntry in reportTimeEntries)
            {

                TimeEntryEntity entity = new TimeEntryEntity(timeEntry.TaskId, timeEntry.Id);

                entity.ProjectId = timeEntry.ProjectId;
                entity.TaskName = timeEntry.TaskName;
                entity.Start = timeEntry.Start;
                entity.UserName = timeEntry.UserName;
                entity.Billable = timeEntry.Billable;

                table.InsertOrReplace(entity);
            }
        }
    }
}
