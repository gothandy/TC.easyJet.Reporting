using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Vincente.Azure
{
    public class TableClient
    {
        private CloudTableClient tableClient;

        public TableClient(string connectionString)
        {
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
