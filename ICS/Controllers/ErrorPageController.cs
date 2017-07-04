using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class ErrorPageController : Controller
    {
        [Route("403")]
        public ActionResult Forbidden()
        {
            return View();
        }

        [Route("404")]
        public ActionResult NotFound()
        {
            return View();
        }
        [Route("500")]
        public ActionResult ServerError()
        {           
            return View();
        }
        [Route("400")]
        public ActionResult Badrequest()
        {
            return View();
        }
        [Route("Error")]
        public ActionResult DefaultErrorpage()
        {
            return View();
        }
    }
}