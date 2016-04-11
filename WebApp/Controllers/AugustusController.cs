using Augustus.Domain.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Vincente.Data.Entities;
using Vincente.Data.Tables;

namespace Vincente.WebApp.Controllers
{
    public class AugustusController : ApiController
    {
        private IEnumerable<Activity> invoiceData;

        public AugustusController(InvoiceByMonth invoiceData)
        {
            this.invoiceData = invoiceData.Query();
        }

        public Account Get()
        {
            Account account = GetAccount();

            var invoices = GetInvoices();

            account.Opportunities.First().Invoices = invoices;

            foreach (var invoice in invoices)
            {
                invoice.WorkDoneItems = GetWorkDoneItems(invoice);
            }

            return account;
        }

        private List<WorkDoneItem> GetWorkDoneItems(Invoice invoice)
        {
            return (from i in invoiceData
                    where i.Invoice == invoice.InvoiceDate
                    group i by new
                    {
                        i.Month
                    } into g
                    orderby g.Key.Month ascending
                    select new WorkDoneItem
                    {
                        WorkDoneDate = g.Key.Month,
                        Margin = g.Sum(i => i.Billable)
                    }).ToList();
        }

        private List<Invoice> GetInvoices()
        {
            return (from e in invoiceData
                    where e.Invoice != null && e.Month <= e.Invoice
                    group e by new
                    {
                        e.Invoice
                    }
                        into g
                    orderby g.Key.Invoice ascending
                    select new Invoice
                    {
                        Name = g.Key.Invoice.Value.ToString("yyyy MMM"),
                        InvoiceDate = g.Key.Invoice,
                        Revenue = g.Sum(e => e.Billable)
                    }).ToList();
        }

        private static Account GetAccount()
        {
            return new Account
            {
                Name = "easyJet",
                Opportunities = new List<Opportunity>
                {
                    new Opportunity
                    {
                        Name = "Team Budget"
                    }
                }
            };
        }
    }
}
