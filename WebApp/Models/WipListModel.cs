using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincente.WebApp.Models
{
    public class WipListModel
    {
        public int? ListIndex { get; set; }
        public string ListName { get; set; }
        public int? Count { get; set; }
        public decimal? Billable { get; set; }
        public decimal? Blocked { get; set; }
    }
}