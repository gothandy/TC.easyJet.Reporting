using Azure.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace Azure.Tables
{
    public class CardTable
    {
        private CloudTable table;
        private TableBatchOperation batchOperation;

        public CardTable(TableClient client)
        {
            table = client.GetTable("Cards");
            batchOperation = new TableBatchOperation();
        }

        public IEnumerable<CardEntity> Query()
        {
            TableQuery<CardEntity> query = new TableQuery<CardEntity>();
            return table.ExecuteQuery(query);
        }

        public void BatchInsertOrReplace(CardEntity entity)
        {

            batchOperation.InsertOrReplace(entity);

            if (batchOperation.Count == 100)
            {
                table.ExecuteBatch(batchOperation);
                batchOperation = new TableBatchOperation();
            }
        }

        public void ExecuteBatch()
        {
            table.ExecuteBatch(batchOperation);
        }

        public void CreateIfNotExists()
        {
            table.CreateIfNotExists();
        }
    }
}
