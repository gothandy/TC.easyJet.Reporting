using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Tables;
using Vincente.WebApp.Models;

namespace WebApp.Controllers
{
    public class WipController : Controller
    {
        private CardsWithTime cardsWithTime;

        public WipController(CardsWithTime cardsWithTime)
        {
            this.cardsWithTime = cardsWithTime;
        }

        // GET: Wip
        public ActionResult ByList()
        {
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
                   Billable = g.Sum(e => e.Billable),
                   Blocked = g.Sum(e => (e.Blocked.GetValueOrDefault() ? e.Billable: 0))
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
                        e.Blocked
                    } into g
                    select new CardWithTime()
                    {
                        CardId = g.Key.CardId,
                        ListName = g.Key.ListName,
                        DomId = g.Key.DomId,
                        Epic = g.Key.Epic,
                        Name = g.Key.Name,
                        TaskId = g.Key.TaskId,
                        Billable = g.Sum(e => e.Billable),
                        Blocked = g.Key.Blocked
                    };

            return View(data);
        }
    }
}