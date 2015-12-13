using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TC.easyJet.Reporting;

namespace AzureMapping
{
    class Program
    {
        static void Main(string[] args)
        {

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=tceasyjetreporting;AccountKey=XWTDcIlFnS5ZHdaW0bhBvADcAtGlSQgBYlfeeYcynSeJaFRBnRCDp4nqRJUAymjHTSyGRvpDkqYMG7AcE+tvWw==";


            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("TimeEntries");

            TableQuery<TimeEntryEntity> query = new TableQuery<TimeEntryEntity>();

            foreach (TimeEntryEntity entity in table.ExecuteQuery(query))
            {
                var update = false;
                var domId = GetDomIdFromName(entity.TaskName);

                if (entity.DomId != domId)
                {
                    entity.DomId = domId;
                    update = true;
                }

                if (update)
                {
                    TableOperation operation = TableOperation.Replace(entity);
                    table.Execute(operation);
                }
            }
        }

        private static string GetDomIdFromName(string taskName)
        {
            string[] words = taskName.Split(' ');

            foreach (string word in words)
            {

                if (word.StartsWith("20") && word.Contains(".") && word.Length > 9)
                {
                    return word;
                }
            }

            return null;
        }
    }
}
