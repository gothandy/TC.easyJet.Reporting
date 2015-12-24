using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vincente.Azure;
using Vincente.Azure.Entities;
using Vincente.Azure.Tables;

namespace WebApp.Models
{
    public class JoinClient
    {
        private CardTable cardTable;
        private TimeEntryTable timeEntryTable;

        public JoinClient(TableClient tableClient)
        {
            cardTable = new CardTable(tableClient);
            timeEntryTable = new TimeEntryTable(tableClient);
        }

        public IEnumerable<JoinModel> GetJoinedData()
        {

            return from timeEntry in timeEntryTable.Query()
                   join card in cardTable.Query()
                   on timeEntry.DomId equals card.DomId
                   where card.Invoice == null
                   select new JoinModel()
                   {
                       ListIndex = card.ListIndex,
                       ListName = card.ListName,
                       Epic = card.Epic,
                       CardId = card.RowKey,
                       DomId = card.DomId,
                       Name = card.Name,
                       Month = new DateTime(timeEntry.Start.Value.Year, timeEntry.Start.Value.Month, 1),
                       UserName = timeEntry.UserName,
                       Billable = ((decimal)timeEntry.Billable) / 100
                   };
        }

        public IEnumerable<TimeEntryEntity> GetTimeEntriesByMonth()
        {
            var data = timeEntryTable.Query();

            return GroupByMonth(data);
        }

        public IEnumerable<CardEntity> GetCards()
        {
            return cardTable.Query();
        }

        public IEnumerable<JoinModel> GetHousekeeping()
        {
            return
                from timeEntry in GroupByMonth(timeEntryTable.Query())
                where timeEntry.Housekeeping != null && timeEntry.Start > new System.DateTime(2015, 6, 30)
                select new JoinModel()
                {
                    Month = new DateTime(timeEntry.Start.Value.Year, timeEntry.Start.Value.Month, 1),
                    Epic = "Housekeeping",
                    ListIndex = null,
                    ListName = null,
                    DomId = null,
                    Name = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    Billable = ((decimal)timeEntry.Billable) / 100,
                    Invoice = new DateTime(timeEntry.Start.Value.Year, timeEntry.Start.Value.Month, 1)
                };
        }

        public IEnumerable<JoinModel> GetStories()
        {
            return
                from timeEntry in GroupByMonth(timeEntryTable.Query())
                join card in cardTable.Query()
                on timeEntry.DomId equals card.DomId
                orderby timeEntry.PartitionKey, card.Epic, card.ListIndex, card.Name, timeEntry.UserName
                select new JoinModel()
                {
                    Month = new DateTime(timeEntry.Start.Value.Year, timeEntry.Start.Value.Month, 1),
                    Epic = card.Epic,
                    ListIndex = card.ListIndex,
                    ListName = card.ListName,
                    CardId = card.RowKey,
                    DomId = timeEntry.DomId,
                    Name = card.Name,
                    UserName = timeEntry.UserName,
                    Billable = ((decimal)timeEntry.Billable) / 100,
                    Invoice = card.Invoice
                };
        }

        public IEnumerable<TimeEntryEntity> GetOrphans()
        {
            return
                from timeEntry in timeEntryTable.Query()
                where
                    timeEntry.Housekeeping == null &&
                    timeEntry.DomId == null &&
                    timeEntry.Start > new System.DateTime(2015, 6, 30)
                orderby timeEntry.Start
                select new TimeEntryEntity()
                { 
                    Start = timeEntry.Start,
                    Housekeeping = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    TaskId = timeEntry.TaskId,
                };
        }

        private static IEnumerable<TimeEntryEntity> GroupByMonth(IEnumerable<TimeEntryEntity> query)
        {
            var result =
                from e in query
                group e by new
                {
                    Start = new DateTime(e.Start.GetValueOrDefault().Year, e.Start.GetValueOrDefault().Month, 1),
                    e.UserName,
                    e.DomId,
                    e.Housekeeping

                } into g
                select new TimeEntryEntity()
                {
                    Start = g.Key.Start,
                    UserName = g.Key.UserName,
                    DomId = g.Key.DomId,
                    Housekeeping = g.Key.Housekeeping,
                    Billable = g.Sum(e => e.Billable)
                };

            return result;
        }
    }
}