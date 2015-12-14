using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace TC.easyJet.Reporting
{
    public class TimeEntryEntity : TableEntity
    {
        public long? ProjectId { get; set; }
        public DateTime? Start { get; set; }
        public string TaskName { get; set; }
        public string UserName { get; set; }
        public long Billable { get; set; }
        public DateTime? Month { get; set; }
        public string DomId { get; set; }

        public TimeEntryEntity(long? taskId, long? id)
        {
            this.PartitionKey = taskId.ToString();
            this.RowKey = id.ToString();
        }

        public TimeEntryEntity() { }

        public TimeEntryEntity(long? taskId, long? id, long? projectId, string taskName, DateTime? start, string userName, long? billable) : this(taskId, id)
        {
            this.ProjectId = projectId.GetValueOrDefault();
            this.TaskName = taskName;
            this.Start = start.GetValueOrDefault();
            this.UserName = userName;
            this.Billable = billable.GetValueOrDefault();

            UpdateDomId();
            UpdateMonth();
        }

        public bool UpdateMonth()
        {
            var update = false;
            var month = new DateTime(this.Start.GetValueOrDefault().Year, this.Start.GetValueOrDefault().Month, 1);

            if (this.Month != month)
            {
                this.Month = month;
                update = true;
            }

            return update;
        }

        public bool UpdateDomId()
        {
            var update = false;
            var domId = GetDomIdFromName(this.TaskName);

            if (this.DomId != domId)
            {
                this.DomId = domId;
                update = true;
            }

            return update;
        }

        private static string GetDomIdFromName(string taskName)
        {
            string[] words = taskName.Split(' ');

            foreach (string word in words)
            {

                if (word.StartsWith("20") && word.Contains(".") && word.Length > 9)
                {
                    return word;
                }
            }

            return null;
        }
    }
}
