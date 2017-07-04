using ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICS.Controllers
{
    public class NotTranslatedController : Controller
    {
        private ICSDBContext db = new ICSDBContext();
        public JsonResult Get(int[] ID)
        {
            var v = from a in db.Languages.Where(x => !ID.Contains(x.ID))
                    select new
                    {
                        ID = a.ID,
                        Language_Short = a.Language_Short
                    };
            return new JsonResult { Data = v, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}