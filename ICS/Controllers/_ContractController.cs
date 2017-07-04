using ICS.Models;
using System.Linq;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class _ContractController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        // GET: _Contract
        public ActionResult Index()
        {
            return View(db.Contracts.OrderByDescending(x => x.ID).ToList());
        }
    }
}