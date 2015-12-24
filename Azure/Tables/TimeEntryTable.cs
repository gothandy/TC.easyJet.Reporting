using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;
using Vincente.Data.Tables;

namespace Vincente.Azure.Tables
{
    public class TimeEntryTable : ITimeEntryTable
    {
        private CloudTable table;
        private TableBatchOperation batchOperation;
        private string lastPartitionKeyUsed;

        public TimeEntryTable(TableClient client)
        {
            table = client.GetTable("TimeEntries");
            batchOperation = new TableBatchOperation();
        }

        public IEnumerable<TimeEntryEntity> Query()
        {
            TableQuery<TimeEntryEntity> query = new TableQuery<TimeEntryEntity>();

            return table.ExecuteQuery(query);
        }

        public bool Exists()
        {
            return table.Exists();
        }

        public void InsertOrReplace(TimeEntryEntity entity)
        {
            TableOperation operation = TableOperation.InsertOrReplace(entity);
            table.Execute(operation);
        }

        public void BatchInsertOrReplace(TimeEntry timeEntry)
        {
            var partitionKey = timeEntry.Start.ToString("yyyy MM");
            var rowKey = timeEntry.Id.ToString();

            TimeEntryEntity entity = new TimeEntryEntity(partitionKey, rowKey)
            {
                Billable = (long)(timeEntry.Billable * 100),
                DomId = timeEntry.DomId,
                Housekeeping = timeEntry.Housekeeping,
                Start = timeEntry.Start,
                TaskId = timeEntry.TaskId,
                UserName = timeEntry.UserName
            };

            if (partitionKeyChanged(entity)) ExecuteBatchAndCreateNew();

            batchOperation.InsertOrReplace(entity);

            if (batchOperation.Count == 100) ExecuteBatchAndCreateNew();
        }

        private void ExecuteBatchAndCreateNew()
        {
            table.ExecuteBatch(batchOperation);
            batchOperation = new TableBatchOperation();
        }

        private bool partitionKeyChanged(TimeEntryEntity entity)
        {
            var value = true;

            if (lastPartitionKeyUsed == null) value = false;
            if (lastPartitionKeyUsed == entity.PartitionKey) value = false;

            lastPartitionKeyUsed = entity.PartitionKey;
            return value;
        }

        public void Create()
        {
            table.Create();
        }

        public void DeleteIfExists()
        {
            table.DeleteIfExists();
        }

        public void ExecuteBatch()
        {
            table.ExecuteBatch(batchOperation);
        }
    }
}
