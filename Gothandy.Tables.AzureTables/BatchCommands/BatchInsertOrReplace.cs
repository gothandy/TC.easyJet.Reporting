﻿using Gothandy.Tables.AzureTables.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;

namespace Gothandy.Tables.AzureTables.BatchCommands
{
    public class BatchInsertOrReplace : IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            batchOperation.InsertOrReplace(entity);
        }
    }
}
