using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private JoinClient joinClient;

        public InvoiceController(JoinClient joinClient)
        {
            this.joinClient = joinClient;
        }

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