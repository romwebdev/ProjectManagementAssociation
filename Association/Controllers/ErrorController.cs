using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Association.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Error()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Error");
            }
            return View();
        }
    }
}