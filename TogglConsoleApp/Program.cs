using System;
using System.Collections.Generic;
using System.Configuration;
using Vincente.Azure.Converters;
using Vincente.Azure.Entities;
using Vincente.Azure;
using Vincente.Toggl.DataObjects;
using Microsoft.WindowsAzure.Storage;
using Vincente.Data.Entities;

namespace TogglConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureConnectionString = CheckAndGetAppSettings("azureConnectionString");
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureTimeEntryTable = azureTableClient.GetTableReference("TimeEntries");

            var togglApiKey = CheckAndGetAppSettings("togglApiKey");
            var togglWorkspaceId = 605632;
            var togglClientId = 15242883;
            var togglWorkspace = new Vincente.Toggl.Workspace(togglApiKey, togglWorkspaceId);

            var azureTableExists = azureTimeEntryTable.Exists();
            if (!azureTableExists) azureTimeEntryTable.Create();

            var togglTimeEntries = GetTogglTimeEntries(togglWorkspace, togglClientId, !azureTableExists);

            Console.Out.WriteLine("{0} Time Entries Found.", togglTimeEntries.Count);

            AzureTable<TimeEntry, TimeEntryEntity> timeEntryTable =
                new AzureTable<TimeEntry, TimeEntryEntity>(azureTimeEntryTable, new TimeEntryConverter());

            TogglToData.Execute(timeEntryTable, togglTimeEntries);
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }


        private static List<ReportTimeEntry> GetTogglTimeEntries(Vincente.Toggl.Workspace togglWorkspace, int clientId, bool getAll)
        {
            DateTime until = DateTime.Now;
            DateTime since = getAll ? new DateTime(2015, 1, 1) : until.AddMonths(-1);

            Console.Out.WriteLine("Toggl time entries from {0} to {1}", since, until);

            return togglWorkspace.GetReportTimeEntries(clientId, since, until);
        }
    }
}

