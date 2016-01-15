using Gothandy.Tables.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;

namespace Vincente.Data.Tables
{
    public class ActivityByDay : ITableRead<Activity>
    {
        private ActivityBase activityBase;

        public ActivityByDay(ActivityBase activityBase)
        {
            this.activityBase = activityBase;
        }

        public IEnumerable<Activity> Query()
        {
            return
                from a in activityBase.Query()
                group a by new
                {
                    a.DomId,
                    a.Month,
                    a.Epic,
                    a.IsBlocked,
                    a.BlockedReason,
                    a.ReuseDA,
                    a.ReuseFCP,
                    a.ListIndex,
                    a.ListName,
                    a.CardId,
                    a.Name,
                    a.UserId,
                    a.UserName,
                    a.TeamName,
                    a.Invoice,
                    a.TaskId,
                    a.Start,
                    a.End,
                    a.IsWip,
                }
                into g
                select new Activity()
                {
                    Month = g.Key.Month,
                    Epic = g.Key.Epic,
                    ReuseDA = g.Key.ReuseDA,
                    ReuseFCP = g.Key.ReuseFCP,
                    ListIndex = g.Key.ListIndex,
                    ListName = g.Key.ListName,
                    CardId = g.Key.CardId,
                    DomId = g.Key.DomId,
                    Name = g.Key.Name,
                    UserId = g.Key.UserId,
                    UserName = g.Key.UserName,
                    TeamName = g.Key.TeamName,
                    Invoice = g.Key.Invoice,
                    TaskId = g.Key.TaskId,
                    IsBlocked = g.Key.IsBlocked,
                    BlockedReason = g.Key.BlockedReason,
                    Blocked = g.Sum(a => a.Blocked),
                    IsWip = g.Key.IsWip,
                    Wip = g.Sum(a => a.Wip),
                    Start = g.Key.Start,
                    End = g.Key.End,
                    Billable = g.Sum(a => a.Billable)
                };
        }
    }
}
