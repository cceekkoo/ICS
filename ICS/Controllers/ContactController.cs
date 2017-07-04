using ICS.Models;
using ICS.Models.Merge;
using ICS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        private ICSDBContext db = new ICSDBContext();

        private ContactMerge MyMethod()
        {
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];
            TempData["active"] = "3";

            ContactMerge contactMerge = new ContactMerge();
            contactMerge.site_Contents = db.Site_Contents.Where(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID >= 13 && x.Value_ID <= 31).ToList();
            contactMerge.image = db.Site_Images.FirstOrDefault(x => x.ID == 2).image;
            contactMerge.menu = db.Menus_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 3).Text;
            contactMerge.captchaLanguage = db.Languages.Find(CurrentLanguage.Language).ForCaptcha;
            return contactMerge;

        }

        private ContactMerge MyMethod(SendEmail sendEmail)
        {
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];
            TempData["active"] = "3";

            ContactMerge contactMerge = new ContactMerge();
            contactMerge.site_Contents = db.Site_Contents.Where(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID >= 13 && x.Value_ID <= 31).ToList();
            contactMerge.image = db.Site_Images.FirstOrDefault(x => x.ID == 2).image;
            contactMerge.menu = db.Menus_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 3).Text;
            contactMerge.sendEmail = sendEmail;
            contactMerge.captchaLanguage = db.Languages.Find(CurrentLanguage.Language).ForCaptcha;
            return contactMerge;

        }

        public ActionResult Index()
        {
            return View(MyMethod());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SendEmail sendEmail)
        {
            try
            {
                var response = Request["g-recaptcha-response"];
                //secret that was generated in key value pair
                const string secret = "6Lc5Vh8UAAAAAM7zLzmUY72ApD7LoPPi2N9xAq6_";

                var client = new WebClient();
                var reply =
                    client.DownloadString(
                        string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

                var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                //when response is false check for the error message
                if (!captchaResponse.Success)
                {
                    ViewBag.MessageSend = db.Site_Contents.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 47).Text;
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        MailMessage mail = new MailMessage();

                        mail.From = new MailAddress("info@intercs.az");
                        foreach (var item in db.SendEmailToes)
                        {
                            mail.To.Add(item.Email);
                        }

                        //set the content 
                        mail.Subject = "";
                        mail.Body = "Ad:" + sendEmail.Name + "  Email: " + sendEmail.Email + "  Nömrə: " + sendEmail.PhoneNumber + "  Mesaj: " + sendEmail.Message;


                        //send the message 
                        SmtpClient smtp = new SmtpClient("mail.intercs.az", 8889);

                        //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                        NetworkCredential Credentials = new NetworkCredential("info@intercs.az", "ICS123456@");
                        smtp.Credentials = Credentials;

                        smtp.Send(mail);
                        ModelState.Clear();
                        ViewBag.MessageSend = db.Site_Contents.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 51).Text;
                        return View(MyMethod());
                    }
                }
                return View(MyMethod(sendEmail));
            }
            catch
            {
                ViewBag.MessageSend = db.Site_Contents.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 54).Text;
                return View(MyMethod(sendEmail));
            }
        }
    }
}