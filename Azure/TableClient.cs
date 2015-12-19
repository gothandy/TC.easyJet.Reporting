using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Azure
{
    public class TableClient
    {
        private CloudTableClient tableClient;

        public TableClient(string accountName, string accountKey)
        {
            var connectionString = String.Format(
                "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                accountName, accountKey);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            tableClient = storageAccount.CreateCloudTableClient();
        }

        public CloudTable GetTable(string tableName)
        {

            CloudTable table = tableClient.GetTableReference(tableName);

            return table;
        }
    }
}
