using Gothandy.Tables.Bulk;
using Gothandy.Trello;
using System;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace Vincente.WebJobs.TrelloToCard
{
    public class TrelloToCardWebJob
    {
        private CloudTable azureCardTable;
        private ListNameTable azureReplaceTable;
        private Workspace trelloWorkspace;

        public TrelloToCardWebJob(Workspace trelloWorkspace, CloudTable azureCardTable, ListNameTable azureReplaceTable)
        {
            this.trelloWorkspace = trelloWorkspace;
            this.azureCardTable = azureCardTable;
            this.azureReplaceTable = azureReplaceTable;
        }

        public void Execute()
        {
            var trelloData = new TrelloData(trelloWorkspace, azureReplaceTable);

            Console.WriteLine("______________");
            Console.WriteLine("Trello To Card");
            ConsoleOutCounts(trelloData);

            CardTable cardTable = new CardTable(azureCardTable);
            azureCardTable.CreateIfNotExists();

            var oldCards = cardTable.Query().ToList();
            var newCards = TrelloDataToCardTable.Execute(trelloData);

            var operations = new Operations<Card>(cardTable);
            var results = operations.BatchCompare(oldCards, newCards);

            ConsoleOutResults(results);
        }

        private static void ConsoleOutCounts(TrelloData trelloData)
        {
            Console.Out.WriteLine("{0} Cards Found", trelloData.Cards.Count);
            Console.Out.WriteLine("{0} Labels Found", trelloData.Labels.Count);
            Console.Out.WriteLine("{0} Lists Found", trelloData.Lists.Count);
            Console.Out.WriteLine("{0} Replaces Found", trelloData.Replaces.Count);
        }

        private static void ConsoleOutResults(Results results)
        {
            Console.Out.WriteLine("{0} Cards Inserted", results.Inserted);
            Console.Out.WriteLine("{0} Cards Ignored", results.Ignored);
            Console.Out.WriteLine("{0} Cards Replaced", results.Replaced);
            Console.Out.WriteLine("{0} Cards Deleted", results.Deleted);
        }
    }
}
