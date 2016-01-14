using Gothandy.StartUp;
using Gothandy.Tables.Bulk;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Gothandy.Trello.DataObjects;
using Gothandy.TrelloConsoleApp.Operations;
using Vincente.Config;

namespace TrelloConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigBuilder.Build();

            #region Dependancies
            var azureStorageAccount = CloudStorageAccount.Parse(config.azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureCardTable = azureTableClient.GetTableReference("Cards");

            var trelloWorkspace = new Gothandy.Trello.Workspace(config.trelloKey, config.trelloToken, config.trelloBoardId);

            var azureBlobClient = azureStorageAccount.CreateCloudBlobClient();
            var azureBlobContainer = azureBlobClient.GetContainerReference(config.azureBlobContainerName);
            var azureReplaceBlob = azureBlobContainer.GetBlockBlobReference(config.azureReplacePath);
            var azureReplaceTable = new ListNameTable(azureReplaceBlob);
            #endregion

            List<TrelloCard> trelloCards = trelloWorkspace.GetCards();
            List<Label> trelloLabels = trelloWorkspace.GetLabels();
            List<List> trelloLists = trelloWorkspace.GetLists();
            List<Replace> azureReplaces = azureReplaceTable.Query().ToList();

            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            Console.Out.WriteLine("{0} Cards Found", trelloCards.Count);
            Console.Out.WriteLine("{0} Labels Found", trelloLabels.Count);
            Console.Out.WriteLine("{0} Lists Found", trelloLists.Count);
            Console.Out.WriteLine("{0} Replaces Found", azureReplaces.Count);

            CardTable cardTable = new CardTable(azureCardTable);
            azureCardTable.CreateIfNotExists();
            
            var oldCards = cardTable.Query().ToList();
            var newCards = TrelloToData.Execute(trelloCards, trelloLabels, trelloLists, azureReplaces);

            var operations = new Operations<Card>(cardTable);
            var results = operations.BatchCompare(oldCards, newCards);

            Console.Out.WriteLine("{0} Cards Inserted", results.Inserted);
            Console.Out.WriteLine("{0} Cards Ignored", results.Ignored);
            Console.Out.WriteLine("{0} Cards Replaced", results.Replaced);
            Console.Out.WriteLine("{0} Cards Deleted", results.Deleted);
        }
    }
}
