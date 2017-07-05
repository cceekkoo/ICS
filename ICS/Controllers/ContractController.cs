using ICS.Models;
using ICS.Models.Merge;
using ICS.Utilities;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class ContractController : Controller
    {
        private ICSDBContext db = new ICSDBContext();

        private ContractMerge MyMethod()
        {
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];
            TempData["active"] = "5";

            ContractMerge contractMerge = new ContractMerge();
            contractMerge.site_Contents = db.Site_Contents.Where(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID >= 84 && x.Value_ID <= 121).ToList();
            contractMerge.menu = db.Menus_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 7).Text;
            contractMerge.captchaLanguage = db.Languages.Find(CurrentLanguage.Language).ForCaptcha;
            return contractMerge;
        }

        private ContractMerge MyMethod(Contract contract)
        {
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];

            ContractMerge contractMerge = new ContractMerge();
            contractMerge.site_Contents = db.Site_Contents.Where(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID >= 84 && x.Value_ID <= 121).ToList();
            contractMerge.contract = contract;
            contractMerge.menu = db.Menus_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 7).Text;
            contractMerge.captchaLanguage = db.Languages.Find(CurrentLanguage.Language).ForCaptcha;
            return contractMerge;
        }

        // GET: Contract
        public ActionResult Index()
        {
            return View(MyMethod());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Contract contract)
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
                        mail.Subject = "YENİ MÜQAVİLƏ FORMU";
                        mail.Body = "Şirkətin adı: " + contract.Company_Name + "\n\nŞirkətin sahibi: " + contract.Company_Manager +
                                    "\n\nÜnvan: " + contract.Address + "\n\nVÖEN (Şirkət): " + contract.Company_VOEN +
                                    "\n\nBankın adı: " + contract.Bank_Name + "\n\nHesablaşma hesabı: " + contract.Settlement_Account +
                                    "\n\nKod: " + contract.Code + "\n\nVÖEN (Bank): " + contract.Bank_VOEN +
                                    "\n\nMüxbir hesabı: " + contract.Correspondent_Account + "\n\nSwift: " + contract.Swift;


                        //send the message 
                        SmtpClient smtp = new SmtpClient("mail.intercs.az", 8889);

                        //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                        NetworkCredential Credentials = new NetworkCredential("info@intercs.az", "ICS123456@");
                        smtp.Credentials = Credentials;
                        contract.Send_Date = DateTime.Now;
                        db.Contracts.Add(contract);
                        db.SaveChanges();

                        smtp.Send(mail);
                        ModelState.Clear();
                        ViewBag.MessageSend = db.Site_Contents.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 51).Text;
                        return View(MyMethod());
                    }
                }
                return View(MyMethod(contract));
            }
            catch
            {
                ViewBag.MessageSend = db.Site_Contents.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 54).Text;
                return View(MyMethod(contract));
            }
        } 
    }
}