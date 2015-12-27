using System.Collections.Generic;
using Vincente.Data.Interfaces;
using Vincente.Formula;
using Vincente.Trello.DataObjects;

namespace TrelloConsoleApp
{
    public class TrelloToData
    {
        public static void Execute(ITableWrite<Vincente.Data.Entities.Card> table, List<TrelloCard> cards, List<Label> labels, List<List> lists)
        {
            foreach (TrelloCard card in cards)
            {
                Vincente.Data.Entities.Card data = GetDataFromTrello(card, labels, lists);

                table.BatchInsertOrReplace(data);
            }

            table.BatchComplete();
        }

        private static Vincente.Data.Entities.Card GetDataFromTrello(TrelloCard card, List<Label> labels, List<List> lists)
        {
            var list = List.GetList(card.IdList, lists);
            var listIndex = lists.IndexOf(list);
            var listName = list.Name;
            var nameLabels = Label.GetNameLabels(card.IdLabels, labels);
            var cardName = card.Name;
            var cardId = card.Id;

            return
                new Vincente.Data.Entities.Card()
                {
                    DomId = FromName.GetDomID(cardName),
                    Id = card.Id,
                    ListIndex = listIndex,
                    ListName = listName,
                    Name = FromName.GetShortName(cardName),
                    Epic = FromLabels.GetEpic(nameLabels),
                    Invoice = FromLabels.GetInvoice(nameLabels, listName)
                };
        }
    }
}
