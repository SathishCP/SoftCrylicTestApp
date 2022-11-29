using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftCrylicTestApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public RedirectResult Swagger()
        {
            return new RedirectResult("/swagger");
        }
    }
}