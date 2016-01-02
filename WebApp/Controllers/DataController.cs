using System.Web.Mvc;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;

namespace Vincente.WebApp.Controllers
{
    public class DataController : Controller
    {
        private ICardRead cards;
        private ITaskRead tasks;
        private ITimeEntryRead timeEntries;
        private InvoiceData invoiceData;

        public DataController(ICardRead cards, ITaskRead tasks, ITimeEntryRead timeEntries, InvoiceData invoiceData)
        {
            this.cards = cards;
            this.tasks = tasks;
            this.timeEntries = timeEntries;
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

        public ActionResult Tasks()
        {
            return View(tasks.Query());
        }

        public ActionResult Toggl()
        {
            return View(timeEntries.Query());
        }

        public ActionResult AllByMonth()
        {
            return View(invoiceData.Query());
        }
    }
}
