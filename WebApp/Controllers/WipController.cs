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
               select new CardWithTime()
               {
                   ListIndex = g.Key.ListIndex,
                   ListName = g.Key.ListName,
                   Billable = g.Sum(e => e.Billable)
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
                        e.Epic,
                        e.DomId,
                        e.Name
                    } into g
                    select new CardWithTime()
                    {
                        CardId = g.Key.CardId,
                        ListName = g.Key.ListName,
                        Epic = g.Key.Epic,
                        DomId = g.Key.DomId,
                        Name = g.Key.Name,
                        Billable = g.Sum(e => e.Billable)
                    };

            return View(data);
        }
    }
}