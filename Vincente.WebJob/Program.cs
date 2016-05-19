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

            CheckLastRunTimeAndAction(lastRunTimes, t => t.TrelloToCard, 5, ()=> { Console.WriteLine("TrelloToCard"); });
            CheckLastRunTimeAndAction(lastRunTimes, t => t.TogglToTimeEntry, 15, () => { Console.WriteLine("TogglToTimeEntry"); });
            CheckLastRunTimeAndAction(lastRunTimes, t => t.TogglToTask, 60, () => { Console.WriteLine("TogglToTask"); });

            jobScheduler.End(lastRunTimes);
        }

        private static void CheckLastRunTimeAndAction(LastRunTimes target, Expression<Func<LastRunTimes, DateTime>> expression, int minutes, Action action)
        {
            var body = (MemberExpression)expression.Body;
            var prop = (PropertyInfo)body.Member;

            var value = (DateTime)prop.GetValue(target);

            if (TimeSinceLastRun(value, minutes))
            {
                action();

                prop.SetValue(target, DateTime.UtcNow, null);
            }
        }

        private static bool TimeSinceLastRun(DateTime dateTime, int minutes)
        {
            var since = DateTime.UtcNow.Subtract(dateTime);

            return (since > new TimeSpan(0, minutes - 2, 30));
        }
    }
}
