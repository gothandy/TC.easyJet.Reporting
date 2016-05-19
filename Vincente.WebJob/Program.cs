using Gothandy.StartUp;
using Microsoft.WindowsAzure.Storage;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Vincente.Config;

namespace Vincente.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            var config = ConfigBuilder.Build();

            #region Dependancies
            var azureStorageAccount = CloudStorageAccount.Parse(config.azureConnectionString);
            var azureBlobClient = azureStorageAccount.CreateCloudBlobClient();
            var azureBlobContainer = azureBlobClient.GetContainerReference(config.azureBlobContainerName);
            var azureLastRunTimesBlob = azureBlobContainer.GetBlockBlobReference("LastRunTimes.xml");
            var jobScheduler = new JobScheduler(azureLastRunTimesBlob);
            #endregion

            jobScheduler.Begin();

            jobScheduler.CheckAndRun(t => t.TrelloToCard, 5, ()=> { Console.WriteLine("Trello To Card"); });
            jobScheduler.CheckAndRun(t => t.TogglToTimeEntry, 15, () => { Console.WriteLine("Toggl To Time Entry"); });
            jobScheduler.CheckAndRun(t => t.TogglToTask, 60, () => { Console.WriteLine("Toggl To Task"); });

            jobScheduler.End();
        }
    }
}
