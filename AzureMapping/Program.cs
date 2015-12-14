using Azure.Tables;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using TC.easyJet.Reporting;

namespace AzureMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            var azureAccountKey = args[0];

            TimeEntryTable table = new TimeEntryTable(azureAccountKey);

            foreach (TimeEntryEntity entity in table.Query())
            {
                var update = false;

                update = entity.UpdateDomId();
                update = entity.UpdateMonth() || update;

                if (update) table.Replace(entity);
            }
        }
    }
}
