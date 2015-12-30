﻿using Gothandy.Tables.Azure.BatchCommands;
using Gothandy.Tables.Azure.Interfaces;
using Gothandy.Tables.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;

namespace Gothandy.Tables.Azure
{

    public abstract class AzureTable<T, U> : ITableWrite<T> 
        where U : TableEntity, new() 
    {
        private CloudTable table;
        private TableBatchOperation batchOperation;
        private IConverter<T, U> converter;
        private string lastPartitionKeyUsed;

        public AzureTable(CloudTable table, IConverter<T, U> converter)
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
