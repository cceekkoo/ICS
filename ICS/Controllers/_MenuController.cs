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
    public class _MenuController : Controller
    {
        private ICSDBContext db = new ICSDBContext();

        private MenuAdminMerge menuAdmin
        {
            get
            {
                MenuAdminMerge menu = new MenuAdminMerge();
                menu.menus = db.Menus_Translate.OrderBy(x => x.Value_ID).ThenBy(x => x.Language_ID).ToList();
                menu.defaultLanguageID = db.Languages.FirstOrDefault().ID;
                menu.languages = db.Languages.Where(x => x.ID != db.Languages.FirstOrDefault().ID);
                ViewBag.Parent_ID = new SelectList(db.Menus.Where(x => x.Parent_ID == null).
                    Join(db.Menus_Translate.Where(x=>x.Language_ID == db.Languages.FirstOrDefault().ID), a => a.ID,
              m => m.Value_ID,
              (a, m) => new { a.ID, m.Text }), "ID", "Text");
                return menu;
            }
            set
            {
                menuAdmin = value;
            }
        }

        // GET: Menustest
        public ActionResult Index()
        {
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
            return View(menuAdmin);
        }

        public ActionResult CreateTranslate()
        {
            return RedirectToAction("", "_Menu");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTranslate([Bind(Include = "ID,Text")]Menus_Translate menu_Translate, int Language_ID)
        {
            try
            {
                menu_Translate.Language_ID = Language_ID;
                if (ModelState.IsValid)
                {
                    using (ICSDBContext context = new ICSDBContext())
                        menu_Translate.Value_ID = context.Menus_Translate.Find(menu_Translate.ID).Menu.ID;
                    db.Menus_Translate.Add(menu_Translate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                menuAdmin.menu_Translate = menu_Translate;
                int[] ID = db.Menus_Translate.
                    Where(y => y.Value_ID == menu_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", menuAdmin);
            }
            catch
            {
                menuAdmin.menu_Translate = menu_Translate;
                int[] ID = db.Menus_Translate.
                    Where(y => y.Value_ID == menu_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", menuAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_Menu");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Text,Menu")] Menus_Translate menu_Translate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Menu menu = new Menu();
                    using (ICSDBContext context = new ICSDBContext())
                        menu.ID = context.Menus_Translate.Find(menu_Translate.ID).Value_ID;
                    menu.Url = menu_Translate.Menu.Url;
                    menu.Sorting = menu_Translate.Menu.Sorting;
                    menu.Parent_ID = menu_Translate.Menu.Parent_ID;
                    db.Entry(menu).State = EntityState.Modified;
                    db.Menus_Translate.Attach(menu_Translate);
                    db.Entry(menu_Translate).Property(x => x.Text).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                menuAdmin.menu_Translate = menu_Translate;
                return View("Index", menuAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                menuAdmin.menu_Translate = menu_Translate;
                return View("Index", menuAdmin);
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