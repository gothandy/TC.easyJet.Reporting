using System;
using Microsoft.WindowsAzure.Storage.Table;
using Formula;

namespace Azure.Entities
{
    public class TimeEntryEntity : TableEntity
    {
        public long? TaskId { get; set; }
        public string DomId { get; set; }
        public DateTime? Month { get; set; }
        public string UserName { get; set; }
        public long Billable { get; set; }
        public string Housekeeping { get; set; }
        
        public TimeEntryEntity(DateTime? start, long? id)
        {
            if (start == null) throw new ArgumentNullException("start");

            var month = MonthFromStart(start);

            var partitionKey = month.ToString("yyyy MM");

            this.PartitionKey = partitionKey;
            this.RowKey = id.ToString();
            this.Month = month;
        }

        private static DateTime MonthFromStart(DateTime? start)
        {
            return new DateTime(start.GetValueOrDefault().Year, start.GetValueOrDefault().Month, 1);
        }

        public TimeEntryEntity() { }

        public TimeEntryEntity(DateTime? start, long? id, long? projectId, long? taskId, string taskName, string userName, long? billable) : this(start, id)
        {
            this.TaskId = taskId;
            this.UserName = userName;
            this.Billable = billable.GetValueOrDefault();
            this.DomId = FromName.GetDomID(taskName);
            this.Housekeeping = FromProject.IfHouseKeepingReturnTaskName(projectId, taskName);
        }
    }
}
