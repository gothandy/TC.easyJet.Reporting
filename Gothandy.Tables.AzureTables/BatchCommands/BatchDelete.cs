using Gothandy.Tables.AzureTables.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;

namespace Gothandy.Tables.AzureTables.BatchCommands
{
    public class BatchDelete : IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            entity.ETag = "*"; // Always overwrite (ignore concurrency).
            batchOperation.Delete(entity);
        }
    }
}
