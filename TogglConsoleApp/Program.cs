using System;
using System.Collections.Generic;
using System.Configuration;
using Vincente.Azure.Entities;
using Vincente.Azure.Tables;
using Vincente.Toggl.DataObjects;

namespace TogglConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureConnectionString = CheckAndGetAppSettings("azureConnectionString");
            var togglApiKey = CheckAndGetAppSettings("togglApiKey");

            var togglWorkspaceId = 605632;
            var togglClientId = 15242883;

            var azureTableClient = new Vincente.Azure.TableClient(azureConnectionString);
            TimeEntryTable azureTimeEntryTable = new TimeEntryTable(azureTableClient);

            var azureTableExists = azureTimeEntryTable.Exists();

            if (!azureTableExists) azureTimeEntryTable.Create();

            List<ReportTimeEntry> togglTimeEntries;

            if (azureTableExists)
            {
                togglTimeEntries = TogglFromLastMonth(togglApiKey, togglWorkspaceId, togglClientId);
            }
            else
            {
                
                togglTimeEntries = TogglFromJan2015(togglApiKey, togglWorkspaceId, togglClientId);
            }

            TogglToAzure2(azureTimeEntryTable, togglTimeEntries);

        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }


        private static List<ReportTimeEntry> TogglFromJan2015(string togglApiKey, int togglWorkspaceId, int togglClientId)
        {
            var until = DateTime.Now;
            var since = new DateTime(2015, 1, 1);

            return GetTogglTimeEntries(togglApiKey, togglWorkspaceId, togglClientId, until, since);
        }

        private static List<ReportTimeEntry> TogglFromLastMonth(string togglApiKey, int togglWorkspaceId, int togglClientId)
        {
            var until = DateTime.Now;
            var since = until.AddMonths(-1);

            return GetTogglTimeEntries(togglApiKey, togglWorkspaceId, togglClientId, until, since);
        }

        private static List<ReportTimeEntry> GetTogglTimeEntries(string apiKey, int workspaceId, int clientId, DateTime until, DateTime since)
        {
            Console.Out.WriteLine("Toggl time entries from {0} to {1}", since, until);

            var workspace = new Vincente.Toggl.Workspace(apiKey, workspaceId);

            return workspace.GetReportTimeEntries(clientId, since, until);
        }

        private static void TogglToAzure2(TimeEntryTable azureTimeEntry, List<ReportTimeEntry> togglTimeEntries)
        {
            Console.Out.WriteLine("{0} Time Entries Found.", togglTimeEntries.Count);

            foreach (ReportTimeEntry timeEntry in togglTimeEntries)
            {
                TimeEntryEntity entity = new TimeEntryEntity(
                    timeEntry.Start, timeEntry.Id, timeEntry.ProjectId, timeEntry.TaskId, timeEntry.TaskName, timeEntry.UserName, timeEntry.Billable);

                azureTimeEntry.BatchInsertOrReplace(entity);

                //Use for testing to remove batch complexity.
                //table.InsertOrReplace(entity);
            }

            azureTimeEntry.ExecuteBatch();
        }
    }
}

