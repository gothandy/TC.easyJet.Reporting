using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gothandy.Tables.Azure
{
    public class Results
    {
        public int Replaced { get; set; }
        public int Inserted { get; set; }
        public int Deleted { get; set; }
        public int Ignored { get; set; }
    }
}
