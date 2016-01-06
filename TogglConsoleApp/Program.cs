using Gothandy.StartUp;
using Gothandy.Tables.Bulk;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            var azureConnectionString = Tools.CheckAndGetAppSettings("azureConnectionString");
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureBlobClient = azureStorageAccount.CreateCloudBlobClient();
            var azureTimeEntryTable = azureTableClient.GetTableReference("TimeEntries");
            var azureVincenteContainer = azureBlobClient.GetContainerReference("vincente");

            var togglApiKey = Tools.CheckAndGetAppSettings("togglApiKey");
            var togglWorkspaceId = 605632;
            var togglClientId = 15242883;
            var togglWorkspace = new Vincente.Toggl.Workspace(togglApiKey, togglWorkspaceId);

            var teamList = GetTeamList(azureVincenteContainer);
            Console.Out.WriteLine("{0} Team List Items", teamList.Count);

            var getAll = !azureTimeEntryTable.Exists();
            if (getAll) azureTimeEntryTable.Create();
            //getAll = true;

            var togglTimeEntries = GetTogglTimeEntries(togglWorkspace, togglClientId, getAll);

            Console.Out.WriteLine("{0} Time Entries Found.", togglTimeEntries.Count);

            var timeEntryTable = new TimeEntryTable(azureTimeEntryTable);

            var newTimeEntries = TogglToData.Execute(timeEntryTable, togglTimeEntries, teamList);
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

        private static List<Team> GetTeamList(CloudBlobContainer blobContainer)
        {
            CloudBlockBlob teamListJson = blobContainer.GetBlockBlobReference("TeamList.json");

            if (!teamListJson.Exists()) return new List<Team>();

            string json;
            using (var memoryStream = new MemoryStream())
            {
                teamListJson.DownloadToStream(memoryStream);
                json = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            return JsonConvert.DeserializeObject<List<Team>>(json);
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
            return getAll ? until.AddYears(-1) : until.AddMonths(-1);
        }

        private static DateTime GetUntil()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
        }
    }
}

