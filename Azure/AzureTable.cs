using Gothandy.Tables.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Interfaces;
using Vincente.Data.Interfaces;

namespace Vincente.Azure
{
    public interface IBatchCommand
    {
        void Execute(TableBatchOperation batchOperation, TableEntity entity);
    }

    public class BatchInsert : IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            batchOperation.Insert(entity);
        }
    }

    public class BatchReplace : IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            entity.ETag = "*"; // Always overwrite (ignore concurrency).
            batchOperation.Replace(entity);
        }
    }

    public class BatchInsertOrReplace : IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            batchOperation.InsertOrReplace(entity);
        }
    }

    public class BatchDelete: IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            entity.ETag = "*"; // Always overwrite (ignore concurrency).
            batchOperation.Delete(entity);
        }
    }


    public abstract class AzureTable<T, U> : ITableWrite<T> 
        where U : TableEntity, new() 
    {
        private CloudTable table;
        private TableBatchOperation batchOperation;
        private IConverter<T, U> converter;
        private string lastPartitionKeyUsed;

        internal AzureTable(CloudTable table, IConverter<T, U> converter)
        {
            this.table = table;
            this.converter = converter;
        }

        public IEnumerable<T> Query()
        {
            TableQuery<U> query = new TableQuery<U>();

            IEnumerable<U> result = table.ExecuteQuery(query);

            return from entity in result select converter.Read(entity);
        }

        public void BatchInsert(T item)
        {
            CommandExecute(new BatchInsert(), item);
        }

        public void BatchReplace(T item)
        {
            CommandExecute(new BatchReplace(), item);
        }

        public void BatchInsertOrReplace(T item)
        {
            CommandExecute(new BatchInsertOrReplace(), item);
        }

        public void BatchDelete(T item)
        {
            CommandExecute(new BatchDelete(), item);
        }
        public void BatchComplete()
        {
            if (batchOperation != null) table.ExecuteBatch(batchOperation);
        }

        private void CommandExecute(IBatchCommand batchCommand, T item)
        {
            if (batchOperation == null) batchOperation = new TableBatchOperation();

            U azureEntity = converter.Write(item);

            if (partitionKeyChanged(azureEntity.PartitionKey)) ExecuteBatchAndCreateNew();

            batchCommand.Execute(batchOperation, azureEntity);

            if (batchOperation.Count == 100) ExecuteBatchAndCreateNew();
        }

        private void ExecuteBatchAndCreateNew()
        {
            table.ExecuteBatch(batchOperation);
            batchOperation = new TableBatchOperation();
        }

        private bool partitionKeyChanged(string partitionKey)
        {
            bool value = true;

            if (lastPartitionKeyUsed == null) value = false;
            if (lastPartitionKeyUsed == partitionKey) value = false;

            lastPartitionKeyUsed = partitionKey;
            return value;
        }
    }
}
