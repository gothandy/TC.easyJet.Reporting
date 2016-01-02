using Gothandy.Tables.Bulk;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Vincente.Trello.DataObjects;
using Vincente.TrelloConsoleApp.Operations;

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

            List<TrelloCard> trelloCards = trelloWorkspace.GetCards();
            List<Label> trelloLabels = trelloWorkspace.GetLabels();
            List<List> trelloLists = trelloWorkspace.GetLists();

            Console.Out.WriteLine("{0} Cards Found", trelloCards.Count);
            Console.Out.WriteLine("{0} Labels Found", trelloLabels.Count);
            Console.Out.WriteLine("{0} Lists Found", trelloLists.Count);

            CardTable cardTable = new CardTable(azureCardTable);

            azureCardTable.CreateIfNotExists();
            
            var oldCards = cardTable.Query().ToList();
            var newCards = TrelloToData.Execute(trelloCards, trelloLabels, trelloLists);

            var operations = new Operations<Card>(cardTable);
            var results = operations.BatchCompare(oldCards, newCards);

            Console.Out.WriteLine("{0} Cards Inserted", results.Inserted);
            Console.Out.WriteLine("{0} Cards Ignored", results.Ignored);
            Console.Out.WriteLine("{0} Cards Replaced", results.Replaced);
            Console.Out.WriteLine("{0} Cards Deleted", results.Deleted);
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }
    }
}
