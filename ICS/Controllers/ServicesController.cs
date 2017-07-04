using ICS.Models;
using ICS.Models.Merge;
using ICS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class ServicesController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        // GET: Services
        public ActionResult Index()
        {
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];
            TempData["active"] = "4";

            ServicesMerge servicesMerge = new ServicesMerge();
            servicesMerge.services = db.Services_Translate.Where(x => x.Language_ID == CurrentLanguage.Language).ToList();
            servicesMerge.image = db.Site_Images.FirstOrDefault(x => x.ID == 2).image;
            servicesMerge.menu = db.Menus_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 4).Text;
            return View(servicesMerge);
        }
    }
}