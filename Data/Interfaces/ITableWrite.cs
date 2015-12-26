using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincente.Data.Interfaces
{
    public interface ITableWrite<T> : ITableRead<T>
    {
        void BatchInsertOrReplace(T item);
        void ExecuteBatch();
    }
}
