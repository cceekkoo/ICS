using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICS.Models.AdminMerge
{
    public class Site_ContentsAdminMerge
    {
        public IEnumerable<Site_Contents> site_Contents { get; set; }
        public Site_Contents site_Content { get; set; }
        public IEnumerable<Language> languages { get; set; }
        public int defaultLanguageID { get; set; }
    }
}