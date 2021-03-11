using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlightCenterWebApi.Controllers
{
    public class AnnonymousController : Controller
    {
        // GET: Annonymous
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UpcomingDepartures()
        {
            return View("UpcomingDepartures");
        }
        public ActionResult UpcomingArrivals()
        {
            return new FilePathResult("~/Views/Annonymous/UpcomingArrivals.html", "text/html");
        }
        public ActionResult Test()
        {
            return new FilePathResult("~/Views/Annonymous/Test.html", "text/html");
        }
    }
}