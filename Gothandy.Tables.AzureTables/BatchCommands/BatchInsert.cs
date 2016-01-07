﻿using Gothandy.Tables.AzureTables.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;

namespace Gothandy.Tables.AzureTables.BatchCommands
{
    public class BatchInsert : IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            batchOperation.Insert(entity);
        }
    }
}
