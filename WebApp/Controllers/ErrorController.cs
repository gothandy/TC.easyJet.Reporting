using System.Web.Mvc;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;
using System.Linq;
using Vincente.Data.Entities;
using System.Collections.Generic;
using System;

namespace Vincente.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        private IEnumerable<Card> cards;
        private IEnumerable<Task> tasks;
        private IEnumerable<TimeEntry> timeEntries;

        public ErrorController(ICardRead cards, ITaskRead tasks, ITimeEntryRead timeEntries)
        {
            this.cards = cards.Query();
            this.tasks = tasks.Query();
            this.timeEntries = timeEntries.Query();
        }

        // GET: Error
        public ActionResult Summary()
        {

            ViewBag.DuplicateTasks = DuplicateTasks().Count();
            ViewBag.DuplicateDomIds = DuplicateDomIds().Count();
            ViewBag.NullDomIds = GetNullDomIds().Count();
            ViewBag.CardsWithoutTime = GetCardsWithoutTime().Count();
            ViewBag.TimeWithoutCards = GetTimeWithoutCards().Count();

            return View();
        }

        // GET: Error/TaskDuplicates
        public ActionResult TaskDuplicates()
        {
            var duplicates =
                from c in cards
                join t in tasks on c.Id equals t.CardId
                join i in DuplicateTasks() on c.Id equals i
                select t;

            return View(duplicates);
        }

        // GET: Error/DomIdDuplicates
        public ActionResult DomIdDuplicates()
        {
            var duplicates =
                (from c in cards
                 join d in DuplicateDomIds() on c.DomId equals d
                 select c);

            return View(duplicates);
        }

        // GET: Error/NullDomIds
        public ActionResult NullDomIds()
        {
            return View(GetNullDomIds());
        }

        // GET: Error/CardsWithoutTime
        public ActionResult CardsWithoutTime()
        {
            return View(GetCardsWithoutTime());
        }

        // GET: Error/TimeWithoutCards
        public ActionResult TimeWithoutCards()
        {
            return View(GetTimeWithoutCards());
        }



        private List<string> DuplicateTasks()
        {
            return
                (from c in cards
                 join t in tasks on c.Id equals t.CardId
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
                (from c in cards
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
                (from c in cards
                 where c.DomId == null && c.ListName != "No Time In Toggl"
                 select c).ToList();
        }

        private IEnumerable<Card> GetCardsWithoutTime()
        {
            return
                from c in cards
                where (NoTimeEntries(c.DomId))
                    && c.ListName != "No Time In Toggl"
                    && !c.ListName.StartsWith("Backlog")
                select c;
        }

        private bool NoTimeEntries(string domId)
        {
            if (domId == null) return false;

            var count =
                (from t in timeEntries
                 where t.DomId == domId
                 select t).Count();

            return count == 0;
        }


        private IEnumerable<TimeEntry> GetTimeWithoutCards()
        {
            return
                from t in timeEntries
                where CheckForTimeWithoutCards(t.DomId) &&
                    t.Housekeeping == null &&
                    t.Start > new DateTime(2015, 7, 1)
                select t;
        }

        private bool CheckForTimeWithoutCards(string domId)
        {
            if (domId == null) return true;

            var count =
                (from c in cards
                 where c.DomId == domId
                 select c).Count();

            return count == 0;
        }
    }
}