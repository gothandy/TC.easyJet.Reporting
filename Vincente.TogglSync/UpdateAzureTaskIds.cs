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
            var azureCardTable = azureTableClient.GetTableReference("Cards");
            var cardTable = new CardTable(azureCardTable);
            var cards = cardTable.Query().ToList();
            var togglTaskTable = new TaskTable(togglWorkspace);
            var togglProjects = GetTogglProjects(togglWorkspace, togglClientId);

            var updates = new List<Card>();

            foreach (Project project in togglProjects)
            {
                var togglTasks = togglTaskTable.GetTasks((int)project.Id);

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
            if (togglTasks != null)
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

                foreach (long taskId in taskIds)
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

        private static List<Project> GetTogglProjects(Toggl.Workspace togglWorkspace, int togglClientId)
        {
            var togglProjectTable = new ProjectTable(togglWorkspace);

            return togglProjectTable.GetProjects(togglClientId);
        }
    }
}
