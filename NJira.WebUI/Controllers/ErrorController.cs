using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NJira.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Oops()
        {
            ViewBag.Error = (TempData.Count != 0) ? TempData["Error"].ToString() : string.Empty;
            return View();
        }
    }
}