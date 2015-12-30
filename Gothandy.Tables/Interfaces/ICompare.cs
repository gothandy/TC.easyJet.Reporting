using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gothandy.Tables.Interfaces
{
    public interface ICompare<T>
    {
        bool ValueEquals(T other);
        bool KeyEquals(T other);
    }
}
