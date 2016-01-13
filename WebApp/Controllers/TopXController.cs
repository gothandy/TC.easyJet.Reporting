using Gothandy.Mvc.Navigation.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Tables;
using Vincente.WebApp.Models;

namespace WebApp.Controllers
{
    public class TopXController : BaseController
    {
        private const int topX = 10;

        private List<TopXModel> topXData;

        public TopXController(CardsWithTime cardsWithTime)
        {
            topXData =
               (from e in cardsWithTime.Query()
               where e.Invoice == null
               group e by new
               {
                   e.DomId,
                   e.Epic,
                   e.Name,
                   e.CardId,
                   e.IsBlocked
               }
               into g
               select new TopXModel()
               {
                   DomId = g.Key.DomId,
                   Epic = g.Key.Epic,
                   Name = g.Key.Name,
                   CardId = g.Key.CardId,
                   Billable = g.Sum(e => e.Billable),
                   Months = g.Select(e => e.Month).Distinct().Count()
               }).ToList();
        }

        public ActionResult ByBillable()
        {
            ViewBag.Title = "Top 10 Billable";

            var data =
               (from t in topXData
               orderby t.Billable descending
               select t).Take(topX);

            return View("TopX", data);
        }

        public ActionResult ByMonths()
        {
            ViewBag.Title = "Top 10 Months";

            var data =
               (from t in topXData
                orderby t.Months descending
                select t).Take(topX);

            return View("TopX", data);
        }
    }
}