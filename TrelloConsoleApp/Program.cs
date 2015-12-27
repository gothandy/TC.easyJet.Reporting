using System;
using System.Collections.Generic;
using System.Configuration;
using Vincente.Azure.Converters;
using Vincente.Azure.Entities;
using Vincente.Azure;
using Vincente.Data.Entities;
using Vincente.Trello.DataObjects;
using Microsoft.WindowsAzure.Storage;

namespace TrelloConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var azureConnectionString = CheckAndGetAppSettings("azureConnectionString");
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureCardTable = azureTableClient.GetTableReference("Cards");

            var trelloToken = CheckAndGetAppSettings("trelloToken");
            var trelloKey = "3ba00ca224256611c3ccbac183364259";
            var trelloBoardId = "5596a7b7ac88c077383d281c";
            var trelloWorkspace = new Vincente.Trello.Workspace(trelloKey, trelloToken, trelloBoardId);

            List<TrelloCard> cards = trelloWorkspace.GetCards();
            List<Label> labels = trelloWorkspace.GetLabels();
            List<List> lists = trelloWorkspace.GetLists();

            Console.Out.WriteLine("{0} Cards Found", cards.Count);
            Console.Out.WriteLine("{0} Labels Found", labels.Count);
            Console.Out.WriteLine("{0} Lists Found", lists.Count);

            AzureTable<Card, CardEntity> cardTable = new AzureTable<Card, CardEntity>(azureCardTable, new CardConverter());

            azureCardTable.CreateIfNotExists();

            TrelloToData.Execute(cardTable, cards, labels, lists);
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }
    }
}
