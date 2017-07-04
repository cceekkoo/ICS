using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICS.Models.AdminMerge
{
    public class LanguageAdminMerge
    {
        public IEnumerable<Language> languages { get; set; }
        public Language language { get; set; }
    }
}