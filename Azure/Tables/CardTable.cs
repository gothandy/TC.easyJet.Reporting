using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using TrelloToAzure.Azure;

namespace Azure.Tables
{
    public class CardTable
    {
        private CloudTable table;
        private TableBatchOperation batchOperation;

        public CardTable(string accountKey)
        {
            table = GetTable(accountKey, "Cards");
            batchOperation = new TableBatchOperation();
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

        private static CloudTable GetTable(string accountKey, string tableName)
        {
            var connectionString = String.Format(
                "DefaultEndpointsProtocol=https;AccountName=tceasyjetreporting;AccountKey={0}",
                accountKey);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            //table.Delete();
            table.CreateIfNotExists();

            return table;
        }
    }
}
