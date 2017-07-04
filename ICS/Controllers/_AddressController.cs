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
    public class _AddressController : Controller
    {
        private ICSDBContext db = new ICSDBContext();

        private AddressAdminMerge addressAdmin
        {
            get
            {
                AddressAdminMerge address = new AddressAdminMerge();
                address.addresses = db.Address_Translate.OrderBy(x => x.Value_ID).ThenBy(x => x.Language_ID).ToList();
                address.defaultLanguageID = db.Languages.FirstOrDefault().ID;
                address.languages = db.Languages.Where(x => x.ID != db.Languages.FirstOrDefault().ID);
                return address;
            }
            set
            {
                addressAdmin = value;
            }
        }

        // GET: Addressstest
        public ActionResult Index()
        {
            ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
            return View(addressAdmin);
        }

        public ActionResult Create()
        {
            return RedirectToAction("", "_Address");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Text,Address")]Address_Translate address_Translate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    address_Translate.Text = address_Translate.Text == null ? "" : address_Translate.Text;
                    address_Translate.Language_ID = db.Languages.FirstOrDefault().ID;
                    address_Translate.Value_ID = address_Translate.Address.ID;
                    db.Address_Translate.Add(address_Translate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                addressAdmin.address_Translate = address_Translate;
                ViewBag.ShowModal = "AddModal";
                return View("Index", addressAdmin);
            }
            catch
            {
                addressAdmin.address_Translate = address_Translate;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "AddModal";
                return View("Index", addressAdmin);
            }
        }

        public ActionResult CreateTranslate()
        {
            return RedirectToAction("", "_Address");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateTranslate([Bind(Include = "ID,Text")]Address_Translate address_Translate, int Language_ID)
        {
            try
            {
                address_Translate.Language_ID = Language_ID;
                if (ModelState.IsValid)
                {
                    address_Translate.Text = address_Translate.Text == null ? "" : address_Translate.Text;
                    using (ICSDBContext context = new ICSDBContext())
                        address_Translate.Value_ID = context.Address_Translate.Find(address_Translate.ID).Address.ID;
                    db.Address_Translate.Add(address_Translate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                addressAdmin.address_Translate = address_Translate;
                int[] ID = db.Address_Translate.
                    Where(y => y.Value_ID == address_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", addressAdmin);
            }
            catch
            {
                addressAdmin.address_Translate = address_Translate;
                int[] ID = db.Address_Translate.
                    Where(y => y.Value_ID == address_Translate.Value_ID).Select(z => z.Language_ID).ToArray();
                ViewBag.Translated = ID;
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "TranslateModal";
                return View("Index", addressAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_Address");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Text,Address")] Address_Translate address_Translate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Address address = new Address();
                    using (ICSDBContext context = new ICSDBContext())
                        address.ID = context.Address_Translate.Find(address_Translate.ID).Value_ID;
                    address.icon = address_Translate.Address.icon;
                    db.Entry(address).State = EntityState.Modified;
                    db.Address_Translate.Attach(address_Translate);
                    db.Entry(address_Translate).Property(x => x.Text).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                addressAdmin.address_Translate = address_Translate;
                return View("Index", addressAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                addressAdmin.address_Translate = address_Translate;
                return View("Index", addressAdmin);
            }
        }
               
        public ActionResult Delete()
        {
            return RedirectToAction("", "_Address");
        }

        // POST: Addressstest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            Address_Translate address_Translate = new Address_Translate();
            try
            {
                if (id == null) return RedirectToAction("BadRequest", "ErrorPage");

                address_Translate = db.Address_Translate.Find(id);

                if (address_Translate == null) return RedirectToAction("NotFound", "ErrorPage");

                db.Addresses.Remove(address_Translate.Address);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "DeleteModal";
                ViewBag.Language_ID = new SelectList(db.Languages, "ID", "Language_Short");
                addressAdmin.address_Translate = address_Translate;
                return View("Index", addressAdmin);
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