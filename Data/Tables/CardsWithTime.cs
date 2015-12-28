using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Data.Tables
{
    public class CardsWithTime : ITableRead<CardWithTime>
    {
        private ITableRead<Card> cards;
        private ITableRead<TimeEntry> timeEntriesByMonth;

        public CardsWithTime(ITableRead<Card> cards, ITableRead<TimeEntry> timeEntriesByMonth)
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
