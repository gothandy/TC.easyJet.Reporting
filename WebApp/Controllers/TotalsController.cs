using Gothandy.Mvc.Navigation.Controllers;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Tables;
using Vincente.WebApp.Models;

namespace Vincente.WebApp.Controllers
{
    public class TotalsController : BaseController
    {
        private CardsByMonth cardsWithTime;

        public TotalsController(CardsByMonth cardsWithTime)
        {
            this.cardsWithTime = cardsWithTime;
        }

        // GET: Totals
        public ActionResult Index()
        {
            ViewBag.Title = "Epics";

            var data =
                (from c in cardsWithTime.Query()
                group c by new
                {
                    Epic = c.Epic,

                } into g
                select new TopXModel()
                {
                    Epic = g.Key.Epic,
                    BillableWip = g.Sum(c => c.Invoice == null ? c.Billable : null),
                    Billable = g.Sum(c => c.Billable)
                }).OrderByDescending(g => g.Billable);

            return View(data);
        }
    }
}