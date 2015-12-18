using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula
{
    public class FromProject
    {
        public static string IfHouseKeepingReturnTaskName(long? projectId, string taskName)
        {
            if (projectId == null) return null;
            if (projectId == 5170140) return taskName;
            return null;
        }
    }
}
