using System.Collections.Generic;

namespace ICS.Models.AdminMerge
{
    public class StepAdminMerge
    {
        public IEnumerable<Steps_Translate> steps { get; set; }
        public Steps_Translate step_Translate { get; set; }
        public IEnumerable<Language> languages { get; set; }
        public int defaultLanguageID { get; set; }
    }
}