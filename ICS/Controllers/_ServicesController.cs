using ICS.Models;
using ICS.Models.AdminMerge;
using ICS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class _ServicesController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        private CustomMethods customMethods = new CustomMethods();
        private Encryption hash = new Encryption();

        private ServicesAdminMerge servicesAdmin
        {
            get
            {
                ServicesAdminMerge service = new ServicesAdminMerge();
                service.services = db.Services_Translate.OrderBy(x => x.Value_ID).ThenBy(x => x.Language_ID).ToList();
                service.defaultLanguageID = db.Languages.FirstOrDefault().ID;
                service.languages = db.Languages.Where(x => x.ID != db.Languages.FirstOrDefault().ID);
                return service;
            }
            set
            {
                servicesAdmin = value;
            }
        }
        
        public ActionResult Index()
        {
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
            return View(servicesAdmin);
        }

        public ActionResult Create()
        {
            return RedirectToAction("", "_Services");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Title,Text")]Services_Translate services_Translate, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                customMethods.ImageUploadValidation(ModelState, file, "services_Translate.Service.image");

                if (ModelState.IsValid)
                {
                    services_Translate.Service = new Service();
                    services_Translate.Service.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";

                    services_Translate.Text = services_Translate.Text == null ? "" : services_Translate.Text;
                    services_Translate.Language_ID = db.Languages.FirstOrDefault().ID;
                    services_Translate.Value_ID = services_Translate.Service.ID;
                    db.Services_Translate.Add(services_Translate);
                    db.SaveChanges();

                    customMethods.ImageUpload(file, services_Translate.Service.image);

                    dbContextTransaction.Commit();
                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                servicesAdmin.services_Translate = services_Translate;
                ViewBag.ShowModal = "AddModal";
                return View("Index", servicesAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                servicesAdmin.services_Translate = services_Translate;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "AddModal";
                return View("Index", servicesAdmin);
            }
        }

        public ActionResult CreateTranslate()
        {
            return RedirectToAction("", "_Services");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateTranslate([Bind(Include = "ID,Title,Text")]Services_Translate services_Translate, int Language_ID, HttpPostedFileBase file)
        {
            try
            {
                services_Translate.Language_ID = Language_ID;
                if (ModelState.IsValid)
                {
                    services_Translate.Text = services_Translate.Text == null ? "" : services_Translate.Text;
                    using (ICSDBContext context = new ICSDBContext())
                        services_Translate.Value_ID = context.Services_Translate.Find(services_Translate.ID).Service.ID;
                    db.Services_Translate.Add(services_Translate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                servicesAdmin.services_Translate = services_Translate;
                int[] ID = db.Services_Translate.
                    Where(y => y.Value_ID == services_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", servicesAdmin);
            }
            catch
            {
                servicesAdmin.services_Translate = services_Translate;
                int[] ID = db.Services_Translate.
                    Where(y => y.Value_ID == services_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", servicesAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_Services");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Title,Text")] Services_Translate services_Translate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Services_Translate.Attach(services_Translate);
                    db.Entry(services_Translate).Property(x => x.Title).IsModified = true;
                    db.Entry(services_Translate).Property(x => x.Text).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                servicesAdmin.services_Translate = services_Translate;
                return View("Index", servicesAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                servicesAdmin.services_Translate = services_Translate;
                return View("Index", servicesAdmin);
            }
        }

        public ActionResult ImageChange()
        {
            return RedirectToAction("", "_Services");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ImageChange(int? id, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            string image = "";
            Services_Translate services_Translate = new Services_Translate();
            try
            {
                if (id == null) return RedirectToAction("BadRequest", "ErrorPage");

                services_Translate = db.Services_Translate.Find(id);

                if (services_Translate == null) return RedirectToAction("NotFound", "ErrorPage");

                customMethods.ImageUploadValidation(ModelState, file, "services_Translate.Service.image");

                if (ModelState.IsValid)
                {
                    image = services_Translate.Service.image;
                    services_Translate.Service.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Entry(services_Translate.Service).State = EntityState.Modified;
                    db.SaveChanges();

                    customMethods.ImageUpload(file, services_Translate.Service.image);

                    dbContextTransaction.Commit();

                    customMethods.ImageDelete(image);

                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                ViewBag.ShowModal = "ImageModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                servicesAdmin.services_Translate = services_Translate;
                return View("Index", servicesAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "ImageModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                services_Translate.Service.image = image;
                servicesAdmin.services_Translate = services_Translate;
                return View("Index", servicesAdmin);
            }
        }

        public ActionResult Delete()
        {
            return RedirectToAction("", "_Services");
        }

        // POST: Aboutstest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            Services_Translate services_Translate = new Services_Translate();
            try
            {
                if (id == null) return RedirectToAction("BadRequest", "ErrorPage");

                services_Translate = db.Services_Translate.Find(id);

                if (services_Translate == null) return RedirectToAction("NotFound", "ErrorPage");

                string image = services_Translate.Service.image;

                db.Services.Remove(services_Translate.Service);
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
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                servicesAdmin.services_Translate = services_Translate;
                return View("Index", servicesAdmin);
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