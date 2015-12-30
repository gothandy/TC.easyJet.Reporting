using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.Formula;
using Vincente.Trello.DataObjects;

namespace TrelloConsoleApp
{

    public class TrelloToData
    {
        public static TrelloToDataResults Execute(ICardWrite table, List<TrelloCard> trelloCards, List<Label> labels, List<List> lists)
        {

            var currentCards =
                (from trelloCard in trelloCards
                 select GetDataFromTrello(trelloCard, labels, lists)).ToList();

            var previousCards = table.Query().ToList();

            TrelloToDataResults results = new TrelloToDataResults();

            InsertReplaceOrIgnore(table, currentCards, previousCards, results);

            Delete(table, trelloCards, previousCards, results);

            table.BatchComplete();

            return results;
        }

        private static void InsertReplaceOrIgnore(ICardWrite table, List<Card> currentCards, List<Card> previousCards, TrelloToDataResults results)
        {
            foreach (Card currentCard in currentCards)
            {

                var previousCard = (from c in previousCards
                                    where c.Id == currentCard.Id
                                    select c).FirstOrDefault();

                if (previousCard == null)
                {
                    results.Inserted++;
                    table.BatchInsert(currentCard);
                }
                else if (previousCard.Equals(currentCard))
                {
                    results.Ignored++;
                }
                else
                {
                    results.Replaced++;
                    table.BatchReplace(currentCard);
                }
            }
        }

        private static void Delete(ICardWrite table, List<TrelloCard> trelloCards, List<Card> previousCards, TrelloToDataResults results)
        {
            foreach (Card previousCard in previousCards)
            {
                var currentCard =
                    (from c in trelloCards
                     where c.Id == previousCard.Id
                     select c).FirstOrDefault();

                if (currentCard == null)
                {
                    results.Deleted++;
                    table.BatchDelete(previousCard);
                }

            }
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
