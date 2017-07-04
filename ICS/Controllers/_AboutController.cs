using ICS.Models;
using ICS.Models.AdminMerge;
using ICS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class _AboutController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        private CustomMethods customMethods = new CustomMethods();
        private Encryption hash = new Encryption();

        private AboutAdminMerge aboutAdmin
        {
            get
            {
                AboutAdminMerge about = new AboutAdminMerge();
                about.abouts = db.About_Translate.OrderBy(x => x.Value_ID).ThenBy(x => x.Language_ID).ToList();
                about.defaultLanguageID = db.Languages.FirstOrDefault().ID;
                about.languages = db.Languages.Where(x => x.ID != db.Languages.FirstOrDefault().ID);
                return about;
            }
            set
            {
                aboutAdmin = value;
            }
        }

        // GET: Aboutstest
        public ActionResult Index()
        {
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
            return View(aboutAdmin);
        }
        
        public ActionResult CreateTranslate()
        {
            return RedirectToAction("", "_About");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateTranslate([Bind(Include = "ID,Title,Text")]About_Translate about_Translate, int Language_ID, HttpPostedFileBase file)
        {
            try
            {
                about_Translate.Language_ID = Language_ID;
                if (ModelState.IsValid)
                {
                    about_Translate.Text = about_Translate.Text == null ? "" : about_Translate.Text;
                    using (ICSDBContext context = new ICSDBContext())
                        about_Translate.Value_ID = context.About_Translate.Find(about_Translate.ID).About.ID;
                    db.About_Translate.Add(about_Translate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                aboutAdmin.about_Translate = about_Translate;
                int[] ID = db.About_Translate.
                    Where(y => y.Value_ID == about_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", aboutAdmin);
            }
            catch
            {
                aboutAdmin.about_Translate = about_Translate;
                int[] ID = db.About_Translate.
                    Where(y => y.Value_ID == about_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", aboutAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_About");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Title,Text")] About_Translate about_Translate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.About_Translate.Attach(about_Translate);
                    db.Entry(about_Translate).Property(x => x.Title).IsModified = true;
                    db.Entry(about_Translate).Property(x => x.Text).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                aboutAdmin.about_Translate = about_Translate;
                return View("Index", aboutAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                aboutAdmin.about_Translate = about_Translate;
                return View("Index", aboutAdmin);
            }
        }

        public ActionResult ImageChange()
        {
            return RedirectToAction("", "_About");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImageChange(int? id, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            string image = "";
            About_Translate about_Translate = new About_Translate();
            try
            {
                if (id == null) return RedirectToAction("BadRequest", "ErrorPage");

                about_Translate = db.About_Translate.Find(id);

                if (about_Translate == null) return RedirectToAction("NotFound", "ErrorPage");

                customMethods.ImageUploadValidation(ModelState, file, "about_Translate.About.image");

                if (ModelState.IsValid)
                {
                    image = about_Translate.About.image;
                    about_Translate.About.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Entry(about_Translate.About).State = EntityState.Modified;
                    db.SaveChanges();

                    customMethods.ImageUpload(file, about_Translate.About.image);

                    dbContextTransaction.Commit();

                    customMethods.ImageDelete(image);

                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                ViewBag.ShowModal = "ImageModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                aboutAdmin.about_Translate = about_Translate;
                return View("Index", aboutAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "ImageModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                aboutAdmin.about_Translate = about_Translate;
                return View("Index", aboutAdmin);
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