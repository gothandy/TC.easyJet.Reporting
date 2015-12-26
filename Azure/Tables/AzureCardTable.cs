using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Azure.Tables
{
    public class AzureCardTable : ITableWrite<Card>
    {
        private CloudTable table;
        private TableBatchOperation batchOperation;

        public AzureCardTable(TableClient client)
        {
            table = client.GetTable("Cards");
            batchOperation = new TableBatchOperation();
        }

        public IEnumerable<Card> Query()
        {
            TableQuery<CardEntity> query = new TableQuery<CardEntity>();
            var result = table.ExecuteQuery(query);

            return
                from entity in result
                select new Card()
                {
                    DomId = entity.DomId,
                    Epic = entity.Epic,
                    Invoice = entity.Invoice,
                    ListIndex = entity.ListIndex,
                    ListName = entity.ListName,
                    Name = entity.Name,
                    Timestamp = entity.Timestamp.LocalDateTime
                };
        }

        public void BatchInsertOrReplace(Card card)
        {

            CardEntity entity = new CardEntity(card.Id)
            {
                DomId = card.DomId,
                Epic = card.Epic,
                Invoice = card.Invoice,
                ListIndex = card.ListIndex,
                ListName = card.ListName,
                Name = card.Name
            };

            batchOperation.InsertOrReplace(entity);

            if (batchOperation.Count == 100)
            {
                table.ExecuteBatch(batchOperation);
                batchOperation = new TableBatchOperation();
            }
        }

        public void ExecuteBatch()
        {
            table.ExecuteBatch(batchOperation);
        }

        public void CreateIfNotExists()
        {
            table.CreateIfNotExists();
        }
    }
}
