using ICS.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class _PartialController : Controller
    {     
        // GET: Partial
        [ChildActionOnly]
        public ActionResult _HeaderPartial()
        {
            return PartialView("_HeaderPartial");
        }

        [ChildActionOnly]
        public ActionResult _LeftSideMenuPartial()
        {
            return PartialView("_LeftSideMenuPartial");
        }

        [ChildActionOnly]
        public ActionResult _FooterPartial()
        {
            return PartialView("_FooterPartial");
        }
    }
}