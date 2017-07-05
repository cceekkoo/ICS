using System.Collections.Generic;

namespace ICS.Models.AdminMerge
{
    public class ServicesAdminMerge
    {
        public IEnumerable<Services_Translate> services { get; set; }
        public Services_Translate services_Translate { get; set; }
        public IEnumerable<Language> languages { get; set; }
        public int defaultLanguageID { get; set; }
    }
}