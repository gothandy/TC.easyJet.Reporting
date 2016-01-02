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
    public static class UpdateAzureTaskIds
    {
        public static void Execute(Toggl.Workspace togglWorkspace, CloudTableClient azureTableClient, int togglClientId)
        {
            var azureCardCloudTable = azureTableClient.GetTableReference("Cards");
            var azureCardTable = new CardTable(azureCardCloudTable);
            var azureCards = azureCardTable.Query().ToList();

            var togglProjectTable = new ProjectTable(togglWorkspace);
            var togglProjects = togglProjectTable.GetProjects(togglClientId);
            var togglTaskTable = new TaskTable(togglWorkspace);
            var togglTasks = GetTogglTasks(togglProjects, togglTaskTable);

            var updates = UpdateAzureCards(azureCardTable, azureCards, togglTasks);

            Console.Out.WriteLine("{0} Azure card updates made.", updates);

            var deletes = DeleteEmptyDuplicateTasks(azureCards, togglProjects, togglTasks);

        }

        private static int DeleteEmptyDuplicateTasks(List<Card> azureCards, List<Project> togglProjects, List<Task> togglTasks)
        {
            foreach (Card card in azureCards)
            {
                if (card.DomId != null)
                {
                    var originalDomId = string.Concat(card.DomId.Substring(1), " ");

                    var tasks =
                        (from t in togglTasks
                         where t.Name.Contains(originalDomId)
                         select t).ToList();

                    var trackedCount =
                        (from t in togglTasks
                         where t.Name.Contains(originalDomId) && t.TrackedSeconds != null
                         select t).Count();

                    if (tasks.Count > 1 && trackedCount > 0)
                    {
                        foreach (Task task in tasks)
                        {

                            var project =
                                (from p in togglProjects
                                 where p.Id == task.ProjectId
                                 select p).First();

                            if (task.TrackedSeconds == null)
                            {
                                Console.WriteLine("{0} {1} {2} DELETE", card.DomId, project.Name, task.TrackedSeconds.GetValueOrDefault());
                            }
                        }
                    }
                }
            }
            return 0;
        }

        private static List<Task> GetTogglTasks(List<Project> projects, TaskTable taskTable)
        {
            var tasks = new List<Task>();

            foreach (Project project in projects)
            {
                var projectTasks = taskTable.GetTasks((int)project.Id);

                if (projectTasks != null) tasks.AddRange(projectTasks);

                System.Threading.Thread.Sleep(1000);
            }

            return tasks;
        }

        private static int UpdateAzureCards(CardTable cardTable, List<Card> cards, List<Task> togglTasks)
        {
            var count = 0;
            if (togglTasks != null)
            {
                foreach (Card card in cards)
                {
                    if (card.DomId != null)
                    {
                        var originalDomId = string.Concat(card.DomId.Substring(1), " ");

                        var taskIds =
                            (from t in togglTasks
                             where t.Name.Contains(originalDomId) && t.TrackedSeconds.GetValueOrDefault() > 0
                             select t.Id.GetValueOrDefault()).ToList();

                        if (card.TaskIds == null) card.TaskIds = new List<long>();

                        if (!card.TaskIds.SequenceEqual(taskIds))
                        {
                            card.TaskIds = taskIds;
                            cardTable.BatchReplace(card);
                            count++;
                        }
                    }
                }
                cardTable.BatchComplete();
            }
            return count;
        }




    }
}
