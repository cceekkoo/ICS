using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ICS.Models;

namespace ICS.Controllers
{
    public class About_TranslateController : Controller
    {
        private ICSDBContext db = new ICSDBContext();

        // GET: About_Translate
        public ActionResult Index()
        {
            var about_Translate = db.About_Translate.Include(a => a.About).Include(a => a.Language);
            return View(about_Translate.ToList());
        }

        // GET: About_Translate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About_Translate about_Translate = db.About_Translate.Find(id);
            if (about_Translate == null)
            {
                return HttpNotFound();
            }
            return View(about_Translate);
        }

        // GET: About_Translate/Create
        public ActionResult Create()
        {
            ViewBag.Value_ID = new SelectList(db.Abouts, "ID", "image");
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
            return View();
        }

        // POST: About_Translate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Text,Value_ID,Language_ID")] About_Translate about_Translate)
        {
            if (ModelState.IsValid)
            {
                db.About_Translate.Add(about_Translate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Value_ID = new SelectList(db.Abouts, "ID", "image", about_Translate.Value_ID);
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short", about_Translate.Language_ID);
            return View(about_Translate);
        }

        // GET: About_Translate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About_Translate about_Translate = db.About_Translate.Find(id);
            if (about_Translate == null)
            {
                return HttpNotFound();
            }
            ViewBag.Value_ID = new SelectList(db.Abouts, "ID", "image", about_Translate.Value_ID);
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short", about_Translate.Language_ID);
            return View(about_Translate);
        }

        // POST: About_Translate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Text,Value_ID,Language_ID")] About_Translate about_Translate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(about_Translate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Value_ID = new SelectList(db.Abouts, "ID", "image", about_Translate.Value_ID);
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short", about_Translate.Language_ID);
            return View(about_Translate);
        }

        // GET: About_Translate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About_Translate about_Translate = db.About_Translate.Find(id);
            if (about_Translate == null)
            {
                return HttpNotFound();
            }
            return View(about_Translate);
        }

        // POST: About_Translate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            About_Translate about_Translate = db.About_Translate.Find(id);
            db.About_Translate.Remove(about_Translate);
            db.SaveChanges();
            return RedirectToAction("Index");
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
