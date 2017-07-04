using ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICS.Utilities
{
    public class CurrentLanguage
    {
        public static int Language
        {
            get
            {
                int lang;
                using (ICSDBContext db = new ICSDBContext())
                    if (!int.TryParse(HttpContext.Current.Request.Cookies["lang"] == null ? "" 
                        : HttpContext.Current.Request.Cookies["lang"].Value, out lang))
                        return db.Languages.FirstOrDefault().ID;
                return lang;
            }
        }
    }
}