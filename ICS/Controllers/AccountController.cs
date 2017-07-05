using ICS.Models;
using ICS.Models.AdminMerge;
using ICS.Utilities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ICS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        private CustomMethods customMethods = new CustomMethods();
        private Encryption hash = new Encryption();
        private AccountAdminMerge accountAdmin
        {
            get
            {
                AccountAdminMerge account = new AccountAdminMerge();
                account.user = db.Users.Find(Convert.ToInt32(User.Identity.Name));
                return account;
            }
            set
            {
                accountAdmin = value;
            }
        }

        public ActionResult Index()
        {
            return View(accountAdmin);
        }

        // GET: Users/Edit/5
        public ActionResult Edit()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username")] User edituser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ICSDBContext context = new ICSDBContext())
                    {
                        User newuser = context.Users.Find(Convert.ToInt32(User.Identity.Name));
                        edituser.ID = newuser.ID;
                        edituser.Password = newuser.Password;
                        edituser.image = newuser.image;
                    }
                    db.Users.Attach(edituser);
                    db.Entry(edituser).Property(x => x.Username).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "EditModal";
                accountAdmin.edituser = edituser;
                return View("Index", accountAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "EditModal";
                accountAdmin.edituser = edituser;
                db = new ICSDBContext();
                return View("Index", accountAdmin);
            }
        }

        public ActionResult ChangePassword()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = db.Users.Find(Convert.ToInt32(User.Identity.Name));
                    user.Password = hash.MD5(hash.SHA384(hash.SHA1(hash.SHA256(changePassword.NewPassword))));
                    db.Users.Attach(user);
                    db.Entry(user).Property(x => x.Password).IsModified = true;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                ViewBag.ShowModal = "AddModal";
                accountAdmin.changePassword = changePassword;
                return View("Index", accountAdmin);
            }
            catch
            {
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "AddModal";
                accountAdmin.changePassword = changePassword;
                return View("Index", accountAdmin);
            }
        }

        public ActionResult ImageChange()
        {
            return RedirectToAction("", "_Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImageChange(HttpPostedFileBase file)
        {
            var dbContextTransaction = db.Database.BeginTransaction();
            string image = "";
            User user = db.Users.Find(Convert.ToInt32(User.Identity.Name));
            try
            {
                customMethods.ImageUploadValidation(ModelState, file, "user.image");

                if (ModelState.IsValid)
                {
                    image = user.image;
                    user.image = hash.MD5(string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now))
                        .Replace("-", "").ToLower() + ".jpg";
                    db.Users.Attach(user);
                    db.Entry(user).Property(x => x.image).IsModified = true;
                    db.SaveChanges();

                    customMethods.ImageUpload(file, user.image);

                    dbContextTransaction.Commit();

                    customMethods.ImageDelete(image);

                    return RedirectToAction("Index");
                }

                dbContextTransaction.Rollback();
                ViewBag.ShowModal = "ImageModal";
                accountAdmin.user = user;
                return View("Index", accountAdmin);
            }
            catch
            {
                dbContextTransaction.Rollback();
                ViewBag.Message = "Səhv aşkarlandı. Bir daha yoxlayın";
                ViewBag.ShowModal = "ImageModal";
                accountAdmin.user = user;
                return View("Index", accountAdmin);
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("", "Abouts");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login user)
        {
            if (ModelState.IsValid)
            {
                Encryption hash = new Encryption();
                string password = hash.MD5(hash.SHA384(hash.SHA1(hash.SHA256(user.Password))));
                var usr = db.Users.Where(x => x.Username.Equals(user.Username) &&
                                                 x.Password.Equals(password)).FirstOrDefault();

                if (usr != null)
                {
                    FormsAuthentication.SetAuthCookie(usr.ID.ToString(), false);
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Message = "Username ya Password səhvdir!";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
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