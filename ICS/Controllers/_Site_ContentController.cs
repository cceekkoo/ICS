using ICS.Models;
using ICS.Models.AdminMerge;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    [Authorize]
    public class _Site_ContentController : Controller
    {
        private ICSDBContext db = new ICSDBContext();

        private Site_ContentsAdminMerge site_ContentAdmin
        {
            get
            {
                Site_ContentsAdminMerge site_Content = new Site_ContentsAdminMerge();
                site_Content.site_Contents = db.Site_Contents.OrderBy(x => x.Value_ID).ThenBy(x => x.Language_ID).ToList();
                site_Content.defaultLanguageID = db.Languages.FirstOrDefault().ID;
                site_Content.languages = db.Languages.Where(x => x.ID != db.Languages.FirstOrDefault().ID);
                return site_Content;
            }
            set
            {
                site_ContentAdmin = value;
            }
        }

        // GET: Site_Contentstest
        public ActionResult Index()
        {
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
            return View(site_ContentAdmin);
        }

        public ActionResult CreateTranslate()
        {
            return RedirectToAction("", "_Site_Content");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTranslate([Bind(Include = "ID,Text")]Site_Contents site_Content, int Language_ID)
        {
            try
            {
                site_Content.Language_ID = Language_ID;
                if (ModelState.IsValid)
                {
                    site_Content.Value_ID = site_Content.ID;
                    db.Site_Contents.Add(site_Content);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                site_ContentAdmin.site_Content = site_Content;
                int[] ID = db.Site_Contents.
                    Where(y => y.Value_ID == site_Content.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", site_ContentAdmin);
            }
            catch
            {
                site_ContentAdmin.site_Content = site_Content;
                int[] ID = db.Site_Contents.
                    Where(y => y.Value_ID == site_Content.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", site_ContentAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_Site_Content");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Text")] Site_Contents site_Content)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Site_Contents.Attach(site_Content);
                    db.Entry(site_Content).Property(x => x.Text).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                site_ContentAdmin.site_Content = site_Content;
                return View("Index", site_ContentAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                site_ContentAdmin.site_Content = site_Content;
                return View("Index", site_ContentAdmin);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}