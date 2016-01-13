using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincente.WebApp.Models
{
    public class TopXModel
    {
        public string CardId { get; set; }
        public long? TaskId { get; set; }
        public int? ListIndex { get; set; }
        public string ListName { get; set; }
        public string DomId { get; set; }
        public string Name { get; set; }
        public string Epic { get; set; }
        public bool? Blocked { get; set; }
        public decimal? Billable { get; set; }
        public int? Months { get; set; }

    }
}