using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincente.WebApp.Helpers
{
    public class CurrencyHelper
    {
        public static string Format(decimal? currency)
        {
            return string.Format("{0:n2}", currency);
        }
    }
}