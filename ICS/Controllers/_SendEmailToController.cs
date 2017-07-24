using ICS.Models;
using ICS.Models.AdminMerge;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    [Authorize]
    public class _SendEmailToController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        private SendEmailToAdminMerge sendEmailTos = new SendEmailToAdminMerge();

        private SendEmailToAdminMerge sendEmailToAdmin
        {
            get
            {
                sendEmailTos.sendEmailToes = db.SendEmailToes.ToList();
                return sendEmailTos;
            }
            set
            {
                sendEmailToAdmin = value;
            }
        }

        // GET: SendEmailTostest
        public ActionResult Index()
        {
            return View(sendEmailToAdmin);
        }

        public ActionResult Create()
        {
            return RedirectToAction("", "_SendEmailTo");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Email")]SendEmailTo sendEmailTo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.SendEmailToes.Add(sendEmailTo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ShowModal = "AddModal";
                sendEmailToAdmin.sendEmailTo = sendEmailTo;
                return View("Index", sendEmailToAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "AddModal";
                sendEmailToAdmin.sendEmailTo = sendEmailTo;
                db = new ICSDBContext();
                return View("Index", sendEmailToAdmin);
            }
        }

        public ActionResult Edit()
        {
            return RedirectToAction("", "_SendEmailTo");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Email")] SendEmailTo sendEmailTo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(sendEmailTo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                sendEmailToAdmin.sendEmailTo = sendEmailTo;
                return View("Index", sendEmailToAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                sendEmailToAdmin.sendEmailTo = sendEmailTo;
                db = new ICSDBContext();
                return View("Index", sendEmailToAdmin);
            }
        }

        public ActionResult Delete()
        {
            return RedirectToAction("", "_SendEmailTo");
        }

        // POST: SendEmailTostest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            SendEmailTo sendEmailTo = new SendEmailTo();
            try
            {
                if (id == null) return RedirectToAction("Index");

                sendEmailTo = db.SendEmailToes.Find(id);

                if (sendEmailTo == null) return RedirectToAction("Index");

                db.SendEmailToes.Remove(sendEmailTo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "DeleteModal";
                
                sendEmailToAdmin.sendEmailTo = sendEmailTo;
                db = new ICSDBContext();
                return View("Index", sendEmailToAdmin);
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