using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.Data.Interfaces.ViewInterfaces;

namespace Vincente.Data.Tables
{
    public class CardsWithTime : ICardsWithTime
    {
        private ITableRead<Card> cards;
        private ITimeEntriesByMonth timeEntriesByMonth;

        public CardsWithTime(ITableRead<Card> cards, ITimeEntriesByMonth timeEntriesByMonth)
        {
            this.cards = cards;
            this.timeEntriesByMonth = timeEntriesByMonth;
        }

        public IEnumerable<CardWithTime> Query()
        {
            return
                from timeEntry in timeEntriesByMonth.Query()
                join card in cards.Query()
                on timeEntry.DomId equals card.DomId
                orderby timeEntry.Month, card.Epic, card.ListIndex, card.Name, timeEntry.UserName
                select new CardWithTime()
                {
                    Month = timeEntry.Month,
                    Epic = card.Epic,
                    ListIndex = card.ListIndex,
                    ListName = card.ListName,
                    CardId = card.Id,
                    DomId = timeEntry.DomId,
                    Name = card.Name,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    Invoice = card.Invoice
                };
        }
    }
}
