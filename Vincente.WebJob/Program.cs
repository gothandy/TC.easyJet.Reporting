using Gothandy.StartUp;
using Microsoft.WindowsAzure.Storage;
using System;
using System.IO;
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

            #endregion

            var jobScheduler = new JobScheduler(azureLastRunTimesBlob);


            var lastRunTimes = jobScheduler.Begin();

            if (HowLongAgo(lastRunTimes.TrelloToCard) > new TimeSpan(0,2,30))
            {
                Console.Write("TrelloToCard");
                lastRunTimes.TrelloToCard = DateTime.UtcNow;
            }

            if (HowLongAgo(lastRunTimes.TogglToTimeEntry) > new TimeSpan(0, 12, 30))
            {
                Console.Write("TogglToTimeEntry");
                lastRunTimes.TogglToTimeEntry = DateTime.UtcNow;
            }

            if (HowLongAgo(lastRunTimes.TogglToTask) > new TimeSpan(0, 52, 30))
            {
                Console.Write("TogglToTask");
                lastRunTimes.TogglToTask = DateTime.UtcNow;
            }

            jobScheduler.End(lastRunTimes);
        }

        private static TimeSpan HowLongAgo(DateTime dateTime)
        {
            return DateTime.UtcNow.Subtract(dateTime);
        }
    }
}
