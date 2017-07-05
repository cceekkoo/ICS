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
    public class _ContactController : Controller
    {
        private ICSDBContext db = new ICSDBContext();

        private ContactAdminMerge contactAdmin
        {
            get
            {
                ContactAdminMerge contacts = new ContactAdminMerge();
                contacts.contacts = db.Contacts.ToList();
                return contacts;
            }
            set
            {
                contactAdmin = value;
            }
        }

        // GET: Contactstest
        public ActionResult Index()
        {
            return View(contactAdmin);
        }

        public ActionResult Create()
        {
            return RedirectToAction("", "_Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Text,icon")]Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ShowModal = "AddModal";
                return View("Index", contactAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "AddModal";
                contactAdmin.contact = contact;
                return View("Index", contactAdmin);
            }
        }
      
        public ActionResult Edit()
        {
            return RedirectToAction("", "_Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Text,icon")] Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                contactAdmin.contact = contact;
                return View("Index", contactAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                contactAdmin.contact = contact;
                return View("Index", contactAdmin);
            }
        }
               
        public ActionResult Delete()
        {
            return RedirectToAction("", "_Contact");
        }

        // POST: Contactstest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            Contact contact = new Contact();
            try
            {
                if (id == null) return RedirectToAction("BadRequest", "ErrorPage");

                contact = db.Contacts.Find(id);

                if (contact == null) return RedirectToAction("NotFound", "ErrorPage");

                db.Contacts.Remove(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "DeleteModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                contactAdmin.contact = contact;
                return View("Index", contactAdmin);
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