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
            var azureTableClient = new Vincente.Azure.TableClient(azureConnectionString);
            var azureTimeEntryTable = new TimeEntryTable(azureTableClient);

            var togglApiKey = CheckAndGetAppSettings("togglApiKey");
            var togglWorkspaceId = 605632;
            var togglClientId = 15242883;
            var togglWorkspace = new Vincente.Toggl.Workspace(togglApiKey, togglWorkspaceId);

            var azureTableExists = CreateIfNotExists(azureTimeEntryTable);
            var togglTimeEntries = GetTogglTimeEntries(togglWorkspace, togglClientId, !azureTableExists);

            Console.Out.WriteLine("{0} Time Entries Found.", togglTimeEntries.Count);

            TogglToAzure.Execute(azureTimeEntryTable, togglTimeEntries);

        }

        private static bool CreateIfNotExists(TimeEntryTable azureTimeEntryTable)
        {
            var azureTableExists = azureTimeEntryTable.Exists();
            if (!azureTableExists) azureTimeEntryTable.Create();
            return azureTableExists;
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

