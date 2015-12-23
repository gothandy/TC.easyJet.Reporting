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
            IEnumerable<JoinModel> stories = joinClient.GetStories();
            IEnumerable<JoinModel> housekeeping = joinClient.GetHousekeeping();

            var data = stories.Concat(housekeeping);

            var result =
                from e in data
                where e.Invoice != null
                group e by new
                {
                    e.Invoice
                }
                into g
                select new JoinModel()
                {
                    Invoice = g.Key.Invoice,
                    Billable = g.Sum(e => e.Billable)
                };

            return View(result);
        }

        public ActionResult Detail(int year, int month)
        {
            var invoice = new DateTime(year, month, 1);

            IEnumerable<JoinModel> stories = joinClient.GetStories();
            IEnumerable<JoinModel> housekeeping = joinClient.GetHousekeeping();

            var data = stories.Concat(housekeeping);

            var result =
                from e in data
                where e.Invoice == invoice
                group e by new
                {
                    e.Invoice,
                    e.DomId,
                    e.Epic,
                    e.Name
                }
                into g
                orderby g.Key.Epic, g.Key.DomId
                select new JoinModel()
                {
                    Invoice = g.Key.Invoice,
                    DomId = g.Key.DomId,
                    Epic = g.Key.Epic,
                    Name = g.Key.Name,
                    Billable = g.Sum(e => e.Billable)
                };

            return View(result);
        }
    }
}