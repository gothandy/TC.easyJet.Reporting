using Gothandy.StartUp;
using Gothandy.Tables.Bulk;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Gothandy.Toggl.DataObjects;
using Vincente.Config;

namespace TogglConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            var config = ConfigBuilder.Build();

            #region Dependancies
            var azureStorageAccount = CloudStorageAccount.Parse(config.azureConnectionString);

            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureTimeEntryTable = azureTableClient.GetTableReference(config.azureTimeEntriesTableName);

            var azureBlobClient = azureStorageAccount.CreateCloudBlobClient();
            var azureBlobContainer = azureBlobClient.GetContainerReference(config.azureBlobContainerName);
            var azureTeamListBlob = azureBlobContainer.GetBlockBlobReference(config.azureTeamListPath);
            var azureTeamTable = new ReplaceTable(azureTeamListBlob);

            var togglWorkspace = new Gothandy.Toggl.Workspace(config.togglApiKey, config.togglWorkspaceId);
            #endregion
            
            var azureTeamList = azureTeamTable.Query().ToList();
            Console.Out.WriteLine("{0} Team List Items", azureTeamList.Count);

            var getAll = !azureTimeEntryTable.Exists();
            if (getAll) azureTimeEntryTable.Create();
            //getAll = true;

            var togglTimeEntries = GetTogglTimeEntries(togglWorkspace, config.togglClientId, getAll);

            Console.Out.WriteLine("{0} Time Entries Found.", togglTimeEntries.Count);

            var timeEntryTable = new TimeEntryTable(azureTimeEntryTable);

            var newTimeEntries = TogglToData.Execute(timeEntryTable, togglTimeEntries, azureTeamList);
            var oldTimeEntries = GetOldTimeEntries(timeEntryTable, getAll);

            Console.Out.WriteLine("{0} New Count.", newTimeEntries.Count);
            Console.Out.WriteLine("{0} Old Count.", oldTimeEntries.Count);

            var operations = new Operations<TimeEntry>(timeEntryTable);

            var results = operations.BatchCompare(oldTimeEntries, newTimeEntries);

            Console.Out.WriteLine("{0} Time Entries Inserted", results.Inserted);
            Console.Out.WriteLine("{0} Time Entries Ignored", results.Ignored);
            Console.Out.WriteLine("{0} Time Entries Replaced", results.Replaced);
            Console.Out.WriteLine("{0} Time Entries Deleted", results.Deleted);
        }


        private static List<ReportTimeEntry> GetTogglTimeEntries(Gothandy.Toggl.Workspace togglWorkspace, int clientId, bool getAll)
        {
            DateTime until = GetUntil();
            DateTime since = GetSince(getAll, until);

            Console.Out.WriteLine("Toggl time entries from {0} to {1}", since, until);

            var table = new Gothandy.Toggl.Tables.TimeEntryTable(togglWorkspace);

            return table.GetReportTimeEntries(clientId, since, until);
        }

        private static List<TimeEntry> GetOldTimeEntries(TimeEntryTable table, bool getAll)
        {
            DateTime until = GetUntil();
            DateTime since = GetSince(getAll, until);

            return
                (from timeEntry in table.Query()
                 where timeEntry.Start >= since
                 select timeEntry).ToList();
        }

        private static DateTime GetSince(bool getAll, DateTime until)
        {
            return getAll ? until.AddYears(-1) : until.AddMonths(-1);
        }

        private static DateTime GetUntil()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
        }
    }
}

