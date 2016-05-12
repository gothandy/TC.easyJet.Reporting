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

            var lastRunTimes = jobScheduler.Begin();

            if (TimeSinceLastRun(lastRunTimes.TrelloToCard, 5))
            {
                Console.Write("TrelloToCard");
                lastRunTimes.TrelloToCard = DateTime.UtcNow;
            }

            if (TimeSinceLastRun(lastRunTimes.TogglToTimeEntry, 15))
            {
                Console.Write("TogglToTimeEntry");
                lastRunTimes.TogglToTimeEntry = DateTime.UtcNow;
            }

            if (TimeSinceLastRun(lastRunTimes.TogglToTask, 60))
            {
                Console.Write("TogglToTask");
                lastRunTimes.TogglToTask = DateTime.UtcNow;
            }

            jobScheduler.End(lastRunTimes);
        }

        private static bool TimeSinceLastRun(DateTime dateTime, int minutes)
        {
            var since = DateTime.UtcNow.Subtract(dateTime);

            return (since > new TimeSpan(0, minutes - 2, 30));
        }

        // Linq example
        void GetString<T>(string input, T target, Expression<Func<T, string>> outExpr)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var expr = (MemberExpression)outExpr.Body;
                var prop = (PropertyInfo)expr.Member;
                prop.SetValue(target, input, null);
            }
        }
    }
}
