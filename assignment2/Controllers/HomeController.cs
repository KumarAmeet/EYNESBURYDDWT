using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Food Deliver Report";
            return View();
        }

        public ActionResult Details()
        {
            ViewBag.Title = "Deliver Details";
            return View();
        }

        public ActionResult OrderHubs()
        {
            ViewBag.Title = "Hub Order Report";
            return View();
        }

        public ActionResult Payment()
        {
            ViewBag.Title = "Payment Analysis";
            return View();
        }
    }
}