using Microsoft.WindowsAzure.Storage.Table;

namespace Gothandy.Tables.Azure.Interfaces
{
    public interface IBatchCommand
    {
        void Execute(TableBatchOperation batchOperation, TableEntity entity);
    }
}
