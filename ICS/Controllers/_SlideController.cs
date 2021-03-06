﻿using ICS.Models;
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
    public class _SlideController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        private CustomMethods customMethods = new CustomMethods();
        private Encryption hash = new Encryption();
        private SlideAdminMerge slide = new SlideAdminMerge();

        private SlideAdminMerge slideAdmin
        {
            get
            {
                slide.slides = db.Slide_Translate.OrderBy(x => x.Value_ID).ThenBy(x => x.Language_ID).ToList();
                slide.defaultLanguageID = db.Languages.FirstOrDefault().ID;
                slide.languages = db.Languages.Where(x => x.ID != db.Languages.FirstOrDefault().ID);
                return slide;
            }
            set
            {
                slideAdmin = value;
            }
        }

        // GET: Slidestest
        public ActionResult Index()
        {
            
            return View(slideAdmin);
        }

        public ActionResult Create()
        {
            return RedirectToAction("", "_Slide");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Title,Text")]Slide_Translate slide_Translate, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                customMethods.ImageUploadValidation(ModelState, file, "slide_Translate.Slide.image");

                if (ModelState.IsValid)
                {
                    slide_Translate.Slide = new Slide();
                    slide_Translate.Slide.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";

                    slide_Translate.Text = slide_Translate.Text == null ? "" : slide_Translate.Text;
                    slide_Translate.Language_ID = db.Languages.FirstOrDefault().ID;
                    slide_Translate.Value_ID = slide_Translate.Slide.ID;
                    db.Slide_Translate.Add(slide_Translate);
                    db.SaveChanges();

                    customMethods.ImageUpload(file, slide_Translate.Slide.image);

                    dbContextTransaction.Commit();
                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                slideAdmin.slide_Translate = slide_Translate;
                ViewBag.ShowModal = "AddModal";
                return View("Index", slideAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                slideAdmin.slide_Translate = slide_Translate;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "AddModal";
                db = new ICSDBContext();
                return View("Index", slideAdmin);
            }
        }

        public ActionResult CreateTranslate()
        {
            return RedirectToAction("", "_Slide");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateTranslate([Bind(Include = "ID,Title,Text")]Slide_Translate slide_Translate, int Language_ID, HttpPostedFileBase file)
        {
            try
            {
                slide_Translate.Language_ID = Language_ID;
                if (ModelState.IsValid)
                {
                    slide_Translate.Text = slide_Translate.Text == null ? "" : slide_Translate.Text;
                    using (ICSDBContext context = new ICSDBContext())
                        slide_Translate.Value_ID = context.Slide_Translate.Find(slide_Translate.ID).Slide.ID;
                    db.Slide_Translate.Add(slide_Translate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                slideAdmin.slide_Translate = slide_Translate;
                int[] ID = db.Slide_Translate.
                    Where(y => y.Value_ID == slide_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", slideAdmin);
            }
            catch
            {
                slideAdmin.slide_Translate = slide_Translate;
                int[] ID = db.Slide_Translate.
                    Where(y => y.Value_ID == slide_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "TranslateModal";
                db = new ICSDBContext();
                return View("Index", slideAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_Slide");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Title,Text")] Slide_Translate slide_Translate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Slide_Translate.Attach(slide_Translate);
                    db.Entry(slide_Translate).Property(x => x.Title).IsModified = true;
                    db.Entry(slide_Translate).Property(x => x.Text).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                
                slideAdmin.slide_Translate = slide_Translate;
                return View("Index", slideAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                
                slideAdmin.slide_Translate = slide_Translate;
                db = new ICSDBContext();
                return View("Index", slideAdmin);
            }
        }

        public ActionResult ImageChange()
        {
            return RedirectToAction("", "_Slide");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImageChange(int? id, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            string image = "";
            Slide_Translate slide_Translate = new Slide_Translate();
            try
            {
                if (id == null) return RedirectToAction("Index");

                slide_Translate = db.Slide_Translate.Find(id);

                if (slide_Translate == null) return RedirectToAction("Index");

                customMethods.ImageUploadValidation(ModelState, file, "slide_Translate.Slide.image");

                if (ModelState.IsValid)
                {
                    image = slide_Translate.Slide.image;
                    slide_Translate.Slide.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Entry(slide_Translate.Slide).State = EntityState.Modified;
                    db.SaveChanges();

                    customMethods.ImageUpload(file, slide_Translate.Slide.image);

                    dbContextTransaction.Commit();

                    customMethods.ImageDelete(image);

                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                ViewBag.ShowModal = "ImageModal";
                
                slideAdmin.slide_Translate = slide_Translate;
                return View("Index", slideAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "ImageModal";
                
                slideAdmin.slide_Translate = slide_Translate;
                db = new ICSDBContext();
                return View("Index", slideAdmin);
            }
        }

        public ActionResult Delete()
        {
            return RedirectToAction("", "_Slide");
        }

        // POST: Slidestest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            Slide_Translate slide_Translate = new Slide_Translate();
            try
            {
                if (id == null) return RedirectToAction("Index");

                slide_Translate = db.Slide_Translate.Find(id);

                if (slide_Translate == null) return RedirectToAction("Index");

                string image = slide_Translate.Slide.image;

                db.Slides.Remove(slide_Translate.Slide);
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
                
                slideAdmin.slide_Translate = slide_Translate;
                db = new ICSDBContext();
                return View("Index", slideAdmin);
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