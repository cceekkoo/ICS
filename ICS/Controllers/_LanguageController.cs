using ICS.Models;
using ICS.Models.AdminMerge;
using ICS.Utilities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    [Authorize]
    public class _LanguageController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        private CustomMethods customMethods = new CustomMethods();
        private Encryption hash = new Encryption();
        private LanguageAdminMerge language = new LanguageAdminMerge();

        private LanguageAdminMerge languageAdmin
        {
            get
            {
                language.languages = db.Languages.ToList();
                return language;
            }
            set
            {
                languageAdmin = value;
            }
        }

        // GET: Languagestest
        public ActionResult Index()
        {
            
            return View(languageAdmin);
        }

        public ActionResult Create()
        {
            return RedirectToAction("", "_Language");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Language_Short,Language_Full,ForCaptcha")]Language language, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                customMethods.ImageUploadValidation(ModelState, file, "language.image");

                if (ModelState.IsValid)
                {
                    language.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Languages.Add(language);
                    db.SaveChanges();

                    customMethods.ImageUpload(file, language.image);

                    dbContextTransaction.Commit();
                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                languageAdmin.language = language;
                ViewBag.ShowModal = "AddModal";
                return View("Index", languageAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                languageAdmin.language = language;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "AddModal";
                db = new ICSDBContext();
                return View("Index", languageAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_Language");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Language_Short,Language_Full,ForCaptcha")] Language language)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ICSDBContext context = new ICSDBContext())
                        language.image = context.Languages.Find(language.ID).image;
                    db.Entry(language).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                languageAdmin.language = language;
                return View("Index", languageAdmin);
            }
            catch 
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                languageAdmin.language = language;
                db = new ICSDBContext();
                return View("Index", languageAdmin);
            }
        }

        public ActionResult ImageChange()
        {
            return RedirectToAction("", "_Language");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImageChange(int? id, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            string image = "";
            Language language = new Language();
            try
            {
                if (id == null) return RedirectToAction("Index");

                language = db.Languages.Find(id);

                if (language == null) return RedirectToAction("Index");

                customMethods.ImageUploadValidation(ModelState, file, "language.image");

                if (ModelState.IsValid)
                {
                    image = language.image;
                    language.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Languages.Attach(language);
                    db.Entry(language).Property(x => x.image).IsModified = true;
                    db.SaveChanges();

                    customMethods.ImageUpload(file, language.image);

                    dbContextTransaction.Commit();

                    customMethods.ImageDelete(image);

                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                ViewBag.ShowModal = "ImageModal";
                languageAdmin.language = language;
                return View("Index", languageAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "ImageModal";
                languageAdmin.language = language;
                db = new ICSDBContext();
                return View("Index", languageAdmin);
            }
        }

        public ActionResult Delete()
        {
            return RedirectToAction("", "_Language");
        }

        // POST: Languagestest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            Language language = new Language();
            try
            {
                if (id == null) return RedirectToAction("Index");

                language = db.Languages.Find(id);

                if (language == null) return RedirectToAction("Index");

                string image = language.image;

                db.Languages.Remove(language);
                db.SaveChanges();

                customMethods.ImageDelete(image);

                dbContextTransaction.Commit();
                return RedirectToAction("Index");
            }
            catch
            {
                dbContextTransaction.Rollback();
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "DeleteModal";
                languageAdmin.language = language;
                db = new ICSDBContext();
                return View("Index", languageAdmin);
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