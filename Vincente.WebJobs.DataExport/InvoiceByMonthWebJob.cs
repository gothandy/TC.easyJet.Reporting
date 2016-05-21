using System;
using Microsoft.WindowsAzure.Storage.Blob;
using Vincente.Data.Tables;
using System.Linq;
using System.Collections.Generic;
using Vincente.Data.Entities;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Vincente.WebJobs.DataExport
{
    public class InvoiceByMonthWebJob
    {
        private CloudBlockBlob azureInvoiceByMonthBlob;
        private InvoiceByMonth invoiceByMonthQuery;

        public InvoiceByMonthWebJob(InvoiceByMonth invoiceByMonthQuery, CloudBlockBlob azureInvoiceByMonthBlob)
        {
            this.invoiceByMonthQuery = invoiceByMonthQuery;
            this.azureInvoiceByMonthBlob = azureInvoiceByMonthBlob;
        }

        public void Execute()
        {
            var data = invoiceByMonthQuery.Query().ToList();

            var xml = GetXml<List<Activity>>(data);

            azureInvoiceByMonthBlob.UploadText(xml);

            Console.WriteLine("{0} activities written to {1}", data.Count, azureInvoiceByMonthBlob.Name);
        }

        private static string GetXml<T>(T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlSerializer.Serialize(writer, obj);
                return stringWriter.ToString();
            }
        }
    }
}
