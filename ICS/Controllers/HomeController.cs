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
    public class HomeController : Controller
    {
        // GET: Home
        private ICSDBContext db = new ICSDBContext();
        public ActionResult Index()
        {
            //throw new HttpException(500, "");
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];
            TempData["active"] = "1";
            
            HomeMerge homeMerge = new HomeMerge();
            homeMerge.step = db.Steps_Translate.Where(x => x.Language_ID == CurrentLanguage.Language).ToList();
            homeMerge.slide = db.Slide_Translate.Where(x => x.Language_ID == CurrentLanguage.Language).ToList();
            homeMerge.slideActive = homeMerge.slide.FirstOrDefault().Value_ID;
            return View(homeMerge);
        }
    }
}