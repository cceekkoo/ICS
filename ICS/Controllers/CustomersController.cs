using ICS.Models;
using ICS.Models.Merge;
using System.Linq;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        private ICSDBContext db = new ICSDBContext();
        public ActionResult Index()
        {
            TempData["controller"] = ControllerContext.RouteData.Values["controller"];
            TempData["action"] = ControllerContext.RouteData.Values["action"];
            TempData["id"] = ControllerContext.RouteData.Values["id"];
            TempData["active"] = "5";

            CustomersMerge customersMerge = new CustomersMerge();
            customersMerge.customers = db.Customers.ToList();
            customersMerge.image = db.Site_Images.FirstOrDefault(x => x.ID == 2).image;
            customersMerge.menu = "Customers";/*db.Menus_Translate.FirstOrDefault(x => x.Language_ID == CurrentLanguage.Language && x.Value_ID == 2).Text*/
            return View(customersMerge);
        }
    }
}