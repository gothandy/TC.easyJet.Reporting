using Gothandy.Tables.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Data.Tables
{
    public class CardsWithTime : ITableRead<CardWithTime>
    {
        private ICardRead cards;
        private TimeEntriesByMonth timeEntriesByMonth;

        public CardsWithTime(ICardRead cards, TimeEntriesByMonth timeEntriesByMonth)
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
                    IsBlocked = card.Blocked,
                    BlockedReason = card.BlockedReason,
                    ReuseDA = card.ReuseDA,
                    ReuseFCP = card.ReuseFCP,
                    ListIndex = card.ListIndex,
                    ListName = card.ListName,
                    CardId = card.Id,
                    DomId = timeEntry.DomId,
                    Name = card.Name,
                    UserName = timeEntry.UserName,
                    TeamName = timeEntry.TeamName,
                    Billable = timeEntry.Billable,
                    Invoice = card.Invoice,
                    TaskId = timeEntry.TaskId
                };
        }
    }
}
