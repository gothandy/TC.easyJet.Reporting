using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Formula;
using Vincente.Trello.DataObjects;

namespace Vincente.TrelloConsoleApp.Operations
{

    public class TrelloToData
    {
        public static List<Card> Execute(List<TrelloCard> trelloCards, List<Label> labels, List<List> lists, List<Replace> replaces)
        {
            return
                (from trelloCard in trelloCards
                 select GetDataFromTrello(trelloCard, labels, lists, replaces)).ToList();
        }

        private static Card GetDataFromTrello(TrelloCard card, List<Label> labels, List<List> lists, List<Replace> replaces)
        {
            var list = List.GetList(card.IdList, lists);
            var listIndex = lists.IndexOf(list);
            var listName = list.Name;
            var nameLabels = Label.GetNameLabels(card.IdLabels, labels);
            var cardName = card.Name;
            var cardId = card.Id;
            var epic = FromLabels.GetEpic(nameLabels);
            var blocked = FromLabels.GetBlocked(nameLabels).Count > 0;
            var newCard = new Vincente.Data.Entities.Card()
                {
                    DomId = FromName.GetDomID(cardName),
                    Id = card.Id,
                    ListIndex = listIndex,
                    ListName = GetNewValue(listName, replaces),
                    Name = FromName.GetShortName(cardName, epic),
                    Epic = epic,
                    Blocked = blocked,
                    BlockedReason = FromLabels.GetBlockedReason(nameLabels),
                    ReuseDA = FromLabels.GetReuseDA(nameLabels),
                    ReuseFCP = FromLabels.GetReuseFCP(nameLabels),
                    Invoice = FromLabels.GetInvoice(nameLabels, listName)
                };

            return newCard;
        }

        private static string GetNewValue(string oldValue, List<Replace> replaces)
        {
            var matches =
                from r in replaces
                where r.Old == oldValue
                select r;

            if (matches.Count() == 0) return oldValue;

            return matches.First().New;
        }
    }
}
