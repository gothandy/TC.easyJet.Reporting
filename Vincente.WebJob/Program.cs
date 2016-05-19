using Gothandy.StartUp;
using Gothandy.Trello;
using Microsoft.WindowsAzure.Storage;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Vincente.Azure.Tables;
using Vincente.Config;
using Vincente.WebJobs.TrelloToCard;

namespace Vincente.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            var config = ConfigBuilder.Build();

            #region Dependancies

            // Job Scheduler
            var azureStorageAccount = CloudStorageAccount.Parse(config.azureConnectionString);
            var azureBlobClient = azureStorageAccount.CreateCloudBlobClient();
            var azureBlobContainer = azureBlobClient.GetContainerReference(config.azureBlobContainerName);
            var azureLastRunTimesBlob = azureBlobContainer.GetBlockBlobReference("LastRunTimes.xml");
            var jobScheduler = new JobScheduler(azureLastRunTimesBlob);

            // Trello To Task
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureCardTable = azureTableClient.GetTableReference("Cards");
            var azureReplaceBlob = azureBlobContainer.GetBlockBlobReference(config.azureReplacePath);
            var azureReplaceTable = new ListNameTable(azureReplaceBlob);
            var trelloWorkspace = new Workspace(config.trelloKey, config.trelloToken, config.trelloBoardId);
            var trelloToCardWebJob = new TrelloToCardWebJob(trelloWorkspace, azureCardTable, azureReplaceTable);

            #endregion

            jobScheduler.Begin();

            jobScheduler.CheckAndRun(t => t.TrelloToCard, 5, trelloToCardWebJob.Execute);
            jobScheduler.CheckAndRun(t => t.TogglToTimeEntry, 15, () => { Console.WriteLine("Toggl To Time Entry"); });
            jobScheduler.CheckAndRun(t => t.TogglToTask, 60, () => { Console.WriteLine("Toggl To Task"); });

            jobScheduler.End();
        }
    }
}
