﻿using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult About()
        {
            return View("About");
        }
    }
}