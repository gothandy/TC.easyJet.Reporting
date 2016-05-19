using Gothandy.StartUp;
using Gothandy.Toggl.Tables;
using Microsoft.WindowsAzure.Storage;
using System;
using Vincente.Azure.Tables;
using Vincente.Config;
using Vincente.WebJobs.TogglToTask;

namespace Vincente.TogglSync
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
            var azureTaskTable = new Azure.Tables.TaskTable(azureTableClient.GetTableReference("Tasks"));
            var azureCardTable = new CardTable(azureTableClient.GetTableReference("Cards"));

            var trelloWorkspace = new Gothandy.Trello.Workspace(config.trelloKey, config.trelloToken, config.trelloBoardId);

            var togglWorkspace = new Gothandy.Toggl.Workspace(config.togglApiKey, config.togglWorkspaceId);
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

            //togglToTaskWebJob.Execute();
            Console.WriteLine("This Web Job has been retired. See VincenteWebJob.");
        }
    }
}
