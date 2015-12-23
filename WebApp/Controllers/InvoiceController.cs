using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Detail(int year, int month)
        {
            return View();
        }
    }
}