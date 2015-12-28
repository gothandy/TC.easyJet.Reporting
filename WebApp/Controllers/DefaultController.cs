using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using WebApp.Models;
using Vincente.Data.Interfaces;
using Vincente.Data.Entities;
using Vincente.WebApp.Controllers;
using Vincente.WebApp.Models;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        private ModelParameters p;

        public DefaultController(ModelParameters modelParameters)
        {
            p = modelParameters;
        }
        // GET: Default
        public ActionResult Index()
        {
            DefaultModel model = new DefaultModel(p);

            return View(model);
        }
    }
}