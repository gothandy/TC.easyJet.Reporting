using Gothandy.Mvc.Navigation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.WebApp.Models;

namespace Vincente.WebApp.Controllers
{
    public class ErrorController : BaseController
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
            List<ErrorModel> list = new List<ErrorModel>();

            list.Add(new ErrorModel() { Text = "Duplicate Tasks", Action = "TaskDuplicates", Count = DuplicateTasks().Count() });
            list.Add(new ErrorModel() { Text = "Dom Id Duplicates", Action = "DomIdDuplicates", Count = DuplicateDomIds().Count() });
            list.Add(new ErrorModel() { Text = "Null Dom Ids", Action = "NullDomIds", Count = GetNullDomIds().Count() });
            list.Add(new ErrorModel() { Text = "Cards Without Time", Action = "CardsWithoutTime", Count = GetCardsWithoutTime().Count() });
            list.Add(new ErrorModel() { Text = "Time Without Cards", Action = "TimeWithoutCards", Count = GetTimeWithoutCards().Count() });
            list.Add(new ErrorModel() { Text = "Time After Invoice", Action = "TimeAfterInvoice", Count = GetTimeAfterinvoice().Count() });

            return View(list);
        }

        private object GetTimeAfterInvoice()
        {
            throw new NotImplementedException();
        }

        // GET: Error/TaskDuplicates
        public ActionResult TaskDuplicates()
        {
            ViewBag.Title = "Task Duplicates";

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
            ViewBag.Title = "Dom Id Duplicates";

            var duplicates =
                (from c in cards
                 join d in DuplicateDomIds() on c.DomId equals d
                 select c);

            return View(duplicates);
        }

        // GET: Error/NullDomIds
        public ActionResult NullDomIds()
        {
            ViewBag.Title = "Null Dom Id";

            return View(GetNullDomIds());
        }

        // GET: Error/CardsWithoutTime
        public ActionResult CardsWithoutTime()
        {
            ViewBag.Title = "Cards Without Time";

            return View(GetCardsWithoutTime());
        }

        // GET: Error/TimeWithoutCards
        public ActionResult TimeWithoutCards()
        {
            ViewBag.Title = "Time Without Cards";

            return View(GetTimeWithoutCards());
        }

        // Get Error/TimeAfterInvoice
        public ActionResult TimeAfterInvoice()
        {
            ViewBag.Title = "Time After invoice";

            return View(GetTimeAfterinvoice());
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
                    && c.ListName != "Ready for Dev"
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

        private IEnumerable<Activity> GetTimeAfterinvoice()
        {
            return
                from t in timeEntries
                join c in cards on t.DomId equals c.DomId
                where t.Month > c.Invoice
                group t by new
                {
                    Invoice = c.Invoice,
                    Epic = c.Epic,
                    DomId = c.DomId,
                    TaskId = t.TaskId
                } into g
                select new Activity
                {
                    Invoice = g.Key.Invoice,
                    Epic = g.Key.Epic,
                    DomId = g.Key.DomId,
                    TaskId = g.Key.TaskId,
                    Billable = g.Sum(t => t.Billable)
                };
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