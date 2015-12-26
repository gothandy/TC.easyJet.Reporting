﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincente.Azure.Interfaces
{
    public interface IConverter<T,U> where U : TableEntity
    {
        T DataFromAzure(U azureEntity);
        U AzureFromData(T dataEntity);
    }
}
