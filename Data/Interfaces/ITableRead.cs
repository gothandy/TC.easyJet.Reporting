using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincente.Data.Interfaces
{
    public interface ITableRead<T>
    {
        IEnumerable<T> Query();
    }
}
