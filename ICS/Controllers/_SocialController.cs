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
    public class _SocialController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        private CustomMethods customMethods = new CustomMethods();
        private Encryption hash = new Encryption();

        private SocialAdminMerge socialAdmin
        {
            get
            {
                SocialAdminMerge socials = new SocialAdminMerge();
                socials.socials = db.Socials.ToList();
                return socials;
            }
            set
            {
                socialAdmin = value;
            }
        }

        // GET: Socialstest
        public ActionResult Index()
        {
            return View(socialAdmin);
        }

        public ActionResult Create()
        {
            return RedirectToAction("", "_Social");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Url,image")]Social social, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                customMethods.ImageUploadValidation(ModelState, file, "social.image");

                if (ModelState.IsValid)
                {
                    social.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Socials.Add(social);
                    db.SaveChanges();

                    customMethods.ImageUpload(file, social.image);

                    dbContextTransaction.Commit();
                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                socialAdmin.social = social;
                ViewBag.ShowModal = "AddModal";
                return View("Index", socialAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                socialAdmin.social = social;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "AddModal";
                db = new ICSDBContext();
                return View("Index", socialAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_Social");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Url")] Social social)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ICSDBContext context = new ICSDBContext())
                        social.image = context.Socials.Find(social.ID).image;
                    db.Socials.Attach(social);
                    db.Entry(social).Property(x => x.Url).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                socialAdmin.social = social;
                return View("Index", socialAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                socialAdmin.social = social;
                db = new ICSDBContext();
                return View("Index", socialAdmin);
            }
        }

        public ActionResult ImageChange()
        {
            return RedirectToAction("", "_Social");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImageChange(int? id, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            string image = "";
            Social social = new Social();
            try
            {
                if (id == null) return RedirectToAction("Index");

                social = db.Socials.Find(id);

                if (social == null) return RedirectToAction("Index");

                customMethods.ImageUploadValidation(ModelState, file, "social.image");

                if (ModelState.IsValid)
                {
                    image = social.image;
                    social.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Entry(social).State = EntityState.Modified;
                    db.SaveChanges();

                    customMethods.ImageUpload(file, social.image);

                    dbContextTransaction.Commit();

                    customMethods.ImageDelete(image);

                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                ViewBag.ShowModal = "ImageModal";
                
                socialAdmin.social = social;
                return View("Index", socialAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "ImageModal";
                socialAdmin.social = social;
                db = new ICSDBContext();
                return View("Index", socialAdmin);
            }
        }

        public ActionResult Delete()
        {
            return RedirectToAction("", "_Social");
        }

        // POST: Socialstest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            Social social = new Social();
            try
            {
                if (id == null) return RedirectToAction("Index");

                social = db.Socials.Find(id);

                if (social == null) return RedirectToAction("Index");

                db.Socials.Remove(social);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "DeleteModal";
                
                socialAdmin.social = social;
                db = new ICSDBContext();
                return View("Index", socialAdmin);
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