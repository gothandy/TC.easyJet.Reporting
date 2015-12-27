using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using WebApp.Models;
using Vincente.Data.Interfaces;
using Vincente.Data.Entities;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        private DefaultModel model;

        public DefaultController(DefaultModel model)
        {
            this.model = model;
        }

        // GET: Default
        public ActionResult Index()
        {
            return View(model);
        }
    }
}