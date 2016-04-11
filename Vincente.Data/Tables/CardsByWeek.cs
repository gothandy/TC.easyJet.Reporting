using Gothandy.Tables.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Data.Tables
{
    public class CardsByWeek : ITableRead<Activity>
    {
        private ICardRead cards;
        private TimeEntriesByWeek timeEntriesByWeek;

        public CardsByWeek(ICardRead cards, TimeEntriesByWeek timeEntriesByWeek)
        {
            this.cards = cards;
            this.timeEntriesByWeek = timeEntriesByWeek;
        }

        public IEnumerable<Activity> Query()
        {
            return
                from timeEntry in timeEntriesByWeek.Query()
                join card in cards.Query()
                on timeEntry.DomId equals card.DomId
                orderby timeEntry.Week, card.Epic, card.ListIndex, card.Name, timeEntry.UserName
                select new Activity()
                {
                    Week = timeEntry.Week,
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
