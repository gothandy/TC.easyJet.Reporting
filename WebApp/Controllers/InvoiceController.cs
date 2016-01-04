using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Tables;
using Vincente.WebApp.Models;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private IEnumerable<CardWithTime> invoiceData;

        public InvoiceController(InvoiceData invoiceData)
        {
            this.invoiceData = invoiceData.Query();
        }

        // GET: Invoice
        public ActionResult List()
        {
            var invoice =
                from e in invoiceData
                where e.Invoice != null && e.Month <= e.Invoice
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
                    Previous = g.Sum(e => (e.Month == g.Key.Invoice.Value.AddMonths(-1)) ? e.Billable : 0),
                    Total = g.Sum(e => e.Billable)
                };

            var currentMonth = invoice.Max(i => (i.Invoice)).Value;
            var nextMonth = currentMonth.AddMonths(1);

            var wip =
                from e in invoiceData
                where e.Invoice == null
                select new CardWithTime()
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

            ViewBag.Invoice = invoice;
            ViewBag.CurrentCurrent = GetCurrentCurrent(invoice);
            ViewBag.CurrentPrevious = GetCurrentPrevious(invoice);
            ViewBag.FutureCurrent = GetFutureCurrent(invoice);
            ViewBag.FuturePrevious = GetFuturePrevious(invoice);

            var result =
                from e in invoiceData
                where e.Invoice == invoice && e.Month <= invoice
                group e by new
                {
                    e.Invoice,
                    e.Epic
                }
                into g
                orderby g.Key.Epic
                select new CardWithTime()
                {
                    Invoice = g.Key.Invoice,
                    Epic = g.Key.Epic,
                    Billable = g.Sum(e => e.Billable)
                };

            return View(result);
        }

        private decimal GetCurrentCurrent(DateTime invoice)
        {
            return
                (from e in invoiceData
                 where e.Invoice == invoice && e.Month == invoice
                 select e.Billable).Sum().GetValueOrDefault();
        }

        private decimal GetCurrentPrevious(DateTime invoice)
        {
            return
                (from e in invoiceData
                 where e.Invoice == invoice && e.Month < invoice
                 select e.Billable).Sum().GetValueOrDefault();
        }

        private decimal GetFutureCurrent(DateTime invoice)
        {
            return
                (from e in invoiceData
                 where (e.Invoice == null || e.Invoice > invoice) && e.Month == invoice
                 select e.Billable).Sum().GetValueOrDefault();
        }

        private decimal GetFuturePrevious(DateTime invoice)
        {
            return
                (from e in invoiceData
                 where (e.Invoice == null || e.Invoice > invoice) && e.Month < invoice
                 select e.Billable).Sum().GetValueOrDefault();
        }

        public ActionResult Detail(int year, int month, string epic)
        {
            var invoice = new DateTime(year, month, 1);

            var result =
                from e in invoiceData
                where e.Invoice == invoice && e.Epic == epic && e.Month <= invoice
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
                select new CardWithTime()
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