using Azure.Tables;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using TC.easyJet.Reporting;
using Toggl.DataObjects;
using TrelloToAzure.Azure;
using TrelloToAzure.Trello.DataObjects;
using System.Configuration;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureAccountKey = GetKeyFromArgsOrAppSettings(args, 0, "azureAccountKey");
            var togglApiKey = GetKeyFromArgsOrAppSettings(args, 1, "togglApiKey");
            var trelloToken = GetKeyFromArgsOrAppSettings(args, 2, "trelloToken");

            var togglWorkspaceId = 605632;
            var togglClientId = 15242883;

            var trelloKey = "3ba00ca224256611c3ccbac183364259";
            var trelloBoardId = "5596a7b7ac88c077383d281c";

            TimeEntryTable table = new TimeEntryTable(azureAccountKey);

            if (table.Exists())
            {
                TogglToAzureLastMonth(table, togglApiKey, togglWorkspaceId, togglClientId);
            }
            else
            {
                table.Create();
                TogglToAzureFromJan2015(table, togglApiKey, togglWorkspaceId, togglClientId);
            }

            TrelloToAzure(azureAccountKey, trelloToken, trelloKey, trelloBoardId);
        }

        private static string GetKeyFromArgsOrAppSettings(string[] args, int index, string name)
        {
            if (args.Length < index && ConfigurationManager.AppSettings[name] == null) throw new ArgumentNullException(name);

            if (args.Length == 0) return ConfigurationManager.AppSettings[name];

            return args[index];
        }

        private static void AzureDeleteTimeEntryTableIfExists(string azureAccountKey)
        {
            TimeEntryTable table = new TimeEntryTable(azureAccountKey);

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

        private static void TrelloToAzure(string accountKey, string trelloToken, string trelloKey, string trelloBoardId)
        {
            var workspace = new Trello.Workspace(trelloKey, trelloToken);

            List<Card> cards = workspace.GetCards(trelloBoardId);
            List<Label> labels = workspace.GetLabels(trelloBoardId);
            List<List> lists = workspace.GetLists(trelloBoardId);

            CardTable table = new CardTable(accountKey);

            foreach (Card card in cards)
            {

                var listName = List.GetList(card.IdList, lists).Name;
                var nameLabels = Label.GetNameLabels(card.IdLabels, labels);
                var cardName = card.Name;
                var cardId = card.Id;

                CardEntity entity = new CardEntity(trelloBoardId, cardId, listName, nameLabels, cardName);

                table.BatchInsertOrReplace(entity);
            }

            table.ExecuteBatch();
        }
    }
}
