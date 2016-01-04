using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincente.WebApp.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public decimal LastMonth { get; set; }
        public decimal ThisMonth { get; set; }
        public decimal LastWeek { get; set; }
        public decimal ThisWeek { get; set; }
        public decimal Yesterday { get; set; }
        public decimal Today { get; set; }
        public decimal Total { get; set; }
    }
}