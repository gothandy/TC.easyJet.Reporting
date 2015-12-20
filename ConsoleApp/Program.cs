using System;
using System.Collections.Generic;
using System.Configuration;
using Vincente.Azure.Entities;
using Vincente.Azure.Tables;
using Vincente.Toggl.DataObjects;
using Vincente.Trello.DataObjects;

namespace Vincente.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var azureConnectionString = ConfigurationManager.AppSettings["azureConnectionString"];
            var togglApiKey = ConfigurationManager.AppSettings["togglApiKey"];
            var trelloToken = ConfigurationManager.AppSettings["trelloToken"];

            var togglWorkspaceId = 605632;
            var togglClientId = 15242883;

            var trelloKey = "3ba00ca224256611c3ccbac183364259";
            var trelloBoardId = "5596a7b7ac88c077383d281c";

            var trelloWorkspace = new Trello.Workspace(trelloKey, trelloToken, trelloBoardId);
            var azureTableClient = new Azure.TableClient(azureConnectionString);

            TogglToAzure(togglApiKey, togglWorkspaceId, togglClientId, azureTableClient);

            TrelloToAzure(azureTableClient, trelloWorkspace);
        }

        private static void TogglToAzure(string togglApiKey, int togglWorkspaceId, int togglClientId, Azure.TableClient azureTableClient)
        {
            TimeEntryTable table = new TimeEntryTable(azureTableClient);

            if (table.Exists())
            {
                TogglToAzureLastMonth(table, togglApiKey, togglWorkspaceId, togglClientId);
            }
            else
            {
                table.Create();
                TogglToAzureFromJan2015(table, togglApiKey, togglWorkspaceId, togglClientId);
            }
        }

        private static string GetKeyFromArgsOrAppSettings(string[] args, int index, string name)
        {
            if (args.Length < index && ConfigurationManager.AppSettings[name] == null) throw new ArgumentNullException(name);

            if (args.Length == 0) return ConfigurationManager.AppSettings[name];

            return args[index];
        }

        private static void AzureDeleteTimeEntryTableIfExists(Azure.TableClient client)
        {
            TimeEntryTable table = new TimeEntryTable(client);

            table.DeleteIfExists();
        }

        private static void TogglToAzureFromJan2015(TimeEntryTable table, string togglApiKey, int togglWorkspaceId, int togglClientId)
        {
            var until = DateTime.Now;
            var since = new DateTime(2015, 1, 1);

            TogglToAzure(table, togglApiKey, togglWorkspaceId, togglClientId, until, since);
        }

        private static void TogglToAzureLastMonth(TimeEntryTable table, string togglApiKey, int togglWorkspaceId, int togglClientId)
        {
            var until = DateTime.Now;
            var since = until.AddMonths(-1);

            TogglToAzure(table, togglApiKey, togglWorkspaceId, togglClientId, until, since);
        }

        private static void TogglToAzure(TimeEntryTable table, string apiKey, int workspaceId, int clientId, DateTime until, DateTime since)
        {
            var workspace = new Toggl.Workspace(apiKey, workspaceId);

            List<ReportTimeEntry> reportTimeEntries = workspace.GetReportTimeEntries(clientId, since, until);

            foreach (ReportTimeEntry timeEntry in reportTimeEntries)
            {
                TimeEntryEntity entity = new TimeEntryEntity(
                    timeEntry.Start, timeEntry.Id, timeEntry.ProjectId, timeEntry.TaskId, timeEntry.TaskName, timeEntry.UserName, timeEntry.Billable);

                table.BatchInsertOrReplace(entity);

                //Use for testing to remove batch complexity.
                //table.InsertOrReplace(entity);
            }

            table.ExecuteBatch();
        }

        private static void TrelloToAzure(Azure.TableClient tableClient, Trello.Workspace workspace)
        {
            List<Card> cards = workspace.GetCards();
            List<Label> labels = workspace.GetLabels();
            List<List> lists = workspace.GetLists();

            CardTable table = new CardTable(tableClient);
            table.CreateIfNotExists();

            foreach (Card card in cards)
            {

                var listName = List.GetList(card.IdList, lists).Name;
                var nameLabels = Label.GetNameLabels(card.IdLabels, labels);
                var cardName = card.Name;
                var cardId = card.Id;

                CardEntity entity = new CardEntity(cardId, listName, nameLabels, cardName);

                table.BatchInsertOrReplace(entity);
            }

            table.ExecuteBatch();
        }
    }
}
