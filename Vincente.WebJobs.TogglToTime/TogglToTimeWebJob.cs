using Gothandy.Tables.Bulk;
using Gothandy.Toggl.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Gothandy.Toggl;
using Microsoft.WindowsAzure.Storage.Table;

namespace Vincente.WebJobs.TogglToTime
{
    public class TogglToTimeWebJob
    {
        private int togglClientId;
        private CloudTable azureTimeEntryTable;
        private ReplaceTable azureTeamTable;
        private Workspace togglWorkspace;

        public TogglToTimeWebJob(int togglClientId, CloudTable azureTimeEntryTable, ReplaceTable azureTeamTable, Workspace togglWorkspace)
        {
            this.togglClientId = togglClientId;
            this.azureTimeEntryTable = azureTimeEntryTable;
            this.azureTeamTable = azureTeamTable;
            this.togglWorkspace = togglWorkspace;
        }

        public void Execute()
        {
            var azureTeamList = azureTeamTable.Query().ToList();
            Console.Out.WriteLine("{0} Team List Items", azureTeamList.Count);

            var createTable = !azureTimeEntryTable.Exists();
            if (createTable) azureTimeEntryTable.Create();
            var getAll = false; //createTable;

            var togglTimeEntries = GetTogglTimeEntries(togglWorkspace, togglClientId, getAll);

            Console.Out.WriteLine("{0} Time Entries Found.", togglTimeEntries.Count);

            var timeEntryTable = new TimeEntryTable(azureTimeEntryTable);

            var newTimeEntries = TogglToData.Execute(timeEntryTable, togglTimeEntries, azureTeamList);
            var oldTimeEntries = GetOldTimeEntries(timeEntryTable, getAll);

            Console.Out.WriteLine("{0} New Count.", newTimeEntries.Count);
            Console.Out.WriteLine("{0} Old Count.", oldTimeEntries.Count);

            var operations = new Operations<TimeEntry>(timeEntryTable);

            var results = operations.BatchCompare(oldTimeEntries, newTimeEntries);

            ConsoleOutResults(results);
        }

        private static void ConsoleOutResults(Results results)
        {
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
