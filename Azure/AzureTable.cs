using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Interfaces;
using Vincente.Data.Interfaces;

namespace Vincente.Azure
{
    public class AzureTable<T, U> : ITableWrite<T> 
        where U : TableEntity, new() 
    {
        private CloudTable table;
        private TableBatchOperation batchOperation;
        private IConverter<T, U> converter;

        public AzureTable(CloudTable table, IConverter<T, U> converter)
        {
            this.table = table;
            this.batchOperation = new TableBatchOperation();
            this.converter = converter;
        }

        public IEnumerable<T> Query()
        {
            TableQuery<U> query = new TableQuery<U>();

            IEnumerable<U> result = table.ExecuteQuery(query);

            return
                from entity in result
                select converter.Read(entity);
        }

        public void BatchInsertOrReplace(T entity)
        {

            U azureEntity = converter.Write(entity);

            batchOperation.InsertOrReplace(azureEntity);

            if (batchOperation.Count == 100)
            {
                table.ExecuteBatch(batchOperation);
                batchOperation = new TableBatchOperation();
            }
        }

        public void BatchComplete()
        {
            table.ExecuteBatch(batchOperation);
        }
    }
}
