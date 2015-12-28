using System.Web.Mvc;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;

namespace Vincente.WebApp.Controllers
{
    public class DataController : Controller
    {
        private ICardRead cards;
        private TimeEntriesByMonth timeEntriesByMonth;
        private InvoiceData invoiceData;

        public DataController(ICardRead cards, TimeEntriesByMonth timeEntriesByMonth, InvoiceData invoiceData)
        {
            this.cards = cards;
            this.timeEntriesByMonth = timeEntriesByMonth;
            this.invoiceData = invoiceData;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Trello()
        {
            return View(cards.Query());
        }

        public ActionResult Toggl()
        {
            return View(timeEntriesByMonth.Query());
        }

        public ActionResult AllByMonth()
        {
            return View(invoiceData.Query());
        }
    }
}
