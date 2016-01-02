using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Toggl.Tables;
using Vincente.TogglSync.Operations;
using Vincente.Trello.DataObjects;

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
            var azureCardTable = GetAzureCardTable();
            var azureCards = azureCardTable.Query().ToList();

            var trelloLabels = GetTrelloLabels();

            var togglWorkspace = GetTogglWorkspace();
            var togglTaskTable = new TaskTable(togglWorkspace);
            var togglProjectTable = new ProjectTable(togglWorkspace);
            var togglProjects = togglProjectTable.GetProjects(togglClientId);
            var togglTemplate = togglProjectTable.GetProject(toggleProjectTemplateId);

            var togglTasks = GetAllTogglTasks.Execute(togglTaskTable, togglProjects);
            Console.Out.WriteLine("{0} Toggl Tasks Returned", togglTasks.Count);

            var created = SyncTogglProjects.Execute(togglProjectTable, togglProjects, togglTemplate, trelloLabels);
            Console.Out.WriteLine("{0} Toggl Projects Created", created.Count);

            var updated = UpdateAzureTaskIds.Execute(azureCardTable, azureCards, togglTasks);
            Console.Out.WriteLine("{0} Azure Cards Updated", updated.Count);

            var deleted = DeleteEmptyDuplicateTasks.Execute(azureCards, togglProjects, togglTasks);
            Console.Out.WriteLine("{0} Empty Duplicate Tasks Deleted", deleted.Count);

            ConsoleOutWriteList("\"{0}\" Toggl Project Created", created);
            ConsoleOutWriteList("\"{0}\" Azure Card Updated", updated);
            ConsoleOutWriteList("\"{0}\" Toggl Task Deleted", deleted);
        }

        private static void ConsoleOutWriteList(string format, List<string> logEntries)
        {
            if (logEntries.Count > 0) Console.Out.WriteLine("----------");
            foreach (string logEntry in logEntries)
            {
                Console.Out.WriteLine(format, limitStringLength(logEntry, 40));
            }
        }

        private static string limitStringLength(string input, int length)
        {
            if (input.Length < length) return input;
            return string.Format("{0} ...", input.Substring(0, length));
        }

        private static CardTable GetAzureCardTable()
        {
            var azureConnectionString = CheckAndGetAppSettings("azureConnectionString");
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureCardCloudTable = azureTableClient.GetTableReference("Cards");

            return new CardTable(azureCardCloudTable);
        }

        private static List<Label> GetTrelloLabels()
        {
            var trelloToken = CheckAndGetAppSettings("trelloToken");
            var trelloWorkspace = new Trello.Workspace(trelloKey, trelloToken, trelloBoardId);

            return trelloWorkspace.GetLabels();
        }

        private static Toggl.Workspace GetTogglWorkspace()
        {
            Toggl.Workspace togglWorkspace;
            var togglApiKey = CheckAndGetAppSettings("togglApiKey");
            togglWorkspace = new Toggl.Workspace(togglApiKey, togglWorkspaceId);
            return togglWorkspace;
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }
    }
}
