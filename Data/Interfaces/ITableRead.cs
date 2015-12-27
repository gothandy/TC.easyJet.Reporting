using System.Collections.Generic;

namespace Vincente.Data.Interfaces
{
    public interface ITableRead<T>
    {
        IEnumerable<T> Query();
    }
}
