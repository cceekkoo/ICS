using ICS.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class ChangeLanguageController : Controller
    {
        private ICSDBContext db = new ICSDBContext();

        public ActionResult Index(int? id)
        {
            if (id == null) return RedirectToAction("", "Home");

            Language language = db.Languages.Find(id);
            if (language == null) return RedirectToAction("", "Home");

            else
            {
                HttpCookie langCookie = new HttpCookie("lang", language.ID.ToString());
                langCookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(langCookie);
            }

            string newid = (TempData["id"] == null) ? "" : TempData["id"].ToString();
            string action = (TempData["action"] == null) ? "" : TempData["action"].ToString();
            string controller = (TempData["controller"] == null) ? "Home" : TempData["controller"].ToString();

            return RedirectToAction(action, controller, new { id = newid });
        }       
    }
}