using ICS.Models;
using System.Linq;
using System.Web.Mvc;

namespace ICS.Controllers
{
    [Authorize]
    public class _OrderController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        // GET: _Contract
        public ActionResult Index()
        {
            return View(db.Orders.OrderByDescending(x => x.ID).ToList());
        }
    }
}