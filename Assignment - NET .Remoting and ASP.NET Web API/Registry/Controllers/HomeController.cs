using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBWebService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Kalk Services Registry";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Kalk Services provides simple calculation functions for users.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me at 19754655@student.curtin.edu.au.";

            return View();
        }
    }
}