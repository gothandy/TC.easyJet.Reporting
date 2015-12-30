using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Interfaces;
using Vincente.Formula;
using Vincente.Trello.DataObjects;

namespace TrelloConsoleApp
{

    public class TrelloToData
    {
        public static TrelloToDataResults Execute(ICardWrite table, List<TrelloCard> currentCards, List<Label> labels, List<List> lists)
        {
            TrelloToDataResults results = new TrelloToDataResults();

            var previousCards = table.Query().ToList();

            foreach (TrelloCard currentCard in currentCards)
            {
                var previousCard = (from c in previousCards
                                    where c.Id == currentCard.Id
                                    select c).FirstOrDefault();

                var data = GetDataFromTrello(currentCard, labels, lists);

                if (previousCard == null)
                {
                    results.Inserted++;
                    table.BatchInsert(data);
                }
                else if (previousCard.Equals(data))
                {
                    results.Ignored++;
                }
                else
                {
                    results.Replaced++;
                    table.BatchReplace(data);
                }
            }

            foreach (Vincente.Data.Entities.Card previousCard in previousCards)
            {
                var currentCard = 
                    (from c in currentCards
                     where c.Id == previousCard.Id
                     select c).FirstOrDefault();

                if (currentCard == null)
                {
                    results.Deleted++;
                    table.BatchDelete(previousCard);
                }

            }

            table.BatchComplete();

            return results;
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

    public class TrelloToDataResults
    {
        public int Replaced { get; set; }
        public int Inserted { get; set; }
        public int Deleted { get; set; }
        public int Ignored { get; set; }
    }
}
