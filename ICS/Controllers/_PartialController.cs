using ICS.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ICS.Controllers
{ 

    [Authorize]
    public class _PartialController : Controller
    {
        private ICSDBContext db = new ICSDBContext();

        [ChildActionOnly]
        public ActionResult _HeaderPartial()
        {
            ViewBag.User = db.Users.Find(Convert.ToInt32(User.Identity.Name));
            return PartialView("_HeaderPartial");
        }

        [ChildActionOnly]
        public ActionResult _LeftSideMenuPartial()
        {
            ViewBag.User = db.Users.Find(Convert.ToInt32(User.Identity.Name));
            return PartialView("_LeftSideMenuPartial");
        }

        [ChildActionOnly]
        public ActionResult _FooterPartial()
        {
            return PartialView("_FooterPartial");
        }
    }
}