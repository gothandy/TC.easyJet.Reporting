using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Formula;
using Gothandy.Trello.DataObjects;
using TrelloConsoleApp.Models;

namespace Gothandy.TrelloConsoleApp.Operations
{
    public class TrelloDataToCardTable
    {
        public static List<Card> Execute(TrelloData trelloData)
        {
            return
                (from trelloCard in trelloData.Cards
                 select GetDataFromTrello(trelloCard, trelloData)).ToList();
        }

        private static Card GetDataFromTrello(TrelloCard card, TrelloData trelloData)
        {
            var list = List.GetList(card.IdList, trelloData.Lists);
            var listIndex = trelloData.Lists.IndexOf(list);
            var listName = list.Name;
            var nameLabels = Label.GetNameLabels(card.IdLabels, trelloData.Labels);
            var cardName = card.Name;
            var cardId = card.Id;
            var epic = FromLabels.GetEpic(nameLabels);
            var blocked = FromLabels.GetBlocked(nameLabels).Count > 0;
            var newCard = new Card()
                {
                    DomId = FromName.GetDomID(cardName),
                    Id = card.Id,
                    ListIndex = listIndex,
                    ListName = GetNewValue(listName, trelloData.Replaces),
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
