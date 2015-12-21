﻿using System;
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

            var azureConnectionString = CheckAndGetAppSettings("azureConnectionString");
            var togglApiKey = CheckAndGetAppSettings("togglApiKey");
            var trelloToken = CheckAndGetAppSettings("trelloToken");

            var togglWorkspaceId = 605632;
            var togglClientId = 15242883;

            var trelloKey = "3ba00ca224256611c3ccbac183364259";
            var trelloBoardId = "5596a7b7ac88c077383d281c";

            var trelloWorkspace = new Trello.Workspace(trelloKey, trelloToken, trelloBoardId);
            var azureTableClient = new Azure.TableClient(azureConnectionString);

            TogglToAzure(togglApiKey, togglWorkspaceId, togglClientId, azureTableClient);

            TrelloToAzure(azureTableClient, trelloWorkspace);
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];

            if (value == null) throw new ArgumentNullException(name);

            return value;
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
            Console.Out.WriteLine("Toggl time entries from {0} to {1}", since, until);

            var workspace = new Toggl.Workspace(apiKey, workspaceId);

            List<ReportTimeEntry> reportTimeEntries = workspace.GetReportTimeEntries(clientId, since, until);

            Console.Out.WriteLine("{0} Time Entries Found.", reportTimeEntries.Count);

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

            Console.Out.WriteLine("{0} Cards Found.", cards.Count);
            Console.Out.WriteLine("{0} Labels Found.", labels.Count);
            Console.Out.WriteLine("{0} Lists Found.", lists.Count);

            CardTable table = new CardTable(tableClient);
            table.CreateIfNotExists();

            foreach (Card card in cards)
            {
                var list = List.GetList(card.IdList, lists);
                var listIndex = lists.IndexOf(list);
                var listName = list.Name;
                var nameLabels = Label.GetNameLabels(card.IdLabels, labels);
                var cardName = card.Name;
                var cardId = card.Id;

                CardEntity entity = new CardEntity(cardId, listIndex, listName, nameLabels, cardName);

                table.BatchInsertOrReplace(entity);
            }

            table.ExecuteBatch();
        }
    }
}
