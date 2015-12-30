using Gothandy.Tables.Azure.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;

namespace Gothandy.Tables.Azure.BatchCommands
{
    public class BatchInsertOrReplace : IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            batchOperation.InsertOrReplace(entity);
        }
    }
}
