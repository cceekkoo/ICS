using System.Collections.Generic;

namespace ICS.Models.AdminMerge
{
    public class SlideAdminMerge
    {
        public IEnumerable<Slide_Translate> slides { get; set; }
        public Slide_Translate slide_Translate { get; set; }
        public IEnumerable<Language> languages { get; set; }
        public int defaultLanguageID { get; set; }
    }
}