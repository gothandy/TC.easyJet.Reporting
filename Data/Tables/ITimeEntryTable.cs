using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vincente.Data.Entities;

namespace Vincente.Data.Tables
{
    public interface ITimeEntryTable
    {
        void BatchInsertOrReplace(TimeEntry timeEntry);
        void ExecuteBatch();
    }
}
