using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class InvoiceModel
    {
        public DateTime? Invoice { get; set; }
        public decimal? Current { get; set; }
        public decimal? Previous { get; set; }
        public decimal? Total { get; set; }
    }
}