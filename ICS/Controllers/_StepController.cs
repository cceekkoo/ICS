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
    public class _StepController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        private CustomMethods customMethods = new CustomMethods();
        private Encryption hash = new Encryption();

        private StepAdminMerge stepAdmin
        {
            get
            {
                StepAdminMerge step = new StepAdminMerge();
                step.steps = db.Steps_Translate.OrderBy(x => x.Value_ID).ThenBy(x => x.Language_ID).ToList();
                step.defaultLanguageID = db.Languages.FirstOrDefault().ID;
                step.languages = db.Languages.Where(x => x.ID != db.Languages.FirstOrDefault().ID);
                return step;
            }
            set
            {
                stepAdmin = value;
            }
        }

        // GET: Stepstest
        public ActionResult Index()
        {
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
            return View(stepAdmin);
        }

        public ActionResult CreateTranslate()
        {
            return RedirectToAction("", "_Step");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTranslate([Bind(Include = "ID,Text")]Steps_Translate step_Translate, int Language_ID, HttpPostedFileBase file)
        {
            try
            {
                step_Translate.Language_ID = Language_ID;
                if (ModelState.IsValid)
                {
                    using (ICSDBContext context = new ICSDBContext())
                        step_Translate.Value_ID = context.Steps_Translate.Find(step_Translate.ID).Step.ID;
                    db.Steps_Translate.Add(step_Translate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                stepAdmin.step_Translate = step_Translate;
                int[] ID = db.Steps_Translate.
                    Where(y => y.Value_ID == step_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", stepAdmin);
            }
            catch
            {
                stepAdmin.step_Translate = step_Translate;
                int[] ID = db.Steps_Translate.
                    Where(y => y.Value_ID == step_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", stepAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_Step");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Text,Step")] Steps_Translate step_Translate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Step step = new Step();
                    using (ICSDBContext context = new ICSDBContext())
                    {
                        Step newStep = context.Steps_Translate.Find(step_Translate.ID).Step;
                        step.ID = newStep.ID;
                        step.image = newStep.image;
                    }
                    step.Url = step_Translate.Step.Url;
                    db.Entry(step).State = EntityState.Modified;
                    db.Steps_Translate.Attach(step_Translate);
                    db.Entry(step_Translate).Property(x => x.Text).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                stepAdmin.step_Translate = step_Translate;
                return View("Index", stepAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                stepAdmin.step_Translate = step_Translate;
                return View("Index", stepAdmin);
            }
        }

        public ActionResult ImageChange()
        {
            return RedirectToAction("", "_Step");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImageChange(int? id, HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            string image = "";
            Steps_Translate step_Translate = new Steps_Translate();
            try
            {
                if (id == null) return RedirectToAction("BadRequest", "ErrorPage");

                step_Translate = db.Steps_Translate.Find(id);

                if (step_Translate == null) return RedirectToAction("NotFound", "ErrorPage");

                customMethods.ImageUploadValidation(ModelState, file, "step_Translate.Step.image");

                if (ModelState.IsValid)
                {
                    image = step_Translate.Step.image;
                    step_Translate.Step.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Entry(step_Translate.Step).State = EntityState.Modified;
                    db.SaveChanges();

                    customMethods.ImageUpload(file, step_Translate.Step.image);

                    dbContextTransaction.Commit();

                    customMethods.ImageDelete(image);

                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                ViewBag.ShowModal = "ImageModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                stepAdmin.step_Translate = step_Translate;
                return View("Index", stepAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "ImageModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                stepAdmin.step_Translate = step_Translate;
                return View("Index", stepAdmin);
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