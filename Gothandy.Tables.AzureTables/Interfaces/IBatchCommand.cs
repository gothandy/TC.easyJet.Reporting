﻿using Microsoft.WindowsAzure.Storage.Table;

namespace Gothandy.Tables.AzureTables.Interfaces
{
    public interface IBatchCommand
    {
        void Execute(TableBatchOperation batchOperation, TableEntity entity);
    }
}
