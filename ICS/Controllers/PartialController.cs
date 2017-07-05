using ICS.Models;
using ICS.Models.Merge;
using ICS.Utilities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class PartialController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        // GET: Partial
        [ChildActionOnly]
        public ActionResult _HeaderPartial()
        {
            HeaderPartialMerge headerPartialMerge = new HeaderPartialMerge();
            headerPartialMerge.site_Content = db.Site_Contents.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 34);
            headerPartialMerge.language = db.Languages.Where(x => x.ID != CurrentLanguage.Language).ToList();
            headerPartialMerge.currentLanguage = db.Languages.Find(CurrentLanguage.Language);
            headerPartialMerge.menus = db.Menus_Translate.Where(x => x.Language_ID == CurrentLanguage.Language).OrderBy(x=>x.Menu.Sorting).ToList();
            headerPartialMerge.activeMenu = Convert.ToInt32(TempData["active"]);
            headerPartialMerge.social = db.Socials.ToList();

            return PartialView("_HeaderPartial", headerPartialMerge);
        }
        [ChildActionOnly]
        public ActionResult _FooterPartial()
        {
            FooterPartialMerge footerPartialMerge = new FooterPartialMerge();
            footerPartialMerge.site_Contents = db.Site_Contents.Where(x => x.Language_ID == CurrentLanguage.Language && (x.Value_ID == 7 || x.Value_ID == 10)).ToList();
            footerPartialMerge.about = db.About_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language);
            footerPartialMerge.address = db.Address_Translate.Where(x => x.Language_ID == CurrentLanguage.Language).ToList();
            footerPartialMerge.contact = db.Contacts.ToList();
            footerPartialMerge.social = db.Socials.ToList();

            return PartialView("_FooterPartial", footerPartialMerge);
        }
    }
}