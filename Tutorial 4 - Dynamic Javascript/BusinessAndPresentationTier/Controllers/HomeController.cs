using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessAndPresentationTier.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Users()
        {
            ViewBag.Title = "Users";

            return View();
        }

        public ActionResult Accounts()
        {
            ViewBag.Title = "Accounts";

            return View();
        }

        public ActionResult Transactions()
        {
            ViewBag.Title = "Transactions";

            return View();
        }
    }
}
