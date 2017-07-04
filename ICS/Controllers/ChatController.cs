using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return Redirect("https://dashboard.tawk.to/login");
        }
    }
}