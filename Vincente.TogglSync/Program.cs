using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Vincente.Formula;
using Vincente.Toggl.DataObjects;
using Vincente.Toggl.Tables;

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
            var trelloWorkspace = GetTrelloWorkspace();
            var togglWorkspace = GetTogglWorkspace();

            SyncTogglProjects(trelloWorkspace, togglWorkspace);
            SyncTogglTasks(togglWorkspace, azureTableClient);
        }

        private static void SyncTogglTasks(Toggl.Workspace togglWorkspace, CloudTableClient azureTableClient)
        {
            var azureCardTable = azureTableClient.GetTableReference("Cards");
            var cardTable = new CardTable(azureCardTable);
            var cards = cardTable.Query().ToList();
            var togglTaskTable = new TaskTable(togglWorkspace);
            var togglProjects = GetTogglProjects(togglWorkspace);

            var updates = new List<Card>();

            foreach (Project project in togglProjects)
            {
                var togglTasks = GetTogglTasks(togglTaskTable, project);

                UpdateAzureCards(cardTable, cards, updates, togglTasks);
            }

            foreach (Card card in updates)
            {
                cardTable.BatchReplace(card);
            }

            cardTable.BatchComplete();

            Console.Out.WriteLine("{0} Azure card updates made.", updates.Count);
        }

        private static void UpdateAzureCards(CardTable cardTable, List<Card> cards, List<Card> updates, List<Task> togglTasks)
        {
            if (togglTasks.Count > 0)
            {
                foreach (Card card in cards)
                {
                    if (card.DomId != null)
                    {
                        var originalDomId = string.Concat(card.DomId.Substring(1), " ");

                        var taskIds =
                            (from t in togglTasks
                             where t.Name.Contains(originalDomId)
                             select t.Id.GetValueOrDefault()).ToList();

                        if (taskIds.Count > 0)
                        {
                            UpdateCardTable(cardTable, card, taskIds, updates);
                        }
                    }
                }
            }
        }

        private static void UpdateCardTable(CardTable cardTable, Card card, List<long> taskIds, List<Card> updates)
        {
            if (taskIds != null)
            {
                var updated = false;

                foreach(long taskId in taskIds)
                {
                    if (card.TaskIds == null) card.TaskIds = new List<long>();
                    if (!card.TaskIds.Contains(taskId))
                    {
                        card.TaskIds.Add(taskId);
                        updated = true;
                    }
                }

                if (updated)
                {
                    if (!updates.Contains(card)) updates.Add(card);
                }
            }
        }

        private static List<Project> GetTogglProjects(Toggl.Workspace togglWorkspace)
        {
            var togglProjectTable = new ProjectTable(togglWorkspace);

            return togglProjectTable.GetProjects(togglClientId);
        }

        private static List<Task> GetTogglTasks(TaskTable togglTaskTable, Project project)
        {


            var togglTasks = new List<Task>();

            var tasks = togglTaskTable.GetTasks((int)project.Id);

            if (tasks != null) togglTasks.AddRange(tasks);

            return togglTasks;
        }

        private static void SyncTogglProjects(Trello.Workspace trelloWorkspace, Toggl.Workspace togglWorkspace)
        {
            var trelloLabels =
                (from l in trelloWorkspace.GetLabels()
                 select l.Name).ToList();

            var trelloEpics = FromLabels.GetEpics(trelloLabels);

            var togglProjectTable = new ProjectTable(togglWorkspace);

            var togglProjects =
                (from p in togglProjectTable.GetProjects(togglClientId)
                 select p.Name).ToList();

            Project togglTemplate = togglProjectTable.GetProject(toggleProjectTemplateId);

            var createdCount = 0;

            foreach (string epic in trelloEpics)
            {
                if (!togglProjects.Contains(epic))
                {
                    Project project = new Project()
                    {
                        WorkspaceId = togglWorkspaceId,
                        ClientId = togglClientId,
                        Name = epic,
                        IsPrivate = togglTemplate.IsPrivate,
                        Color = togglTemplate.Color,
                        Rate = togglTemplate.Rate
                    };

                    ProjectTable projectTable = new ProjectTable(togglWorkspace);

                    projectTable.Create(project);

                    Console.Out.WriteLine("\"{0}\" toggl Project created.", project.Name);

                    createdCount++;
                }
            }

            if (createdCount == 0) Console.Out.WriteLine("All toggl projects in sync.");
        }

        private static CloudTableClient GetAzureTableClient()
        {
            var azureConnectionString = CheckAndGetAppSettings("azureConnectionString");
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            return azureTableClient;
        }

        private static Toggl.Workspace GetTogglWorkspace()
        {
            Toggl.Workspace togglWorkspace;
            var togglApiKey = CheckAndGetAppSettings("togglApiKey");
            togglWorkspace = new Toggl.Workspace(togglApiKey, togglWorkspaceId);
            return togglWorkspace;
        }

        private static Trello.Workspace GetTrelloWorkspace()
        {
            Trello.Workspace trelloWorkspace;
            var trelloToken = CheckAndGetAppSettings("trelloToken");

            trelloWorkspace = new Trello.Workspace(trelloKey, trelloToken, trelloBoardId);
            return trelloWorkspace;
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }
    }
}
