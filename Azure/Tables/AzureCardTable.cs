using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Converters;
using Vincente.Azure.Entities;
using Vincente.Azure.Interfaces;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Azure.Tables
{
    public class AzureCardTable : ITableWrite<Card>
    {
        private CloudTable table;
        private TableBatchOperation batchOperation;
        private IConverter<Card, CardEntity> converter;

        public AzureCardTable(TableClient client)
        {
            table = client.GetTable("Cards");
            batchOperation = new TableBatchOperation();
            converter = new CardConverter();
        }

        public IEnumerable<Card> Query()
        {
            TableQuery<CardEntity> query = new TableQuery<CardEntity>();
            var result = table.ExecuteQuery(query);

            return
                from entity in result
                select converter.DataFromAzure(entity);
        }

        public void BatchInsertOrReplace(Card card)
        {

            CardEntity entity = converter.AzureFromData(card);

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
