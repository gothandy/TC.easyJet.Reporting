using Gothandy.StartUp;
using Microsoft.WindowsAzure.Storage;
using System;
using Vincente.Azure.Tables;
using Vincente.Config;
using Gothandy.Trello;
using Vincente.WebJobs.TrelloToCard;

namespace TrelloConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigBuilder.Build();

            #region Dependancies
            var azureStorageAccount = CloudStorageAccount.Parse(config.azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureCardTable = azureTableClient.GetTableReference("Cards");

            var azureBlobClient = azureStorageAccount.CreateCloudBlobClient();
            var azureBlobContainer = azureBlobClient.GetContainerReference(config.azureBlobContainerName);
            var azureReplaceBlob = azureBlobContainer.GetBlockBlobReference(config.azureReplacePath);
            var azureReplaceTable = new ListNameTable(azureReplaceBlob);

            var trelloWorkspace = new Workspace(config.trelloKey, config.trelloToken, config.trelloBoardId);

            var trelloToCardWebJob = new TrelloToCardWebJob(trelloWorkspace, azureCardTable, azureReplaceTable);
            #endregion

            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            //trelloToCardWebJob.Execute();
            Console.WriteLine("This Web Job has been retired. See VincenteWebJob.");
        }
    }
}
