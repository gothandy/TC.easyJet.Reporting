using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vincente.Azure.Entities;
using Vincente.Azure.Interfaces;
using Vincente.Data.Entities;

namespace Vincente.Azure.Converters
{
    public class TimeEntryConverter : IConverter<TimeEntry, TimeEntryEntity>
    {
        public TimeEntry Read(TimeEntryEntity azureEntity)
        {
            return new TimeEntry()
            {
                Billable = ((decimal)azureEntity.Billable) / 100,
                DomId = azureEntity.DomId,
                Housekeeping = azureEntity.Housekeeping,
                Month = new DateTime(
                        azureEntity.Start.GetValueOrDefault().Year,
                        azureEntity.Start.GetValueOrDefault().Month, 1),
                Start = azureEntity.Start.GetValueOrDefault(),
                TaskId = azureEntity.TaskId.GetValueOrDefault(),
                Timestamp = azureEntity.Timestamp.LocalDateTime,
                UserName = azureEntity.UserName
            };
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
                UserName = timeEntry.UserName
            };
        }
    }
}
