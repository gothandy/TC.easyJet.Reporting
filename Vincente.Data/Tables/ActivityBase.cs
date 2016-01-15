using Gothandy.Tables.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Data.Tables
{
    public class ActivityBase : ITableRead<Activity>
    {
        private ICardRead cards;
        private ITimeEntryRead timeEntries;

        public ActivityBase(ICardRead cards, ITimeEntryRead timeEntries)
        {
            this.cards = cards;
            this.timeEntries = timeEntries;
        }

        public IEnumerable<Activity> Query()
        {
            return
                from t in timeEntries.Query()
                join c in cards.Query()
                on t.DomId equals c.DomId
                select new Activity()
                {
                    Month = t.Month,
                    Epic = c.Epic,
                    ReuseDA = c.ReuseDA,
                    ReuseFCP = c.ReuseFCP,
                    ListIndex = c.ListIndex,
                    ListName = c.ListName,
                    CardId = c.Id,
                    DomId = c.DomId,
                    Name = c.Name,
                    UserId = t.UserId,
                    UserName = t.UserName,
                    TeamName = t.TeamName,
                    Invoice = c.Invoice,
                    TaskId = t.TaskId,
                    IsBlocked = c.Blocked,
                    BlockedReason = c.BlockedReason,
                    Blocked = c.Blocked.GetValueOrDefault() ? (decimal?)t.Billable : null,
                    Billable = t.Billable,
                    Start = new DateTime(t.Start.Year, t.Start.Month, t.Start.Day),
                    End = new DateTime(t.Start.Year, t.Start.Month, t.Start.Day).AddDays(1),
                    IsWip = (c.Invoice == null),
                    Wip = (c.Invoice == null) ? (decimal?)t.Billable : null
                };
        }
    }
}
