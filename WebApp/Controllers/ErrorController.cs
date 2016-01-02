using System.Web.Mvc;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;
using System.Linq;
using Vincente.Data.Entities;
using System.Collections.Generic;

namespace Vincente.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        private ICardRead cards;

        public ErrorController(ICardRead cards)
        {
            this.cards = cards;
        }

        // GET: Error
        public ActionResult Summary()
        {
            ViewBag.DuplicateTasks =
                (from c in cards.Query()
                 where (c.TaskIds == null ? false : c.TaskIds.Count > 1)
                 select c).Count();

            return View();
        }

        // GET: Error/TaskDuplicates
        public ActionResult TaskDuplicates()
        {
            IEnumerable<Card> duplicates =
                from c in cards.Query()
                where (c.TaskIds == null ? false : c.TaskIds.Count > 1)
                select c;

            return View(duplicates);
        }
    }
}