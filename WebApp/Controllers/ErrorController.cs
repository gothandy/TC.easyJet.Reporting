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

            ViewBag.DuplicateTasks = DuplicateTasks().Count();
            ViewBag.DuplicateDomIds = DuplicateDomIds().Count();
            ViewBag.NullDomIds = GetNullDomIds().Count();

            return View();
        }

        // GET: Error/TaskDuplicates
        public ActionResult TaskDuplicates()
        {
            var duplicates =
                from c in cards.Query()
                join t in tasks.Query() on c.Id equals t.CardId
                join i in DuplicateTasks() on c.Id equals i
                select t;

            return View(duplicates);
        }

        // GET: Error/DomIdDuplicates
        public ActionResult DomIdDuplicates()
        {
            var duplicates =
                (from c in cards.Query()
                 join d in DuplicateDomIds() on c.DomId equals d
                 select c);

            return View(duplicates);
        }

        // GET: Error/NullDomIds
        public ActionResult NullDomIds()
        {
            return View(GetNullDomIds());
        }

        private List<string> DuplicateTasks()
        {
            return
                (from c in cards.Query()
                 join t in tasks.Query() on c.Id equals t.CardId
                 group c by new
                 {
                     c.Id
                 } into g
                 where g.Count() > 1
                 select g.Key.Id).ToList();
        }

        private List<string> DuplicateDomIds()
        {
            return
                (from c in cards.Query()
                 where c.DomId != null
                 group c by new
                 {
                     c.DomId
                 }
                 into g
                 where g.Count() > 1
                 select g.Key.DomId).ToList();
        }

        private List<Card> GetNullDomIds()
        {
            return
                (from c in cards.Query()
                 where c.DomId == null && c.ListName != "No Time In Toggl"
                 select c).ToList();
        }
    }
}