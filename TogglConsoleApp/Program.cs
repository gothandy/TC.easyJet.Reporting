using Gothandy.Tables.Bulk;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Vincente.Toggl.DataObjects;

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

            TimeEntryTable timeEntryTable = new TimeEntryTable(azureTimeEntryTable);

            var newTimeEntries = TogglToData.Execute(timeEntryTable, togglTimeEntries);
            var oldTimeEntries = GetOldTimeEntries(timeEntryTable, !azureTableExists);

            var operations = new Operations<TimeEntry>(timeEntryTable);

            var results = operations.BatchCompare(oldTimeEntries, newTimeEntries);

            Console.Out.WriteLine("{0} Time Entries Inserted", results.Inserted);
            Console.Out.WriteLine("{0} Time Entries Ignored", results.Ignored);
            Console.Out.WriteLine("{0} Time Entries Replaced", results.Replaced);
            Console.Out.WriteLine("{0} Time Entries Deleted", results.Deleted);
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }

        private static List<ReportTimeEntry> GetTogglTimeEntries(Vincente.Toggl.Workspace togglWorkspace, int clientId, bool getAll)
        {
            DateTime until = GetUntil();
            DateTime since = GetSince(getAll, until);

            Console.Out.WriteLine("Toggl time entries from {0} to {1}", since, until);

            var table = new Vincente.Toggl.Tables.TimeEntryTable(togglWorkspace);

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
            return getAll ? new DateTime(2015, 1, 1) : until.AddMonths(-1);
        }

        private static DateTime GetUntil()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
        }
    }
}

