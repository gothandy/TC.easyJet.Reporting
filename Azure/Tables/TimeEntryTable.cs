using Azure.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Azure.Tables
{
    public class TimeEntryTable
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

        public void Replace(TimeEntryEntity entity)
        {
            TableOperation operation = TableOperation.Replace(entity);
            table.Execute(operation);
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

        public void BatchInsertOrReplace(TimeEntryEntity entity)
        {
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
