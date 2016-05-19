using Gothandy.StartUp;
using Microsoft.WindowsAzure.Storage;
using System;
using Vincente.Azure.Tables;
using Vincente.Config;
using Vincente.WebJobs.TogglToTime;
using Vincente.WebJobs.TrelloToCard;

namespace Vincente.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            var config = ConfigBuilder.Build();

            #region Dependancies

            // Job Scheduler
            var azureStorageAccount = CloudStorageAccount.Parse(config.azureConnectionString);
            var azureBlobClient = azureStorageAccount.CreateCloudBlobClient();
            var azureBlobContainer = azureBlobClient.GetContainerReference(config.azureBlobContainerName);
            var azureLastRunTimesBlob = azureBlobContainer.GetBlockBlobReference("LastRunTimes.xml");
            var jobScheduler = new JobScheduler(azureLastRunTimesBlob);

            // Trello To Task
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureCardTable = azureTableClient.GetTableReference("Cards");
            var azureReplaceBlob = azureBlobContainer.GetBlockBlobReference(config.azureReplacePath);
            var azureReplaceTable = new ListNameTable(azureReplaceBlob);
            var trelloWorkspace = new Gothandy.Trello.Workspace(config.trelloKey, config.trelloToken, config.trelloBoardId);
            var trelloToCardWebJob = new TrelloToCardWebJob(trelloWorkspace, azureCardTable, azureReplaceTable);

            // Toggl To Time Entry
            var azureTimeEntryTable = azureTableClient.GetTableReference(config.azureTimeEntriesTableName);
            var azureTeamListBlob = azureBlobContainer.GetBlockBlobReference(config.azureTeamListPath);
            var azureTeamTable = new ReplaceTable(azureTeamListBlob);
            var togglWorkspace = new Gothandy.Toggl.Workspace(config.togglApiKey, config.togglWorkspaceId);
            var togglToTimeEntry = new TogglToTimeEntryWebJob(config.togglClientId, azureTimeEntryTable, azureTeamTable, togglWorkspace);

            #endregion

            jobScheduler.Begin();

            jobScheduler.CheckAndRun(t => t.TrelloToCard, 5, trelloToCardWebJob.Execute);
            jobScheduler.CheckAndRun(t => t.TogglToTimeEntry, 15, togglToTimeEntry.Execute);
            jobScheduler.CheckAndRun(t => t.TogglToTask, 60, () => { Console.WriteLine("Toggl To Task"); });

            jobScheduler.End();
        }
    }
}
