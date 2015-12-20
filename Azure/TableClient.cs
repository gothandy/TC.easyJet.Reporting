using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Vincente.Azure
{
    public class TableClient
    {
        private CloudTableClient tableClient;

        public TableClient(string accountName, string accountKey)
        {
            var connectionString = String.Format(
                "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                accountName, accountKey);

            connectionString = @"TableEndpoint=https://tceasyjetreportingdev.table.core.windows.net/;AccountName=tceasyjetreportingdev;AccountKey=oSOXb7rDbT6VhHbQdoWM92nfvuCpEru70HLotZZwtbdRdRIbv0ITLRrS+KfJWb5kikycWGDSVErfAQ1SzUtUgw==";

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
