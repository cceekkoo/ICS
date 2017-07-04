using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return Redirect("http://mail.intercs.az/Login.aspx");
        }
    }
}