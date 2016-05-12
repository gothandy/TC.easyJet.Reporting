using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Vincente.WebJob
{
    public class JobScheduler
    {
        private CloudBlockBlob azureLastRunTimesBlob;

        public JobScheduler(CloudBlockBlob azureLastRunTimesBlob)
        {
            this.azureLastRunTimesBlob = azureLastRunTimesBlob;


        }

        public LastRunTimes Begin()
        {
            if (azureLastRunTimesBlob.Exists())
            {
                var readXml = azureLastRunTimesBlob.DownloadText();
                return JobScheduler.GetObject<LastRunTimes>(readXml);
            }
            else
            {
                return new LastRunTimes();
            }
        }

        public void End(LastRunTimes lastRunTimes)
        {
            var writeXml = JobScheduler.GetXml<LastRunTimes>(lastRunTimes);

            azureLastRunTimesBlob.UploadText(writeXml);
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
