using Gothandy.StartUp;
using Gothandy.Toggl.Tables;
using Gothandy.Trello.DataObjects;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Config;
using Vincente.TogglSync.Operations;
using Vincente.WebJobs.TogglToTask;

namespace Vincente.TogglSync
{
    class Program
    {
        static void Main(string[] args)
        {
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
            #endregion

            Execute(togglToTaskData);
        }

        private static void Execute(TogglToTaskData togglToTaskData)
        {
            var azureCards = togglToTaskData.azureCardTable.Query().ToList();
            var trelloLabels = togglToTaskData.trelloWorkspace.GetLabels();
            var togglProjects = togglToTaskData.togglProjectTable.GetProjects(togglToTaskData.togglClientId);
            var togglTemplate = togglToTaskData.togglProjectTable.GetProject(togglToTaskData.togglProjectTemplateId);

            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            var togglTasks = GetAllTogglTasks.Execute(togglToTaskData.togglTaskTable, togglProjects);
            Console.Out.WriteLine("{0} Toggl Tasks Returned", togglTasks.Count);

            var created = SyncTogglProjects.Execute(togglToTaskData.togglProjectTable, togglProjects, togglTemplate, trelloLabels);
            Console.Out.WriteLine("{0} Toggl Projects Created", created.Count);

            var results = UpdateAzureTasks.Execute(togglToTaskData.azureTaskTable, azureCards, togglProjects, togglTasks);
            Console.Out.WriteLine("{0} Tasks Inserted", results.Inserted);
            Console.Out.WriteLine("{0} Tasks Ignored", results.Ignored);
            Console.Out.WriteLine("{0} Tasks Replaced", results.Replaced);
            Console.Out.WriteLine("{0} Tasks Deleted", results.Deleted);
        }
    }
}
