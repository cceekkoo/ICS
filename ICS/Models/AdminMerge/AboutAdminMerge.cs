using System.Collections.Generic;

namespace ICS.Models.AdminMerge
{
    public class AboutAdminMerge
    {
        public IEnumerable<About_Translate> abouts { get; set; }
        public About_Translate about_Translate { get; set; }
        public IEnumerable<Language> languages { get; set; }
        public int defaultLanguageID { get; set; }
    }
}