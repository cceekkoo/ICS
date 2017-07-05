using ICS.Models;
using ICS.Models.Merge;
using ICS.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class OrderController : Controller
    {
        private ICSDBContext db = new ICSDBContext();

        private OrdersMerge MyMethod()
        {
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];
            TempData["active"] = "5";

            OrdersMerge ordersMerge = new OrdersMerge();
            ordersMerge.site_Contents = db.Site_Contents.Where(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID >= 57 && x.Value_ID <= 81).ToList();

            OrderServiceMerge orderServiceMerge = new OrderServiceMerge();
            List<ServicesForOrder> servicesForOrders = new List<ServicesForOrder>();
            foreach (var item in db.Services_Translate.Where(x => x.Language_ID == CurrentLanguage.Language))
            {
                ServicesForOrder servicesForOrder = new ServicesForOrder();
                servicesForOrder.ID = item.ID;
                servicesForOrder.Title = item.Title;
                servicesForOrder.Text = item.Text;
                servicesForOrder.Value_ID = item.Value_ID;
                servicesForOrder.Language_ID = item.Language_ID;
                servicesForOrder.Is_Selected = false;
                servicesForOrders.Add(servicesForOrder);
            }
            orderServiceMerge.servicesForOrder = servicesForOrders;
            ordersMerge.orderServiceMerge = orderServiceMerge;
            ordersMerge.menu = db.Menus_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 6).Text;
            ordersMerge.captchaLanguage = db.Languages.Find(CurrentLanguage.Language).ForCaptcha;
            return ordersMerge;
        }

        private OrdersMerge MyMethod(OrderServiceMerge orderServiceMerge)
        {
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];

            OrdersMerge ordersMerge = new OrdersMerge();
            ordersMerge.site_Contents = db.Site_Contents.Where(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID >= 57 && x.Value_ID <= 81).ToList();
            ordersMerge.orderServiceMerge = orderServiceMerge;
            ordersMerge.menu = db.Menus_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 6).Text;
            ordersMerge.captchaLanguage = db.Languages.Find(CurrentLanguage.Language).ForCaptcha;
            return ordersMerge;
        }

        // GET: Order
        public ActionResult Index()
        {
            return View(MyMethod());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(OrderServiceMerge orderServiceMerge)
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

                        if (!orderServiceMerge.servicesForOrder.Any(x => x.Is_Selected == true))
                            ModelState.AddModelError("orderServiceMerge.servicesForOrder", "***");

                        if (ModelState.IsValid)
                        {
                            MailMessage mail = new MailMessage();

                            mail.From = new MailAddress("info@intercs.az");
                            foreach (var item in db.SendEmailToes)
                            {
                                mail.To.Add(item.Email);
                            }

                            string selectedServices = "\n\nSeçilmiş xidmətlər:";
                            int j = 0;
                            foreach (var item in orderServiceMerge.servicesForOrder.Where(x => x.Is_Selected == true))
                            {
                                j++;
                                selectedServices += "\n" + j + ". " + db.Services_Translate.
                                    FirstOrDefault(x => x.Value_ID == item.Value_ID && x.Language_ID == CurrentLanguage.Language).Title;
                            }

                            //set the content 
                            mail.Subject = "YENİ SİFARİŞ";
                            mail.Body = "Şirkətin adı: " + orderServiceMerge.order.Company_Name + "\n\nVÖEN: " +
                                        orderServiceMerge.order.VOEN + "\n\nEmail: " + orderServiceMerge.order.Email + "\n\nƏlaqə üçün şəxs: " +
                                        orderServiceMerge.order.Contact_Name + "\n\nTelefon: " +
                                        orderServiceMerge.order.Phone + selectedServices + "\n\nSifarişə əlavə: " +
                                        orderServiceMerge.order.Additional_Order;


                            //send the message 
                            SmtpClient smtp = new SmtpClient("mail.intercs.az", 8889);

                            //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                            NetworkCredential Credentials = new NetworkCredential("info@intercs.az", "ICS123456@");
                            smtp.Credentials = Credentials;

                            db.Orders.Add(orderServiceMerge.order);
                            db.SaveChanges();
                            foreach (var item in orderServiceMerge.servicesForOrder.Where(x => x.Is_Selected == true))
                            {
                                Order_Services order_Services = new Order_Services();
                                order_Services.Order_ID = orderServiceMerge.order.ID;
                                order_Services.Service_ID = item.Value_ID;
                                db.Order_Services.Add(order_Services);
                            }
                            db.SaveChanges();

                            smtp.Send(mail);
                            ModelState.Clear();
                            ViewBag.MessageSend = db.Site_Contents.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 51).Text;
                            return View(MyMethod());
                        }
                    }
                return View(MyMethod(orderServiceMerge));
            }
            catch
            {
                ViewBag.MessageSend = db.Site_Contents.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 54).Text;
                return View(MyMethod(orderServiceMerge));
            }
        }
    }
}