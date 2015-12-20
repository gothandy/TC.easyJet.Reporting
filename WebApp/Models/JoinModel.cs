using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class JoinModel
    {
        public string DomId { get; set; }
        public string List { get; set; }
        public string Name { get; set; }
        public string Epic { get; set; }
        public DateTime? Invoice { get; set; }
        public DateTime? Month { get; set; }
        public string UserName { get; set; }
        public long? Billable { get; set; }
        public string Type { get; set; }

    }
}