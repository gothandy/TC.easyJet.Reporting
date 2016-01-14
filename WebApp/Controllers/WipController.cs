using Gothandy.Mvc.Navigation.Controllers;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Tables;
using Vincente.WebApp.Models;

namespace WebApp.Controllers
{
    public class WipController : BaseController
    {
        private CardsByMonth cardsWithTime;

        public WipController(CardsByMonth cardsWithTime)
        {
            this.cardsWithTime = cardsWithTime;
        }

        // GET: Wip
        public ActionResult ByList()
        {
            ViewBag.Title = "Work In Progress";

            var data =
               from e in cardsWithTime.Query()
               where e.Invoice == null
               group e by new
               {
                   e.ListIndex,
                   e.ListName
               }
               into g
               orderby g.Key.ListIndex
               select new WipListModel()
               {
                   ListIndex = g.Key.ListIndex,
                   ListName = g.Key.ListName,
                   Count = g.Select(e => e.DomId).Distinct().Count(),
                   Billable = g.Sum(e => e.Billable),
                   Blocked = g.Sum(e => (e.IsBlocked.GetValueOrDefault() ? e.Billable: 0))
               };

            return View(data);
        }

        // GET: Wip/1
        public ActionResult Detail(int? list)
        {
            
            var data =
                from e in cardsWithTime.Query()
                    where e.Invoice == null && e.ListIndex == list
                    group e by new
                    {
                        e.CardId,
                        e.ListName,
                        e.DomId,
                        e.Epic,
                        e.Name,
                        e.TaskId,
                        e.IsBlocked
                    } into g
                    select new TopXModel()
                    {
                        CardId = g.Key.CardId,
                        ListName = g.Key.ListName,
                        DomId = g.Key.DomId,
                        Epic = g.Key.Epic,
                        Name = g.Key.Name,
                        TaskId = g.Key.TaskId,
                        Months = g.Select(e => e.Month).Distinct().Count(),
                        Billable = g.Sum(e => e.Billable),
                        IsBlocked = g.Key.IsBlocked
                    };

            ViewBag.Title = data.First().ListName;

            return View(data);
        }
    }
}