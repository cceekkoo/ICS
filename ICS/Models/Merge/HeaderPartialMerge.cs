using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICS.Models.Merge
{
    public class HeaderPartialMerge
    {
        public Site_Contents site_Content { get; set; }
        public IEnumerable<Language> language { get; set; }
        public Language currentLanguage { get; set; }
        public IEnumerable<Menus_Translate> menus { get; set; }
        public int activeMenu { get; set; }
        public IEnumerable<Social> social { get; set; }

    }
}