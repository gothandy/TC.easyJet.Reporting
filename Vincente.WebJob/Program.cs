using Gothandy.StartUp;
using Gothandy.Toggl.Tables;
using Microsoft.WindowsAzure.Storage;
using System;
using Vincente.Azure.Tables;
using Vincente.Config;
using Vincente.WebJobs.TogglToTask;
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
            var azureCardTableRef = azureTableClient.GetTableReference("Cards");
            var azureReplaceBlob = azureBlobContainer.GetBlockBlobReference(config.azureReplacePath);
            var azureReplaceTable = new ListNameTable(azureReplaceBlob);
            var trelloWorkspace = new Gothandy.Trello.Workspace(config.trelloKey, config.trelloToken, config.trelloBoardId);
            var trelloToCardWebJob = new TrelloToCardWebJob(trelloWorkspace, azureCardTableRef, azureReplaceTable);

            // Toggl To Time Entry
            var azureTimeEntryTable = azureTableClient.GetTableReference(config.azureTimeEntriesTableName);
            var azureTeamListBlob = azureBlobContainer.GetBlockBlobReference(config.azureTeamListPath);
            var azureTeamTable = new ReplaceTable(azureTeamListBlob);
            var togglWorkspace = new Gothandy.Toggl.Workspace(config.togglApiKey, config.togglWorkspaceId);
            var togglToTimeEntryWebJob = new TogglToTimeEntryWebJob(config.togglClientId, azureTimeEntryTable, azureTeamTable, togglWorkspace);

            // Toggl To Task
            var azureTaskTableRef = azureTableClient.GetTableReference("Tasks");
            var azureTaskTable = new Azure.Tables.TaskTable(azureTaskTableRef);
            var azureCardTable = new CardTable(azureCardTableRef);
            var togglTaskTable = new Gothandy.Toggl.Tables.TaskTable(togglWorkspace);
            var togglProjectTable = new ProjectTable(togglWorkspace);
            var togglToTaskData = new TogglToTaskData
            {
                togglClientId = config.togglClientId,
                togglProjectTemplateId = config.togglProjectTemplateId,
                azureCardTable = azureCardTable,
                azureTaskTable = azureTaskTable,
                trelloWorkspace = trelloWorkspace,
                togglTaskTable = togglTaskTable,
                togglProjectTable = togglProjectTable
            };
            var togglToTaskWebJob = new TogglToTaskWebJob(togglToTaskData);

            #endregion

            jobScheduler.Begin();

            jobScheduler.CheckAndRun(t => t.TrelloToCard, 5, trelloToCardWebJob.Execute);
            jobScheduler.CheckAndRun(t => t.TogglToTimeEntry, 15, togglToTimeEntryWebJob.Execute);
            jobScheduler.CheckAndRun(t => t.TogglToTask, 60, togglToTaskWebJob.Execute);

            jobScheduler.End();
        }
    }
}
