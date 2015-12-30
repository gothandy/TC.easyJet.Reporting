using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincente.Data.Interfaces
{
    public interface ITableWrite<T> : ITableRead<T>
    {
        void BatchInsert(T item);
        void BatchReplace(T item);
        void BatchInsertOrReplace(T item);
        void BatchDelete(T item);
        void BatchComplete();
    }
}
