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
    public class _Site_ImagesController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        private CustomMethods customMethods = new CustomMethods();
        private Encryption hash = new Encryption();

        private Site_ImagesAdminMerge site_ImagesAdmin
        {
            get
            {
                Site_ImagesAdminMerge site_Images = new Site_ImagesAdminMerge();
                site_Images.site_Images = db.Site_Images.ToList();
                return site_Images;
            }
            set
            {
                site_ImagesAdmin = value;
            }
        }

        // GET: Site_Imagestest
        public ActionResult Index()
        {
            return View(site_ImagesAdmin);
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_Site_Images");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description")] Site_Images site_Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ICSDBContext context = new ICSDBContext())
                        site_Image.image = context.Site_Images.Find(site_Image.ID).image;
                    db.Site_Images.Attach(site_Image);
                    db.Entry(site_Image).Property(x => x.Description).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                site_ImagesAdmin.site_Image = site_Image;
                return View("Index", site_ImagesAdmin);
            }
            catch
            {
                ViewBag.Mesage = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                site_ImagesAdmin.site_Image = site_Image;
                return View("Index", site_ImagesAdmin);
            }
        }

        public ActionResult ImageChange()
        {
            return RedirectToAction("", "_Site_Images");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImageChange(int? id, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            string image = "";
            Site_Images site_Image = new Site_Images();
            try
            {
                if (id == null) return RedirectToAction("BadRequest", "ErrorPage");

                site_Image = db.Site_Images.Find(id);

                if (site_Image == null) return RedirectToAction("NotFound", "ErrorPage");

                customMethods.ImageUploadValidation(ModelState, file, "site_Images.Site_Images.image");

                if (ModelState.IsValid)
                {
                    image = site_Image.image;
                    site_Image.image = hash.MD5(string.Format("{0:ddMMyyyyhhmms}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Entry(site_Image).State = EntityState.Modified;
                    db.SaveChanges();

                    customMethods.ImageUpload(file, site_Image.image);

                    dbContextTransaction.Commit();

                    customMethods.ImageDelete(image);

                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                ViewBag.ShowModal = "ImageModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                site_ImagesAdmin.site_Image = site_Image;
                return View("Index", site_ImagesAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                ViewBag.Mesage = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "ImageModal";
                site_ImagesAdmin.site_Image = site_Image;
                return View("Index", site_ImagesAdmin);
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