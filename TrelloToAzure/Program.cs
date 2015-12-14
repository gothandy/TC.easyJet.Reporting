using Azure.Tables;
using System;
using System.Collections.Generic;
using TrelloToAzure.Azure;
using TrelloToAzure.Trello;
using TrelloToAzure.Trello.DataObjects;

namespace TrelloToAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureAccountKey = args[0];
            var trelloToken = args[1];
            var trelloKey = "3ba00ca224256611c3ccbac183364259";
            var trelloBoardId = "5596a7b7ac88c077383d281c";

            TrelloToAzure(azureAccountKey, trelloToken, trelloKey, trelloBoardId);
        }

        private static void TrelloToAzure(string accountKey, string trelloToken, string trelloKey, string trelloBoardId)
        {
            var workspace = new Workspace(trelloKey, trelloToken);

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
