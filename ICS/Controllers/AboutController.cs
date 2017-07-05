using ICS.Models;
using ICS.Models.Merge;
using ICS.Utilities;
using System.Linq;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        private ICSDBContext db = new ICSDBContext();        
        public ActionResult Index()
        {
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];
            TempData["active"] = "2";

            AboutMerge aboutMerge = new AboutMerge();
            aboutMerge.about = db.About_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language);
            aboutMerge.image = db.Site_Images.FirstOrDefault(x => x.ID == 2).image;
            aboutMerge.menu = db.Menus_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 2).Text;
            return View(aboutMerge);
        }
    }
}