using Gothandy.Tables.AzureTables.Interfaces;
using Gothandy.DateTime;
using System;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;

namespace Vincente.Azure.Converters
{
    internal class TimeEntryConverter : IConverter<TimeEntry, TimeEntryEntity>
    {
        public TimeEntry Read(TimeEntryEntity azureEntity)
        {
            return new TimeEntry()
            {
                Id = Convert.ToInt64(azureEntity.RowKey),
                Billable = ((decimal)azureEntity.Billable) / 100,
                DomId = azureEntity.DomId,
                Housekeeping = azureEntity.Housekeeping,
                Month = GetMonth(azureEntity.Start),
                Week = GetWeek(azureEntity.Start),
                Start = azureEntity.Start.GetValueOrDefault(),
                TaskId = azureEntity.TaskId.GetValueOrDefault(),
                Timestamp = azureEntity.Timestamp.LocalDateTime,
                UserId = azureEntity.UserId,
                UserName = azureEntity.UserName,
                TeamName = azureEntity.TeamName,
                Description = azureEntity.Description
            };
        }

        private static DateTime GetMonth(DateTime? start)
        {
            return new DateTime(start.GetValueOrDefault().Year,
                                start.GetValueOrDefault().Month, 1);
        }

        private static DateTime GetWeek(DateTime? start)
        {
            return start.GetValueOrDefault().GetStartOfWeek(DayOfWeek.Monday);
        }

        public TimeEntryEntity Write(TimeEntry timeEntry)
        {
            var partitionKey = timeEntry.Start.ToString("yyyy MM");
            var rowKey = timeEntry.Id.ToString();

            return new TimeEntryEntity()
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Billable = (long)(timeEntry.Billable * 100),
                DomId = timeEntry.DomId,
                Housekeeping = timeEntry.Housekeeping,
                Start = timeEntry.Start,
                TaskId = timeEntry.TaskId,
                UserId = timeEntry.UserId,
                UserName = timeEntry.UserName,
                TeamName = timeEntry.TeamName,
                Description = timeEntry.Description
            };
        }
    }
}
