using Azure.Tables;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using TC.easyJet.Reporting;
using Toggl.DataObjects;
using TrelloToAzure.Azure;
using TrelloToAzure.Trello.DataObjects;
using System.Configuration;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureAccountKey = GetKey(args, 0, "azureAccountKey");
            var togglApiKey = GetKey(args, 1, "togglApiKey");
            var trelloToken = GetKey(args, 2, "trelloToken");

            var togglWorkspaceId = 605632;
            var togglClientId = 15242883;

            var since = new DateTime(2015, 12, 1);
            var until = new DateTime(2015, 12, 31);
                        
            var trelloKey = "3ba00ca224256611c3ccbac183364259";
            var trelloBoardId = "5596a7b7ac88c077383d281c";

            TogglToAzure(azureAccountKey, togglApiKey, togglWorkspaceId, togglClientId, since, until);

            TrelloToAzure(azureAccountKey, trelloToken, trelloKey, trelloBoardId);
        }

        private static string GetKey(string[] args, int index, string name)
        {
            if (args.Length == 0) return ConfigurationManager.AppSettings[name];

            return args[index];
        }

        private static void TogglToAzure(string accountKey, string apiKey, int workspaceId, int clientId, DateTime since, DateTime until)
        {
            TimeEntryTable table = new TimeEntryTable(accountKey);

            var workspace = new Toggl.Workspace(apiKey, workspaceId);

            List<ReportTimeEntry> reportTimeEntries = workspace.GetReportTimeEntries(clientId, since, until);

            foreach (ReportTimeEntry timeEntry in reportTimeEntries)
            {
                TimeEntryEntity entity = new TimeEntryEntity(
                    timeEntry.TaskId, timeEntry.Id, timeEntry.ProjectId, timeEntry.TaskName, timeEntry.Start, timeEntry.UserName, timeEntry.Billable);

                table.InsertOrReplace(entity);
            }
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
