using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TC.easyJet.Reporting;

namespace Azure.Tables
{
    public class TimeEntryTable
    {
        private CloudTable table;

        public TimeEntryTable(string accountKey)
        {
            table = GetTable(accountKey, "TimeEntries");
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

        public void InsertOrReplace(TimeEntryEntity entity)
        {
            // Can't use batch operations because of PartitionKey choice.
            TableOperation operation = TableOperation.InsertOrReplace(entity);

            table.Execute(operation);
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
