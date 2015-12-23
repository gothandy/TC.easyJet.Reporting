using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Vincente.Azure;
using Vincente.Azure.Tables;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class WipController : Controller
    {
        private JoinClient joinClient;

        public WipController(JoinClient joinClient)
        {
            this.joinClient = joinClient;
        }

        // GET: Wip
        public ActionResult ByList()
        {
            IEnumerable<JoinModel> data = joinClient.GetJoinedData();

            return View(AllWip(data));
        }

        public ActionResult Detail(int? list)
        {
            IEnumerable<JoinModel> data = joinClient.GetJoinedData();

            return View(GetByList(data, list));
        }

        private static IEnumerable<JoinModel> GetByList(IEnumerable<JoinModel> data, int? list)
        {
            return from e in data
                   where e.ListIndex == list
                   group e by new
                   {
                       e.CardId,
                       e.ListName,
                       e.Epic,
                       e.DomId,
                       e.Name
                   } into g
                   select new JoinModel()
                   {
                       CardId = g.Key.CardId,
                       ListName = g.Key.ListName,
                       Epic = g.Key.Epic,
                       DomId = g.Key.DomId,
                       Name = g.Key.Name,
                       Billable = g.Sum(e => e.Billable)
                   };
        }



        private IEnumerable<JoinModel> AllWip(IEnumerable<JoinModel> data)
        {
            var result =
                from e in data
                
                group e by new
                {
                    e.ListIndex,
                    e.ListName
                } into g
                orderby g.Key.ListIndex
                select new JoinModel()
                {
                    ListIndex = g.Key.ListIndex,
                    ListName = g.Key.ListName,
                    Billable = g.Sum(e => e.Billable)
                };

            return result;
        }
    }
}