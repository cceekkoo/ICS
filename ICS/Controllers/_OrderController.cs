using ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
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