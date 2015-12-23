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

            var invoice =
                from e in data
                where e.Invoice != null
                group e by new
                {
                    e.Invoice
                }
                into g
                orderby g.Key.Invoice descending
                select new InvoiceModel()
                {
                    Invoice = g.Key.Invoice,
                    Current = g.Sum(e => (e.Month == g.Key.Invoice) ? e.Billable : 0),
                    Total = g.Sum(e => e.Billable)
                };

            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var nextMonth = currentMonth.AddMonths(1);

            var wip =
                from e in data
                where e.Invoice == null
                select new JoinModel()
                {
                    Month = e.Month,
                    Billable = e.Billable
                };

            ViewBag.Invoice = nextMonth;
            ViewBag.Previous = wip.Sum(e => (e.Month == currentMonth) ? e.Billable : 0);
            ViewBag.Total = wip.Sum(e => e.Billable);

            return View(invoice);
        }

        public ActionResult ByEpic(int year, int month)
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
                    e.Epic
                }
                into g
                orderby g.Key.Epic
                select new JoinModel()
                {
                    Invoice = g.Key.Invoice,
                    Epic = g.Key.Epic,
                    Billable = g.Sum(e => e.Billable)
                };

            return View(result);
        }

        public ActionResult Detail(int year, int month, string epic)
        {
            var invoice = new DateTime(year, month, 1);

            IEnumerable<JoinModel> stories = joinClient.GetStories();
            IEnumerable<JoinModel> housekeeping = joinClient.GetHousekeeping();

            var data = stories.Concat(housekeeping);

            var result =
                from e in data
                where e.Invoice == invoice && e.Epic == epic
                group e by new
                {
                    e.CardId,
                    e.Invoice,
                    e.Epic,
                    e.DomId,
                    e.Name
                }
                into g
                orderby g.Key.DomId
                select new JoinModel()
                {
                    CardId = g.Key.CardId,
                    Invoice = g.Key.Invoice,
                    Epic = g.Key.Epic,
                    DomId = g.Key.DomId,
                    Name = g.Key.Name,
                    Billable = g.Sum(e => e.Billable)
                };

            return View(result);
        }
    }
}