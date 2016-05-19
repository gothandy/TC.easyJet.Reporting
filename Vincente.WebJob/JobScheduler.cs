using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Linq.Expressions;
using System.Reflection;

namespace Vincente.WebJob
{
    public class JobScheduler
    {
        private LastRunTimes lastRunTimes;
        private CloudBlockBlob azureLastRunTimesBlob;

        public JobScheduler(CloudBlockBlob azureLastRunTimesBlob)
        {
            this.azureLastRunTimesBlob = azureLastRunTimesBlob;


        }

        public void Begin()
        {
            if (azureLastRunTimesBlob.Exists())
            {
                var readXml = azureLastRunTimesBlob.DownloadText();
                lastRunTimes = JobScheduler.GetObject<LastRunTimes>(readXml);
            }
            else
            {
                lastRunTimes = new LastRunTimes();
            }
        }

        public void CheckAndRun(Expression<Func<LastRunTimes, DateTime>> expression, int minutes, Action action)
        {
            var body = (MemberExpression)expression.Body;
            var property = (PropertyInfo)body.Member;

            Console.WriteLine("___________");
            Console.WriteLine(property.Name);
            

            var lastRunTime = (DateTime)property.GetValue(lastRunTimes);

            Console.WriteLine("Last run at {0}", lastRunTime);

            if (TimeSinceLastRun(lastRunTime, minutes))
            {
                action();

                property.SetValue(lastRunTimes, DateTime.UtcNow, null);
            }
        }

        public void End()
        {
            var writeXml = JobScheduler.GetXml<LastRunTimes>(lastRunTimes);

            azureLastRunTimesBlob.UploadText(writeXml);
        }

        private static bool TimeSinceLastRun(DateTime dateTime, int minutes)
        {
            var since = DateTime.UtcNow.Subtract(dateTime);

            return (since > new TimeSpan(0, minutes - 2, 30));
        }

        private static T GetObject<T>(string xml)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (TextReader reader = new StringReader(xml))
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        private static string GetXml<T>(T lastRunTimes)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlSerializer.Serialize(writer, lastRunTimes);
                return stringWriter.ToString();
            }
        }

    }
}
