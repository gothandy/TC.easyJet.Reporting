using Gothandy.Tables.Azure.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;

namespace Gothandy.Tables.Azure.BatchCommands
{
    public class BatchInsert : IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            batchOperation.Insert(entity);
        }
    }
}
