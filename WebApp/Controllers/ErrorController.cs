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
        private ITaskRead tasks;

        public ErrorController(ICardRead cards, ITaskRead tasks)
        {
            this.cards = cards;
            this.tasks = tasks;
        }

        // GET: Error
        public ActionResult Summary()
        {
            ViewBag.DuplicateTasks =
                (from c in cards.Query()
                 join t in tasks.Query() on c.Id equals t.CardId
                 group c by new
                 {
                     c.Id
                 } into g
                 where g.Count() > 1
                 select g).Count();

            ViewBag.DuplicateDomIds =
                (from c in cards.Query()
                 where c.DomId != null
                 group c by new
                 {
                     c.DomId
                 }
                 into g
                 where g.Count() > 1
                 select g).Count();

            return View();
        }

        // GET: Error/TaskDuplicates
        public ActionResult TaskDuplicates()
        {
            var cardIds =
                from c in cards.Query()
                join t in tasks.Query() on c.Id equals t.CardId
                group c by new
                {
                    c.Id
                } into g
                where g.Count() > 1
                select g.Key.Id;

            var duplicates =
                from c in cards.Query()
                join t in tasks.Query() on c.Id equals t.CardId
                join i in cardIds on c.Id equals i
                select t;

            return View(duplicates);
        }

        // GET: Error/DomIdDuplicates
        public ActionResult DomIdDuplicates()
        {
            var domIds =
                from c in cards.Query()
                where c.DomId != null
                group c by new
                {
                    c.DomId
                }
                 into g
                where g.Count() > 1
                select g.Key.DomId;

            var duplicates =
                (from c in cards.Query()
                 join d in domIds on c.DomId equals d
                 select c);

            return View(duplicates);
        }
    }
}