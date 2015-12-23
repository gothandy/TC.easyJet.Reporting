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

        public JoinClient()
        {
            var azureConnectionString = ConfigurationManager.AppSettings["azureConnectionString"];
            var tableClient = new TableClient(azureConnectionString);
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
                       Month = timeEntry.Month,
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
                where timeEntry.Housekeeping != null && timeEntry.Month > new System.DateTime(2015, 6, 30)
                select new JoinModel()
                {
                    Month = timeEntry.Month,
                    Epic = "Housekeeping",
                    ListIndex = null,
                    ListName = null,
                    DomId = null,
                    Name = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    Billable = ((decimal)timeEntry.Billable) / 100,
                    Invoice = timeEntry.Month
                };
        }

        public IEnumerable<JoinModel> GetStories()
        {
            return
                from timeEntry in GroupByMonth(timeEntryTable.Query())
                join card in cardTable.Query()
                on timeEntry.DomId equals card.DomId
                orderby timeEntry.Month, card.Epic, card.ListIndex, card.Name, timeEntry.UserName
                select new JoinModel()
                {
                    Month = timeEntry.Month,
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
                    timeEntry.Month > new System.DateTime(2015, 6, 30)
                orderby timeEntry.Month
                select new TimeEntryEntity()
                {
                    Month = timeEntry.Month,
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
                    e.Month,
                    e.UserName,
                    e.DomId,
                    e.Housekeeping

                } into g
                select new TimeEntryEntity()
                {
                    Month = g.Key.Month,
                    UserName = g.Key.UserName,
                    DomId = g.Key.DomId,
                    Housekeeping = g.Key.Housekeeping,
                    Billable = g.Sum(e => e.Billable)
                };

            return result;
        }
    }
}