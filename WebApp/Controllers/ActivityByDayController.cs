using Gothandy.Mvc.Navigation.Controllers;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;

namespace Vincente.WebApp.Controllers
{
    public class ActivityByDayController : BaseController
    {
        private ActivityByDay activityByDay;

        public ActivityByDayController (ActivityByDay activityByDay)
        {
            this.activityByDay = activityByDay;
        }

        // GET: ActivityByDay
        public ActionResult Index()
        {
            return View("~/Views/Data/ActivityByX.cshtml", activityByDay.Query());
        }
    }
}