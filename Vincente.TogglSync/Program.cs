using Gothandy.StartUp;
using Gothandy.Toggl.Tables;
using Gothandy.Trello.DataObjects;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.TogglSync.Operations;

namespace Vincente.TogglSync
{
    class Program
    {
        const int togglWorkspaceId = 605632;
        const int togglClientId = 15242883;
        const int toggleProjectTemplateId = 12577727;

        const string trelloKey = "3ba00ca224256611c3ccbac183364259";
        const string trelloBoardId = "5596a7b7ac88c077383d281c";

        static void Main(string[] args)
        {
            var azureTableClient = GetAzureTableClient();
            var azureTaskTable = new Azure.Tables.TaskTable(azureTableClient.GetTableReference("Tasks"));
            var azureCardTable = new CardTable(azureTableClient.GetTableReference("Cards"));
            var azureCards = azureCardTable.Query().ToList();

            var trelloLabels = GetTrelloLabels();

            var togglWorkspace = GetTogglWorkspace();
            var togglTaskTable = new Gothandy.Toggl.Tables.TaskTable(togglWorkspace);
            var togglProjectTable = new ProjectTable(togglWorkspace);
            var togglProjects = togglProjectTable.GetProjects(togglClientId);
            var togglTemplate = togglProjectTable.GetProject(toggleProjectTemplateId);

            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            var togglTasks = GetAllTogglTasks.Execute(togglTaskTable, togglProjects);
            Console.Out.WriteLine("{0} Toggl Tasks Returned", togglTasks.Count);

            var created = SyncTogglProjects.Execute(togglProjectTable, togglProjects, togglTemplate, trelloLabels);
            Console.Out.WriteLine("{0} Toggl Projects Created", created.Count);

            var results = UpdateAzureTasks.Execute(azureTaskTable, azureCards, togglProjects, togglTasks);
            Console.Out.WriteLine("{0} Tasks Inserted", results.Inserted);
            Console.Out.WriteLine("{0} Tasks Ignored", results.Ignored);
            Console.Out.WriteLine("{0} Tasks Replaced", results.Replaced);
            Console.Out.WriteLine("{0} Tasks Deleted", results.Deleted);
        }

        private static CloudTableClient GetAzureTableClient()
        {
            var azureConnectionString = Tools.CheckAndGetAppSettings("azureConnectionString");
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            return azureStorageAccount.CreateCloudTableClient();
        }

        private static List<Label> GetTrelloLabels()
        {
            var trelloToken = Tools.CheckAndGetAppSettings("trelloToken");
            var trelloWorkspace = new Gothandy.Trello.Workspace(trelloKey, trelloToken, trelloBoardId);

            return trelloWorkspace.GetLabels();
        }

        private static Gothandy.Toggl.Workspace GetTogglWorkspace()
        {
            Gothandy.Toggl.Workspace togglWorkspace;
            var togglApiKey = Tools.CheckAndGetAppSettings("togglApiKey");
            togglWorkspace = new Gothandy.Toggl.Workspace(togglApiKey, togglWorkspaceId);
            return togglWorkspace;
        }
    }
}
