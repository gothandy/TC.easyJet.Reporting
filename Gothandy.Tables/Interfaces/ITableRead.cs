using System.Collections.Generic;

namespace Gothandy.Tables.Interfaces
{
    public interface ITableRead<T>
    {
        IEnumerable<T> Query();
    }
}
