using System;
using System.Collections.Generic;
using System.Configuration;
using Vincente.Azure.Entities;
using Vincente.Azure.Tables;
using Vincente.Trello.DataObjects;

namespace TrelloConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var azureConnectionString = CheckAndGetAppSettings("azureConnectionString");
            var azureTableClient = new Vincente.Azure.TableClient(azureConnectionString);

            var trelloToken = CheckAndGetAppSettings("trelloToken");
            var trelloKey = "3ba00ca224256611c3ccbac183364259";
            var trelloBoardId = "5596a7b7ac88c077383d281c";
            var trelloWorkspace = new Vincente.Trello.Workspace(trelloKey, trelloToken, trelloBoardId);

            TrelloToAzure(azureTableClient, trelloWorkspace);
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];

            if (value == null) throw new ArgumentNullException(name);

            return value;
        }

        private static void TrelloToAzure(Vincente.Azure.TableClient tableClient, Vincente.Trello.Workspace workspace)
        {
            List<Card> cards = workspace.GetCards();
            List<Label> labels = workspace.GetLabels();
            List<List> lists = workspace.GetLists();

            Console.Out.WriteLine("{0} Cards Found", cards.Count);
            Console.Out.WriteLine("{0} Labels Found", labels.Count);
            Console.Out.WriteLine("{0} Lists Found", lists.Count);

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
