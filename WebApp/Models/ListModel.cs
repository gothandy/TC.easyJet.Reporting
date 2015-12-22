using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ListModel
    {
        public int? ListIndex { get; set; }
        public string ListName { get; set; }
        public decimal? Billable { get; set; }
    }
}