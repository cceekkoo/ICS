using System.Collections.Generic;

namespace ICS.Models.AdminMerge
{
    public class MenuAdminMerge
    {
        public IEnumerable<Menus_Translate> menus { get; set; }
        public Menus_Translate menu_Translate { get; set; }
        public IEnumerable<Language> languages { get; set; }
        public int defaultLanguageID { get; set; }
    }
}